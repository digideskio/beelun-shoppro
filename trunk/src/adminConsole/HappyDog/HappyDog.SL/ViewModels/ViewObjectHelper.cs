using System;
using System.Net;
using System.Collections.ObjectModel;
using HappyDog.SL.Beelun.Shoppro.WSEntryManager;

namespace HappyDog.SL.ViewModels
{
    public static class ViewModelHelper
    {
        /// <summary>
        /// item -> ItemView in collection
        /// </summary>
        /// <param name="itemCollection"></param>
        /// <returns></returns>
        static public ObservableCollection<ItemView> Convert(ObservableCollection<item> itemCollection) 
        {
            ObservableCollection<ItemView> itemViewCollection = new ObservableCollection<ItemView>();
            foreach (item theItem in itemCollection)
            {
                itemViewCollection.Add(new ItemView(theItem));
            }

            return itemViewCollection;
        }

        /// <summary>
        /// category -> CategoryView in collection
        /// </summary>
        /// <param name="itemCollection"></param>
        /// <returns></returns>
        static public ObservableCollection<CategoryView> Convert(ObservableCollection<category> categoryCollection)
        {
            ObservableCollection<CategoryView> categoryViewCollection = new ObservableCollection<CategoryView>();
            foreach (category theCategory in categoryCollection)
            {
                categoryViewCollection.Add(new CategoryView(theCategory));
            }

            return categoryViewCollection;
        }

        /// <summary>
        /// tab -> TabView in collection
        /// </summary>
        /// <param name="itemCollection"></param>
        /// <returns></returns>
        static public ObservableCollection<TabView> Convert(ObservableCollection<tab> tabCollection)
        {
            ObservableCollection<TabView> tabViewCollection = new ObservableCollection<TabView>();
            foreach (tab theTab in tabCollection)
            {
                tabViewCollection.Add(new TabView(theTab));
            }

            return tabViewCollection;
        }

        /// <summary>
        /// user -> UserView in collection
        /// </summary>
        /// <param name="itemCollection"></param>
        /// <returns></returns>
        static public ObservableCollection<UserView> Convert(ObservableCollection<user> userCollection)
        {
            ObservableCollection<UserView> userViewCollection = new ObservableCollection<UserView>();
            foreach (user theUser in userCollection)
            {
                userViewCollection.Add(new UserView(theUser));
            }

            return userViewCollection;
        }

        /// <summary>
        /// order -> OrderView in collection
        /// </summary>
        /// <param name="itemCollection"></param>
        /// <returns></returns>
        static public ObservableCollection<OrderView> Convert(ObservableCollection<order > orderCollection)
        {
            ObservableCollection<OrderView> orderViewCollection = new ObservableCollection<OrderView>();
            foreach (order theOrder in orderCollection)
            {
                orderViewCollection.Add(new OrderView(theOrder));
            }

            return orderViewCollection;
        }

        /// <summary>
        /// Media: model -> view
        /// </summary>
        /// <param name="mediaCollection"></param>
        /// <returns></returns>
        static public ObservableCollection<MediaView> Convert(ObservableCollection<media> mediaCollection)
        {
            ObservableCollection<MediaView> mediaViewCollection = new ObservableCollection<MediaView>();
            foreach (media theMedia in mediaCollection)
            {
                mediaViewCollection.Add(new MediaView(theMedia));
            }

            return mediaViewCollection;        
        }

        /// <summary>
        /// Brand: model -> view
        /// </summary>
        /// <param name="mediaCollection"></param>
        /// <returns></returns>
        static public ObservableCollection<BrandView> Convert(ObservableCollection<brand> brandCollection)
        {
            ObservableCollection<BrandView> brandViewCollection = new ObservableCollection<BrandView>();
            foreach (brand theBrand in brandCollection)
            {
                brandViewCollection.Add(new BrandView(theBrand));
            }

            return brandViewCollection;
        }

        /// <summary>
        /// article -> ArticleView in collection
        /// </summary>
        /// <param name="articleCollection"></param>
        /// <returns></returns>
        static public ObservableCollection<ArticleView> Convert(ObservableCollection<article> articleCollection)
        {
            ObservableCollection<ArticleView> articleViewCollection = new ObservableCollection<ArticleView>();
            foreach (article theArticle in articleCollection)
            {
                articleViewCollection.Add(new ArticleView(theArticle));
            }

            return articleViewCollection;
        }

        /// <summary>
        /// template -> TemplateView in collection
        /// </summary>
        /// <param name="articleCollection"></param>
        /// <returns></returns>
        static public ObservableCollection<TemplateView> Convert(ObservableCollection<template> templateCollection)
        {
            ObservableCollection<TemplateView> templateViewCollection = new ObservableCollection<TemplateView>();
            foreach (template theTemplate in templateCollection)
            {
                templateViewCollection.Add(new TemplateView(theTemplate));
            }

            return templateViewCollection;
        }

        /// <summary>
        /// global -> GlobalView in collection
        /// </summary>
        /// <param name="articleCollection"></param>
        /// <returns></returns>
        static public ObservableCollection<GlobalView> Convert(ObservableCollection<myGlob> globalCollection)
        {
            ObservableCollection<GlobalView> globalViewCollection = new ObservableCollection<GlobalView>();
            foreach (myGlob theGlobal in globalCollection)
            {
                globalViewCollection.Add(new GlobalView(theGlobal));
            }

            return globalViewCollection;
        }

        /// <summary>
        /// global -> GlobalView in collection
        /// </summary>
        /// <param name="articleCollection"></param>
        /// <returns></returns>
        static public GlobalView Convert(myGlob global)
        {
            GlobalView globalView = new GlobalView(global);

            return globalView;
        }

        /// <summary>
        /// global -> GlobalView in collection
        /// </summary>
        /// <param name="articleCollection"></param>
        /// <returns></returns>
        static public ObservableCollection<PaypalView> Convert(ObservableCollection<paypalAccessInfo> paypalCollection)
        {
            ObservableCollection<PaypalView> paypalViewCollection = new ObservableCollection<PaypalView>();
            foreach (paypalAccessInfo thePaypal in paypalCollection)
            {
                paypalViewCollection.Add(new PaypalView(thePaypal));
            }

            return paypalViewCollection;
        }
    }
}
