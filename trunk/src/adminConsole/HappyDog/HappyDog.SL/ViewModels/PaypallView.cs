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
    public class PaypalView : INotifyPropertyChanged, IEditableObject
    {
        #region Properties
        [ReadOnly(true)]
        [Display(Name="ID")]
        public long id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "API UserName", Description = "API User Name")]
        public string apiUserName { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "API Password", Description = "API Password")]
        public string apiPassword { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "API Signature", Description = "API Signature")]
        public string apiSignature { get; set; }

        [Required]
        [Display(Name = "Use Sandbox?", Description = "If use sandbox")]
        public bool useSandbox { get; set; }

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
        public PaypalView() { }

        /// <summary>
        /// Clone
        /// </summary>
        /// <param name="articleView"></param>
        public PaypalView(PaypalView paypallView)
        {
            this.id = paypallView.id;
            this.apiUserName = paypallView.apiUserName;
            this.apiPassword = paypallView.apiPassword;
            this.apiSignature = paypallView.apiSignature;
            this.useSandbox = paypallView.useSandbox;
            
        }


        /// <summary>
        /// Model -> View
        /// </summary>
        /// <param name="original"></param>
        public PaypalView(paypalAccessInfo original)
        {
            this.id = original.id;
            this.apiUserName = original.apiUserName;
            this.apiPassword = original.apiPassword;
            this.apiSignature = original.apiSignature;
            this.useSandbox = original.useSandbox;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// View -> Model
        /// </summary>
        /// <param name="original"></param>
        public void Merge(paypalAccessInfo original, DataFormMode editMode)
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

            original.apiUserName = this.apiUserName;
            original.apiPassword = this.apiPassword;
            original.apiSignature = this.apiSignature;
            original.useSandbox = this.useSandbox;

        }

        /// <summary>
        /// ArticleView merge
        /// </summary>
        /// <param name="articleView"></param>
        public void Restore(PaypalView original)
        {
            this.id = original.id;
            this.apiUserName = original.apiUserName;
            this.apiPassword = original.apiPassword;
            this.apiSignature = original.apiSignature;
            this.useSandbox = original.useSandbox;
        }
        #endregion

        #region IEditableObject methods
        private PaypalView cache = null;

        /// <summary>
        /// 'Edit' button is clicked
        /// Due to binding, all UI changed will be mapped to the object, so we need make a copy of object before edit in case user wants to cancel the editing
        /// </summary>
        public void BeginEdit()
        {
            cache = new PaypalView(this);
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
