using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Threading;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Navigation;

using HappyDog.SL.EventArguments;
using HappyDog.SL.Data;
using HappyDog.SL.Resources;
using HappyDog.SL.UIEffect;
using HappyDog.SL.Controls;
using HappyDog.SL.Content;
using HappyDog.SL.Common;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using HappyDog.SL.ViewModels;

namespace HappyDog.SL.Views
{
    public partial class ItemDetailsPage : Page
    {
        #region Private properties
        /// <summary>
        /// Holding view related object
        /// </summary>
        private ContentPageContext ContentPageCtx = null;
        private DataFormMode editMode;
        private int ItemsCountInCurrentPage = 0;
        private AddImageChildWindow acw = null;

        // A flag to indicate whether the control curent is loaded or not.
        // We might ignore certain event before this.
        private bool contentLoaded = false;
        #endregion

        public ItemDetailsPage()
        {
            InitializeComponent();
        }

        #region Private methods
        /// <summary>
        /// Thread body
        /// </summary>
        /// <param name="state"></param>
        private void StartWork(object state)
        {
            ItemView itemView = state as ItemView;
            item savedItem = null;
            item theItem = new item();
            itemView.Merge(theItem, editMode);
            AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
            EventHandler<save_ItemCompletedEventArgs> h1 = (s, e) =>
            {
                // TODO: handle error in server side
                savedItem = e.Result;
                nextOneAutoResetEvent.Set();
            };
            Globals.WSClient.save_ItemCompleted += h1;
            Globals.WSClient.save_ItemAsync(theItem);
            nextOneAutoResetEvent.WaitOne();
            Globals.WSClient.save_ItemCompleted -= h1;
            itemView.Restore(new ItemView(savedItem));

            ShopproHelper.HideBusyIndicator(this);

            // No unsaved item
            this.editMode = DataFormMode.ReadOnly;

            // If only one new page, return the original one
            if (this.ContentPageCtx.GotoAddNewPage)
            {
                ShopproHelper.GoBack(this);
                //ShopproHelper.GoToContentPage(this, PageEnum.ItemListPage);
            }
        }

        #endregion

        #region Event handlers
        /// <summary>
        /// 'save' or 'cancel' button is hit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemDataForm_EditEnded(object sender, DataFormEditEndedEventArgs e)
        {
            if (!this.contentLoaded)
            {
                return;
            }

            if (e.EditAction == DataFormEditAction.Commit)
            {
                // 'Save' button is hit
                ShopproHelper.ShowBusyIndicator(this);

                this.ContentPageCtx.ListChanged = true;

                // Start data access in another thread to avoid blocking UI thread
                ThreadPool.QueueUserWorkItem(StartWork, this.itemDataForm.CurrentItem);
            }
            else
            {
                // If only one new page, return to the original one
                if (this.ContentPageCtx.GotoAddNewPage)
                {
                    // ShopproHelper.GoToContentPage(this, PageEnum.ItemListPage);
                    ShopproHelper.GoBack(this);
                }            
            }
        }

        /// <summary>
        /// "Edit" button is hit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemDataForm_BeginningEdit(object sender, System.ComponentModel.CancelEventArgs e)
        {
            editMode = DataFormMode.Edit;
        }

        /// <summary>
        /// 'Add' button is hit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemDataForm_AddingNewItem(object sender, DataFormAddingNewItemEventArgs e)
        {
            editMode = DataFormMode.AddNew;
        }

        /// <summary>
        /// 'Back'button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the item list page
            ShopproHelper.GoBack(this);
        }

        /// <summary>
        /// 'Previous/Next' button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemDataForm_CurrentItemChanged(object sender, EventArgs e)
        {
            ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, string.Format("Record {0} of {1} in current page", this.itemDataForm.CurrentIndex + 1, this.ItemsCountInCurrentPage));
        }

        /// <summary>
        /// After all the control are loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemDataForm_ContentLoaded(object sender, DataFormContentLoadEventArgs e)
        {
            this.contentLoaded = true;

            // Add a new item
            if (true == this.ContentPageCtx.GotoAddNewPage)
            {
                ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, @"New item form");

                this.itemDataForm.Header = "Input new item below:";
                // Show save/cancel only
                this.itemDataForm.CommandButtonsVisibility = DataFormCommandButtonsVisibility.Commit | DataFormCommandButtonsVisibility.Cancel;
                this.itemDataForm.AddNewItem();
            }
        }

        /// <summary>
        /// 'Choose a image' button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            // Input targeting textBox as param
            Button b = sender as Button;
            object param = null;
            if (b.Tag as string == "thumbNailButton")
            {
                param = this.itemDataForm.FindNameInContent("thumbNailTextBox");
            }
            else
            {
                param = this.itemDataForm.FindNameInContent("imageTextBox");
            }
            this.acw = new AddImageChildWindow(param);
            this.acw.Closed += new EventHandler(w_Closed);
            this.acw.Show();
        }

        /// <summary>
        /// 'Add image' child window is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void w_Closed(object sender, EventArgs e)
        {
            Debug.WriteLine("in Closed.");
            AddImageChildWindow w = this.acw;
            if (!string.IsNullOrEmpty(w.ResultingImageUrl))
            {
                // Set targeting text box
                (w.Param as TextBox).Text = w.ResultingImageUrl;
            }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.editMode = DataFormMode.ReadOnly;
            this.contentLoaded = false;
            this.ContentPageCtx = ModelProvider.Instance.GetContentPageContext(e.Uri.ToString());
            this.DataContext = ViewModelHelper.Convert(this.ContentPageCtx.DataSource as ObservableCollection<item>);
            this.ItemsCountInCurrentPage = (this.ContentPageCtx.DataSource as ObservableCollection<item>).Count;
            this.itemDataForm.CurrentIndex = this.ContentPageCtx.CurrentIndex;
            ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, string.Format("Record {0} of {1} in current page", this.itemDataForm.CurrentIndex + 1, this.ItemsCountInCurrentPage));
        }

        #endregion
    }
}
