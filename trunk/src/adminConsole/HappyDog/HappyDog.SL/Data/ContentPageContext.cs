using System;
using System.Text;
using System.Collections.Generic;

using HappyDog.SL.Content;
using HappyDog.SL.UIEffect;

namespace HappyDog.SL.Data
{
    /// <summary>
    /// The class representing content page context. 
    /// It holds two kinds of information: (1) which page to show, (2) Which data to bind
    /// So, the page = template + data (or pointers to template and data)
    /// </summary>
    public class ContentPageContext
    {
        /// <summary>
        /// The details view will show 'add new' page if this is set true. Otherwise go to 'read-only' page
        /// </summary>
        private bool gotoAddNewPage = false; // default is false
        public bool GotoAddNewPage { get { return gotoAddNewPage; } set { gotoAddNewPage = value; } }

        /// <summary>
        /// List view will refresh the whole page on startup if 'ListChanged' is set true
        /// </summary>
        public bool ListChanged { get; set; }

        /// <summary>
        /// The data source which will be shared by list view and details view
        /// Often it is a collection
        /// </summary>
        public object DataSource { get; set; }

        /// <summary>
        /// Total number of items in the data context
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// The prop to indicate which particular item to show first in the details view
        /// </summary>
        public int CurrentIndex { get; set; }

        /// <summary>
        /// Definition of SQL clause
        /// </summary>
        public string OrderBy { get; set; }
        public bool Ascending { get; set; }
        public int FirstResult { get; set; }
        public int MaxResult { get; set; }
        public long FilterId { get; set; } // Filter by
        public string SearchText { get; set; }

        /// <summary>
        /// List view has two kinds of mode. One is browsing mode, its data is defined by SQL clause,
        /// the other is search mode which data is defined by search text
        /// </summary>
        private bool inSearchMode = false;
        public bool InSearchMode
        {
            get
            {
                return inSearchMode;
            }
            set
            {
                inSearchMode = value;
                if (inSearchMode == false)
                {
                    SearchText = HappyDog.SL.Resources.UIResources.SEARCHBAR_SEARCHFOR; // Clear the search text
                }
            }
        }

        /// <summary>
        /// Curent page number to support paging
        /// </summary>
        private int currentPageNumber = 1;  // starting from 1
        public int CurrentPageNumber
        {
            get { return currentPageNumber; }
            set
            {
                currentPageNumber = value;
                FirstResult = (currentPageNumber - 1) * ItemsPerPage;
            }

        }
        public int ItemsPerPage { get; set; }


        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public ContentPageContext()
            : this(Globals.ALL_CATEGORIES)
        {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contentPageUri">The page to show</param>
        /// <param name="filterId">The Id of filter. It could be CategoryId in item list; or TabId in category list</param>
        public ContentPageContext(long filterId)
        {
            this.OrderBy = @"id"; // id exists in all object
            this.Ascending = true;
            this.FirstResult = 0;
            this.MaxResult = Globals.LIST_ITEMS_PER_PAGE;
            this.CurrentPageNumber = 1;
            this.ItemsPerPage = Globals.LIST_ITEMS_PER_PAGE;
            this.FilterId = filterId;
            this.InSearchMode = false;
        }
        #endregion

    }
}
