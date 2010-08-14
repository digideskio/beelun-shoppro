using System;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;

using HappyDog.SL.Data;

namespace HappyDog.SL.Common
{
    /// <summary>
    /// This is downloader which will featch certain Url content
    /// </summary>
    public class SyncDownloader
    {
        private AutoResetEvent waitHandle = new AutoResetEvent(false);
        private XElement ret = null;
        private Exception ex = null;
        private RestReader md = null;

        public XElement Get(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            else
            {
                this.md = new RestReader();
                this.md.OnEvent += new EventHandler<DownloadArgs>(md_OnEvent);
                this.md.Start(url, 3, 3, "");
                this.waitHandle.WaitOne();   // TODO: timeout?
                this.md.OnEvent -= new EventHandler<DownloadArgs>(md_OnEvent);
                if (null != this.ex)
                {
                    throw new HDException(this.ToString() + ": ", this.ex);
                }
                else
                {
                    return ret;
                }
            }
        }

        void md_OnEvent(object sender, DownloadArgs e)
        {
            try
            {
                switch (e.status)
                {
                    case DownloadStatus.CompleteOne_Success:
                        ResponseParser rp = new ResponseParser();
                        ret = rp.GetResultXElement(e.data);
                        break;
                    case DownloadStatus.CompleteAll_Success:                        
                        this.waitHandle.Set();
                        break;
                    case DownloadStatus.CompleteAll_Error:
                        this.ex = new Exception(e.data);
                        this.waitHandle.Set();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this.ex = ex;
            }
        }
    }
}
