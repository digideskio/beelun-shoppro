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
using System.Threading;
using System.Collections.ObjectModel;

using HappyDog.SL.EventArguments;
using HappyDog.SL.Data;
using HappyDog.SL.Resources;
using HappyDog.SL.UIEffect;
using HappyDog.SL.Controls;
using HappyDog.SL.Content;
using HappyDog.SL.Common;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using HappyDog.SL.ViewModels;
using System.Windows.Navigation;

namespace HappyDog.SL.Views
{
    public partial class UserDetailsPage : Page
    {
        #region Private properties
        /// <summary>
        /// Holding view related object
        /// </summary>
        private ContentPageContext ContentPageCtx = null;
        private int ItemsCountInCurrentPage = 0;
        #endregion

        #region Constructor
        public UserDetailsPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Thread body for enable/disable user
        /// </summary>
        /// <param name="state"></param>
        private void StartWork_EnableDisableUser(object state)
        {
            EnableUserWrapper enableUserWrapper = state as EnableUserWrapper;

            AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);

            bool resetSuccess = false;
            EventHandler<enableDisableUserAccess_UserCompletedEventArgs> h1 = (s, e) =>
            {
                if (e.Error == null)
                {
                    resetSuccess = e.Result;
                    nextOneAutoResetEvent.Set();
                }
            };
            Globals.WSClient.enableDisableUserAccess_UserCompleted += h1;
            Globals.WSClient.enableDisableUserAccess_UserAsync(enableUserWrapper.UserId, enableUserWrapper.Enabled);
            nextOneAutoResetEvent.WaitOne();
            Globals.WSClient.enableDisableUserAccess_UserCompleted -= h1;

            if (true == resetSuccess)
            {
                ShopproHelper.ShowMessageWindow(this, "Message", "Operation completed successfully.", false);
            }
            else
            {
                ShopproHelper.ShowMessageWindow(this, "Error", "Operation failed. Please retry.", false);
            }
        }

        /// <summary>
        /// Thread body
        /// </summary>
        /// <param name="state"></param>
        private void StartWork_ChangePassword(object state)
        {
            ResetPasswordWrapper passwordWrapper = state as ResetPasswordWrapper;

            AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);

            bool resetSuccess = false;
            EventHandler<resetPassword_UserCompletedEventArgs> h1 = (s, e) =>
            {
                if (e.Error == null)
                {
                    resetSuccess = e.Result;
                    nextOneAutoResetEvent.Set();
                }
            };
            Globals.WSClient.resetPassword_UserCompleted += h1;
            Globals.WSClient.resetPassword_UserAsync(passwordWrapper.UserId, passwordWrapper.NewPassword);
            nextOneAutoResetEvent.WaitOne();
            Globals.WSClient.resetPassword_UserCompleted -= h1;

            if (true == resetSuccess)
            {
                ShopproHelper.ShowMessageWindow(this, "Message", "The password is reset successfully.", false);
            }
            else
            {
                ShopproHelper.ShowMessageWindow(this, "Error", "Fail to reset password. Please retry.", false);
            }
        }

        #endregion

        #region Event handlers
        /// <summary>
        /// 'Back'button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ShopproHelper.GoBack(this);
        }

        /// <summary>
        /// 'Previous/Next' button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemDataForm_CurrentItemChanged(object sender, EventArgs e)
        {
            ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, string.Format("Record {0} of {1} in current page", this.itemDataForm.CurrentIndex + 1, this.ItemsCountInCurrentPage));
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.LockUnlockComboBox.SelectedIndex = -1; // Always select none
            this.ContentPageCtx = this.ContentPageCtx = ModelProvider.Instance.GetContentPageContext(e.Uri.ToString());
            this.DataContext = ViewModelHelper.Convert(this.ContentPageCtx.DataSource as ObservableCollection<user>);
            this.ItemsCountInCurrentPage = (this.ContentPageCtx.DataSource as ObservableCollection<user>).Count;
            this.itemDataForm.CurrentIndex = this.ContentPageCtx.CurrentIndex;
            ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, string.Format("Record {0} of {1} in current page", this.itemDataForm.CurrentIndex + 1, this.ItemsCountInCurrentPage));
        }
        #endregion

        /// <summary>
        /// 'Reset password' button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ResetPasswordWindow w = new ResetPasswordWindow();
            w.Closed += new EventHandler(w_Closed);
            w.Show();
        }

        /// <summary>
        /// 'Reset password' window is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void w_Closed(object sender, EventArgs e)
        {
            ResetPasswordWindow w = sender as ResetPasswordWindow;
            if (w.DialogResult == true)
            {
                string newPassword = w.NewPassword;
                // Call to backend thread
                ThreadPool.QueueUserWorkItem(StartWork_ChangePassword, new ResetPasswordWrapper((this.itemDataForm.CurrentItem as UserView).id, newPassword));
            }
        }

        /// <summary>
        /// Lock/unlock combox box selection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LockUnlock_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = sender as ComboBox;
            if (box.SelectedIndex == -1)
            {
                return;
            }

            // Get 'enabled'
            bool enabled = true;
            bool.TryParse((((ComboBoxItem)box.SelectedItem).Tag as string), out enabled);

            // Get userId
            long userId = (this.itemDataForm.CurrentItem as UserView).id;

            ThreadPool.QueueUserWorkItem(StartWork_EnableDisableUser, new EnableUserWrapper(userId, enabled));            
        }
    }

    class ResetPasswordWrapper
    {
        public long UserId { get; set; }
        public string NewPassword {get;set;}

        public ResetPasswordWrapper(long userId, string newPassword)
        {
            this.UserId = userId;
            this.NewPassword = newPassword;
        }
    }

    class EnableUserWrapper
    {
        public long UserId { get; set; }
        public bool Enabled { get; set; }

        public EnableUserWrapper(long userId, bool enabled)
        {
            this.UserId = userId;
            this.Enabled = enabled;
        }
    }

}
