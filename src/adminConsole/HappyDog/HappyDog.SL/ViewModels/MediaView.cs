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
    public class MediaView : INotifyPropertyChanged, IEditableObject, IDisposable
    {
        #region Private prop
        private string _title;
        private string _fileName;
        private string _caption;
        private string _description;
        private string _myUrl;
        private DateTime _updated;
        private Stream _imageStream;
        #endregion

        #region Public prop
        [ReadOnly(true)]
        [Display(Name = "ID")]
        public long id { get; set; }

        [Display(Name="Title", Description="Title of this image")]
        [Required]
        [StringLength(255)]
        public string title
        {
            get
            {
                return _title;
            }

            set
            {
                _title = value;
                OnPropertyChanged("title");
            }
        }

        [Display(Name = "File name")]
        [Required]
        [StringLength(255)]
        public string fileName
        {
            get
            {
                return _fileName;
            }

            set
            {
                _fileName = value;
                OnPropertyChanged("fileName");
            }
        }

        [Display(Name="Caption")]
        [StringLength(255)]
        public string caption
        {
            get
            {
                return _caption;
            }
            set
            {
                _caption = value;
                OnPropertyChanged("caption");
            }
        }

        [Display(Name = "Description")]
        [StringLength(1024)]
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged("description");
            }
        }

        [Display(Name = "Url", Description = "Location of the uploaded file.")]
        [ReadOnly(true)]
        [StringLength(255)]
        public string myUrl
        {
            get
            {
                return _myUrl;
            }
            set
            {
                _myUrl = value;
                OnPropertyChanged("myUrl");
            }
        }

        [Display(Name = "Updated", Description="When this image was last updated")]
        [ReadOnly(true)]
        public DateTime updated
        {
            get
            {
                return _updated;
            }
            set
            {
                _updated = value;
                OnPropertyChanged("updated");
            }
        }

        /// <summary>
        /// The steam of this media
        /// 
        /// Here is the way to show a media: web url -> stream -> image
        /// Web url to stream requires network transfer, it should be running inside background thread to avoid blocking UI thread
        /// Binding to Image will prevent us from re-binding it again.(Just like Control.Children.Add()) Data pager's go to previous page dont work.
        /// So we have to binding to Stream and provide a converter to convert Stream to Image
        /// </summary>
        public Stream ImageStream
        {
            get {
                return _imageStream;
            }

            set {
                _imageStream = value;
                OnPropertyChanged("ImageStream");
            }
        }

        #endregion

        #region Constructor/Destructor
        public MediaView() { }

        public MediaView(media origianl)
        {
            this.id = origianl.id;
            this.title = origianl.title;
            this.caption = origianl.caption;
            this.description = origianl.description;
            this.updated = origianl.updated;
            this.myUrl = origianl.myUrl;
            this.fileName = origianl.fileName;
        }

        public MediaView(MediaView original)
        {
            this.id = original.id;
            this.title = original.title;
            this.caption = original.caption;
            this.description = original.description;
            this.updated = original.updated;
            this.myUrl = original.myUrl;
            this.fileName = original.fileName;
        }

        /// <summary>
        /// Used for cleaning up unmanaged resources
        /// </summary>
        ~MediaView()
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
            if (this.ImageStream != null)
            {
                try
                {
                    this.ImageStream.Close();
                }
                catch (Exception) { };  // Swallow any exception occuring
            }
        }

        /// <summary>
        /// View -> Model
        /// </summary>
        /// <param name="origianl"></param>
        public void Merge(media original, DataFormMode editMode)
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

            original.caption = this.caption;
            original.description = this.description;
            original.myUrl = null;  // myUrl is readonly. We set it as null here.
            original.title = this.title;
            original.fileName = this.fileName;
            original.updated = this.updated;
            original.updatedSpecified = true;   // MUST
        }

        /// <summary>
        /// Copy values back to this object from a cached object
        /// </summary>
        /// <param name="origianl"></param>
        public void Restore(MediaView cachedObject)
        {
            this.caption = cachedObject.caption;
            this.description = cachedObject.description;
            this.id = cachedObject.id;
            this.title = cachedObject.title;
            this.myUrl = cachedObject.myUrl;
            this.fileName = cachedObject.fileName;
            this.updated = cachedObject.updated;
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
        private MediaView cache = null;

        /// <summary>
        /// 'Edit' button is clicked
        /// Due to binding, all UI changed will be mapped to the object, so we need make a copy of object before edit in case user wants to cancel the editing
        /// </summary>
        public void BeginEdit()
        {
            cache = new MediaView(this);
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
