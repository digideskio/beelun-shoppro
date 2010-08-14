using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Threading;
using System.Collections.ObjectModel;

using HappyDog.SL.Controls;
using HappyDog.SL.Data;
using HappyDog.SL.Resources;
using HappyDog.SL.EventArguments;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using HappyDog.SL.Common;
using HappyDog.SL.Content;
using System.Windows.Navigation;

namespace HappyDog.SL.Views
{
    public partial class BrandListPage : Page
    {
        #region Public prop
        public bool NeedUpdateTableData { get { return needUpdateTableData; } set { needUpdateTableData = value; if (needUpdateTableData == true) NeedUpdateTableUI = true; } }
        public bool NeedUpdateTableUI { get { return needUpdateTableUI; } set { needUpdateTableUI = value; } }
        public string CurrentViewTitle = @"Brand list view";
        #endregion

        #region Private prop
        private ContentPageContext ContentPageCtx;
        private string _CurrentBodyViewTitleDesc = @"All brands";
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
        private bool needUpdateTableData = true;
        private bool needUpdateTableUI = true;
        private List<long> selectedItemId = new List<long>();   // Currently selected itemID
        private bool isInitDone = false;
        #endregion

        #region Private methods
        /// <summary>
        /// Go to search mode
        /// </summary>
        /// <param name="text"></param>
        private void GoToSearchMode(string text)
        {
            this.ContentPageCtx.InSearchMode = true;
            this.ContentPageCtx.SearchText = text;
            this.ContentPageCtx.FirstResult = 0;
        }

        /// <summary>
        /// Back to normal mode
        /// </summary>
        private void GoToNormalMode()
        {
            this.ContentPageCtx.InSearchMode = false;
            this.ContentPageCtx.SearchText = string.Empty;
            this.ContentPageCtx.FirstResult = 0;
        }

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
        /// Update the entire UI
        /// </summary>
        private void BeginUpdateEntireUI()
        {
            this.RefreshGrid();

            // If no need to update data, return
            if (this.NeedUpdateTableData == false)
            {
                this.UpdateTableUI();
            }
            else
            {
                // Start data access in another thread to avoid blocking UI thread
                ThreadPool.QueueUserWorkItem(StartWork);
            }
        }

        /// <summary>
        /// Thread body
        /// </summary>
        /// <param name="state"></param>
        private void StartWork(object state)
        {
            // Busy
            ShopproHelper.ShowBusyIndicator(this);

            AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);

            // Next, get the list
            Globals.WSClient.getAll_BrandCompleted += new EventHandler<getAll_BrandCompletedEventArgs>(WSClient_getAll_BrandCompleted);
            Globals.WSClient.getAll_BrandAsync();
        
        }

        /// <summary>
        /// Update table UI
        /// </summary>
        private void UpdateTableUI()
        {
            if (this.NeedUpdateTableUI)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    MyList.DataSource = this.ContentPageCtx.DataSource;
                    int totalPageCount = ((this.ContentPageCtx.TotalItemCount - 1) / Globals.LIST_ITEMS_PER_PAGE) + 1;
                    this.RefreshGrid();

                    ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, this.CurrentViewTitle);

                    this.NeedUpdateTableUI = false;
                    this.NeedUpdateTableData = false;
                });
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
        /// Refresh the whole page
        /// </summary>
        private void RefreshPage()
        {
            this.NeedUpdateTableData = true;
            this.BeginUpdateEntireUI();
        }

        #endregion

        #region Constructor
        public BrandListPage()
        {
            InitializeComponent();
            this.MyList.DblClick += new HappyDog.SL.Content.DblClickEvent(MyList_DblClick);
        }
        #endregion

        #region Event handler    

        /// <summary>
        /// Call back of media getByCondition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WSClient_getAll_BrandCompleted(object sender, getAll_BrandCompletedEventArgs e)
        {
            // Release event handler
            Globals.WSClient.getAll_BrandCompleted -= new EventHandler<getAll_BrandCompletedEventArgs>(WSClient_getAll_BrandCompleted);

            if (e.Error == null)
            {
                this.NeedUpdateTableUI = true;

                // Array -> ObservableCollection
                ObservableCollection<brand> categoryCollection = new ObservableCollection<brand>();
                foreach (brand i in e.Result)
                {
                    categoryCollection.Add(i);
                }
                this.ContentPageCtx.TotalItemCount = categoryCollection.Count;
                this.ContentPageCtx.DataSource = categoryCollection;
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
        /// Double click one list item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MyList_DblClick(object sender, MouseEventArgs e)
        {
            // Caculate current index
            this.ContentPageCtx.CurrentIndex = Convert.ToInt32(MyList.FocusedRowVisibleIndex.ToString());
            this.ContentPageCtx.GotoAddNewPage = false;
            ShopproHelper.GoToContentPage(this, PageEnum.BrandDetailsPage);
        }

        /// <summary>
        /// 'Add' button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            this.ContentPageCtx.GotoAddNewPage = true;  // Go to 'New' page directly
            ShopproHelper.GoToContentPage(this, PageEnum.BrandDetailsPage);
        }

        /// <summary>
        /// 'Delete' button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedItemId != null && this.selectedItemId.Count != 0)
            {
                ShopproHelper.ShowBusyIndicator(this);

                WSEntryManagerClient wsClient = Globals.NewWSClient;

                wsClient.removeMany_BrandCompleted += (s, e1) =>
                {
                    // Won't check status. Do this in any event
                    this.selectedItemId.Clear();
                    this.NeedUpdateTableData = true;
                    this.NeedUpdateTableUI = true;
                    this.BeginUpdateEntireUI();
                };
                wsClient.removeMany_BrandAsync(this.selectedItemId.ToArray());
            }
            else
            {
                ShopproHelper.ShowMessageWindow(this, "Warning", "There are no selected records.", false);
            }
        }

        /// <summary>
        /// Sorting is changed
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

        }

        /// <summary>
        /// 'Checked' is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemSelected_Checked(object sender, RoutedEventArgs e)
        {
            this.selectedItemId.Add(long.Parse(((CheckBox)sender).Tag.ToString()));
        }

        /// <summary>
        /// 'Uncheck' is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemSelected_Unchecked(object sender, RoutedEventArgs e)
        {
            this.selectedItemId.Remove(long.Parse(((CheckBox)sender).Tag.ToString()));
        }

        /// <summary>
        /// Search button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SummaryListToolbar_SearchList(object sender, ListSearchArgs e)
        {
            if (e.InSearchMode)
            {
                this.GoToSearchMode(e.SearchText);
                this.NeedUpdateTableData = true;
                this.NeedUpdateTableUI = true;
                this.BeginUpdateEntireUI();
            }
            else
            {
                this.GoToNormalMode();
                this.NeedUpdateTableData = true;
                this.NeedUpdateTableUI = true;
                this.BeginUpdateEntireUI();
            }
        }

        private void SummaryListToolbar_PageChanged(object sender, ListPageChangedArgs e)
        {
            this.NeedUpdateTableData = true;
            this.BeginUpdateEntireUI();
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
