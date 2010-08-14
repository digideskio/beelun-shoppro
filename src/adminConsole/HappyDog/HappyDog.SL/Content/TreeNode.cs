using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

using HappyDog.SL.Data;

namespace HappyDog.SL.Content
{
    /// <summary>
    /// Tree node definition
    /// </summary>
    public class ViewItem
    {
        /// <summary>
        /// Starting from 1
        /// </summary>
        public int ViweId { get; set; }

        /// <summary>
        /// 0 means no parent view
        /// </summary>
        public int ParentViewId { get; set; }

        /// <summary>
        /// Name of this view
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// Description of the view
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Current page Uri defined by SL navigation
        /// </summary>
        public string ContentPageUri { get; set; }

        /// <summary>
        /// Counter of how many objects are created
        /// </summary>
        public static int Counter { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="viewId"></param>
        /// <param name="parentViewId"></param>
        /// <param name="viewName"></param>
        /// <param name="description"></param>
        /// <param name="contentPage"></param>
        public ViewItem(int viewId, int parentViewId, string viewName, string description, PageEnum contentPage)
        {
            this.ViweId = viewId;
            this.ParentViewId = parentViewId;
            this.ViewName = viewName;
            this.Description = description;
            this.ContentPageUri = string.Format("/{0}", contentPage.ToString(), viewId);

            ++ViewItem.Counter;
        }

        public ViewItem(int viewId, long filterId, int parentViewId, string viewName, string description, PageEnum contentPage)
        {
            this.ViweId = viewId;
            this.ParentViewId = parentViewId;
            this.ViewName = viewName;
            this.Description = description;
            this.ContentPageUri = string.Format("/{0}/{1}", contentPage.ToString(), filterId);

            ++ViewItem.Counter;
        }

    }

    /// <summary>
    /// Helper class of view item for tree view
    /// It is actually another form of ViewItem class
    /// </summary>
    public class NewViewItem
    {
        public int ViewId { get; set; }
        public string ViewName { get; set; }
        public string Description { get; set; }
        public List<NewViewItem> SubViews { get; set; }
        public string ContentPageUri { get; set; }
    }
}
