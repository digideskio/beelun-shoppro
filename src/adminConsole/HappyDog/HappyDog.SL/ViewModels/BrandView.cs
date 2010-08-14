using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using HappyDog.SL.Content;

namespace HappyDog.SL.ViewModels
{
    /// <summary>
    /// A view wrapper object for HappyDog.SL.Beelun.Shoppro.WebService.MediaManager.Media
    /// </summary>
    public class BrandView : INotifyPropertyChanged, IEditableObject, IDisposable
    {
        #region Private prop
        private string _name;
        private string _image;
        private string _webSite;
        #endregion

        #region Public prop
        [ReadOnly(true)]
        [Display(Name = "ID")]
        public long id { get; set; }

        [Display(Name="Name", Description="The name of brand")]
        [Required]
        [StringLength(255)]
        public string name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        [Display(Name = "Image")]
        [Required]
        [StringLength(255)]
        public string image
        {
            get
            {
                return _image;
            }

            set
            {
                _image = value;
                OnPropertyChanged("image");
            }
        }

        [Display(Name="WebSite")]
        [StringLength(255)]
        public string webSite
        {
            get
            {
                return _webSite;
            }
            set
            {
                _webSite = value;
                OnPropertyChanged("webSite");
            }
        }

        #endregion

        #region Constructor/Destructor
        public BrandView() { }

        public BrandView(brand origianl)
        {
            this.id = origianl.id;
            this.name = origianl.name;
            this.image = origianl.image;
            this.webSite = origianl.webSite;
        }

        public BrandView(BrandView original)
        {
            this.id = original.id;
            this.name = original.name;
            this.image = original.image;
            this.webSite = original.webSite;
        }

        /// <summary>
        /// Used for cleaning up unmanaged resources
        /// </summary>
        ~BrandView()
        {
            this.Dispose();
        }

        #endregion

        #region Public methods
        /// <summary>
        /// Release the stream
        /// </summary>
        public void Dispose()
        {

        }
        /// <summary>
        /// View -> Model
        /// </summary>
        /// <param name="origianl"></param>
        public void Merge(brand original, DataFormMode editMode)
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
            original.image = this.image;
            original.webSite = this.webSite;
        }

        /// <summary>
        /// Copy values back to this object from a cached object
        /// </summary>
        /// <param name="origianl"></param>
        public void Restore(BrandView cachedObject)
        {
            this.id = cachedObject.id;
            this.name = cachedObject.name;
            this.image = cachedObject.image;
            this.webSite = cachedObject.webSite;
        }
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

        #region IEditableObject methods
        private BrandView cache = null;

        /// <summary>
        /// 'Edit' button is clicked
        /// Due to binding, all UI changed will be mapped to the object, so we need make a copy of object before edit in case user wants to cancel the editing
        /// </summary>
        public void BeginEdit()
        {
            cache = new BrandView(this);
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
            // We should not clear cache here. If save fails, we need use cached data to restore original UI.

            // Save to the backend
            //cache = null;
        }

        #endregion
    }
}
