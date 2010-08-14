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

using HappyDog.SL.EventArguments;

namespace HappyDog.SL.Controls
{
    public partial class SearchNListPagingToolbar : UserControl
    {
        #region Member Variables
        private int currentPageNumber = 1;
        private int totalPageCount = 0;
        private bool isGroupingOn = false;
        #endregion

        #region Dependency prop
        /// <summary>
        /// PromptText dependency prop
        /// </summary>
        public static DependencyProperty PromptTextProperty = DependencyProperty.Register("PromptText", typeof(string), typeof(SearchNListPagingToolbar), new PropertyMetadata(new PropertyChangedCallback(OnPromptTextPropertyValueChanged)));
        public string PromptText
        {
            get { return (string)this.GetValue(PromptTextProperty); }

            set
            {
                base.SetValue(PromptTextProperty, (string)value);
                SearchBox.PromptText = (string)value;
            }
        }
        public static void OnPromptTextPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SearchNListPagingToolbar toolBar = d as SearchNListPagingToolbar;
            toolBar.SearchBox.PromptText = toolBar.PromptText;
        }
        #endregion

        #region Public Events
        public event EventHandler<ListSearchArgs> SearchList;
        public event EventHandler<ListPageChangedArgs> PageChanged;
        #endregion

        #region Constructor
        public SearchNListPagingToolbar()
        {
            InitializeComponent();
            UpdatePageButtons();
        }
        #endregion

        #region Event Handlers
        private void FirstPageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CurrentPageNumber = 1;
        }

        private void PreviousPageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CurrentPageNumber--;
        }

        private void NextPageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CurrentPageNumber++;
        }

        private void LastPageButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CurrentPageNumber = totalPageCount;
        }

        private void ShowGroupPanelToggleButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //isGroupingOn = (bool)ShowGroupPanelToggleButton.IsChecked;

            //if (GroupingStateChanged != null)
            //    GroupingStateChanged(this, new EventArgs());
        }

        private void SearchBox_SearchList(object sender, ListSearchArgs e)
        {
            if (this.SearchList != null)
            {
                this.SearchList(sender, e);
            }
        }

        #endregion

        #region public methods
        public void Reset()
        {
            // Reset this back to initial value
            currentPageNumber = 1;
            totalPageCount = 0;
        }
        #endregion

        #region Public Properties
        public int CurrentPageNumber
        {
            get { return currentPageNumber; }
            set 
            {
                if (value < 1 || value > totalPageCount)
                {
                    // TODO: better exception handling
                    throw new Exception("Invalid page number");
                }

                if (value != currentPageNumber)
                {
                    currentPageNumber = value;
                    UpdatePageNumberLabel();
                    UpdatePageButtons();

                    if (PageChanged != null)
                        PageChanged(this, new ListPageChangedArgs(currentPageNumber));
                }
            }
        }

        public int TotalPageCount
        {
            get { return totalPageCount; }
            set
            {
                totalPageCount = value;

                if (currentPageNumber > totalPageCount)
                    CurrentPageNumber = totalPageCount;

                UpdatePageNumberLabel();
                UpdatePageButtons();
            }
        }

        public bool IsGroupingOn
        {
            get { return isGroupingOn; }
            set 
            { 
                isGroupingOn = value;
                //ShowGroupPanelToggleButton.IsChecked = isGroupingOn;
            }
        }
        #endregion

        #region Private Functions

        private void UpdatePageNumberLabel()
        {
            // TODO: hard coded string
            PageNumberLabel.Text = "Page " + currentPageNumber.ToString() + " of " + totalPageCount.ToString();
        }

        private void UpdatePageButtons()
        {
            if (totalPageCount == 0)
            {
                FirstPageButton.IsEnabled = false;
                PreviousPageButton.IsEnabled = false;
                NextPageButton.IsEnabled = false;
                LastPageButton.IsEnabled = false;
            }
            else
            {
                FirstPageButton.IsEnabled = (currentPageNumber != 1);
                PreviousPageButton.IsEnabled = (currentPageNumber != 1);
                NextPageButton.IsEnabled = (currentPageNumber < totalPageCount);
                LastPageButton.IsEnabled = (currentPageNumber < totalPageCount);
            }
        }
        #endregion

    }
}
