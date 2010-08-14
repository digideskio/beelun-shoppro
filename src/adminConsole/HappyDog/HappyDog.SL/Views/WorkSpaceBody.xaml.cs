using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Navigation;

using HappyDog.SL.Content;
using HappyDog.SL.Data;
using HappyDog.SL.EventArguments;

namespace HappyDog.SL.Views
{
    public partial class WorkSpaceBody : Page
    {
        #region private vars
        private bool bIsExpanderExpanded = true;
        private List<NewViewItem> myTreeView = null;
        private DispatcherTimer treeViewSelectionTimer = null;
        private bool isInitDone = false;
        #endregion

        #region Constructor
        public WorkSpaceBody()
        {
            InitializeComponent();
        }
        #endregion

        #region Private methods

        #region Build tree in side bar
        private List<NewViewItem> BuildTree(ref List<ViewItem> origObjs)
        {
            if (null == origObjs)
            {
                return null;
            }

            // Create a new collection
            List<NewViewItem> tree = new List<NewViewItem>();

            // Debug
            foreach (ViewItem v in origObjs)
            {
                Debug.WriteLine(v.ViweId + ":" + v.ParentViewId);
            }

            // And call our recursive method specifying "0" as current parent
            AddLevel(ref origObjs, ref tree, 0);

            return tree;
        }

        private void AddLevel(ref List<ViewItem> origObjs, ref List<NewViewItem> tree, int parent)
        {
            NewViewItem child;
            List<NewViewItem> childCollection;

            // Get all children of current parent
            List<ViewItem> o = (from achild in origObjs where achild.ParentViewId == parent select achild).ToList();

            foreach (ViewItem v in o)
            {
                // for one of those children create a new object
                child = new NewViewItem()
                {
                    ViewId = v.ViweId,
                    // View = v.viewClass,
                    ContentPageUri = v.ContentPageUri,
                    ViewName = v.ViewName,
                    Description = v.Description,
                    SubViews = new List<NewViewItem>()
                };
                tree.Add(child);
                childCollection = child.SubViews;

                // Recursively add all its children
                AddLevel(ref origObjs, ref childCollection, v.ViweId);
            }
        }
        #endregion


        /// <summary>
        /// Gets a value passed in through the query if available.
        /// </summary>
        /// <param name="argName">The key to find.</param>
        /// <returns>The value associated with the key if available, otherwise "{Not Specified}".</returns>
        private string GetArgument(string argName)
        {
            if (NavigationContext.QueryString.ContainsKey(argName))
            {
                return NavigationContext.QueryString[argName];
            }
            return "{Not Specified}";
        }
        #endregion

        #region Public methods
        /// <summary>
        /// For switch items, refer to:
        /// adminConsole\HappyDog\HappyDog.SL\Controls\Header.xaml
        /// </summary>
        /// <param name="targetView"></param>
        public void UpdateTreeView(string targetView)
        {
            List<ViewItem> origObjs = null;

            // TODO: put hard-codeded string into resource?
            switch (targetView)
            {
                case "DashBoard":
                    origObjs = ModelProvider.Instance.MDC.dashBoardTreeView;
                    break;

                case "Item":
                    origObjs = ModelProvider.Instance.MDC.itemsTreeView;
                    break;

                case "User":
                    origObjs = ModelProvider.Instance.MDC.usersTreeView;
                    break;

                case "Order":
                    origObjs = ModelProvider.Instance.MDC.ordersTreeView;
                    break;

                case "Setting":
                    origObjs = ModelProvider.Instance.MDC.settingsTreeView;
                    break;

                default:
                    throw new NotImplementedException("Not supported view:" + targetView);
            }
            this.myTreeView = BuildTree(ref origObjs);
            this.NavigationSideBar.DataContext = this.myTreeView;
            //LayoutRoot.DataContext = this.myTreeView;
            if (this.myTreeView != null && this.myTreeView.Count != 0)
            {
                // Create a timer to select the first item in another thread
                // From:  http://www.telerik.com/community/forums/silverlight/treeview/select-a-node-from-code-behind.aspx
                treeViewSelectionTimer = new DispatcherTimer();
                treeViewSelectionTimer.Interval = TimeSpan.FromMilliseconds(100); // 0.1 sec
                treeViewSelectionTimer.Tick += new EventHandler(Timer_Tick);
                treeViewSelectionTimer.Start();
            }
        }
        #endregion

        // Executes when the user navigates to this page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Debug.WriteLine(string.Format("View:{0}, isInitDone:{1}", this.GetArgument("targetView"), this.isInitDone));
            if (!this.isInitDone)
            {
                this.isInitDone = true;
                this.UpdateTreeView(this.GetArgument("targetView"));
            }
        }

        /// <summary>
        /// Select the first item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eArgs"></param>
        private void Timer_Tick(object sender, EventArgs eArgs)
        {
            TreeViewItem tvi = myTree.ItemContainerGenerator.ContainerFromIndex(0) as TreeViewItem;
            if (null != tvi)
            {
                tvi.IsSelected = true;
                treeViewSelectionTimer.Tick -= new EventHandler(Timer_Tick);
            }
        }

        #region For expander only
        /// <summary>
        /// Event handler when expander is collasped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OWAExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            if (true == bIsExpanderExpanded)
            {
                OWAExpander.Template = this.Resources["T1"] as ControlTemplate;
                bIsExpanderExpanded = false;
                //OWAExpander.SetValue(Canvas.ZIndexProperty, 200);
                //ViewBody.SetValue(Canvas.ZIndexProperty, -200);
            }
        }

        /// <summary>
        /// Event handler when expander is expanded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OWAExpander_Expanded(object sender, RoutedEventArgs e)
        {
            if (false == bIsExpanderExpanded)
            {
                OWAExpander.Template = this.Resources["T2"] as ControlTemplate;
                bIsExpanderExpanded = true;
            }
        }
        #endregion

        /// <summary>
        /// Event handler when selected item is changed.
        /// Notes: All requests should go to this.CurrentPage_NavigateRequest()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // Change main view body
            NewViewItem nvi = e.NewValue as NewViewItem;
            Debug.WriteLine("Tree selection changed, now go to: " + nvi.ContentPageUri);
            if (nvi.ContentPageUri.Contains("ItemList"))
            {
                this.ContentFrame.Navigate(new Uri(nvi.ContentPageUri + "/" + "Items in category: " + nvi.ViewName, UriKind.Relative));
            }
            else
            {
                this.ContentFrame.Navigate(new Uri(nvi.ContentPageUri, UriKind.Relative));
            }
        }

        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {

        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
