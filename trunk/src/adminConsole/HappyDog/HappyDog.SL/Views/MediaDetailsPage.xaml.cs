using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.Collections.ObjectModel;

using HappyDog.SL.EventArguments;
using HappyDog.SL.Data;
using HappyDog.SL.Resources;
using HappyDog.SL.UIEffect;
using HappyDog.SL.Controls;
using HappyDog.SL.Content;
using HappyDog.SL.Common;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using HappyDog.SL.ViewModels;
using System.Windows.Navigation;

namespace HappyDog.SL.Views
{
    public partial class MediaDetailsPage : Page
    {
        #region Private prop
        private ContentPageContext ContentPageCtx = null;
        private DataFormMode editMode;
        private int countInCurrentPage = 0;
        private FileInfo fileInfo = null;

        // A flag to indicate whether the control curent is loaded or not.
        // We might ignore certain event before this.
        private bool contentLoaded = false;
        #endregion

        #region Private methods
        /// <summary>
        /// Thread body
        /// </summary>
        /// <param name="state"></param>
        private void StartWork(object state)
        {
            MediaView mediaView = state as MediaView;
            media savedMedia = null;
            media theMedia = new media();
            mediaView.Merge(theMedia, editMode);

            // Save to backend
            if (this.editMode == DataFormMode.Edit)
            {
                // Edit mode
                AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
                EventHandler<save_MediaCompletedEventArgs> h1 = (s, e) =>
                {
                    // TODO: handle error from server side
                    savedMedia = e.Result;
                    nextOneAutoResetEvent.Set();
                };
                Globals.WSClient.save_MediaCompleted += h1;
                Globals.WSClient.save_MediaAsync(theMedia);
                nextOneAutoResetEvent.WaitOne();
                Globals.WSClient.save_MediaCompleted -= h1;
            }
            else
            {
                //
                // Add new mode
                //

                // Get file content
                byte[] content = ShopproHelper.ReadFully(fileInfo.OpenRead());

                // Save to the backend
                AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
                EventHandler<createNew_MediaCompletedEventArgs> h3 = (s, e) =>
                {
                    // TODO: handle error from server side
                    savedMedia = e.Result;
                    nextOneAutoResetEvent.Set();
                };
                Globals.WSClient.createNew_MediaCompleted += h3;
                Globals.WSClient.createNew_MediaAsync(theMedia, content);
                nextOneAutoResetEvent.WaitOne();
                Globals.WSClient.createNew_MediaCompleted -= h3;
            }

            // Check return result. If failure, savedCategory will be null
            if (savedMedia != null)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    mediaView.Restore(new MediaView(savedMedia));
                });
            }
            else
            {
                // Show error message
                ShopproHelper.ShowMessageWindow(this, "Error", "Fail to save this entry.", false);

                // Back to readonly mode
                this.Dispatcher.BeginInvoke(delegate()
                {
                    // Restore cached UI data
                    mediaView.CancelEdit();
                    this.mediaDataForm.CancelEdit();
                });
            }

            // No unsaved item
            this.editMode = DataFormMode.ReadOnly;
            this.fileInfo = null;

            // Hide busy indicator
            ShopproHelper.HideBusyIndicator(this);

            // Go to original page if necessary
            if (this.ContentPageCtx.GotoAddNewPage)
            {
                ShopproHelper.GoToContentPage(this, PageEnum.MediaListPage);
            }
        }
        #endregion

        public MediaDetailsPage()
        {
            InitializeComponent();
        }

        private void mediaDataForm_AddingNewItem(object sender, DataFormAddingNewItemEventArgs e)
        {
            editMode = DataFormMode.AddNew;
        }

        private void mediaDataForm_BeginningEdit(object sender, System.ComponentModel.CancelEventArgs e)
        {
            editMode = DataFormMode.Edit;
        }

        private void mediaDataForm_ContentLoaded(object sender, DataFormContentLoadEventArgs e)
        {
            this.contentLoaded = true;

            // Add a new item
            if (true == this.ContentPageCtx.GotoAddNewPage)
            {
                ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, @"New media form");
                this.mediaDataForm.Header = "Create a new media below:";
                // Show save/cancel only
                this.mediaDataForm.CommandButtonsVisibility = DataFormCommandButtonsVisibility.Commit | DataFormCommandButtonsVisibility.Cancel;
                this.mediaDataForm.AddNewItem();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ShopproHelper.GoBack(this);
        }

        private void mediaDataForm_EditEnded(object sender, DataFormEditEndedEventArgs e)
        {
            if (!this.contentLoaded)
            {
                // If content is not laoded yet, ignore this event. This could happen when we change data form data context.
                return;
            }

            // If 'new' but there is no file selected, show error
            if (e.EditAction == DataFormEditAction.Commit && this.editMode == DataFormMode.AddNew && this.fileInfo == null)
            {
                ShopproHelper.ShowMessageWindow(this, "Error", "A file must be selected.", false);
                return;
            }

            if (e.EditAction == DataFormEditAction.Commit)
            {
                // 'Save' button is hit
                ShopproHelper.ShowBusyIndicator(this);

                this.ContentPageCtx.ListChanged = true;

                // Start data access in another thread to avoid blocking UI thread
                ThreadPool.QueueUserWorkItem(StartWork, this.mediaDataForm.CurrentItem);
            }
            else
            {
                this.fileInfo = null;

                // If only one new page, return to the original one
                if (this.ContentPageCtx.GotoAddNewPage)
                {
                    ShopproHelper.GoToContentPage(this, PageEnum.MediaListPage);
                }
            }

        }

        private void mediaDataForm_CurrentItemChanged(object sender, EventArgs e)
        {
            ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, string.Format("Record {0} in current page", this.mediaDataForm.CurrentIndex + 1));
        }

        /// <summary>
        /// 'Select a file' button is clicked
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
                    ((TextBlock)this.mediaDataForm.FindNameInContent("fileName")).Text = this.fileInfo.Name;
                }
            }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.editMode = DataFormMode.ReadOnly;
            this.contentLoaded = false;
            this.ContentPageCtx = ModelProvider.Instance.GetContentPageContext(e.Uri.ToString());
            this.DataContext = ViewModelHelper.Convert(this.ContentPageCtx.DataSource as ObservableCollection<media>);
            this.countInCurrentPage = (this.ContentPageCtx.DataSource as ObservableCollection<media>).Count;
            this.mediaDataForm.CurrentIndex = this.ContentPageCtx.CurrentIndex;
            ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, string.Format("Record {0} of {1} in current page", this.mediaDataForm.CurrentIndex + 1, this.countInCurrentPage));
        }
    }
}
