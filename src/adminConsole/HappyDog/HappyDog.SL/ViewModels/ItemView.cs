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
    public class ItemView : INotifyPropertyChanged, IEditableObject
    {
        #region Properties
        [ReadOnly(true)]
        [Display(Name="ID")]
        public long id { get; set; }

        [ReadOnly(true)]
        [Display(Name = "NetSuite Id", Description = "Id in NetSuite DB")]
        public long netsuiteId { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Name", Description = "Name of this item")]
        public string name { get; set; }

        [Display(Name = "Template", Description = "The template to use for formatting the this item")]
        public template theTemplate { get; set; }

        // Refer to this on how to binding: http://stackoverflow.com/questions/1820751/silverlight-bind-collection-to-combobox-in-dataform-using-mvvm
        [Required]
        [Display(Name = "Brand")]
        public brand theBrand { get; set; }

        [Required]
        [Display(Name = "Sell price")]
        public decimal sellPrice { get; set; }

        [Required]
        [Display(Name = "List Price")]
        public decimal listPrice { get; set; }

        [Required]
        [Display(Name = "Inventory number", Description = "# in the stock")]
        public long inventoryNumber { get; set; }

        [StringLength(255)]
        [Display(Name = "Serial number")]
        public string serialNumber { get; set; }

        [Required]
        [StringLength(1024)]
        [Display(Name = "Short description")]
        public string shortDescription { get; set; }

        [Required]
        [Display(Name = "Detailed Description")]
        public string detailedDescription { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Thumbnail", Description = "A smaller image")]
        public string thumbNail { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Image", Description="The bigger image")]
        public string image { get; set; }

        [Required]
        [Display(Name = "Shown?", Description = "Shown in web site?")]
        public bool isShown { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Page title")]
        public string pageTitle { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Url", Description="The url of this item")]
        public string url { get; set; }

        [Required]
        [StringLength(4000)]
        [Display(Name = "Keywords", Description = "Keywords in HTML page")]
        public string keywords { get; set; }

        [Required]
        [StringLength(4000)]
        [Display(Name = "Description")]
        public string description { get; set; }

        [StringLength(4000)]
        [Display(Name = "Meta tags", Description = "Meta tags in html")]
        public string metaTag { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Updated", Description = "When this item was last updated")]
        public System.DateTime updated { get; set; }

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
        public ItemView() { }

        /// <summary>
        /// Clone
        /// </summary>
        /// <param name="itemView"></param>
        public ItemView(ItemView itemView)
        {
            this.id = itemView.id;
            this.netsuiteId = itemView.netsuiteId;
            this.name = itemView.name;
            this.theBrand = itemView.theBrand;
            this.sellPrice = itemView.sellPrice;
            this.listPrice = itemView.listPrice;
            this.inventoryNumber = itemView.inventoryNumber;
            this.serialNumber = itemView.serialNumber;
            this.shortDescription = itemView.shortDescription;
            this.detailedDescription = itemView.detailedDescription;
            this.thumbNail = itemView.thumbNail;
            this.image = itemView.image;
            this.isShown = itemView.isShown;
            this.pageTitle = itemView.pageTitle;
            this.url = itemView.url;
            this.keywords = itemView.keywords;
            this.description = itemView.description;
            this.metaTag = itemView.metaTag;
            this.updated = itemView.updated;
            this.theTemplate = itemView.theTemplate;
        }


        /// <summary>
        /// Model -> View
        /// </summary>
        /// <param name="original"></param>
        public ItemView(item original)
        {
            this.id = original.id;
            this.netsuiteId = original.netSuiteId;
            this.name = original.name;
            this.theBrand = original.brand;
            this.sellPrice = original.sellPrice;
            this.listPrice = original.listPrice;
            this.inventoryNumber = original.inventoryNumber;
            this.serialNumber = original.serialNumber;
            this.shortDescription = original.shortDescription;
            this.detailedDescription = original.detailedDescription;
            this.thumbNail = original.thumbNail;
            this.image = original.image;
            this.isShown = original.isShown;
            this.pageTitle = original.pageTitle;
            this.url = original.url;
            this.keywords = original.keywords;
            this.description = original.description;
            this.metaTag = original.metaTag;
            this.updated = original.updated;
            this.theTemplate = original.template;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// View -> Model
        /// </summary>
        /// <param name="original"></param>
        public void Merge(item original, DataFormMode editMode)
        {
            if (editMode == DataFormMode.Edit)
            {
                original.id = this.id;
                original.netSuiteId = this.netsuiteId;
                original.idSpecified = true;
                original.netSuiteIdSpecified = true;
            }
            else
            {
                original.idSpecified = false;
                original.netSuiteIdSpecified = false;
            }
            original.name = this.name;
            original.brand = this.theBrand;
            original.sellPrice = this.sellPrice;
            original.listPrice = this.listPrice;
            original.inventoryNumber = this.inventoryNumber;
            original.serialNumber = this.serialNumber;
            original.shortDescription = this.shortDescription;
            original.detailedDescription = this.detailedDescription;
            original.thumbNail = this.thumbNail;
            original.image = this.image;
            original.isShown = this.isShown;
            original.pageTitle = this.pageTitle;
            original.url = this.url;
            original.keywords = this.keywords;
            original.description = this.description;
            original.metaTag = this.metaTag;
            original.updated = this.updated;
            original.template = this.theTemplate;

            // Set 'specified'
            original.sellPriceSpecified = true;
            original.listPriceSpecified = true;
            original.inventoryNumberSpecified = true;
        }

        /// <summary>
        /// ItemView merge
        /// </summary>
        /// <param name="itemView"></param>
        public void Restore(ItemView original)
        {
            this.id = original.id;
            this.netsuiteId = original.netsuiteId;
            this.name = original.name;
            this.theBrand = original.theBrand;
            this.sellPrice = original.sellPrice;
            this.listPrice = original.listPrice;
            this.inventoryNumber = original.inventoryNumber;
            this.serialNumber = original.serialNumber;
            this.shortDescription = original.shortDescription;
            this.detailedDescription = original.detailedDescription;
            this.thumbNail = original.thumbNail;
            this.image = original.image;
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
        private ItemView cache = null;

        /// <summary>
        /// 'Edit' button is clicked
        /// Due to binding, all UI changed will be mapped to the object, so we need make a copy of object before edit in case user wants to cancel the editing
        /// </summary>
        public void BeginEdit()
        {
            cache = new ItemView(this);
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
