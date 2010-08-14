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
    /// A wrapper class for model object: HappyDog.SL.Beelun.Shoppro.WebService.ItemManager.item
    /// </summary>
    public class GlobalView : INotifyPropertyChanged, IEditableObject
    {
        #region Properties
        [ReadOnly(true)]
        [Display(Name="ID")]
        public long id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Shop Name")]
        public string shopName { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Slogan")]
        public string slogan { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Phone Number")]
        public string phoneNumber { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "logo's Absolute Url")]
        public string logoAbsoluteUrl { get; set; }

        [Required]
        [StringLength(4000)]
        [Display(Name = "Address", Description = "Address")]
        public string address { get; set; }

        [Required]
        [Display(Name = "Error 404 Page", Description = "No such Page found")]
        public string page404 { get; set; }

        [Required]
        [Display(Name = "Error 500 Page", Description = "Internal Server Error")]
        public string page500 { get; set; }

        [Required]
        [Display(Name = "NoSearchResult Page", Description = "Page used for indicating that there is not search result.")]
        public string pageNoSearchResult { get; set; }

        [Required]
        [Display(Name = "Footer", Description = "Footer")]
        public string footer { get; set; }

        [Required]
        [StringLength(2000)]
        [Display(Name = "Google CustSearch Code", Description = "GoogleCustSearchCode")]
        public string googleCustSearchCode { get; set; }

        [Required]
        [Display(Name = "Signup Agreement", Description = "SignupAgreement")]
        public string signupAgreement { get; set; }

        [ReadOnly(true)]
        [StringLength(255)]
        [Display(Name = "Version", Description = "Version")]
        public string version { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Site Type", Description = "en-US or zhs-CN")]
        public siteTypeEnum siteType { get; set; }

        [Required]
        [StringLength(2000)]
        [Display(Name = "Unlock Email Template", Description = "Email Template used when user wants to unlock its newly created account")]
        public string unlockEmailTemplate { get; set; }

        [Required]
        [StringLength(2000)]
        [Display(Name = "Reset Password Mail Template", Description = "Mail Template sent out when user wants to reset ones password")]
        public string resetPasswordMailTemplate { get; set; }
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
        public GlobalView() { }

        /// <summary>
        /// Clone
        /// </summary>
        /// <param name="articleView"></param>
        public GlobalView(GlobalView globalView)
        {
            this.id = globalView.id;
            this.shopName = globalView.shopName;
            this.slogan = globalView.slogan;
            this.phoneNumber = globalView.phoneNumber;
            this.logoAbsoluteUrl = globalView.logoAbsoluteUrl;
            this.address = globalView.address;
            this.page404 = globalView.page404;
            this.page500 = globalView.page500; ;
            this.pageNoSearchResult = globalView.pageNoSearchResult;
            this.footer = globalView.footer;
            this.googleCustSearchCode = globalView.googleCustSearchCode;
            this.signupAgreement = globalView.signupAgreement;
            this.version = globalView.version;
            this.siteType = globalView.siteType;
            this.unlockEmailTemplate = globalView.unlockEmailTemplate;
            this.resetPasswordMailTemplate = globalView.resetPasswordMailTemplate;
            
        }


        /// <summary>
        /// Model -> View
        /// </summary>
        /// <param name="original"></param>
        public GlobalView(myGlob original)
        {
            this.id = original.id;
            this.shopName = original.shopName;
            this.slogan = original.slogan;
            this.phoneNumber = original.phoneNumber;
            this.logoAbsoluteUrl = original.logoAbsoluteUrl;
            this.address = original.address;
            this.page404 = original.page404;
            this.page500 = original.page500;
            this.pageNoSearchResult = original.pageNoSearchResult;
            this.footer = original.footer;
            this.googleCustSearchCode = original.googleCustSearchCode;
            this.signupAgreement = original.signupAgreement;
            this.version = original.version;
            this.siteType = original.siteType;
            this.unlockEmailTemplate = original.unlockEmailTemplate;
            this.resetPasswordMailTemplate = original.resetPasswordMailTemplate;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// View -> Model
        /// </summary>
        /// <param name="original"></param>
        public void Merge(myGlob original, DataFormMode editMode)
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

            original.shopName = this.shopName;
            original.slogan = this.slogan;
            original.phoneNumber = this.phoneNumber;
            original.logoAbsoluteUrl = this.logoAbsoluteUrl;
            original.address = this.address;
            original.page404 = this.page404;
            original.page500 = this.page500;
            original.pageNoSearchResult = this.pageNoSearchResult;
            original.footer = this.footer;
            original.googleCustSearchCode = this.googleCustSearchCode;
            original.signupAgreement = this.signupAgreement;
            original.version = this.version;
            original.siteType = this.siteType;
            original.unlockEmailTemplate = this.unlockEmailTemplate;
            original.resetPasswordMailTemplate = this.resetPasswordMailTemplate;

            original.siteTypeSpecified = true;

        }

        /// <summary>
        /// ArticleView merge
        /// </summary>
        /// <param name="articleView"></param>
        public void Restore(GlobalView original)
        {
            this.id = original.id;
            this.shopName = original.shopName;
            this.slogan = original.slogan;
            this.phoneNumber = original.phoneNumber;
            this.logoAbsoluteUrl = original.logoAbsoluteUrl;
            this.address = original.address;
            this.page404 = original.page404;
            this.page500 = original.page500;
            this.pageNoSearchResult = original.pageNoSearchResult;
            this.footer = original.footer;
            this.googleCustSearchCode = original.googleCustSearchCode;
            this.signupAgreement = original.signupAgreement;
            this.version = original.version;
            this.siteType = original.siteType;
            this.unlockEmailTemplate = original.unlockEmailTemplate;
            this.resetPasswordMailTemplate = original.resetPasswordMailTemplate;
        }
        #endregion

        #region IEditableObject methods
        private GlobalView cache = null;

        /// <summary>
        /// 'Edit' button is clicked
        /// Due to binding, all UI changed will be mapped to the object, so we need make a copy of object before edit in case user wants to cancel the editing
        /// </summary>
        public void BeginEdit()
        {
            cache = new GlobalView(this);
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
