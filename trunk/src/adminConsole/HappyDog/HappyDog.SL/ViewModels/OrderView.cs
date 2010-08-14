using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using HappyDog.SL.Content;
using System.Collections.Generic;

namespace HappyDog.SL.ViewModels
{
    /// <summary>
    /// A wrapper class for model object: order
    /// </summary>
    public class OrderView : INotifyPropertyChanged, IEditableObject
    {
        #region Private prop
        private orderStatusEnum _status = orderStatusEnum.NOTPAID;
        #endregion

        #region Public Properties
        [ReadOnly(true)]
        [Display(Name="ID")]
        public long id { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Order Date", Description = "When this order was created")]
        public System.DateTime orderDate { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Order No.", Description = "Order NO. used for identifying the single order in the system")]
        public string serialNumber { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Amount", Description = "Total value of this order")]
        public decimal amount { get; set; }

        [Required]
        [Display(Name = "Status", Description = "Status of this order")]
        public orderStatusEnum status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged("status");
            }
        }

        [ReadOnly(true)]
        [Display(Name = "User", Description = "When this order was created")]
        public user user { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Shipping Address", Description = "Shipping Address")]
        public address shippingAddress { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Billing Address", Description = "Billing Address")]
        public address billingAddress { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Same Address?", Description = "Same Address")]
        public bool sameAddress { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Order Item Set", Description = "Order Item Set")]
        public orderItem[] orderItemSet { get; set; }

        #endregion

        #region For INotifyPropertyChanged
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises a property changed notification for the specified property name.
        /// </summary>
        /// <param name="propName">The name of the property that changed.</param>
        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor is a must. Otherwise 'add' button will be greyed out.
        /// </summary>
        public OrderView() { }

        /// <summary>
        /// Clone
        /// </summary>
        /// <param name="itemView"></param>
        public OrderView(OrderView orderView)
        {
            this.id = orderView.id;
            this.orderDate = orderView.orderDate;
            this.status = orderView.status;
            this.user = orderView.user;
            this.shippingAddress = orderView.shippingAddress;
            this.billingAddress = orderView.billingAddress;
            this.sameAddress = orderView.sameAddress;
            this.orderItemSet = orderView.orderItemSet;
            this.serialNumber = orderView.serialNumber;
            this.amount = orderView.amount;
        }


        /// <summary>
        /// Model -> View
        /// </summary>
        /// <param name="original"></param>
        public OrderView(order original)
        {
            this.id = original.id;
            this.orderDate = original.orderDate;
            this.status = original.status;
            this.user = original.user;
            this.shippingAddress = original.shippingAddress;
            this.billingAddress = original.billingAddress;
            this.sameAddress = original.sameAddress;
            this.orderItemSet = original.orderItemSet;
            this.amount = original.amount;
            this.serialNumber = original.serialNumber;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// View -> Model
        /// TODO: we should either:
        /// (1) remove this method since we should not allow update the order from admin
        /// (2) fix this method by applying required xxxSpecified to 'number' type(int,decimal, etc).
        /// </summary>
        /// <param name="original"></param>
        public void Merge(order original, DataFormMode editMode)
        {
            if (editMode == DataFormMode.Edit)
            {
                original.id = this.id;
                original.idSpecified = true;
            }
            else
            {
                original.idSpecified = false;
            }
            original.orderDate = this.orderDate;
            original.status = this.status;
            original.user = this.user;
            original.shippingAddress = this.shippingAddress;
            original.billingAddress = this.billingAddress;
            original.sameAddress = this.sameAddress;
            original.orderItemSet = this.orderItemSet;
            original.amount = this.amount;
            original.serialNumber = this.serialNumber;

        }

        /// <summary>
        /// OrderView restore
        /// </summary>
        /// <param name="itemView"></param>
        public void Restore(OrderView original)
        {
            this.id = original.id;
            this.orderDate = original.orderDate;
            this.status = original.status;
            this.user = original.user;
            this.shippingAddress = original.shippingAddress;
            this.billingAddress = original.billingAddress;
            this.sameAddress = original.sameAddress;
            this.orderItemSet = original.orderItemSet;
            this.serialNumber = original.serialNumber;
            this.amount = original.amount;
        }
        #endregion

        #region IEditableObject methods
        private OrderView cache = null;

        /// <summary>
        /// 'Edit' button is clicked
        /// Due to binding, all UI changed will be mapped to the object, so we need make a copy of object before edit in case user wants to cancel the editing
        /// </summary>
        public void BeginEdit()
        {
            cache = new OrderView(this);
        }

        /// <summary>
        /// 'Cancel' button is clicked
        /// </summary>
        public void CancelEdit()
        {
            Restore(cache);
            cache = null;
        }

        /// <summary>
        /// 'Save' button is clicked
        /// </summary>
        public void EndEdit()
        {
            // Save to the backend
            // cache = null;
        }

        #endregion

    }
}
