using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Windows.Browser;

namespace HappyDog.SL.Common
{
    /// <summary>
    /// A download class for hanlding multiple downloads used in the scenarios such as download many entiy xml for processing
    /// Usage: 
    ///     List<string> ls = new List<string>();
    ///     ls.Add(...);r
    ///     RestReader md = new RestReader();
    ///     md.OnSuccessResult += new EventHandler<DownloadArgs>(md_OnSuccessResult);
    ///     md.OnDone += new EventHandler<DownloadArgs>(md_OnDone);
    ///     md.Start(ls.ToArray(), 3, 3);
    /// Notes: 
    ///     OnSuccessResult() will be called one by one, but the sequence is not determined
    /// </summary>
    public class RestReader
    {
        #region A internal enum for tracking state switch
        private enum OnTheFlyState
        {
            NOT_STARTED,
            ONGOING,
            SUCCESS,
            ERROR
        }
        #endregion

        #region private properties
        bool bStarted = false;
        private int maxRetry = 3;
        private int maxOutstandingRequests = 5;
        private string taskID = string.Empty;
        #endregion

        #region private things controlling download process
        const int DefaultTimeout = 3 * 60 * 1000; // 3 minutes timeout. In intranet, 3 min should be enough
        private static object syncRoot = new object();  // A big lock. We possibly need optimize this if this is called heavily.
        private int currentOutstandingRequest = 0;
        private int remainingRequests = 0;
        // private CookieContainer myCookie = new CookieContainer();
        private Dictionary<string, OnTheFlyState> trackingStatus = new Dictionary<string, OnTheFlyState>();
        private Dictionary<string, StatusCode> statusCodes = new Dictionary<string, StatusCode>();
        private Dictionary<string, int> RemainingRetries = new Dictionary<string, int>();
        private Dictionary<string, string> urlMapping = new Dictionary<string, string>(); // <New, Old>
        #endregion

        #region public event handler
        public event EventHandler<DownloadArgs> OnEvent;   // Fired when (1) all download are successful, or (2) one of download failed after retries
        #endregion

        #region public methods
        /// <summary>
        /// Constructor
        /// </summary>
        public RestReader() { }

        /// <summary>
        /// A single url version
        /// </summary>
        /// <param name="url"></param>
        /// <param name="maxRetry"></param>
        /// <param name="maxOutstandingRequests"></param>
        /// <param name="taskID"></param>
        public void Start(string url, int maxRetry, int maxOutstandingRequests, string taskID)
        {
            string[] urls = new string[1];
            urls[0] = url;
            this.Start(urls, maxRetry, maxOutstandingRequests, taskID);
        }

        /// <summary>
        /// Start Async download
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="maxRetry"></param>
        /// <param name="maxOutstandingRequests"></param>
        public void Start(string[] urls, int maxRetry, int maxOutstandingRequests, string taskID)
        {
            // Check event handler
            if (this.OnEvent == null || this.bStarted == true)
            {
                return;
            }

            // Clean string array
            urls = this.CleanStrings(urls);

            this.bStarted = true;

            // Set remaining request
            this.currentOutstandingRequest = 0;
            this.maxRetry = maxRetry;
            this.maxOutstandingRequests = maxOutstandingRequests;
            this.remainingRequests = urls.Length;
            this.taskID = taskID;

            // Normalize url
            this.urlMapping = this.NormalizeUrl(urls);
            urls = this.urlMapping.Keys.ToArray();

            foreach (string oneUrl in urls)
            {
                trackingStatus.Add(oneUrl, OnTheFlyState.NOT_STARTED);
                RemainingRetries.Add(oneUrl, maxRetry);
            }

            lock (syncRoot)
            {
                TrySendNewRequests();
            }
        }

        #endregion

        #region private methods

        /// <summary>
        /// Remove null or empty string from a string array
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        private string[] CleanStrings(string[] strs)
        {
            List<string> ret = new List<string>();
            foreach (string s in strs)
            {
                if (string.IsNullOrEmpty(s) == false)
                {
                    ret.Add(s);
                }
            }

            return ret.ToArray();
        }

        /// <summary>
        /// Abort requst upon timeout
        /// </summary>
        /// <param name="state"></param>
        /// <param name="timedOut"></param>
        private static void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                HttpWebRequest request = state as HttpWebRequest;
                if (request != null)
                {
                    request.Abort();
                }
            }
        }

        /// <summary>
        /// Send new request if we still have free slots
        /// ASSUME: You've hold the lock
        /// </summary>
        private void TrySendNewRequests()
        {
            if (this.remainingRequests != 0)
            {
                string[] urls = urlMapping.Keys.ToArray();
                foreach (string oneUrl in urls)
                {
                    if (this.currentOutstandingRequest < this.maxOutstandingRequests && this.trackingStatus[oneUrl] == OnTheFlyState.NOT_STARTED)
                    {
                        // web request
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(oneUrl));
                        IAsyncResult result = (IAsyncResult)req.BeginGetResponse(new AsyncCallback(WebComplete), req);

                        // this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
                        // TODO: will get a NotSupportedMethod if we add below line.
                        // So we might have to figure out another way to handle time out
                        // ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), req, DefaultTimeout, true);

                        this.trackingStatus[oneUrl] = OnTheFlyState.ONGOING;
                        ++this.currentOutstandingRequest;
                    }
                }
            }
        }

        /// <summary>
        /// Event handler for request complete
        /// </summary>
        /// <param name="a"></param>
        private void WebComplete(IAsyncResult a)
        {
            HttpWebRequest req = (HttpWebRequest)a.AsyncState;
            string requestUrl = req.RequestUri.AbsoluteUri;
            // Console.WriteLine("back: " + requestUrl);

            lock (syncRoot)
            {
                try
                {
                    // Decrease outstanding request first
                    --this.currentOutstandingRequest;

                    HttpWebResponse res = (HttpWebResponse)req.EndGetResponse(a);
                    StatusCode thisRequestStatusCode = (StatusCode)res.StatusCode;

                    if (thisRequestStatusCode == StatusCode.OK)
                    {
                        // Get response content
                        Stream dataStream = res.GetResponseStream();
                        StreamReader reader = new StreamReader(dataStream);
                        string responseContent = reader.ReadToEnd();

                        // Call process data
                        string oldUrl = string.Empty;
                        urlMapping.TryGetValue(requestUrl, out oldUrl);
                        this.OnEvent(this, new DownloadArgs(DownloadStatus.CompleteOne_Success, taskID, oldUrl, responseContent));

                        // Change counters
                        --this.remainingRequests;
                        this.trackingStatus[requestUrl] = OnTheFlyState.SUCCESS;
                    }
                    else
                    {
                        ProcessErrorRequest(requestUrl, thisRequestStatusCode);
                    }
                }
                catch (Exception)
                {
                    ProcessErrorRequest(requestUrl, StatusCode.IntertalServerError);
                }
                finally
                {

                    // We always try to send new ones
                    TrySendNewRequests();

                    // Now, we will check whether everything is done
                    if (this.remainingRequests == 0)
                    {
                        StringBuilder sbd = new StringBuilder();
                        bool bGetError = false;
                        foreach (string theUrl in trackingStatus.Keys)
                        {
                            if (this.trackingStatus[theUrl] == OnTheFlyState.ERROR)
                            {
                                bGetError = true;
                                string oldUrl;
                                urlMapping.TryGetValue(theUrl, out oldUrl);
                                sbd.AppendLine(string.Format("{0}|{1}", this.statusCodes[theUrl].ToString(), oldUrl));
                            }
                        }
                        if (bGetError)
                        {
                            this.OnEvent(this, new DownloadArgs(DownloadStatus.CompleteAll_Error, this.taskID, string.Empty, sbd.ToString()));
                        }
                        else
                        {
                            this.OnEvent(this, new DownloadArgs(DownloadStatus.CompleteAll_Success, this.taskID, string.Empty, string.Empty));
                        }
                    }
                }
            } // Lock
        }

        /// <summary>
        /// One error request happens
        /// </summary>
        /// <param name="requestUrl"></param>
        private void ProcessErrorRequest(string requestUrl, StatusCode code)
        {
            --this.RemainingRetries[requestUrl];
            if (this.RemainingRetries[requestUrl] != 0)
            {
                // Mark it as not started to make it reschedulable
                this.trackingStatus[requestUrl] = OnTheFlyState.NOT_STARTED;
            }
            else
            {
                // Something bad happends. One url fails to download after all retries
                --this.remainingRequests;
                this.trackingStatus[requestUrl] = OnTheFlyState.ERROR;
                this.statusCodes[requestUrl] = code;
            }
        }

        /// <summary>
        /// Keep url consistent across after calling into new Uri
        /// </summary>
        /// <param name="theUrls"></param>
        /// <returns></returns>
        private Dictionary<string, string> NormalizeUrl(string[] theUrls)
        {
            if (null == theUrls)
            {
                return null;
            }
            else
            {
                Dictionary<string, string> mapping = new Dictionary<string, string>();
                foreach (string oneUrl in theUrls)
                {
                    Uri localUrl = new Uri(oneUrl);
                    mapping.Add(localUrl.AbsoluteUri, oneUrl);
                }
                return mapping;
            }
        }
        #endregion
    }

    #region event arguments
    public class DownloadArgs : EventArgs
    {
        public DownloadStatus status { get; set; }
        public string data { get; set; }
        public string url { get; set; }
        public string taskID { get; set; }

        public DownloadArgs(DownloadStatus status, string taskID, string url, string data)
        {
            this.status = status;
            this.data = data;
            this.url = url;
            this.taskID = taskID;
        }
    }

    public enum DownloadStatus
    {
        CompleteAll_Error = 0,
        CompleteAll_Success,
        CompleteOne_Success
    }

    public enum StatusCode
    {
        OK = 200,                       // for successfull requests 
        IntertalServerError = 500       // if an unrecoverable application error occured. 
    }

    #endregion
}
