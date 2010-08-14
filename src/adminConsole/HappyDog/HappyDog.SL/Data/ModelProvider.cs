using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.Threading;
using System.ServiceModel;
using System.Windows.Browser;

using HappyDog.SL.Resources;
using HappyDog.SL.EventArguments;
using HappyDog.SL.Common;
using HappyDog.SL.Content;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;

[assembly: InternalsVisibleTo("HappyDog.Test")]
namespace HappyDog.SL.Data
{
    /// <summary>
    /// Object Model which implemented as a singleton
    /// Refer to: http://msdn.microsoft.com/en-us/library/ms998558.aspx
    /// </summary>
    public class ModelProvider
    {
        #region private properties
        private static volatile ModelProvider instance;
        private static object syncRoot = new object();
        private string password = null;
        private CallbackDelegate callBack = null;
        // The event
        private AutoResetEvent nextOneAutoResetEvent = new AutoResetEvent(false);
        private ModelDataContainer mdc = new ModelDataContainer();
        #endregion

        #region public properties
        public ModelDataContainer MDC { get { return mdc; } }
        public string UserId { get; set; }
        public string Password { set { this.password = value; } }    // TODO: Should we save password locally?
        #endregion

        #region public event hanlder
        public event EventHandler<HDCallBackEventArgs> AsyncHanlder;
        public event EventHandler<HDCallBackEventArgs> onProgress;
        #endregion

        #region Delegate definition
        public delegate void CallbackDelegate(HDCallBackEventArgs args);
        #endregion

        #region public methods
        /// <summary>
        /// Get instance
        /// </summary>
        public static ModelProvider Instance
        {
            get
            {
                if (null == instance)
                {
                    lock (syncRoot)
                    {
                        if (null == instance)
                        {
                            instance = new ModelProvider();
                        }
                    }
                }
                return instance;
            }
        }

        #region Public methods
        public ContentPageContext GetContentPageContext(string contentPageUri)
        {
            ContentPageContext cpc = null;
            if (this.mdc.ContentPageContextContainer.TryGetValue(contentPageUri, out cpc))
            {
                return cpc;
            }
            return null;
        }

        public string GetContentPageUri(PageEnum pageEnum)
        {
            return @"/" + pageEnum.ToString();
        }

        public string GetContentPageUri(PageEnum pageEnum, long filterId)
        {
            return string.Format(@"/{0}/{1}", pageEnum.ToString(), filterId);
        }

        public void AddViewContext(PageEnum pageEnum, ContentPageContext ctx)
        {
            this.mdc.ContentPageContextContainer[GetContentPageUri(pageEnum)] = ctx;
        }

        public void AddViewContext(PageEnum pageEnum, long fid, ContentPageContext ctx)
        {
            this.mdc.ContentPageContextContainer[GetContentPageUri(pageEnum, fid)] = ctx;
        }

        #endregion

        /// <summary>
        /// Initialize entities by calling backend schema
        /// </summary>
        public void InitSchemaAsync()
        {
            // Dash board pages
            this.mdc.dashBoardTreeView = new List<ViewItem>();
            ViewItem db1 = new ViewItem(1, 0, "Sales Trend Chart", "D1", PageEnum.DashBoardPage1);
            this.mdc.dashBoardTreeView.Add(db1);
            ContentPageContext dbCtx = new ContentPageContext();
            this.AddViewContext(PageEnum.DashBoardPage1, dbCtx);

            db1 = new ViewItem(2, 0, "User Trend Chart", "D2", PageEnum.DashBoardPage2);
            this.mdc.dashBoardTreeView.Add(db1);
            dbCtx = new ContentPageContext();
            this.AddViewContext(PageEnum.DashBoardPage2, dbCtx);


            //
            // Add tree view item for web site page
            //
            this.mdc.itemsTreeView = new List<ViewItem>();
            ViewItem vi = new ViewItem(1, Globals.ALL_CATEGORIES, 0, "All Items", "All available items", PageEnum.ItemListPage);
            this.mdc.itemsTreeView.Add(vi);
            ContentPageContext ctx = new ContentPageContext();
            this.AddViewContext(PageEnum.ItemListPage, Globals.ALL_CATEGORIES, ctx);
            this.AddViewContext(PageEnum.ItemDetailsPage, Globals.ALL_CATEGORIES, ctx);

            ViewItem cvi = new ViewItem(2, 0, "Categories", "All available categories", PageEnum.CategoryListPage);
            this.mdc.itemsTreeView.Add(cvi);
            ctx = new ContentPageContext();
            this.AddViewContext(PageEnum.CategoryListPage, ctx);
            this.AddViewContext(PageEnum.CategoryDetailsPage, ctx);

            ViewItem tvi = new ViewItem(3, 0, "Tabs", "All available tabs", PageEnum.TabListPage);
            this.mdc.itemsTreeView.Add(tvi);
            ctx = new ContentPageContext();
            this.AddViewContext(PageEnum.TabListPage, ctx);
            this.AddViewContext(PageEnum.TabDetailsPage, ctx);

            ViewItem mediaView = new ViewItem(4, 0, "Media library", "All available images, files, videos, etc", PageEnum.MediaListPage);
            this.mdc.itemsTreeView.Add(mediaView);
            ctx = new ContentPageContext();
            this.AddViewContext(PageEnum.MediaListPage, ctx);
            this.AddViewContext(PageEnum.MediaDetailsPage, ctx);

            ViewItem avi = new ViewItem(5, 0, "Article", "Article Setting", PageEnum.ArticleListPage);
            this.mdc.itemsTreeView.Add(avi);
            ctx = new ContentPageContext();
            this.AddViewContext(PageEnum.ArticleListPage, ctx);
            this.AddViewContext(PageEnum.ArticleDetailsPage, ctx);

            ViewItem bvi = new ViewItem(6, 0, "Brand", "All available brands", PageEnum.BrandListPage);
            this.mdc.itemsTreeView.Add(bvi);
            ctx = new ContentPageContext();
            this.AddViewContext(PageEnum.BrandListPage, ctx);
            this.AddViewContext(PageEnum.BrandDetailsPage, ctx);

            ViewItem templateViewItem = new ViewItem(7, 0, "Template", "All available templates", PageEnum.TemplateListPage);
            this.mdc.itemsTreeView.Add(templateViewItem);
            ctx = new ContentPageContext();
            this.AddViewContext(PageEnum.TemplateListPage, ctx);
            this.AddViewContext(PageEnum.TemplateDetailsPage, ctx);

            //
            //Add tree view item for users page
            //
            this.mdc.usersTreeView = new List<ViewItem>();
            ViewItem uvi = new ViewItem(1, 0, "Users", "All available users", PageEnum.UserListPage);
            this.mdc.usersTreeView.Add(uvi);
            ctx = new ContentPageContext();
            this.AddViewContext(PageEnum.UserListPage, ctx);
            this.AddViewContext(PageEnum.UserDetailsPage, ctx);

            //
            //Add tree view item for orders page
            //
            this.mdc.ordersTreeView = new List<ViewItem>();
            ViewItem ovi = new ViewItem(1, 0, "Orders", "All available orders", PageEnum.OrderListPage);
            this.mdc.ordersTreeView.Add(ovi);
            this.AddViewContext(PageEnum.OrderListPage, new ContentPageContext());
            ctx = new ContentPageContext();
            this.AddViewContext(PageEnum.OrderListPage, ctx);
            this.AddViewContext(PageEnum.OrderDetailsPage, ctx);

            //
            // Setting
            //
            this.mdc.settingsTreeView = new List<ViewItem>();
            ViewItem pvi = new ViewItem(1, 0, "Paypal", "Paypal Setting", PageEnum.PaypalPage);
            //pvi.viewClass = new ContentPageCtx(PageEnum.OrderDetailsPage);
            this.mdc.settingsTreeView.Add(pvi);
            this.AddViewContext(PageEnum.PaypalPage, new ContentPageContext());

            ViewItem gvi = new ViewItem(1, 0, "Global", "Global Setting", PageEnum.GlobalPage);
            this.mdc.settingsTreeView.Add(gvi);
            this.AddViewContext(PageEnum.GlobalPage, new ContentPageContext());

            //
            // Do interaction work with backend
            //
            ThreadPool.QueueUserWorkItem(StartWork);
        }

        private void StartWork(object state)
        {
            // Login
            this.onProgress(this, new HDCallBackEventArgs(this, new TaskProgress(UIResources.TASKDSP_LOGIN, 20)));
            WebClient webClient = new WebClient();
            webClient.Headers["content-type"] = "application/x-www-form-urlencoded";
            webClient.Encoding = Encoding.UTF8;
            bool isLoggedIn = false;
            webClient.UploadStringCompleted += (s, e) =>
            {
                if (e.Error == null)
                {
                    string result = e.Result;
                    if (result.Contains("pass"))
                    {
                        isLoggedIn = true;
                    }
                }
                else {
                    isLoggedIn = false;
                }
                nextOneAutoResetEvent.Set();
            };
            StringBuilder sb = new StringBuilder();
            string postData = string.Format(@"u={0}&p={1}", HttpUtility.UrlEncode(this.UserId), HttpUtility.UrlEncode(this.password));
            webClient.UploadStringAsync(new Uri(Globals.WebRootUrl + @"membership/admin-rest-login.html", UriKind.Absolute), "POST", postData);
            nextOneAutoResetEvent.WaitOne();
            if (!isLoggedIn)
            {
                Globals.IsLoggedIn = false;
                this.AsyncHanlder(this, new HDCallBackEventArgs(this, new Exception(UIResources.AUTH_NOROLE), null));
                return;
            }
            else
            {
                Globals.IsLoggedIn = true;
            }           

            // Get all categories
            this.onProgress(this, new HDCallBackEventArgs(this, new TaskProgress("Loading all categories...", 70)));
            EventHandler<getAll_CategoryCompletedEventArgs> h1 = (s, e) =>
            {
                if (e.Error == null)
                {
                    this.MDC.allCategories = new List<category>(e.Result);
                }
                nextOneAutoResetEvent.Set();
            };
            Globals.WSClient.getAll_CategoryCompleted += h1;
            Globals.WSClient.getAll_CategoryAsync();
            nextOneAutoResetEvent.WaitOne();
            Globals.WSClient.getAll_CategoryCompleted -= h1;

            //
            // Add filter by category
            //
            int currentTreeNodeCount = ViewItem.Counter;
            foreach (category c in this.mdc.allCategories)
            {
                // Parent is 1 - items
                ViewItem categoryAvi = new ViewItem((int)c.id + currentTreeNodeCount, (int)c.id, 1, c.name, c.pageTitle, PageEnum.ItemListPage);
                this.mdc.itemsTreeView.Add(categoryAvi);
                ContentPageContext cpc = new ContentPageContext(c.id);
                this.AddViewContext(PageEnum.ItemListPage, c.id, cpc);
                this.AddViewContext(PageEnum.ItemDetailsPage, c.id, cpc);
            }

            // Get all tabs
            this.onProgress(this, new HDCallBackEventArgs(this, new TaskProgress(UIResources.WAITINDICATOR_LOADING, 80)));
            EventHandler<getAll_TabCompletedEventArgs> h2 = (s, e) =>
            {
                if (e.Error == null)
                {
                    this.MDC.allTabs = new List<tab>(e.Result);
                }
                nextOneAutoResetEvent.Set();
            };
            Globals.WSClient.getAll_TabCompleted += h2;
            Globals.WSClient.getAll_TabAsync();
            nextOneAutoResetEvent.WaitOne();
            Globals.WSClient.getAll_TabCompleted -= h2;

            // Get all brands
            this.onProgress(this, new HDCallBackEventArgs(this, new TaskProgress(UIResources.WAITINDICATOR_LOADING, 85)));
            EventHandler<getAll_BrandCompletedEventArgs> h3 = (s, e) =>
            {
                if (e.Error == null)
                {
                    this.MDC.allBrands = new List<brand>(e.Result);
                }
                nextOneAutoResetEvent.Set();
            };
            Globals.WSClient.getAll_BrandCompleted += h3;
            Globals.WSClient.getAll_BrandAsync();
            nextOneAutoResetEvent.WaitOne();
            Globals.WSClient.getAll_BrandCompleted -= h3;

            // Get all item templates
            Globals.WSClient.getByType_TemplateCompleted += new EventHandler<getByType_TemplateCompletedEventArgs>(WSClient_getByType_TemplateCompleted);
            Globals.WSClient.getByType_TemplateAsync(templateTypeEnum.ITEM);
            nextOneAutoResetEvent.WaitOne();
            Globals.WSClient.getByType_TemplateCompleted -= new EventHandler<getByType_TemplateCompletedEventArgs>(WSClient_getByType_TemplateCompleted);

            // Get all category templates
            this.onProgress(this, new HDCallBackEventArgs(this, new TaskProgress(UIResources.WAITINDICATOR_LOADING, 95)));
            EventHandler<getByType_TemplateCompletedEventArgs> h5 = (s, e) =>
            {
                if (e.Error == null)
                {
                    this.MDC.allCategoryTemplates = new List<template>(e.Result);
                }
                nextOneAutoResetEvent.Set();
            };
            Globals.WSClient.getByType_TemplateCompleted += h5;
            Globals.WSClient.getByType_TemplateAsync(templateTypeEnum.CATEGORY);
            nextOneAutoResetEvent.WaitOne();
            Globals.WSClient.getByType_TemplateCompleted -= h5;

            // Call back
            if (null != this.AsyncHanlder)
            {
                this.AsyncHanlder(this, new HDCallBackEventArgs(this, null));
            }
        }

        void WSClient_getByType_TemplateCompleted(object sender, getByType_TemplateCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                this.MDC.allItemTemplates = new List<template>(e.Result);
            }
            nextOneAutoResetEvent.Set();
        }

        /// <summary>
        /// Init internal data container
        /// </summary>
        public void Clean()
        {
            this.Password = null;
            this.UserId = null;
            this.mdc = null;
            this.mdc = new ModelDataContainer();
            Globals.WSClient = null;
        }

        public void LogoutAsync(CallbackDelegate cbd)
        {
            this.callBack = cbd;
            LogoutTask logoutTask = new LogoutTask();
            logoutTask.onComplete += new EventHandler<HDCallBackEventArgs>(logoutTask_onComplete);
            logoutTask.StartAsync(null, 0);
        }

        #endregion

        #region private methods
        private void eb_onComplete(object sender, HDCallBackEventArgs e)
        {
            this.callBack(e);
            this.callBack = null;
        }

        private void tt_onProgress(object sender, HDCallBackEventArgs e)
        {
            if (this.onProgress != null)
            {
                this.onProgress(this, e);
            }
        }

        /// <summary>
        /// Task tracker call back
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tt_onComplete(object sender, HDCallBackEventArgs e)
        {
            if (null != this.AsyncHanlder)
            {
                this.AsyncHanlder(this, e);
            }
        }

        void logoutTask_onComplete(object sender, HDCallBackEventArgs e)
        {
            this.callBack(e);
        }

        /// <summary>
        /// Override default constructor
        /// </summary>
        private ModelProvider() { }        
        #endregion
    }
}
