using System;
using System.Xml.Linq;
using System.Collections.Generic;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;
using HappyDog.SL.Content;

namespace HappyDog.SL.Data
{
    public class ModelDataContainer
    {
        public List<ViewItem> dashBoardTreeView = null;
        public List<ViewItem> itemsTreeView = null;
        public List<ViewItem> settingsTreeView = null;
        public List<ViewItem> usersTreeView = null;
        public List<ViewItem> ordersTreeView = null;

        // <uri, viewClass>
        private Dictionary<string, ContentPageContext> _ContentPageContextContainer = new Dictionary<string, ContentPageContext>();
        public Dictionary<string, ContentPageContext> ContentPageContextContainer
        {
            get
            {
                return _ContentPageContextContainer;
            }
            private set{}
        }

        // Suppose there is no much contents in category and tab, so load them all in system startup
        // TODO: But it is not a good idea to cache the data all the time
        public List<category> allCategories = null;
        public List<tab> allTabs = null;
        public List<brand> allBrands = null;
        public List<template> allItemTemplates = null;
        public List<template> allCategoryTemplates = null;

    }
}
