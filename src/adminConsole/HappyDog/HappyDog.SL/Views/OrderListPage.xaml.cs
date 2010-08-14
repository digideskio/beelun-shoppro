using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ComponentModel;
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
using HappyDog.SL.Common;
using HappyDog.SL.Content;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using System.Windows.Navigation;

namespace HappyDog.SL.Views
{
    public partial class OrderListPage : Page
    {
        #region Public properties
        /// <summary>
        /// Properties to control whether update data/UI or not
        /// </summary>
        public bool NeedUpdateTableData { get { return needUpdateTableData; } set { needUpdateTableData = value; if (needUpdateTableData == true) NeedUpdateTableUI = true; } }
        public bool NeedUpdateTableUI { get { return needUpdateTableUI; } set { needUpdateTableUI = value; } }

        #endregion

        #region Private Member variables
        private string _CurrentBodyViewTitleDesc = @"All orders";
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
        private bool isInitDone = false;

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
            // If update the UI, clear the selected item.
            this.selectedItemId.Clear();

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
            this.ContentPageCtx.CurrentPageNumber = SummaryListToolbar.CurrentPageNumber;

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

            if (this.ContentPageCtx.InSearchMode)
            {
                this.CurrentBodyViewTitleDesc = this.ContentPageCtx.SearchText;
                Globals.WSClient.getByOrderNo_OrderCompleted += new EventHandler<getByOrderNo_OrderCompletedEventArgs>(WSClient_getByOrderNo_OrderCompleted);
                Globals.WSClient.getByOrderNo_OrderAsync(this.ContentPageCtx.SearchText);
            }
            else
            {
                // Get current list count async
                EventHandler<getAllCount_OrderCompletedEventArgs> h2 = (s, e) =>
                {
                    if (e.Error == null)
                    {
                        this.ContentPageCtx.TotalItemCount = e.Result;
                    }
                    nextOneAutoResetEvent.Set();
                };
                Globals.WSClient.getAllCount_OrderCompleted += h2;
                Globals.WSClient.getAllCount_OrderAsync();
                nextOneAutoResetEvent.WaitOne();
                Globals.WSClient.getAllCount_OrderCompleted -= h2;

                // Get the list content of current page
                this.CurrentBodyViewTitleDesc = @"All orders";
                Globals.WSClient.getByCondition_OrderCompleted += new EventHandler<getByCondition_OrderCompletedEventArgs>(WSClient_getByCondition_OrderCompleted);
                Globals.WSClient.getByCondition_OrderAsync(this.ContentPageCtx.OrderBy, this.ContentPageCtx.Ascending, this.ContentPageCtx.FirstResult, this.ContentPageCtx.MaxResult);
            }
        }

        /// <summary>
        /// Call back of get order by orderNo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WSClient_getByOrderNo_OrderCompleted(object sender, getByOrderNo_OrderCompletedEventArgs e)
        {
            Globals.WSClient.getByOrderNo_OrderCompleted -= new EventHandler<getByOrderNo_OrderCompletedEventArgs>(WSClient_getByOrderNo_OrderCompleted);

            if (e.Error == null)
            {
                // Array -> ObservableCollection
                ObservableCollection<order> orderCollection = new ObservableCollection<order>();
                if (e.Result != null)
                {
                    orderCollection.Add(e.Result);
                }

                this.ContentPageCtx.DataSource = orderCollection;
                if (orderCollection.Count == 0)
                {
                    this.CurrentViewTitle = @"Order not found: " + this.CurrentBodyViewTitleDesc;
                }
                else
                {
                    this.CurrentViewTitle = @"Order found: " + this.CurrentBodyViewTitleDesc;
                }
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
        /// Call back of get order by OrderNo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WSClient_getByCondition_OrderCompleted(object sender, getByCondition_OrderCompletedEventArgs e)
        {
            Globals.WSClient.getByCondition_OrderCompleted -= new EventHandler<getByCondition_OrderCompletedEventArgs>(WSClient_getByCondition_OrderCompleted);

            if (e.Error == null)
            {
                // Array -> ObservableCollection
                ObservableCollection<order> orderCollection = new ObservableCollection<order>();
                foreach (order i in e.Result)
                {
                    orderCollection.Add(i);
                }

                this.ContentPageCtx.DataSource = orderCollection;
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
        /// Toggle search mode
        /// </summary>
        /// <param name="targetSearchMode"></param>
        private void SetInSearchMode(bool targetSearchMode)
        {
            this.ContentPageCtx.InSearchMode = targetSearchMode;
            if (this.ContentPageCtx.InSearchMode)
            {
                this.ContentPageCtx.FirstResult = 0;
                //this.ContentPageCtx.SearchText = this.SummaryListToolbar.SearchTextBox.Text;
            }
            else
            {
                this.ContentPageCtx.FirstResult = 0;
                //this.SummaryListToolbar.SearchTextBox.Text = string.Empty;
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
                    SummaryListToolbar.TotalPageCount = totalPageCount;
                    this.RefreshGrid();

                    ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, this.CurrentViewTitle);

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

        /// <summary>
        /// Go to search mode
        /// </summary>
        /// <param name="text"></param>
        private void GoToSearchMode(string text)
        {
            this.ContentPageCtx.InSearchMode = true;
            this.ContentPageCtx.SearchText = text;
        }

        /// <summary>
        /// Back to normal mode
        /// </summary>
        private void GoToNormalMode()
        {
            this.ContentPageCtx.InSearchMode = false;
            this.ContentPageCtx.SearchText = string.Empty;
        }
        #endregion

        #region Constructor
        public OrderListPage()
        {
            InitializeComponent();
            this.MyList.DblClick += new HappyDog.SL.Content.DblClickEvent(MyList_DblClick);
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
            this.ContentPageCtx.CurrentIndex = Convert.ToInt32(MyList.FocusedRowVisibleIndex.ToString()) + (this.ContentPageCtx.CurrentPageNumber - 1) * this.ContentPageCtx.ItemsPerPage;
            this.ContentPageCtx.GotoAddNewPage = false;
            ShopproHelper.GoToContentPage(this, PageEnum.OrderDetailsPage);
        }

        /// <summary>
        /// User hit the seach button
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

        /// <summary>
        /// Page up/down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SummaryListToolbar_PageChanged(object sender, ListPageChangedArgs e)
        {
            // Reload data
            this.NeedUpdateTableData = true;
            this.NeedUpdateTableUI = true;
            this.BeginUpdateEntireUI();
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

            if (SummaryListToolbar.TotalPageCount > 1) // No point in going back to the server if we only have 1 page of data
            {
                // Update orderBy
                this.UpdateOrderBy();
                this.NeedUpdateTableData = true;
                this.NeedUpdateTableUI = true;
                SummaryListToolbar.CurrentPageNumber = 1;
                this.BeginUpdateEntireUI();
            }
        }


        /// <summary>
        /// 'Delete' button clicked
        /// TODO: remove this?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedItemId != null && this.selectedItemId.Count != 0)
            {
            }
            else
            {
                ShopproHelper.ShowMessageWindow(this, "Warning", "There are no selected records.", false);
            }
        }

        /// <summary>
        /// The item is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemSelected_Unchecked(object sender, RoutedEventArgs e)
        {
            this.selectedItemId.Remove(long.Parse(((CheckBox)sender).Tag.ToString()));
        }

        /// <summary>
        /// The item is unchecked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemSelected_Checked(object sender, RoutedEventArgs e)
        {
            this.selectedItemId.Add(long.Parse(((CheckBox)sender).Tag.ToString()));
        }

        /// <summary>
        /// 'Add' button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            this.ContentPageCtx.GotoAddNewPage = true;
            ShopproHelper.GoToContentPage(this, PageEnum.OrderDetailsPage);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, this.CurrentViewTitle);

            if (!this.isInitDone)
            {
                this.isInitDone = true;
                this.ContentPageCtx = ModelProvider.Instance.GetContentPageContext(e.Uri.ToString());
                this.BeginUpdateEntireUI();
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
