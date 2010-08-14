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
using HappyDog.SL.Content;

namespace HappyDog.SL.Views
{
    public partial class LoadingProgress : Page
    {
        #region constructor
        public LoadingProgress()
        {
            InitializeComponent();
        }
        #endregion

        #region update progress
        public void UpdateProgress(int latestPercentage, string msg)
        {
            if (latestPercentage <= MyProgress.Maximum)
            {
                MyProgress.Value = latestPercentage;
            }
            tbWhatIsNew.Text = msg;
        }
        #endregion

        /// <summary>
        /// Progress update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mp_onProgress(object sender, HDCallBackEventArgs e)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                if (e != null)
                {
                    TaskProgress tp = e.Result as TaskProgress;
                    this.UpdateProgress(tp.percentage, tp.TaskDescription);
                }
            });
        }

        /// <summary>
        /// Get the result now
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mp_AsyncHanlder(object sender, HDCallBackEventArgs e)
        {
            this.Dispatcher.BeginInvoke(delegate()
            {
                if (e.Error == null)
                {
                    Globals.IsLoggedIn = true;
                    ConfigurationSettings.LastUserName = ModelProvider.Instance.UserId;
                    this.NavigationService.Navigate(new Uri("/WorkSpace", UriKind.Relative));
                }
                else
                {
                    // For any error during init, we call logout
                    if (Globals.IsLoggedIn)
                    {
                        //ModelProvider.Instance.Logout();
                    }
                    Globals.IsLoggedIn = false;
                    this.UpdateProgress(101, e.Error.Message); // Input an invalid percentage on purpose
                    btnOK.Visibility = Visibility.Visible;
                    btnOK.Focus();
                }
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Switch back to login page
            this.NavigationService.Navigate(new Uri("/Login", UriKind.Relative));
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.NavigationService.Navigate(new Uri("/Login", UriKind.Relative));
            }
        }
        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ModelProvider mp = ModelProvider.Instance;
            mp.AsyncHanlder += new EventHandler<HDCallBackEventArgs>(mp_AsyncHanlder);
            mp.onProgress += new EventHandler<HDCallBackEventArgs>(mp_onProgress);
            mp.InitSchemaAsync();
        }

    }
}
