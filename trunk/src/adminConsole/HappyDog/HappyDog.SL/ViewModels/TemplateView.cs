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
    public class TemplateView : INotifyPropertyChanged, IEditableObject
    {
        #region Private prop
        private string _name;
        private string _description;
        private string _fileNamePrefix;
        private string _templateContent;
        private templateTypeEnum _templateType;
        #endregion

        #region Public prop
        public string fileNamePrefix {get;set;}

        [ReadOnly(true)]
        [Display(Name = "ID")]
        public long id { get; set; }

        [Display(Name = "Name", Description = "The name of template")]
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

        [Display(Name = "Description")]
        [StringLength(255)]
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

        [Display(Name = "Content")]
        [StringLength(16777216)]
        [Required]
        public string templateContent
        {
            get
            {
                return _templateContent;
            }
            set
            {
                _templateContent = value;
                OnPropertyChanged("templateContent");
            }
        }

        [Display(Name = "Type")]
        [Required]
        public templateTypeEnum templateType
        {
            get
            {
                return _templateType;
            }
            set
            {
                _templateType = value;
                OnPropertyChanged("templateType");
            }
        }

        #endregion

        #region Public methods
        /// <summary>
        /// View -> Model
        /// </summary>
        /// <param name="origianl"></param>
        public void Merge(template original, DataFormMode editMode)
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
            original.description = this.description;
            original.fileNamePrefix = this.fileNamePrefix;
            original.templateContent = this.templateContent;
            original.templateType = this.templateType;
            original.templateTypeSpecified = true;
        }

        /// <summary>
        /// Copy values back to this object from a cached object
        /// </summary>
        /// <param name="origianl"></param>
        public void Restore(TemplateView cachedObject)
        {
            this.id = cachedObject.id;
            this.name = cachedObject.name;
            this.description = cachedObject.description;
            this.fileNamePrefix = cachedObject.fileNamePrefix;
            this.templateContent = cachedObject.templateContent;
            this.templateType = cachedObject.templateType;
        }
        #endregion

        #region Constructor/Destructor
        public TemplateView() { }

        public TemplateView(template origianl)
        {
            this.id = origianl.id;
            this.name = origianl.name;
            this.description = origianl.description;
            this.templateContent = origianl.templateContent;
            this.templateType = origianl.templateType;
            this.fileNamePrefix = origianl.fileNamePrefix;
        }

        public TemplateView(TemplateView origianl)
        {
            this.id = origianl.id;
            this.name = origianl.name;
            this.description = origianl.description;
            this.templateContent = origianl.templateContent;
            this.templateType = origianl.templateType;
            this.fileNamePrefix = origianl.fileNamePrefix;
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
        private TemplateView cache = null;

        /// <summary>
        /// 'Edit' button is clicked
        /// Due to binding, all UI changed will be mapped to the object, so we need make a copy of object before edit in case user wants to cancel the editing
        /// </summary>
        public void BeginEdit()
        {
            cache = new TemplateView(this);
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
