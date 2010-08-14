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
using HappyDog.SL.ViewModels;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using System.Windows.Navigation;
using System.Diagnostics;

namespace HappyDog.SL.Views
{
    public partial class ItemListPage : Page
    {
        #region Public properties
        /// <summary>
        /// Properties to control whether update data/UI or not
        /// </summary>
        public bool NeedUpdateTableData { get { return needUpdateTableData; } set { needUpdateTableData = value; if (needUpdateTableData == true) NeedUpdateTableUI = true; } }
        public bool NeedUpdateTableUI { get { return needUpdateTableUI; } set { needUpdateTableUI = value; } }
        public bool NeedUpdateCategoryListData { get { return needUpdateCategoryListData; } set { needUpdateCategoryListData = value; } }
        public bool NeedUpdateCategoryListUI { get { return needUpdateCategoryListUI; } set { needUpdateCategoryListUI = value; } }

        #endregion

        #region Private Member variables
        private bool isInitDone = false;
        private bool TriggedFromInitCategoryList { get; set; }
        private string CurrentBodyViewTitleDesc { get; set; }
        private string DefaultTitleDesc { get; set; }
        private string CurrentViewTitle { get; set; }
        private ContentPageContext ContentPageCtx;
        private AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
        private List<long> selectedItemId = new List<long>();   // Currently selected itemID

        private bool needUpdateTableData = true;
        private bool needUpdateTableUI = true;
        private bool needUpdateCategoryListData = true;
        private bool needUpdateCategoryListUI = true;

        #endregion

        #region Constructor
        public ItemListPage()
        {
            InitializeComponent();
            this.MyList.DblClick += new HappyDog.SL.Content.DblClickEvent(MyList_DblClick);
            Debug.WriteLine("Constructor is called.");
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
        /// Upddate category list
        /// </summary>
        private void UpdateCategoryListUI()
        {
            if (this.NeedUpdateCategoryListUI)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    // Update categories
                    Dictionary<long, string> myDic = new Dictionary<long, string>();
                    myDic.Add(Globals.ALL_CATEGORIES, "All"); // TODO: fix hard code
                    foreach (category c in ModelProvider.Instance.MDC.allCategories)
                    {
                        myDic.Add(c.id, c.name);
                    }
                    myDic.Add(Globals.UNCATEGORIED, "Uncategoried"); // TODO: fix hard code
                    this.NeedUpdateCategoryListUI = false;
                    this.NeedUpdateCategoryListData = false;
                    //this.categoriesComboBox.ItemsSource = myDic;
                    //this.TriggedFromInitCategoryList = true;
                    this.CurrentBodyViewTitleDesc = this.DefaultTitleDesc;
                    //this.categoriesComboBox.SelectedIndex = 0;      // This will trigger update the whole UI.
                });
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

            // If no need to update data, return
            if (this.NeedUpdateCategoryListData == false)
            {
                this.UpdateCategoryListUI();
            }
            else
            {
                needUpdateData = true;
            }

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
        /// Interaction about Item/Category mapping
        /// </summary>
        /// <param name="state"></param>
        private void StartWork_C2IMappingRelated(object state)
        {
            List<long> idList = state as List<long>;

            if (idList != null)
            {
                // Start get current mapping status
                AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
                List<MappingView> l = new List<MappingView>();
                EventHandler<getMappingStatus_ItemCompletedEventArgs> h1 = (s, e) =>
                {
                    foreach (mappingStatus m in e.Result)
                    {
                        l.Add(new MappingView(ShopproHelper.ConvertToCheckedValue(m.mappingStatus1.ToString()), m.name, m.id));
                    }
                    nextOneAutoResetEvent.Set();
                };
                Globals.WSClient.getMappingStatus_ItemCompleted += h1;
                Globals.WSClient.getMappingStatus_ItemAsync(idList.ToArray());
                nextOneAutoResetEvent.WaitOne();
                Globals.WSClient.getMappingStatus_ItemCompleted -= h1;

                // Hide busy indicator
                ShopproHelper.HideBusyIndicator(this);

                // Show category picker
                this.Dispatcher.BeginInvoke(delegate()
                {
                    MappingPicker cp = new MappingPicker(l);
                    cp.Title = "Category mapping";
                    cp.Instruction = "Add selected items to category or remove them from category:";
                    cp.Closed += new EventHandler(cp_Closed);
                    cp.Show();
                });
            }
            else
            {
                Dictionary<long, bool> changedMapping = state as Dictionary<long, bool>;
                if (null != changedMapping)
                {
                    // Start get current mapping status
                    AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);

                    // TODO: optimize this?
                    foreach (long cid in changedMapping.Keys)
                    {
                        EventHandler<setMappingStatus_ItemCompletedEventArgs> h2 = (s, e) =>
                        {
                            // TODO: error handling
                            nextOneAutoResetEvent.Set();
                        };
                        Globals.WSClient.setMappingStatus_ItemCompleted += h2;
                        Globals.WSClient.setMappingStatus_ItemAsync(this.selectedItemId.ToArray(), cid, (changedMapping[cid] == true) ? mappingStatusEnum.ALL : mappingStatusEnum.NONE);
                        nextOneAutoResetEvent.WaitOne();
                        Globals.WSClient.setMappingStatus_ItemCompleted -= h2;
                    }
                }

                // Hide busy indicator
                ShopproHelper.HideBusyIndicator(this);
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

            if (this.ContentPageCtx.InSearchMode)
            {
                // In search mode now. Get the count first
                EventHandler<searchByTextCount_ItemCompletedEventArgs> h1 = (s, e) =>
                {
                    if (e.Error == null)
                    {
                        this.ContentPageCtx.TotalItemCount = e.Result;
                    }
                    nextOneAutoResetEvent.Set();
                };
                Globals.WSClient.searchByTextCount_ItemCompleted += h1;
                Globals.WSClient.searchByTextCount_ItemAsync(this.ContentPageCtx.SearchText);
                nextOneAutoResetEvent.WaitOne();
                Globals.WSClient.searchByTextCount_ItemCompleted -= h1;

                // Get current page
                Globals.WSClient.searchByText_ItemCompleted += new EventHandler<HappyDog.SL.Beelun.Shoppro.WSEntryManager.searchByText_ItemCompletedEventArgs>(WSClient_searchByText_ItemCompleted);
                Globals.WSClient.searchByText_ItemAsync(this.ContentPageCtx.SearchText, this.ContentPageCtx.FirstResult, this.ContentPageCtx.MaxResult);
            }
            else
            {
                // Get current list count async
                EventHandler<getCountByCondition_ItemCompletedEventArgs> h3 = (s, e) =>
                {
                    if (e.Error == null)
                    {
                        this.ContentPageCtx.TotalItemCount = e.Result;
                    }
                    nextOneAutoResetEvent.Set();
                };
                Globals.WSClient.getCountByCondition_ItemCompleted += h3;
                Globals.WSClient.getCountByCondition_ItemAsync(this.ContentPageCtx.FilterId);
                nextOneAutoResetEvent.WaitOne();
                Globals.WSClient.getCountByCondition_ItemCompleted -= h3;

                // Update current category if necessary
                if (this.NeedUpdateCategoryListData)
                {
                    EventHandler<getAll_CategoryCompletedEventArgs> h5 = (s, e) =>
                    {
                        if (e.Error == null)
                        {
                            ModelProvider.Instance.MDC.allCategories = new List<category>(e.Result);
                        }
                        nextOneAutoResetEvent.Set();
                    };
                    Globals.WSClient.getAll_CategoryCompleted += h5;
                    Globals.WSClient.getAll_CategoryAsync();
                    nextOneAutoResetEvent.WaitOne();
                    Globals.WSClient.getAll_CategoryCompleted -= h5;
                    this.UpdateCategoryListUI();
                }

                // Get the list content of current page
                Globals.WSClient.getByCondition_ItemCompleted += new EventHandler<getByCondition_ItemCompletedEventArgs>(WSClient_getByCondition_ItemCompleted);
                Globals.WSClient.getByCondition_ItemAsync(this.ContentPageCtx.OrderBy, this.ContentPageCtx.Ascending, this.ContentPageCtx.FirstResult, this.ContentPageCtx.MaxResult, this.ContentPageCtx.FilterId);
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
            this.NeedUpdateCategoryListData = true;
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
            this.ContentPageCtx.CurrentIndex = Convert.ToInt32(MyList.FocusedRowVisibleIndex.ToString()) + (this.ContentPageCtx.CurrentPageNumber - 1) * this.ContentPageCtx.ItemsPerPage;

            // Show readonly page first
            this.ContentPageCtx.GotoAddNewPage = false;

            // Navigate to the item details page
            ShopproHelper.GoToContentPage(this, PageEnum.ItemDetailsPage, this.ContentPageCtx.FilterId);
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
        /// Call back of item get by condition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WSClient_getByCondition_ItemCompleted(object sender, getByCondition_ItemCompletedEventArgs e)
        {
            Globals.WSClient.getByCondition_ItemCompleted -= new EventHandler<getByCondition_ItemCompletedEventArgs>(WSClient_getByCondition_ItemCompleted);

            if (e.Error == null)
            {
                this.NeedUpdateTableUI = true;

                // Array -> ObservableCollection
                ObservableCollection<item> itemCollection = new ObservableCollection<item>();
                foreach (item i in e.Result)
                {
                    itemCollection.Add(i);
                }

                this.ContentPageCtx.DataSource = itemCollection;
                Debug.WriteLine(string.Format("this: {0}, Old CurrentViewTitle: {1}", this.GetHashCode(), this.CurrentViewTitle));
                this.CurrentViewTitle = string.Format(UIResources.MAINVIEW_RECORDSNUMBER, this.CurrentBodyViewTitleDesc, this.ContentPageCtx.TotalItemCount);
                Debug.WriteLine(string.Format("this: {0}, New CurrentViewTitle: {1}", this.GetHashCode(), this.CurrentViewTitle));
                this.UpdateTableUI();
                ShopproHelper.HideBusyIndicator(this);
            }
            else
            {
                ShopproHelper.HideBusyIndicator(this);
                ShopproHelper.ShowMessageWindow(this, "Error", e.Error.Message, false);
            }

            // Detach event handler
            Globals.WSClient.getByCondition_ItemCompleted -= new EventHandler<getByCondition_ItemCompletedEventArgs>(WSClient_getByCondition_ItemCompleted);
        }

        /// <summary>
        /// Call back of get items by search text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WSClient_searchByText_ItemCompleted(object sender, HappyDog.SL.Beelun.Shoppro.WSEntryManager.searchByText_ItemCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.NeedUpdateTableUI = true;

                // Array -> ObservableCollection
                ObservableCollection<item> itemCollection = new ObservableCollection<item>();
                foreach (item i in e.Result)
                {
                    itemCollection.Add(i);
                }

                this.ContentPageCtx.DataSource = itemCollection;
                this.CurrentViewTitle = string.Format(UIResources.MAINVIEW_RECORDSNUMBER, "Search results for:" + this.ContentPageCtx.SearchText, this.ContentPageCtx.TotalItemCount);
                this.UpdateTableUI();
                ShopproHelper.HideBusyIndicator(this);
            }
            else
            {
                ShopproHelper.ShowMessageWindow(this, "Error", e.Error.Message, false);
            }

            // Detach event handler
            Globals.WSClient.searchByText_ItemCompleted -= new EventHandler<HappyDog.SL.Beelun.Shoppro.WSEntryManager.searchByText_ItemCompletedEventArgs>(WSClient_searchByText_ItemCompleted);

        }

        /// <summary>
        /// The category combox (drop down list) selection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void categoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.TriggedFromInitCategoryList == true)
            {
                this.TriggedFromInitCategoryList = false;
                return;
            }

            if (categoriesComboBox.SelectedItem != null)
            {
                KeyValuePair<long, string> curItem = (KeyValuePair<long, string>)categoriesComboBox.SelectedItem;
                this.ContentPageCtx.FilterId = curItem.Key;
                if (this.ContentPageCtx.FilterId >= 0)
                {
                    this.CurrentBodyViewTitleDesc = @"Items in category '" + curItem.Value + @"'";
                }
                else if (this.ContentPageCtx.FilterId == Globals.ALL_CATEGORIES)
                {
                    this.CurrentBodyViewTitleDesc = "All items";
                }
                else if (this.ContentPageCtx.FilterId == Globals.UNCATEGORIED)
                {
                    this.CurrentBodyViewTitleDesc = "Uncategorized items";
                }
                else
                {
                    // Select invalid item. Return now
                    return;
                }
            }
            else
            {
                // After upload the item, categoriesComboBox.SelectedItem is null. We set it as 'all' in this case.
                this.ContentPageCtx.FilterId = Globals.ALL_CATEGORIES;
                this.CurrentBodyViewTitleDesc = "All items";
            }

            // Turn off search mode and then update the list
            this.GoToNormalMode();
            this.NeedUpdateTableData = true;
            this.NeedUpdateTableUI = true;
            this.BeginUpdateEntireUI();
        }

        /// <summary>
        /// Import a file
        /// This is handled by Java side "com.beelun.shoppro.web.FileUploadController"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Import_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            dlg.Filter = "csv file (*.csv)|*.csv";

            bool? retval = dlg.ShowDialog();

            if (retval != null && retval == true)
            {
                // UploadFile(dlg.File.Name, dlg.File.OpenRead());
                // Refer to: com.beelun.shoppro.web.FileUploadController
                ShopproHelper.ShowBusyIndicator(this);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(Globals.WebRootUrl + "admin/import-items.html"));
                request.PostMultiPartAsync(new Dictionary<string, object> { { "fromSL", "1" }, { "importItems", "1" }, { "file", dlg.File } }, new AsyncCallback(asyncResult =>
                {
                    HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult);
                    Stream responseStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    this.Dispatcher.BeginInvoke(delegate
                    {
                        string result = reader.ReadToEnd();
                        response.Close();

                        // See com.beelun.shoppro.web.FileUploadController.onSubmit() for error message
                        if (result == @"success")
                        {
                            ShopproHelper.ShowMessageWindow(this, "Message", @"Done. The file is imported to your web site successfully. Next we will refresh current list.", false);

                            // Refresh the list
                            this.NeedUpdateCategoryListData = true;
                            this.NeedUpdateCategoryListUI = true;
                            this.NeedUpdateTableData = true;
                            this.NeedUpdateTableUI = true;
                            this.ContentPageCtx.InSearchMode = false; // Turn off search mode
                            this.BeginUpdateEntireUI();
                        }
                        else
                        {
                            ShopproHelper.ShowMessageWindow(this, "Error", result, false);
                            // TODO: show error messasge
                            throw new Exception("Handle error message here.");
                        }
                    });
                }));
            }
        }

        /// <summary>
        /// Event handler for export button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Export_Button_Click(object sender, RoutedEventArgs e)
        {
            String theUrl = string.Empty;
            if (this.ContentPageCtx.InSearchMode)
            {
                theUrl = String.Format(@"{0}admin/export-items.html?searchText={1}", Globals.WebRootUrl, this.ContentPageCtx.SearchText);
            }
            else
            {
                theUrl = String.Format(@"{0}admin/export-items.html?orderBy={1}&categoryId={2}&ascending={3}", Globals.WebRootUrl, this.ContentPageCtx.OrderBy, this.ContentPageCtx.FilterId, this.ContentPageCtx.Ascending, this.ContentPageCtx.SearchText);
            }
            System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(theUrl), "_newWindow", "toolbar=0,menubar=0,resizable=0,scrollbars=0,top=350,left=480,width=50,height=50");
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
                Globals.WSClient.removeMany_ItemCompleted += new EventHandler<removeMany_ItemCompletedEventArgs>(WSClient_removeMany_ItemCompleted);
                Globals.WSClient.removeMany_ItemAsync(this.selectedItemId.ToArray());
            }
            else
            {
                ShopproHelper.ShowMessageWindow(this, "Warning", "There are no selected records.", false);
            }
        }

        void WSClient_removeMany_ItemCompleted(object sender, removeMany_ItemCompletedEventArgs e)
        {
            // Won't check status. Do this in any event
            this.selectedItemId.Clear();
            this.NeedUpdateTableData = true;
            this.NeedUpdateTableUI = true;
            this.BeginUpdateEntireUI();

            // Detach event handler
            Globals.WSClient.removeMany_ItemCompleted -= new EventHandler<removeMany_ItemCompletedEventArgs>(WSClient_removeMany_ItemCompleted);
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
            ShopproHelper.GoToContentPage(this, PageEnum.ItemDetailsPage);
        }

        /// <summary>
        /// Category mapping button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CMap_Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedItemId.Count == 0)
            {
                ShopproHelper.ShowMessageWindow(this, "Warning", "Please select items first, and then map/unmap them to categories.", false);
            }
            else
            {
                ShopproHelper.ShowBusyIndicator(this);

                // Get C2I map by sending selectedItemIds to backend ItemManager
                ThreadPool.QueueUserWorkItem(StartWork_C2IMappingRelated, this.selectedItemId);
            }
        }

        /// <summary>
        /// Category picker window is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cp_Closed(object sender, EventArgs e)
        {
            MappingPicker cp = (MappingPicker)sender;
            if (cp.ChangedMapping.Count != 0)
            {
                ShopproHelper.ShowBusyIndicator(this);
                ThreadPool.QueueUserWorkItem(StartWork_C2IMappingRelated, cp.ChangedMapping);
            }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Should be always called to update page title
            ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, this.CurrentViewTitle);
            if (!this.isInitDone)
            {
                this.isInitDone = true;
                this.ContentPageCtx = ModelProvider.Instance.GetContentPageContext(string.Format("/ItemListPage/{0}", this.NavigationContext.QueryString["q"]));
                if (int.Parse(this.NavigationContext.QueryString["q"]) == Globals.ALL_CATEGORIES)
                {
                    this.DefaultTitleDesc = "All items";
                }
                else
                {
                    this.DefaultTitleDesc = this.NavigationContext.QueryString["t"];
                }
                this.RefreshPage();
            }

            if (this.ContentPageCtx.ListChanged)
            {
                this.ContentPageCtx.ListChanged = false;
                this.RefreshPage();
            }

            // Debug.WriteLine("ItemListPage Hash: {0}, Uri: {1}, ContentPageCtx hash: {2}, View title: {3}", this.GetHashCode(), e.Uri.ToString(), this.ContentPageCtx.GetHashCode(), this.CurrentViewTitle);
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
