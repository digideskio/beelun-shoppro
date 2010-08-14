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
    public partial class SearchBox : UserControl
    {
        #region Const
        private const string SearchImageUri = @"../Img/Search_16x16.png";
        private const string StopImageUri = @"../Img/Stop_16x16.png";
        #endregion

        #region Dependency prop
        /// <summary>
        /// Text dependency prop
        /// </summary>
        public static DependencyProperty PromptTextProperty = DependencyProperty.Register("PromptText", typeof(string), typeof(SearchBox), new PropertyMetadata(new PropertyChangedCallback(OnPromptTextPropertyValueChanged)));
        public string PromptText
        {
            get { return (string)this.GetValue(PromptTextProperty); }

            set
            {
                base.SetValue(PromptTextProperty, (string)value);
                SearchTextBox.Text = (string)value;
            }
        }
        public static void OnPromptTextPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SearchBox searchBox = d as SearchBox;
            searchBox.SearchTextBox.Text = searchBox.PromptText;
        }
        #endregion

        #region Private prop
        /// <summary>
        /// To save the initial prompty text
        /// </summary>
        private string savedSearchTextBoxText = null;

        /// <summary>
        /// A flag to indicate whether we need fire the external event
        /// </summary>
        private bool fireExternalEvent = true;
        #endregion

        #region Constructor
        public SearchBox()
        {
            InitializeComponent();
        }
        #endregion

        #region Public event
        public event EventHandler<ListSearchArgs> SearchList;
        #endregion

        #region Private methods
        private void StartSearch()
        {
            // stops the PageChanged event being called when the page number is set 
            // elsewhere in the code to 1

            if (SearchList != null)
            {
                SearchList(this, new ListSearchArgs(SearchTextBox.Text, true));
            }
        }

        private void StopSearch()
        {
            if (SearchList != null)
            {
                SearchList(this, new ListSearchArgs(SearchTextBox.Text, false));
            }
        }

        private void TryToSearch()
        {
            // If search text is empty the button goes to checked state. We:
            // (1) wont't send search request
            // (2) won't change the image
            // (3) Change the state to unchecked state
            if (string.IsNullOrEmpty(SearchTextBox.Text) || SearchTextBox.Text == savedSearchTextBoxText)
            {
                fireExternalEvent = false;
                SearchToggleButton.IsChecked = false;
            }
            else
            {
                Image image = new Image();
                Uri uri = new Uri(StopImageUri, UriKind.Relative);
                ImageSource img = new System.Windows.Media.Imaging.BitmapImage(uri);
                SearchToggleButtonImage.SetValue(Image.SourceProperty, img);

                StartSearch();
            }
        }
        #endregion

        #region Event handlers

        /// <summary>
        /// Uncheck -> checked state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            TryToSearch();
        }

        /// <summary>
        /// checked -> unchecked state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Image image = new Image();
            Uri uri = new Uri(SearchImageUri, UriKind.Relative); // Gif is not working
            ImageSource img = new System.Windows.Media.Imaging.BitmapImage(uri);
            SearchToggleButtonImage.SetValue(Image.SourceProperty, img);

            this.PromptText = savedSearchTextBoxText;

            if (fireExternalEvent)
            {
                StopSearch();
            }
            else
            {
                fireExternalEvent = true;
            }
        }

        /// <summary>
        /// Lost focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // If it is in non-search mode, restore save search text
            if (this.SearchToggleButton.IsChecked == false)
            {
                this.PromptText = savedSearchTextBoxText;
            }
        }

        /// <summary>
        /// Handler when the text box get focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // If this is in non-search mode, clear the box
            if (this.SearchToggleButton.IsChecked == false)
            {
                SearchTextBox.Text = string.Empty;
            }
        }

        /// <summary>
        /// Key down event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SearchToggleButton.IsChecked == true)
                {
                    // If the toggle button is in Checked mode, set IsChecked == true wont fire the event.
                    // So we have to try the search manually
                    this.TryToSearch();
                }
                else
                {
                    SearchToggleButton.IsChecked = true; // Set to search mode
                }
            }
        }

        /// <summary>
        /// The control is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (null == savedSearchTextBoxText)
            {
                savedSearchTextBoxText = SearchTextBox.Text;
            }
        }
        #endregion
    }
}
