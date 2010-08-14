using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace HappyDog.SL.Common
{
    /// <summary>
    /// Log error message somewhere. In my case, show it to message box
    /// </summary>
    public class Logger : UserControl
    {
        private static volatile Logger instance;
        private static object syncRoot = new object();

        /// <summary>
        /// Get instance
        /// </summary>
        public static Logger Instance
        {
            get
            {
                if (null == instance)
                {
                    lock (syncRoot)
                    {
                        if (null == instance)
                        {
                            instance = new Logger();
                        }
                    }
                }
                return instance;
            }
        }

        public void Error(string msg)
        {
            if (Globals.IsDebugOn)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    MessageBox.Show(msg);
                });
            }
        }

        public void Error(Exception ex)
        {
            if (Globals.IsDebugOn)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    MessageBox.Show(ex.Message + GetCause(ex));
                });
            }
        }

        public void Error(string msg, Exception ex)
        {
            if (Globals.IsDebugOn)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    MessageBox.Show(msg + ex.Message + GetCause(ex));
                });
            }
        }

        private static string GetCause(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            Exception currentEx = ex;
            while(currentEx != null)
            {
                sb.AppendLine("caused by:");
                sb.AppendLine(ex.StackTrace);
                currentEx = currentEx.InnerException;
            }
            return sb.ToString();
        }
    }
}
