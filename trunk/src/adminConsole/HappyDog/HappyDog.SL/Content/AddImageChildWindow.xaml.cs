using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;

using HappyDog.SL.Common;
using HappyDog.SL.Controls;
using HappyDog.SL.ViewModels;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;

namespace HappyDog.SL.Content
{
    /// <summary>
    /// The window used for selecting images
    /// TODO: idally item.image and item.thumbNail should be defined as a FK to media to get title/alt/description for better SEO
    /// Media is holding both internal and external resources
    /// </summary>
    public partial class AddImageChildWindow : ChildWindow
    {
        #region Public prop
        /// <summary>
        /// The resulting url after working on this window
        /// </summary>
        public string ResultingImageUrl { get; set; }

        /// <summary>
        /// The parameter which will be passed in constructor(It is the targeting TextBox in current case). 
        /// Caller will refer tto this param in Closed event handler
        /// </summary>
        public object Param { get; set; }
        #endregion

        #region Private prop
        private FileInfo fileInfo = null;   // Used for 'From computer' only
        private AutoResetEvent blockingUIResetEvent = new AutoResetEvent(false);
        private AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public AddImageChildWindow(object param)
        {
            InitializeComponent();
            this.Param = param;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Thread body
        /// </summary>
        /// <param name="state"></param>
        private void StartWork_SaveAMedia(object state)
        {
            ShopproHelper.ShowBusyIndicator(this, "Saving this new media to libary...");

            MediaView mediaView = state as MediaView;
            media savedMedia = null;
            media theMedia = new media();
            mediaView.Merge(theMedia, DataFormMode.AddNew);

            // Get file content
            byte[] content = ShopproHelper.ReadFully(fileInfo.OpenRead());

            // Save to the backend
            EventHandler<createNew_MediaCompletedEventArgs> h1 = (s, e) =>
            {
                // TODO: handle error from server side
                savedMedia = e.Result;
                nextOneAutoResetEvent.Set();
            };

            Globals.WSClient.createNew_MediaCompleted += h1;
            Globals.WSClient.createNew_MediaAsync(theMedia, content);
            nextOneAutoResetEvent.WaitOne();
            Globals.WSClient.createNew_MediaCompleted -= h1;

            // Check return result. If failure, savedCategory will be null
            if (savedMedia != null)
            {
                this.ResultingImageUrl = savedMedia.myUrl;

                this.Dispatcher.BeginInvoke(delegate()
                {
                    mediaView.Restore(new MediaView(savedMedia));
                });
            }
            else
            {
                // Show error message
                ShopproHelper.ShowMessageWindow(this, "Error", "Fail to save this entry.", false);
            }

            this.fileInfo = null;

            // Hide busy indicator
            ShopproHelper.HideBusyIndicator(this);

            // Now close the window
           this.Dispatcher.BeginInvoke(delegate()
           {
               Debug.WriteLine("Going to call Close().");
               // This will close this window automatically: http://msdn.microsoft.com/en-us/library/system.windows.controls.childwindow.close%28VS.95%29.aspx
               this.DialogResult = true;    
               Debug.WriteLine("Called Close().");   
           });
        }

        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            // Switch by selected tab
            switch (this.fromSourceTab.SelectedIndex)
            {
                case 0: // From computer. Create a new media object and then get resulting image url
                    // Start data access in another thread to avoid blocking UI thread
                    // The window will be closed in background thread
                    ThreadPool.QueueUserWorkItem(StartWork_SaveAMedia, this.fromComputerTab.DataContext);
                    break;

                case 1: // From media library
                    MediaView mediaView = this.mediaThumbnailsListBox.SelectedItem as MediaView;
                    this.ResultingImageUrl = mediaView.myUrl;
                    this.DialogResult = true; // Close the window now
                    break;

                default:
                    throw new NotImplementedException("Never hit here.");
            }

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Set dialog result
            Debug.WriteLine("In cancal button click...");
            this.ResultingImageUrl = null;
            this.DialogResult = false;
        }

        /// <summary>
        /// "From computer tab" is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fromComputerTab_Loaded(object sender, RoutedEventArgs e)
        {
            MediaView mediaView = new MediaView();
            this.fromComputerTab.DataContext = mediaView;
        }

        /// <summary>
        /// 'From media library' tab is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fromMediaLibray_Loaded(object sender, RoutedEventArgs e)
        {
            if (null == this.fromMediaLibray.DataContext)
            {
                this.fromMediaLibray.DataContext = new MediasViewModel();
            }
        }

        /// <summary>
        /// 'File selector' in from computer tab is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fileSelector_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            dlg.Filter = "all files (*.*)|*.*";

            bool? retval = dlg.ShowDialog();

            if (retval != null && retval == true)
            {
                FileInfo fileInfo = dlg.File;

                // Verify file length. 8MB is upper limit
                if (fileInfo.Length > Globals.MAX_UPLOAD_SIZE)
                {
                    this.fileInfo = null;
                    ShopproHelper.ShowMessageWindow(this, "Error", "File size must be less than 8 MB.", false);
                }
                else
                {
                    this.fileInfo = fileInfo;
                    this.fileName.Text = this.fileInfo.Name;
                }
            }
        }

        /// <summary>
        /// "Search" button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBox_SearchList(object sender, HappyDog.SL.EventArguments.ListSearchArgs e)
        {
            if (e.InSearchMode == true)
            {
                MediasViewModel mediaViewModel = this.fromMediaLibray.DataContext as MediasViewModel;
                if (null != mediaViewModel && !string.IsNullOrEmpty(e.SearchText))
                {
                    mediaViewModel.StartSearch(e.SearchText);
                }
            }
            else
            {
                // Back to none-search mode: re-attach the view model
                this.fromMediaLibray.DataContext = new MediasViewModel();
            }
        }
    }
}

