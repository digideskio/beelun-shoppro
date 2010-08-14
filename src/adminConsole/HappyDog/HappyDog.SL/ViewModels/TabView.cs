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
    /// A wrapper class for model object: HappyDog.SL.Beelun.Shoppro.WebService.TabManager.tab
    /// </summary>
    public class TabView : INotifyPropertyChanged, IEditableObject
    {
        #region Private propeties
        private string _name = null;
        private bool _isShown;
        private string _content;
        private long _showOrder;
        private DateTime _updated;
        private string _pageTitle;
        private string _keywords;
        private string _description;
        private string _metaTag;
        private string _url = null;
        
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

        [Required]
        [Display(Name = "Content")]
        public string content
        {
            get
            {
                return _content;
            }

            set
            {
                _content = value;
                OnPropertyChanged("content");
            }
        }

        [Required]
        [Display(Name = "showOrder")]
        public long showOrder
        {
            get
            {
                return _showOrder;
            }

            set
            {
                _showOrder = value;
                OnPropertyChanged("showOrder");
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
        public TabView() { }

        /// <summary>
        /// Clone
        /// </summary>
        /// <param name="categoryView"></param>
        public TabView(TabView tabView)
        {
            this.id = tabView.id;
            this.name = tabView.name;
            this.isShown = tabView.isShown;
            this.content = tabView.content;
            this.showOrder = tabView.showOrder;
            this.updated = tabView.updated;
            this.pageTitle = tabView.pageTitle;
            this.keywords = tabView.keywords;
            this.description = tabView.description;
            this.metaTag = tabView.metaTag;
            this.url = tabView.url;
        }


        /// <summary>
        /// Model -> View
        /// </summary>
        /// <param name="original"></param>
        public TabView(tab original)
        {
            this.id = original.id;
            this.name = original.name;
            this.isShown = original.isShown;
            this.content = original.content;
            this.showOrder = original.showOrder;
            this.updated = original.updated;
            this.pageTitle = original.pageTitle;
            this.keywords = original.keywords;
            this.description = original.description;
            this.metaTag = original.metaTag;
            this.url = original.url;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// View -> Model
        /// </summary>
        /// <param name="original"></param>
        public void Merge(tab original, DataFormMode editMode)
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
            original.content = this.content;
            original.showOrder = this.showOrder;
            original.pageTitle = this.pageTitle;
            original.url = this.url;
            original.keywords = this.keywords;
            original.description = this.description;
            original.metaTag = this.metaTag;
            original.updated = this.updated;

            original.showOrderSpecified = true;
        }

        /// <summary>
        /// ItemView merge
        /// </summary>
        /// <param name="itemView"></param>
        public void Restore(TabView original)
        {
            this.id = original.id;
            this.name = original.name;
            this.isShown = original.isShown;
            this.content = original.content;
            this.showOrder = original.showOrder;
            this.pageTitle = original.pageTitle;
            this.url = original.url;
            this.keywords = original.keywords;
            this.description = original.description;
            this.metaTag = original.metaTag;
            this.updated = original.updated;
        }
        #endregion

        #region IEditableObject methods
        private TabView cache = null;

        /// <summary>
        /// 'Edit' button is clicked
        /// Due to binding, all UI changed will be mapped to the object, so we need make a copy of object before edit in case user wants to cancel the editing
        /// </summary>
        public void BeginEdit()
        {
            cache = new TabView(this);
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
