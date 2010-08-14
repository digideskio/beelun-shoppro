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
    public partial class OrderDetailsPage : Page
    {
        #region Private properties
        /// <summary>
        /// Holding view related object
        /// </summary>
        private ContentPageContext ContentPageCtx = null;
        private DataFormMode editMode;
        private int ItemsCountInCurrentPage = 0;
        #endregion

        public OrderDetailsPage()
        {
            InitializeComponent();
        }

        #region Private methods
        /// <summary>
        /// Thread body for changing order status
        /// </summary>
        /// <param name="state"></param>
        private void StartWork_ChangeOrderStatus(object state)
        {
            OrderStatusChanging change = state as OrderStatusChanging;

            OrderView orderView = change.orderView;
            order savedOrder = null;
            AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
            EventHandler<changeOrderStatus_OrderCompletedEventArgs> h1 = (s, e) =>
            {
                if (e.Error == null)
                {
                    savedOrder = e.Result;
                    nextOneAutoResetEvent.Set();
                }
            };
            Globals.WSClient.changeOrderStatus_OrderCompleted += h1;
            Globals.WSClient.changeOrderStatus_OrderAsync(change.OrderId, change.NewOrderStatus);
            nextOneAutoResetEvent.WaitOne();
            Globals.WSClient.changeOrderStatus_OrderCompleted -= h1;

            if (null != savedOrder)
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    orderView.Restore(new OrderView(savedOrder));
                });
                this.ContentPageCtx.ListChanged = true;
            }
            else
            {
                ShopproHelper.ShowMessageWindow(this, "Error", "Fail to change status of this order", false);
            }

            ShopproHelper.HideBusyIndicator(this);
        }

        /// <summary>
        /// Thread body
        /// </summary>
        /// <param name="state"></param>
        private void StartWork(object state)
        {
            OrderView orderView = state as OrderView;
            order savedOrder = null;
            order theOrder = new order();
            orderView.Merge(theOrder, editMode);
            AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
            EventHandler<save_OrderCompletedEventArgs> h1 = (s, e) =>
            {
                // TODO: handle error in server side
                savedOrder = e.Result;
                nextOneAutoResetEvent.Set();
            };
            Globals.WSClient.save_OrderCompleted += h1;
            Globals.WSClient.save_OrderAsync(theOrder);
            nextOneAutoResetEvent.WaitOne();
            Globals.WSClient.save_OrderCompleted -= h1;
            orderView.Restore(new OrderView(savedOrder));

            ShopproHelper.HideBusyIndicator(this);

            // No unsaved item
            this.editMode = DataFormMode.ReadOnly;

            // If only one new page, return the original one
            if (this.ContentPageCtx.GotoAddNewPage)
            {
                ShopproHelper.GoToContentPage(this, PageEnum.OrderListPage);
            }
        }

        #endregion

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
                // 'Save' button is hit
                ShopproHelper.ShowBusyIndicator(this);

                this.ContentPageCtx.ListChanged = true;

                // Start data access in another thread to avoid blocking UI thread
                ThreadPool.QueueUserWorkItem(StartWork, this.itemDataForm.CurrentItem);
            }
            else
            {
                // If only one new page, return to the original one
                if (this.ContentPageCtx.GotoAddNewPage)
                {
                    ShopproHelper.GoToContentPage(this, PageEnum.OrderListPage);
                }            
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

        /// <summary>
        /// 'Add' button is hit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemDataForm_AddingNewItem(object sender, DataFormAddingNewItemEventArgs e)
        {
            editMode = DataFormMode.AddNew;
        }

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

        #endregion

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.editMode = DataFormMode.ReadOnly;
            this.ContentPageCtx = ModelProvider.Instance.GetContentPageContext(e.Uri.ToString());
            this.DataContext = ViewModelHelper.Convert(this.ContentPageCtx.DataSource as ObservableCollection<order>);
            this.ItemsCountInCurrentPage = (this.ContentPageCtx.DataSource as ObservableCollection<order>).Count;
            this.itemDataForm.CurrentIndex = this.ContentPageCtx.CurrentIndex;
            this.OrderStatusComboBox.SelectedIndex = -1;    // Always show nothing in the list
            ShopproHelper.SetWorkSpaceBodyContentPageTitle(this, string.Format("Record {0} of {1} in current page", this.itemDataForm.CurrentIndex + 1, this.ItemsCountInCurrentPage));
        }

        /// <summary>
        /// Order status combobox selection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderStatus_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get new order status
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedIndex == -1)
            {
                // Select nothing, do nothing
                return;
            }

            ComboBoxItem statusItem = comboBox.SelectedItem as ComboBoxItem;
            orderStatusEnum newOrderStatus = (orderStatusEnum)Enum.Parse(typeof(orderStatusEnum), statusItem.Tag.ToString(), true);

            // Get current order Id
            OrderView orderView = this.itemDataForm.CurrentItem as OrderView;
            long orderId = orderView.id;


            // Busy...
            ShopproHelper.ShowBusyIndicator(this);

            // Call to backend thread
            ThreadPool.QueueUserWorkItem(StartWork_ChangeOrderStatus, new OrderStatusChanging(orderId, newOrderStatus, orderView));
        }
    }

    /// <summary>
    /// A private class for passing changing status parameters
    /// </summary>
    class OrderStatusChanging
    {
        public long OrderId { get; set; }
        public orderStatusEnum NewOrderStatus { get; set; }
        public OrderView orderView { get; set; }
        public OrderStatusChanging(long orderId, orderStatusEnum status, OrderView orderView)
        {
            this.OrderId = orderId;
            this.NewOrderStatus = status;
            this.orderView = orderView;
        }
    }
}
