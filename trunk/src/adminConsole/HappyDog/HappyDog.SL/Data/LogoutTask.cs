using System;
using System.Net;
using System.Threading;
using System.Diagnostics;

using HappyDog.SL.Common;
using HappyDog.SL.EventArguments;


namespace HappyDog.SL.Data
{
    public class LogoutTask : ITask
    {
        private string STRING_LOGOUT = "ADUser?op=logout";

        public event EventHandler<HDCallBackEventArgs> onComplete;
        public string TaskDescription { get; set; }

        private int id = 0;
        private Exception ex = null;

        /// <summary>
        /// Async version
        /// </summary>
        /// <param name="param"></param>
        /// <param name="id"></param>
        public void StartAsync(object param, int id)
        {
            Thread t = new Thread(ThreadBody);
            t.Start();
        }

        /// <summary>
        /// Thread body for logout
        /// </summary>
        private void ThreadBody()
        {
            try
            {
                // Call logout html here
                AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
                WebClient wc = new WebClient();
                wc.OpenReadCompleted += (s, e) =>
                {
                    // Ignore returned data
                    //byte[] byteArray = ShopproHelper.ReadFully(e.Result);
                    //string contentString = System.Text.UTF8Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
                    //Debug.WriteLine(contentString);
                    nextOneAutoResetEvent.Set();
                };
                wc.OpenReadAsync(new Uri(Globals.WebRootUrl + "/logout.html"));
                nextOneAutoResetEvent.WaitOne();
            }
            catch (Exception ex)
            {
                this.ex = ex;
            }
            finally
            {
                if (this.onComplete != null)
                {
                    if (null != this.ex)
                    {
                        this.onComplete(this, new HDCallBackEventArgs(this.id, new HDException("error in logout", this.ex), null));
                    }
                    else
                    {
                        this.onComplete(this, new HDCallBackEventArgs(this.id, null));
                    }
                }
            }
        }
    }
}
