using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

using HappyDog.SL.Common;
using HappyDog.SL.CustomSort;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;

namespace HappyDog.SL.ViewModels
{
    public class MediasViewModel : ViewModelBase
    {
        #region Constructor
        public MediasViewModel()
        {
            DataList = new PagedSortableCollectionView<MediaView>();
            DataList.PageSize = 24; // 24 records per page
            DataList.OnRefresh += new EventHandler<RefreshEventArgs>(DataList_OnRefresh);

            // Start to get data
            GetTotalItemCount();
            GetData();
        }
        #endregion

        #region Private prop
        private string SearchText = null;
        #endregion

        #region Public prop
        /// <summary>
        /// Gets or sets the data list.
        /// </summary>
        /// <value>The data list to data bind ItemsSource.</value>
        public PagedSortableCollectionView<MediaView> DataList { get; private set; }
        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>The size of the page.</value>
        public int PageSize
        {
            get
            {
                return DataList.PageSize;
            }
            set
            {
                if (value == DataList.PageSize) return;
                DataList.PageSize = value;
                RaisePropertyChanged("PageSize");
            }
        }
        #endregion

        #region Public methods
        public void StartSearch(string searchText)
        {
            this.SearchText = searchText;
            GetTotalItemCount();
            GetData();
        }
        #endregion

        #region Event handler
        /// <summary>
        /// Handles the OnRefresh event of the DataList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SilverlightApplication.CustomSort.RefreshEventArgs"/> instance containing the event data.</param>
        void DataList_OnRefresh(object sender, RefreshEventArgs e)
        {
            GetData();
        }

        private void wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            MediaView mediaView = e.UserState as MediaView;
            mediaView.ImageStream = e.Result;
        }
        #endregion

        #region Private prop
        /// <summary>
        /// Get total item count from server
        /// </summary>
        private void GetTotalItemCount()
        {
            WSEntryManagerClient wsClient = Globals.NewWSClient;

            if (null == this.SearchText)
            {
                // TODO: where to release h1?
                EventHandler<getAllCount_MediaCompletedEventArgs> h1 = (s, e) =>
                {
                    DataList.TotalItemCount = e.Result;                    
                };
      
                wsClient.getAllCount_MediaCompleted += h1;
                wsClient.getAllCount_MediaAsync();
            }
            else
            {
                wsClient.searchByTextCount_MediaCompleted += (s, e) =>
                    {
                        DataList.TotalItemCount = e.Result;
                    };
                wsClient.searchByTextCount_MediaAsync(this.SearchText);
            }
        }

        /// <summary>
        /// Gets the data from server.
        /// </summary>
        /// <remarks>
        /// Build paging and sort parameters 
        /// </remarks>
        private void GetData()
        {
            WSEntryManagerClient wsClient = Globals.NewWSClient;

            int firstResult = DataList.PageIndex * DataList.PageSize;
            int maxResult = DataList.PageSize;

            if (null == this.SearchText)
            {
                wsClient.getByCondition_MediaCompleted += (s, e) =>
                    {
                        if (null != e.Error)
                        {
                            // TODO: error handling
                            return;
                        }
                        LoadData(e.Result);
                    };
                wsClient.getByCondition_MediaAsync("id", true, firstResult, maxResult); // Always order by "id"
            }
            else
            {
                wsClient.searchByText_MediaCompleted += (s, e) =>
                    {
                        if (null != e.Error)
                        {
                            return;
                        }
                        LoadData(e.Result);
                    };
                wsClient.searchByText_MediaAsync(this.SearchText, firstResult, maxResult);
            }
        }

        /// <summary>
        /// Load data to the UI
        /// </summary>
        /// <param name="mediaArray"></param>
        private void LoadData(media[] mediaArray)
        {
            DataList.Clear();
            foreach (media theMedia in mediaArray)
            {
                MediaView mediaView = new MediaView(theMedia);
                WebClient wc = new WebClient();
                wc.OpenReadCompleted += new OpenReadCompletedEventHandler(wc_OpenReadCompleted);
                wc.OpenReadAsync(new Uri(mediaView.myUrl), mediaView);
                DataList.Add(mediaView);
            }
        }
        #endregion

    }
}
