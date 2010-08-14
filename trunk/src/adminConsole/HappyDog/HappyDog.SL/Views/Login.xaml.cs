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
using System.Windows.Navigation;

using HappyDog.SL.EventArguments;
using HappyDog.SL.Data;

namespace HappyDog.SL.Views
{
    public partial class Login : Page
    {
        #region Constructors
        public Login()
        {
            InitializeComponent();
        }
        #endregion

        #region Control Event Handlers
        private void LoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Automatically insert the user name last entered as the login, retrieved
            // from isolated storage
            UserNameTextbox.Text = ConfigurationSettings.LastUserName;

            if (string.IsNullOrEmpty(UserNameTextbox.Text))
            {
                UserNameTextbox.Focus();
            }
            else
            {
                PasswordTextbox.Focus();
            }
        }

        private void LoginPage_KeyDown(object sender, KeyEventArgs e)
        {         
            if (e.Key == Key.Enter)
                StartLogin();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            StartLogin();
        }
        #endregion

        #region Private Functions
        private void StartLogin()
        {
            // Disable the button 
            this.LoginButton.IsEnabled = false;

            // Init ModelProvider
            ModelProvider mp = ModelProvider.Instance;
            mp.Clean();
            mp.UserId = UserNameTextbox.Text;
            mp.Password = PasswordTextbox.Password;
            // TODO: remove below in released version
            //mp.UserId = @"admin@smc.com";
            //mp.Password = @"beelun";
            this.NavigationService.Navigate(new Uri("/LoadingProgress", UriKind.Relative));
        }
        #endregion

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

    }
}
