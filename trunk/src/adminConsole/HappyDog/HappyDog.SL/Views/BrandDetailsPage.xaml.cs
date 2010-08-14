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
    public partial class BrandDetailsPage : Page
    {
        #region Private prop
        private ContentPageContext ContentPageCtx = null;
        private DataFormMode editMode;
        private int countInCurrentPage = 0;
        private bool contentLoaded = false;
        #endregion

        #region Private methods
        /// <summary>
        /// Thread body
        /// </summary>
        /// <param name="state"></param>
        private void StartWork(object state)
        {
            BrandView brandView = state as BrandView;
            brand savedBrand = null;
            brand theBrand = new brand();
            brandView.Merge(theBrand, editMode);

            AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
            EventHandler<save_BrandCompletedEventArgs> h1 = (s, e) =>
            {
                // TODO: handle error from server side
                savedBrand = e.Result;
                nextOneAutoResetEvent.Set();
            };
            Globals.WSClient.save_BrandCompleted += h1;
            Globals.WSClient.save_BrandAsync(theBrand);
            nextOneAutoResetEvent.WaitOne();
            Globals.WSClient.save_BrandCompleted -= h1;

            // Check return result. If failure, savedCategory will be null
            if (savedBrand != null)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    brandView.Restore(new BrandView(savedBrand));
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
                    brandView.CancelEdit();
                    this.brandDataForm.CancelEdit();
                });
            }

            // No unsaved item
            this.editMode = DataFormMode.ReadOnly;

            // Hide busy indicator
            ShopproHelper.HideBusyIndicator(this);

            // Go to original page if necessary
            if (this.ContentPageCtx.GotoAddNewPage)
            {
                ShopproHelper.GoToContentPage(this, PageEnum.BrandListPage);
            }
        }
        #endregion

        #region Constructor
        public BrandDetailsPage()
        {
            InitializeComponent();
        }
        #endregion

        private void brandDataForm_AddingNewItem(object sender, DataFormAddingNewItemEventArgs e)
        {
            editMode = DataFormMode.AddNew;
        }

        private void brandDataForm_BeginningEdit(object sender, System.ComponentModel.CancelEventArgs e)
        {
            editMode = DataFormMode.Edit;
        }

        private void brandDataForm_ContentLoaded(object sender, DataFormContentLoadEventArgs e)
        {
            this.contentLoaded = true;
            // Add a new item
            if (true == this.ContentPageCtx.GotoAddNewPage)
            {
                ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, @"New brand form");
                this.brandDataForm.Header = "Create a new brand below:";
                // Show save/cancel only
                this.brandDataForm.CommandButtonsVisibility = DataFormCommandButtonsVisibility.Commit | DataFormCommandButtonsVisibility.Cancel;
                this.brandDataForm.AddNewItem();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ShopproHelper.GoBack(this);
        }

        private void brandDataForm_EditEnded(object sender, DataFormEditEndedEventArgs e)
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
                ThreadPool.QueueUserWorkItem(StartWork, this.brandDataForm.CurrentItem);
            }
            else
            {
                // If only one new page, return to the original one
                if (this.ContentPageCtx.GotoAddNewPage)
                {
                    ShopproHelper.GoToContentPage(this, PageEnum.BrandListPage);
                }
            }

        }

        private void brandDataForm_CurrentItemChanged(object sender, EventArgs e)
        {
            ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, string.Format("Record {0} in current page", this.brandDataForm.CurrentIndex + 1));
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.editMode = DataFormMode.ReadOnly;
            this.contentLoaded = false;
            this.ContentPageCtx = ModelProvider.Instance.GetContentPageContext(e.Uri.ToString());
            this.DataContext = ViewModelHelper.Convert(this.ContentPageCtx.DataSource as ObservableCollection<brand>);
            this.countInCurrentPage = (this.ContentPageCtx.DataSource as ObservableCollection<brand>).Count;
            this.brandDataForm.CurrentIndex = this.ContentPageCtx.CurrentIndex;
            ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, string.Format("Record {0} of {1} in current page", this.brandDataForm.CurrentIndex + 1, this.countInCurrentPage));
        }
    }
}
