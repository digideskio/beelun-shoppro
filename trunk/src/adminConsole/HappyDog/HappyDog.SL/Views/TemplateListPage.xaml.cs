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
using System.Windows.Navigation;

using HappyDog.SL.Controls;
using HappyDog.SL.Data;
using HappyDog.SL.Resources;
using HappyDog.SL.EventArguments;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using HappyDog.SL.Common;
using HappyDog.SL.Content;


namespace HappyDog.SL.Views
{
    public partial class TemplateListPage : Page
    {
        #region Public prop
        public bool NeedUpdateTableData { get { return needUpdateTableData; } set { needUpdateTableData = value; if (needUpdateTableData == true) NeedUpdateTableUI = true; } }
        public bool NeedUpdateTableUI { get { return needUpdateTableUI; } set { needUpdateTableUI = value; } }
        public string CurrentViewTitle = @"Template list view";
        #endregion

        #region Private prop
        private ContentPageContext ContentPageCtx;
        private string _CurrentBodyViewTitleDesc = @"All templates";
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

            // Next, get the list
            Globals.WSClient.getAll_TemplateCompleted += new EventHandler<getAll_TemplateCompletedEventArgs>(WSClient_getAll_TemplateCompleted);
            Globals.WSClient.getAll_TemplateAsync();
        }

        void WSClient_getAll_TemplateCompleted(object sender, getAll_TemplateCompletedEventArgs e)
        {
            Globals.WSClient.getAll_TemplateCompleted -= new EventHandler<getAll_TemplateCompletedEventArgs>(WSClient_getAll_TemplateCompleted);

            if (e.Error == null)
            {
                this.NeedUpdateTableUI = true;

                // Array -> ObservableCollection
                ObservableCollection<template> categoryCollection = new ObservableCollection<template>();
                foreach (template i in e.Result)
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
        public TemplateListPage()
        {
            InitializeComponent();
            this.MyList.DblClick += new HappyDog.SL.Content.DblClickEvent(MyList_DblClick);
        }
        #endregion

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
            ShopproHelper.GoToContentPage(this, PageEnum.TemplateDetailsPage);
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

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            this.ContentPageCtx.GotoAddNewPage = true;  // Go to 'New' page directly
            ShopproHelper.GoToContentPage(this, PageEnum.TemplateDetailsPage);
        }

        private void Refresh_Button_Click(object sender, RoutedEventArgs e)
        {
            this.RefreshPage();
        }

        private void ItemSelected_Checked(object sender, RoutedEventArgs e)
        {
            this.selectedItemId.Add(long.Parse(((CheckBox)sender).Tag.ToString()));
        }

        private void ItemSelected_Unchecked(object sender, RoutedEventArgs e)
        {
            this.selectedItemId.Remove(long.Parse(((CheckBox)sender).Tag.ToString()));
        }

    }
}
