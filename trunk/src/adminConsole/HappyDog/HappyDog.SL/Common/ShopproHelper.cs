using System;
using System.Net;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

using HappyDog.SL.Controls;
using HappyDog.SL.Content;
using HappyDog.SL.Data;


namespace HappyDog.SL.Common
{
    public class ShopproHelper
    {
        private const string BUSY_INDICATOR_NAME = @"shopproBusyIndicator";

        /// <summary>
        /// mappingStatusEnum -> bool?
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static bool? ConvertToCheckedValue(string mappingStatusEnumString)
        {
            switch (mappingStatusEnumString)
            {
                case "ALL":
                    return true;
                case "NONE":
                    return false;
                case "PARTIAL":
                    return null;
            }
            return null;
        }

        /// <summary>
        /// Read a stream into a byte array
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                    {
                        return ms.ToArray();
                    }
                    ms.Write(buffer, 0, read);
                }
            }
        }

        /// <summary>
        /// Show message window
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        /// <param name="showTwoButton"></param>
        public static void ShowMessageWindow(Control userControl, string title, string msg, bool showTwoButton)
        {
            userControl.Dispatcher.BeginInvoke(delegate()
            {
                // Show message box
                BeelunMessageBox msgBox = new BeelunMessageBox();
                msgBox.Title = title;
                msgBox.DataContext = msg;
                if (!showTwoButton)
                {
                    msgBox.CancelButton.Visibility = Visibility.Collapsed; // Hide cancel button
                }
                msgBox.Show();
            });
        }

        /// <summary>
        /// Show busy indicator
        /// </summary>
        /// <param name="userControl"></param>
        public static void ShowBusyIndicator(Control userControl)
        {
            userControl.Dispatcher.BeginInvoke(delegate()
            {
                BusyIndicator busyIndicator = userControl.FindName(BUSY_INDICATOR_NAME) as BusyIndicator;
                if (null != busyIndicator)
                {
                    busyIndicator.IsBusy = true;
                }
                else
                {
                    throw new Exception(@"Busy indicator name should be 'shopproBusyIndicator'");
                }
            });
        }

        /// <summary>
        /// Show busy indicator
        /// </summary>
        /// <param name="userControl"></param>
        /// <param name="content"></param>
        public static void ShowBusyIndicator(Control userControl, string content)
        {
            userControl.Dispatcher.BeginInvoke(delegate()
            {
                BusyIndicator busyIndicator = userControl.FindName(BUSY_INDICATOR_NAME) as BusyIndicator;
                if (null != busyIndicator)
                {
                    busyIndicator.BusyContent = content;
                    busyIndicator.IsBusy = true;
                }
                else
                {
                    throw new Exception(@"Busy indicator name should be 'shopproBusyIndicator'");
                }
            });
        }

        /// <summary>
        /// Hide busy indicator
        /// </summary>
        /// <param name="userControl"></param>
        public static void HideBusyIndicator(Control userControl)
        {
            userControl.Dispatcher.BeginInvoke(delegate()
            {
                BusyIndicator busyIndicator = userControl.FindName(BUSY_INDICATOR_NAME) as BusyIndicator;
                if (null != busyIndicator)
                {
                    busyIndicator.IsBusy = false;
                }
                else
                {
                    throw new Exception(@"Busy indicator name should be 'shopproBusyIndicator'");
                }
            });
        }

        /// <summary>
        /// Get stream from a web url string
        /// </summary>
        /// <param name="webUrlString"></param>
        /// <returns></returns>
        public static Stream GetStreamFromWebUrl(string webUrlString)
        {
            AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
            Stream imageStream = null;
            WebClient wc = new WebClient();
            wc.OpenReadCompleted += (s, e) =>
            {
                // TODO: check return error?
                imageStream = e.Result;
                nextOneAutoResetEvent.Set();
            };
            wc.OpenReadAsync(new Uri(webUrlString));
            // nextOneAutoResetEvent.WaitOne();

            return imageStream;
        }

        private static void MyThreadBody(object state)
        {
            AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
            ThreadParam threadParam = state as ThreadParam;
            WebClient wc = new WebClient();
            wc.OpenReadCompleted += (s, e) =>
            {
                // TODO: check return error?
                threadParam.stream = e.Result;
                nextOneAutoResetEvent.Set();
            };
            wc.OpenReadAsync(new Uri(threadParam.WebUrlString));
            nextOneAutoResetEvent.WaitOne();
            threadParam.ExitEvent.Set();
        }

        internal class ThreadParam {
            public string WebUrlString { get; set; }
            public AutoResetEvent ExitEvent { get; set; }
            public Stream stream { get; set; }
            public ThreadParam(string webUrlString, AutoResetEvent theEvent)
            {
                this.WebUrlString = webUrlString;
                this.ExitEvent = theEvent;
            }
        }

        /// <summary>
        /// Get stream from a web url string
        /// </summary>
        /// <param name="webUrlString"></param>
        /// <returns></returns>
        public static Stream GetStreamFromWebUrl2(string webUrlString)
        {
            AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
            ThreadParam threadParam = new ThreadParam(webUrlString, nextOneAutoResetEvent);
            ThreadPool.QueueUserWorkItem(MyThreadBody, threadParam);
            nextOneAutoResetEvent.WaitOne();
            return threadParam.stream;
        }

        /// <summary>
        /// Get a thumb nail image from a web url string
        /// </summary>
        /// <param name="UrlString"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Image CreateThumbnailImage(string webUrlString, int width)
        {
            // Get the stream
            AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
            Stream imageStream = null;
            WebClient wc = new WebClient();
            wc.OpenReadCompleted += (s, e) =>
                {
                    // TODO: check return error?
                    imageStream = e.Result;
                    nextOneAutoResetEvent.Set();
                };
            wc.OpenReadAsync(new Uri(webUrlString));
            nextOneAutoResetEvent.WaitOne();

            if (null != imageStream)
            {
                Image image = null;
                using (imageStream)
                {
                    image = CreateThumbnailImage(imageStream, width);
                }
                return image;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Create a thumb nail image from a source
        /// (This should be called in UI thread)
        /// Borrowed:
        /// http://www.wintellect.com/CS/blogs/jprosise/archive/2009/12/17/silverlight-s-big-image-problem-and-what-you-can-do-about-it.aspx
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static Image CreateThumbnailImage(Stream stream, int width)
        {
            BitmapImage bi = new BitmapImage();
            bi.SetSource(stream);

            double cx = width;
            double cy = bi.PixelHeight * (cx / bi.PixelWidth);

            Image image = new Image();
            image.Source = bi;

            WriteableBitmap wb1 = new WriteableBitmap((int)cx, (int)cy);
            ScaleTransform transform = new ScaleTransform();
            transform.ScaleX = cx / bi.PixelWidth;
            transform.ScaleY = cy / bi.PixelHeight;
            wb1.Render(image, transform);
            wb1.Invalidate();

            WriteableBitmap wb2 = new WriteableBitmap((int)cx, (int)cy);
            for (int i = 0; i < wb2.Pixels.Length; i++)
            {
                wb2.Pixels[i] = wb1.Pixels[i];
            }
            wb2.Invalidate();

            Image thumbnail = new Image();
            thumbnail.Width = cx;
            thumbnail.Height = cy;
            thumbnail.Source = wb2;

            return thumbnail;
        }

        /// <summary>
        /// Set work space body content title
        /// </summary>
        /// <param name="userControl"></param>
        /// <param name="title"></param>
        public static void SetWorkSpaceBodyContentPageTitle(UserControl userControl, string title)
        {
            TextBlock pageTitle = userControl.FindName("BodyViewTitle") as TextBlock;
            pageTitle.Text = title;
        }

        /// <summary>
        /// Go to centain content page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageEnum"></param>
        public static void GoToContentPage(Page page, PageEnum pageEnum)
        {
            page.Dispatcher.BeginInvoke(delegate()
            {
                page.NavigationService.Navigate(new Uri(ModelProvider.Instance.GetContentPageUri(pageEnum), UriKind.Relative));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageEnum"></param>
        public static void GoToContentPage(Page page, PageEnum pageEnum, long fid)
        {
            page.Dispatcher.BeginInvoke(delegate()
            {
                page.NavigationService.Navigate(new Uri(ModelProvider.Instance.GetContentPageUri(pageEnum, fid), UriKind.Relative));
            });
        }

        public static void GoBack(Page page)
        {
            page.Dispatcher.BeginInvoke(delegate()
            {
                if (page.NavigationService.CanGoBack)
                {
                    page.NavigationService.GoBack();
                }
            });            
        }
    }
}
