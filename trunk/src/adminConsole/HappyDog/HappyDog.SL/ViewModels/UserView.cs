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

namespace HappyDog.SL.ViewModels
{
    /// <summary>
    /// A wrapper class for model object: HappyDog.SL.Beelun.Shoppro.WebService.UserManager.user
    /// </summary>
    public class UserView : INotifyPropertyChanged, IEditableObject
    {
        #region Properties
        [ReadOnly(true)]
        [Display(Name="ID")]
        public long id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Name", Description = "Name of this user")]
        public string name { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required]
        [Display(Name = "Enabled?", Description = "Enable this user?")]
        public bool enabled { get; set; }

        [Required]
        [Display(Name = "Shipping Address")]
        public address shippingAddress { get; set; }

        [Required]
        [Display(Name = "Billing Address")]
        public address billingAddress { get; set; }

        [Required]
        [Display(Name = "Same Address?", Description = "Same address?")]
        public bool sameAddress { get; set; }

        [ReadOnly(true)]
        [Display(Name = "CreatedWhen", Description = "When this user was created")]
        public System.DateTime createdWhen { get; set; }

        [ReadOnly(true)]
        [Display(Name = "LastLogin", Description = "When this user login last time")]
        public System.DateTime lastLogin { get; set; }

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
        public UserView() { }

        /// <summary>
        /// Clone
        /// </summary>
        /// <param name="itemView"></param>
        public UserView(UserView userView)
        {
            this.id = userView.id;
            this.name = userView.name;
            this.email = userView.email;
            this.enabled = userView.enabled;
            this.shippingAddress = userView.shippingAddress;
            this.billingAddress = userView.billingAddress;
            this.sameAddress = userView.sameAddress;
            this.createdWhen = userView.createdWhen;
            this.lastLogin = userView.lastLogin;

        }


        /// <summary>
        /// Model -> View
        /// </summary>
        /// <param name="original"></param>
        public UserView(user original)
        {
            this.id = original.id;
            this.name = original.name;
            this.email = original.email;
            this.enabled = original.enabled;
            this.shippingAddress = original.shippingAddress;
            this.billingAddress = original.billingAddress;
            this.sameAddress = original.sameAddress;
            this.createdWhen = original.createdWhen;
            this.lastLogin = original.lastLogin;

        }
        #endregion

        #region Public methods
        /// <summary>
        /// View -> Model
        /// </summary>
        /// <param name="original"></param>
        public void Merge(user original, DataFormMode editMode)
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
            original.name = this.name;
            original.email = this.email;
            original.enabled = this.enabled;
            original.shippingAddress = this.shippingAddress;
            original.billingAddress = this.billingAddress;
            original.sameAddress = this.sameAddress;
            original.createdWhen = this.createdWhen;
            original.lastLogin = original.lastLogin;
            
        }

        /// <summary>
        /// ItemView merge
        /// </summary>
        /// <param name="itemView"></param>
        public void Restore(UserView original)
        {
            this.id = original.id;
            this.name = original.name;
            this.email = original.email;
            this.enabled = original.enabled;
            this.shippingAddress = original.shippingAddress;
            this.billingAddress = original.billingAddress;
            this.sameAddress = original.sameAddress;
            this.createdWhen = original.createdWhen;
            this.lastLogin = original.lastLogin;
 
        }
        #endregion

        #region IEditableObject methods
        private UserView cache = null;

        /// <summary>
        /// 'Edit' button is clicked
        /// Due to binding, all UI changed will be mapped to the object, so we need make a copy of object before edit in case user wants to cancel the editing
        /// </summary>
        public void BeginEdit()
        {
            cache = new UserView(this);
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
