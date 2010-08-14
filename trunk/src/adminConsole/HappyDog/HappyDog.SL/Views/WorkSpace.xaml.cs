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
using System.Diagnostics;
using System.Windows.Threading;

using HappyDog.SL.EventArguments;
using HappyDog.SL.Data;
using System.Windows.Navigation;
using HappyDog.SL.Content;

namespace HappyDog.SL.Views
{
    public partial class WorkSpace : Page
    {
        #region constructor
        public WorkSpace()
        {
            InitializeComponent();
            this.HeaderArea.LogoutRequest += new EventHandler<NavigateRequestArgs>(HeaderArea_LogoutRequest);
            this.HeaderArea.WorkSpaceBodyChangeRequest += new EventHandler<NavigateRequestArgs>(HeaderArea_WorkSpaceBodyChangeRequest);
        }
        #endregion

        /// <summary>
        /// Request to switch work space body
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HeaderArea_WorkSpaceBodyChangeRequest(object sender, NavigateRequestArgs e)
        {
            this.ContentFrame.Navigate(new Uri(e.Parameters as string, UriKind.Relative));
        }

        /// <summary>
        /// Request to log out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void HeaderArea_LogoutRequest(object sender, NavigateRequestArgs e)
        {
            // TODO: save changed work
            this.NavigationService.Navigate(new Uri("/Login", UriKind.Relative));
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {

        }

    }
}
