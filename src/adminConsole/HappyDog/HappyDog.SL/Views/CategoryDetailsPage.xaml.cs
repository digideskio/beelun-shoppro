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

using HappyDog.SL.EventArguments;
using HappyDog.SL.Data;
using HappyDog.SL.Resources;
using HappyDog.SL.UIEffect;
using HappyDog.SL.Controls;
using HappyDog.SL.Common;
using HappyDog.SL.Content;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using HappyDog.SL.ViewModels;
using System.Windows.Navigation;

namespace HappyDog.SL.Views
{
    public partial class CategoryDetailsPage : Page
    {
        #region Private properties
        /// <summary>
        /// Holding view related object
        /// </summary>
        private ContentPageContext ContentPageCtx = null;
        private DataFormMode editMode;
        private int CategorysCountInCurrentPage = 0;

        // A flag to indicate whether the control curent is loaded or not.
        // We might ignore certain event before this.
        private bool contentLoaded = false;
        #endregion

        public CategoryDetailsPage()
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
            CategoryView categoryView = state as CategoryView;
            category savedCategory = null;
            category theCategory = new category();
            categoryView.Merge(theCategory, editMode);

            // Save to backend
            AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
            EventHandler<save_CategoryCompletedEventArgs> h1 = (s, e) =>
            {
                // TODO: handle error from server side
                savedCategory = e.Result;
                nextOneAutoResetEvent.Set();
            };
            Globals.WSClient.save_CategoryCompleted += h1;
            Globals.WSClient.save_CategoryAsync(theCategory);
            nextOneAutoResetEvent.WaitOne();
            Globals.WSClient.save_CategoryCompleted -= h1;

            // Check return result. If failure, savedCategory will be null
            if (savedCategory != null)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    categoryView.Restore(new CategoryView(savedCategory));
                });
            }
            else
            {
                // Show error message
                ShopproHelper.ShowMessageWindow(this, "Error", "Fail to save this entry. It is probably because you are trying to save a entry with conficting fields. \n'Name' and 'Url' fields are required unique for category.", false);

                // Back to readonly mode
                this.Dispatcher.BeginInvoke(delegate()
                {
                    // Restore cached UI data
                    categoryView.CancelEdit();
                    this.itemDataForm.CancelEdit();
                });
            }

            // Hide busy indicator
            ShopproHelper.HideBusyIndicator(this);

            if (this.ContentPageCtx.GotoAddNewPage)
            {
                ShopproHelper.GoToContentPage(this, PageEnum.CategoryListPage);
            }

            // No unsaved item
            this.editMode = DataFormMode.ReadOnly;
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
                    ShopproHelper.GoToContentPage(this, PageEnum.CategoryListPage);
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
            ShopproHelper.GoBack(this);
        }

        /// <summary>
        /// 'Previous/Next' button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemDataForm_CurrentItemChanged(object sender, EventArgs e)
        {
            ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, string.Format("Record {0} of {1} in current page", this.itemDataForm.CurrentIndex + 1, this.CategorysCountInCurrentPage));
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
                ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, @"New category form");

                this.itemDataForm.Header = "Input new category below:";
                // Show save/cancel only
                this.itemDataForm.CommandButtonsVisibility = DataFormCommandButtonsVisibility.Commit | DataFormCommandButtonsVisibility.Cancel;
                this.itemDataForm.AddNewItem();
            }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.editMode = DataFormMode.ReadOnly;
            this.contentLoaded = false;
            this.ContentPageCtx = ModelProvider.Instance.GetContentPageContext(e.Uri.ToString());
            this.DataContext = ViewModelHelper.Convert(this.ContentPageCtx.DataSource as ObservableCollection<category>);
            this.CategorysCountInCurrentPage = (this.ContentPageCtx.DataSource as ObservableCollection<category>).Count;
            this.itemDataForm.CurrentIndex = this.ContentPageCtx.CurrentIndex;
            ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, string.Format("Record {0} of {1} in current page", this.itemDataForm.CurrentIndex + 1, this.CategorysCountInCurrentPage));
        }

        #endregion
    }
}
