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
    /// A wrapper class for model object: category
    /// </summary>
    public class CategoryView : INotifyPropertyChanged, IEditableObject
    {
        #region Private propeties
        private string _name = null;
        private string _url = null;
        private bool _isShown;
        private DateTime _updated;
        private string _pageTitle;
        private string _keywords;
        private string _description;
        private string _metaTag;
        private template _theTemplate;
        #endregion

        #region Properties
        [ReadOnly(true)]
        [Display(Name="ID")]
        public long id { get; set; }

        /// <summary>
        /// 'name' field is unique in DB. So we need restore to original value on error. So we notifiy changes
        /// </summary>
        [Required]
        [StringLength(255)]
        [Display(Name = "Name", Description = "Name of this category. This field should be unique.")]
        public string name
        {
            get
            {
                return _name;
            }
            set {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        [Display(Name = "Template", Description = "The template to use for formatting this category")]
        public template theTemplate
        {
            get
            {
                return _theTemplate;
            }
            set
            {
                _theTemplate = value;
                OnPropertyChanged("template");
            }
        }

        [Required]
        [Display(Name = "Shown?", Description = "Shown in web site?")]
        public bool isShown
        {
            get
            {
                return _isShown;
            }

            set
            {
                _isShown = value;
                OnPropertyChanged("isShown");
            }
        }

        [ReadOnly(true)]
        [Display(Name = "Updated", Description = "When this category was last updated")]
        public System.DateTime updated
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

        [Required]
        [StringLength(255)]
        [Display(Name = "Page title")]
        public string pageTitle
        {
            get
            {
                return _pageTitle;
            }

            set
            {
                _pageTitle = value;
                OnPropertyChanged("pageTitle");
            }
        }

        [Required]
        [StringLength(4000)]
        [Display(Name = "Keywords", Description = "Keywords in HTML page")]
        public string keywords
        {
            get
            {
                return _keywords;
            }

            set
            {
                _keywords = value;
                OnPropertyChanged("keywords");
            }
        }


        [Required]
        [StringLength(4000)]
        [Display(Name = "Description")]
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

        [StringLength(255)]
        [Display(Name = "Meta tags", Description = "Meta tags in html")]
        public string metaTag
        {
            get
            {
                return _metaTag;
            }

            set
            {
                _metaTag = value;
                OnPropertyChanged("metaTag");
            }
        }

        [Required]
        [StringLength(255)]
        [Display(Name = "Url", Description = "The url of this category. This filed should be unique.")]
        public string url
        {
            get
            {
                return _url;
            }

            set
            {
                _url = value;
                OnPropertyChanged("url");
            }
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

        #region Constructor
        /// <summary>
        /// Default constructor is a must. Otherwise 'add' button will be greyed out.
        /// </summary>
        public CategoryView() { }

        /// <summary>
        /// Clone
        /// </summary>
        /// <param name="categoryView"></param>
        public CategoryView(CategoryView categoryView)
        {
            this.id = categoryView.id;
            this.name = categoryView.name;
            this.isShown = categoryView.isShown;
            this.updated = categoryView.updated;
            this.pageTitle = categoryView.pageTitle;
            this.keywords = categoryView.keywords;
            this.description = categoryView.description;
            this.metaTag = categoryView.metaTag;
            this.url = categoryView.url;
            this.theTemplate = categoryView.theTemplate;
        }


        /// <summary>
        /// Model -> View
        /// </summary>
        /// <param name="original"></param>
        public CategoryView(category original)
        {
            this.id = original.id;
            this.name = original.name;
            this.isShown = original.isShown;
            this.updated = original.updated;
            this.pageTitle = original.pageTitle;
            this.keywords = original.keywords;
            this.description = original.description;
            this.metaTag = original.metaTag;
            this.url = original.url;
            this.theTemplate = original.template;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// View -> Model
        /// </summary>
        /// <param name="original"></param>
        public void Merge(category original, DataFormMode editMode)
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
            original.isShown = this.isShown;
            original.pageTitle = this.pageTitle;
            original.url = this.url;
            original.keywords = this.keywords;
            original.description = this.description;
            original.metaTag = this.metaTag;
            original.updated = this.updated;
            original.updatedSpecified = true;
            original.template = this.theTemplate;
        }

        /// <summary>
        /// ItemView merge
        /// </summary>
        /// <param name="itemView"></param>
        public void Restore(CategoryView original)
        {
            this.id = original.id;
            this.name = original.name;
            this.isShown = original.isShown;
            this.pageTitle = original.pageTitle;
            this.url = original.url;
            this.keywords = original.keywords;
            this.description = original.description;
            this.metaTag = original.metaTag;
            this.updated = original.updated;
            this.theTemplate = original.theTemplate;
        }
        #endregion

        #region IEditableObject methods
        private CategoryView cache = null;

        /// <summary>
        /// 'Edit' button is clicked
        /// Due to binding, all UI changed will be mapped to the object, so we need make a copy of object before edit in case user wants to cancel the editing
        /// </summary>
        public void BeginEdit()
        {
            cache = new CategoryView(this);
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
