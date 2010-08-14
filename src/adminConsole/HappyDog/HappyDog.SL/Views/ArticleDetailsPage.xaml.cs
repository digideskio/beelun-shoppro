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
using HappyDog.SL.Content;
using HappyDog.SL.Common;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using HappyDog.SL.ViewModels;
using System.Windows.Navigation;

namespace HappyDog.SL.Views
{
    public partial class ArticleDetailsPage : Page
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

        public ArticleDetailsPage()
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
            ArticleView articleView = state as ArticleView;
            article savedArticle = null;
            article theArticle = new article();
            articleView.Merge(theArticle, editMode);

            // Save to backend
            AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
            EventHandler<save_ArticleCompletedEventArgs> h1 = (s, e) =>
            {
                // TODO: handle error from server side
                savedArticle = e.Result;
                nextOneAutoResetEvent.Set();
            };
            Globals.WSClient.save_ArticleCompleted += h1;
            Globals.WSClient.save_ArticleAsync(theArticle);
            nextOneAutoResetEvent.WaitOne();
            Globals.WSClient.save_ArticleCompleted -= h1;

            // Check return result. If failure, savedCategory will be null
            if (savedArticle != null)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    articleView.Restore(new ArticleView(savedArticle));
                });
            }
            else
            {
                ShopproHelper.ShowMessageWindow(this, "Error", "Fail to save this entry. It is probably because you are trying to save a entry with conficting fields. \n'Name' and 'Url' fields are required unique for category.", false);

                // Back to readonly mode
                this.Dispatcher.BeginInvoke(delegate()
                {
                    // Restore cached UI data
                    articleView.CancelEdit();
                    this.itemDataForm.CancelEdit();
                });
            }

            ShopproHelper.HideBusyIndicator(this);

            // No unsaved item
            this.editMode = DataFormMode.ReadOnly;

            // If only one new page, return to the original one
            if (this.ContentPageCtx.GotoAddNewPage)
            {
                ShopproHelper.GoToContentPage(this, PageEnum.ArticleListPage);
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
                    ShopproHelper.GoToContentPage(this, PageEnum.ArticleListPage);
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
                ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, @"New article form");

                this.itemDataForm.Header = "Input new article below:";
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
            this.DataContext = ViewModelHelper.Convert(this.ContentPageCtx.DataSource as ObservableCollection<article>);
            this.CategorysCountInCurrentPage = (this.ContentPageCtx.DataSource as ObservableCollection<article>).Count;
            this.itemDataForm.CurrentIndex = this.ContentPageCtx.CurrentIndex;
            ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, string.Format("Record {0} of {1} in current page", this.itemDataForm.CurrentIndex + 1, this.CategorysCountInCurrentPage));
        }

        #endregion

    }
}
