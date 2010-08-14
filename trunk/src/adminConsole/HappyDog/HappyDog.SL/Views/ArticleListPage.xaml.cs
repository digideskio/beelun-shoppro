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
using System.Collections.ObjectModel;
using System.Threading;
using System.ServiceModel;
using System.IO;

using DevExpress.AgDataGrid;
using HappyDog.SL.EventArguments;
using HappyDog.SL.Data;
using HappyDog.SL.Resources;
using HappyDog.SL.UIEffect;
using HappyDog.SL.Controls;
using HappyDog.SL.Content;
using HappyDog.SL.Common;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using System.Windows.Navigation;

namespace HappyDog.SL.Views
{
    public partial class ArticleListPage : Page
    {
        #region Public properties
        /// <summary>
        /// Properties to control whether update data/UI or not
        /// </summary>
        public bool NeedUpdateTableData { get { return needUpdateTableData; } set { needUpdateTableData = value; if (needUpdateTableData == true) NeedUpdateTableUI = true; } }
        public bool NeedUpdateTableUI { get { return needUpdateTableUI; } set { needUpdateTableUI = value; } }

        #endregion

        #region Private Member variables
        private string _CurrentBodyViewTitleDesc = @"All articles";
        private bool isInitDone = false;
        private string CurrentBodyViewTitleDesc
        {
            get
            {
                return _CurrentBodyViewTitleDesc;
            }

            set
            {
                _CurrentBodyViewTitleDesc = value;
            }
        }
        private string CurrentViewTitle { get; set; }
        private ContentPageContext ContentPageCtx;
        private AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
        private List<long> selectedItemId = new List<long>();   // Currently selected itemID

        private bool needUpdateTableData = true;
        private bool needUpdateTableUI = true;

        #endregion

        #region Constructor
        public ArticleListPage()
        {
            InitializeComponent();
            this.MyList.DblClick += new HappyDog.SL.Content.DblClickEvent(MyList_DblClick);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Update orderBy clause
        /// </summary>
        private void UpdateOrderBy()
        {
            if (MyList.SortedColumnCount >= 1)
            {
                this.ContentPageCtx.OrderBy = MyList.SortedColumns[0].FieldName;
                this.ContentPageCtx.Ascending = (MyList.SortedColumns[0].SortOrder == DevExpress.Windows.Data.ColumnSortOrder.Descending) ? (false) : (true);
            }
        }

        /// <summary>
        /// Update the list table data and UI
        /// </summary>
        private void BeginUpdateEntireUI()
        {
            // Refresh grid now
            this.RefreshGrid();

            bool needUpdateData = false;

            if (this.NeedUpdateTableData == false)
            {
                this.UpdateTableUI();
            }
            else
            {
                needUpdateData = true;
            }

            if (!needUpdateData)
            {
                return;
            }

            // Get page number
            this.ContentPageCtx.CurrentPageNumber = 1;

            // Start data access in another thread to avoid blocking UI thread
            ThreadPool.QueueUserWorkItem(StartWork);
        }

        /// <summary>
        /// Thread body
        /// </summary>
        /// <param name="state"></param>
        private void StartWork(object state)
        {
            // Busy
            ShopproHelper.ShowBusyIndicator(this);

            Globals.WSClient.getAll_ArticleCompleted += new EventHandler<getAll_ArticleCompletedEventArgs>(WSClient_getAll_ArticleCompleted);
            Globals.WSClient.getAll_ArticleAsync();
        }

        void WSClient_getAll_ArticleCompleted(object sender, getAll_ArticleCompletedEventArgs e)
        {
            // Release event handler
            Globals.WSClient.getAll_ArticleCompleted -= new EventHandler<getAll_ArticleCompletedEventArgs>(WSClient_getAll_ArticleCompleted);

            if (e.Error == null)
            {
                // Array -> ObservableCollection
                ObservableCollection<article> articleCollection = new ObservableCollection<article>();
                foreach (article i in e.Result)
                {
                    articleCollection.Add(i);
                }
                this.ContentPageCtx.TotalItemCount = articleCollection.Count;
                this.ContentPageCtx.DataSource = articleCollection;
                this.CurrentViewTitle = string.Format(UIResources.MAINVIEW_RECORDSNUMBER, this.CurrentBodyViewTitleDesc, this.ContentPageCtx.TotalItemCount);
                this.UpdateTableUI();
                ShopproHelper.HideBusyIndicator(this);
            }
            else
            {
                ShopproHelper.HideBusyIndicator(this);
                ShopproHelper.ShowMessageWindow(this, "Error", e.Error.Message, false);
            }
        }

        /// <summary>
        /// Refresh data grid. 
        /// Notes: This should be called within a Dispatcher
        /// </summary>
        private void RefreshGrid()
        {
            this.MyList.ExpandAll();

            // Because the AgDataGrid doesn't automatically return to the top when its DataSource
            // is updated, do this now.  Negative effects of not doing so include being presented
            // with an empty grid if fewer results were returned than the previous results.
            if (this.ContentPageCtx.TotalItemCount != 0)
            {
                if (MyList.GroupedColumnCount == 0)
                {
                    MyList.MakeRowVisible(0);
                }
                else
                {
                    MyList.MakeRowVisible(-1); // Top group header row handle
                }
            }
        }

        /// <summary>
        /// Update the whole UI according to latest data model changes
        /// </summary>
        private void UpdateTableUI()
        {
            if (this.NeedUpdateTableUI)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    MyList.DataSource = this.ContentPageCtx.DataSource;
                    int totalPageCount = ((this.ContentPageCtx.TotalItemCount - 1) / Globals.LIST_ITEMS_PER_PAGE) + 1;

                    ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, this.CurrentViewTitle);
                    this.RefreshGrid();

                    this.NeedUpdateTableUI = false;
                    this.NeedUpdateTableData = false;
                });
            }
        }

        /// <summary>
        /// Refresh the whole page
        /// </summary>
        private void RefreshPage()
        {
            this.NeedUpdateTableData = true;
            this.BeginUpdateEntireUI();
        }
        #endregion

        #region Event handlers
        /// <summary>
        /// Double click one list item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyList_DblClick(object sender, MouseEventArgs e)
        {
            // Caculate current index
            this.ContentPageCtx.CurrentIndex = Convert.ToInt32(MyList.FocusedRowVisibleIndex.ToString());
            this.ContentPageCtx.GotoAddNewPage = false;
            ShopproHelper.GoToContentPage(this, PageEnum.ArticleDetailsPage);
        }

        /// <summary>
        /// "Sort by certain column' changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyList_SortingChanged(object sender, DevExpress.Windows.Controls.SortingEventArgs e)
        {
            // Search mode don't support sorting
            if (this.ContentPageCtx.InSearchMode == true)
            {
                // TODO: show message
                return;
            }

            // Category list is not paged. So it is not necesary to re-fetch the data from backend(We assume there is only one admin)
            this.UpdateOrderBy();
            this.NeedUpdateTableData = false;
            this.NeedUpdateTableUI = true;
            this.BeginUpdateEntireUI();
        }

        /// <summary>
        /// 'Delete' button clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedItemId != null && this.selectedItemId.Count != 0)
            {
                ShopproHelper.ShowBusyIndicator(this);
                // TODO: add functionality

                //client.removeManyCompleted += new EventHandler<removeManyCompletedEventArgs>(client_removeCompleted);
                //client.removeManyAsync(this.selectedItemId.ToArray());
            }
            else
            {
                ShopproHelper.ShowMessageWindow(this, "Warning", "There are no selected records.", false);
            }
        }

        /// <summary>
        /// Call back of remove items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_removeCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            // Won't check status. Do this in any event
            this.selectedItemId.Clear();
            this.NeedUpdateTableData = true;
            this.NeedUpdateTableUI = true;
            this.BeginUpdateEntireUI();
        }

        /// <summary>
        /// The item is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemSelected_Unchecked(object sender, RoutedEventArgs e)
        {
            this.selectedItemId.Remove(long.Parse(((CheckBox)sender).Content.ToString()));
        }

        /// <summary>
        /// The item is unchecked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemSelected_Checked(object sender, RoutedEventArgs e)
        {
            this.selectedItemId.Add(long.Parse(((CheckBox)sender).Content.ToString()));
        }

        /// <summary>
        /// 'Add' button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to the item details page
            this.ContentPageCtx.GotoAddNewPage = true;
            ShopproHelper.GoToContentPage(this, PageEnum.ArticleDetailsPage);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, this.CurrentViewTitle);

            if (!this.isInitDone)
            {
                this.isInitDone = true;
                this.ContentPageCtx = ModelProvider.Instance.GetContentPageContext(e.Uri.ToString());
                this.RefreshPage();
            }

            if (this.ContentPageCtx.ListChanged)
            {
                this.ContentPageCtx.ListChanged = false;
                this.RefreshPage();
            }
        }

        /// <summary>
        /// Refresh button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            this.RefreshPage();
        }

        #endregion

    }
}
