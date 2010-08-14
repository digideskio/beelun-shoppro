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
using HappyDog.SL.Common;
using HappyDog.SL.Content;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using HappyDog.SL.ViewModels;
using System.Windows.Navigation;

namespace HappyDog.SL.Views
{
    public partial class GlobalPage : Page
    {
        #region Private properties
        /// <summary>
        /// Holding view related object
        /// </summary>
        private ContentPageContext ContentPageCtx = null;
        private DataFormMode editMode;
        #endregion

        #region Private methods
        /// <summary>
        /// Update the UI
        /// </summary>
        private void BeginUpdateEntireUI()
        {
            ShopproHelper.ShowBusyIndicator(this);

            // Get page number
            this.ContentPageCtx.CurrentPageNumber = 1;

            Globals.WSClient.fetch_MyGlobCompleted += new EventHandler<fetch_MyGlobCompletedEventArgs>(WSClient_fetch_MyGlobCompleted);
            Globals.WSClient.fetch_MyGlobAsync();
        }

        void WSClient_fetch_MyGlobCompleted(object sender, fetch_MyGlobCompletedEventArgs e)
        {
            Globals.WSClient.fetch_MyGlobCompleted -= new EventHandler<fetch_MyGlobCompletedEventArgs>(WSClient_fetch_MyGlobCompleted);

            ShopproHelper.HideBusyIndicator(this);
            if (e.Error == null)
            {
                ObservableCollection<myGlob> globalCollection = new ObservableCollection<myGlob>();
                globalCollection.Add(e.Result as myGlob);
                this.Dispatcher.BeginInvoke(delegate()
                {
                    this.itemDataForm.ItemsSource = ViewModelHelper.Convert(globalCollection as ObservableCollection<myGlob>);
                    this.itemDataForm.CurrentIndex = 0;
                });

            }
            else
            {
                ShopproHelper.ShowMessageWindow(this, "Error", e.Error.Message, false);
            }
        }

        /// <summary>
        /// Thread body
        /// </summary>
        /// <param name="state"></param>
        private void StartWork(object state)
        {
            GlobalView globalView = state as GlobalView;
            myGlob savedGlobal = null;
            myGlob theGlobal = new myGlob();
            globalView.Merge(theGlobal, editMode);

            // Save to backend
            AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
            EventHandler<save_MyGlobCompletedEventArgs> h1 = (s, e) =>
            {
                // TODO: handle error from server side
                savedGlobal = e.Result;
                nextOneAutoResetEvent.Set();
            };
            Globals.WSClient.save_MyGlobCompleted += h1;
            Globals.WSClient.save_MyGlobAsync(theGlobal);
            nextOneAutoResetEvent.WaitOne();
            Globals.WSClient.save_MyGlobCompleted -= h1;

            // Check return result. If failure, savedCategory will be null
            if (savedGlobal != null)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    globalView.Restore(new GlobalView(savedGlobal));
                });
            }
            else
            {
                // Show error message
                ShopproHelper.ShowMessageWindow(this, "Error", "Fail to save this entry.", false);

                // Back to readonly mode
                this.Dispatcher.BeginInvoke(delegate()
                {
                    // Restore cached UI data
                    globalView.CancelEdit();
                    this.itemDataForm.CancelEdit();
                });
            }

            ShopproHelper.HideBusyIndicator(this);

            // No unsaved item
            this.editMode = DataFormMode.ReadOnly;
        }

        #endregion
        public GlobalPage()
        {
            InitializeComponent();
        }

        #region Event handlers

        /// <summary>
        /// 'save' or 'cancel' button is hit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemDataForm_EditEnded(object sender, DataFormEditEndedEventArgs e)
        {
            if (e.EditAction == DataFormEditAction.Commit)
            {
                ShopproHelper.ShowBusyIndicator(this);

                // Start data access in another thread to avoid blocking UI thread
                ThreadPool.QueueUserWorkItem(StartWork, this.itemDataForm.CurrentItem);
            }
        }

        /// <summary>
        /// "Edit" button is hit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemDataForm_BeginningEdit(object sender, System.ComponentModel.CancelEventArgs e)
        {
            editMode = DataFormMode.Edit;
        }

        #endregion
        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.editMode = DataFormMode.ReadOnly;
            this.ContentPageCtx = ModelProvider.Instance.GetContentPageContext(e.Uri.ToString());
            this.BeginUpdateEntireUI();
        }
    }
}
