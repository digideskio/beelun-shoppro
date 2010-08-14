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

using HappyDog.SL.Data;
using HappyDog.SL;
using HappyDog.SL.EventArguments;
using HappyDog.SL.Content;

namespace HappyDog.SL.Controls
{
    /// <summary>
    /// Header control for work space
    /// </summary>
    public partial class Header : UserControl
    {
        #region Dependency prop
        /// <summary>
        /// SelectedButtonIndex dependency prop
        /// </summary>
        public static DependencyProperty SelectedButtonIndexProperty = DependencyProperty.Register("SelectedButtonIndex", typeof(string), typeof(Header), new PropertyMetadata(new PropertyChangedCallback(OnSelectedButtonIndexPropertyValueChanged)));
        public string SelectedButtonIndex
        {
            get { return (string)this.GetValue(SelectedButtonIndexProperty); }

            set
            {
                base.SetValue(SelectedButtonIndexProperty, (string)value);
                SelectedButtonIndexString = (string)value;
            }
        }
        public static void OnSelectedButtonIndexPropertyValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Do nothing

            //Header header = d as Header;
            //SelectedButtonIndexString = header.SelectedButton;
            //// searchBox.SearchTextBox.Text = searchBox.PromptText;
        }
        #endregion

        #region Public event handers
        public event EventHandler<NavigateRequestArgs> LogoutRequest;
        public event EventHandler<NavigateRequestArgs> WorkSpaceBodyChangeRequest;
        #endregion

        #region Private prop
        private ToolbarButton activeButton = null;
        private string SelectedButtonIndexString = null;
        #endregion

        #region Constructor
        public Header()
        {
            InitializeComponent();
            SetLogoImage(); // TODO: fix this
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Show logo image from the server directory
        /// </summary>
        public void SetLogoImage()
        {
            Image image = new Image();
            // Uri uri = new Uri(Globals.WebRootUrl + Globals.RelativeLogoImgUri);
            Uri uri = new Uri("../Img/CompanyLogo.gif", UriKind.Relative); // Gif is not working
            ImageSource img = new System.Windows.Media.Imaging.BitmapImage(uri);
            image.SetValue(Image.SourceProperty, img);
            LogoShowArea.Children.Add(image);
        }

        /// <summary>
        /// userInfo in the format of:
        /// UserDisplayName(EmployeeID)
        /// </summary>
        /// <param name="userInfo"></param>
        public void SetUserArea(string userInfo)
        {
            txtBlkUserName.SetValue(TextBlock.TextProperty, userInfo);
        }

        /// <summary>
        /// Note down current active button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns>true if, switch to new page, false otherwise</returns>
        private bool HandleActiveButton(object sender, MouseButtonEventArgs e)
        {
            // Reclick the same active button, do nothing
            if (sender as ToolbarButton == activeButton)
            {
                return false;
            }

            // Gray out previous active button
            if (activeButton != null)
            {
                activeButton.Unselect();
            }

            // Set new active button
            activeButton = sender as ToolbarButton;
            activeButton.SelectButton();

            // Return
            return true;
        }
        #endregion

        #region Log out button
        private void LogoutButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (activeButton != null)
            {
                activeButton.Unselect();
            }
            ToolbarButton logoutBtn = sender as ToolbarButton;
            logoutBtn.Unselect();
            
            // Try log out
            // TODO: logout
            ModelProvider.Instance.LogoutAsync(Logout_Handle);
        }

        private void Logout_Handle(HDCallBackEventArgs e)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                if (this.LogoutRequest != null)
                {
                    Globals.IsLoggedIn = false;
                    this.LogoutRequest(this, new NavigateRequestArgs(PageEnum.LoginPage, null));                    
                }
            });
        }
        #endregion

        #region Normal functionality button
        /// <summary>
        /// Button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolbarButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (HandleActiveButton(sender, e))
            {
                if (this.WorkSpaceBodyChangeRequest != null)
                {
                    this.WorkSpaceBodyChangeRequest(sender, new NavigateRequestArgs(((FrameworkElement)sender).Tag));
                }
            }
        }

        /// <summary>
        /// In loaded event hanlder, we will switch work space body
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            int buttonIndex = int.Parse(this.SelectedButtonIndex);
            ToolbarButton b = (ToolbarButton)this.ToolBar.ButtonGroup[buttonIndex];
            if (b != null)
            {
                ToolbarButton_MouseLeftButtonDown(b, null);
            }
        }

        #endregion


    }
}
