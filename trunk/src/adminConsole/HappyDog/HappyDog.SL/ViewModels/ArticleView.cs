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
    /// A wrapper class for model object: HappyDog.SL.Beelun.Shoppro.WebService.ArticleManager.article
    /// </summary>
    public class ArticleView : INotifyPropertyChanged, IEditableObject
    {
        #region Properties
        [ReadOnly(true)]
        [Display(Name="ID")]
        public long id { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Title", Description = "Title of this article")]
        public string title { get; set; }

        [Required]
        [Display(Name = "Content", Description = "Content of this article")]
        public string content { get; set; }

        [Required]
        [Display(Name = "Shown?", Description = "Shown in web site?")]
        public bool isShown { get; set; }
        
        [ReadOnly(true)]
        [Display(Name = "Updated", Description = "When this article was last updated")]
        public System.DateTime updated { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Page title")]
        public string pageTitle { get; set; }

        [Required]
        [StringLength(4000)]
        [Display(Name = "Keywords", Description = "Keywords in HTML page")]
        public string keywords { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Description")]
        public string description { get; set; }

        [StringLength(4000)]
        [Display(Name = "Meta tags", Description = "Meta tags in html")]
        public string metaTag { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Url", Description="The url of this article")]
        public string url { get; set; }

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
        public ArticleView() { }

        /// <summary>
        /// Clone
        /// </summary>
        /// <param name="articleView"></param>
        public ArticleView(ArticleView articleView)
        {
            this.id = articleView.id;
            this.title = articleView.title;
            this.content = articleView.content;
            this.isShown = articleView.isShown;
            this.updated = articleView.updated;
            this.pageTitle = articleView.pageTitle;
            this.keywords = articleView.keywords;
            this.description = articleView.description;
            this.metaTag = articleView.metaTag;
            this.url = articleView.url;
        }


        /// <summary>
        /// Model -> View
        /// </summary>
        /// <param name="original"></param>
        public ArticleView(article original)
        {
            this.id = original.id;
            this.title = original.title;
            this.content = original.content;
            this.isShown = original.isShown;
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
        public void Merge(article original, DataFormMode editMode)
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

            original.title = this.title;
            original.content = this.content;
            original.isShown = this.isShown;
            original.updated = this.updated;
            original.pageTitle = this.pageTitle;
            original.keywords = this.keywords;
            original.description = this.description;
            original.metaTag = this.metaTag;
            original.url = this.url;

        }

        /// <summary>
        /// ArticleView merge
        /// </summary>
        /// <param name="articleView"></param>
        public void Restore(ArticleView original)
        {
            this.id = original.id;
            this.title = original.title;
            this.content = original.content;
            this.isShown = original.isShown;
            this.updated = original.updated;
            this.pageTitle = original.pageTitle;
            this.keywords = original.keywords;
            this.description = original.description;
            this.metaTag = original.metaTag;
            this.url = original.url;
        }
        #endregion

        #region IEditableObject methods
        private ArticleView cache = null;

        /// <summary>
        /// 'Edit' button is clicked
        /// Due to binding, all UI changed will be mapped to the object, so we need make a copy of object before edit in case user wants to cancel the editing
        /// </summary>
        public void BeginEdit()
        {
            cache = new ArticleView(this);
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
