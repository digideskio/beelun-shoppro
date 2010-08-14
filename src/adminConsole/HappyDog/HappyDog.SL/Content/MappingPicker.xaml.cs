using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using HappyDog.SL.ViewModels;

namespace HappyDog.SL.Content
{
    /// <summary>
    /// The window to pick which category(-ies) or tab(-es) to map/unmap
    /// </summary>
    public partial class MappingPicker : ChildWindow
    {
        #region Private prop
        private List<MappingView> cache = new List<MappingView>();
        private List<MappingView> cmapList = null;

        // <CategoryId, mapped> or // <TabId, mapped>
        private Dictionary<long, bool> changedMapping = new Dictionary<long, bool>();
        #endregion

        #region Public prop
        /// <summary>
        /// Contains two kinds of changed mapping:
        /// (1) none-ALL to ALL
        /// (2) none-NONE to NONE
        /// Caller will access this public prop to get user's selection
        /// </summary>
        public Dictionary<long, bool> ChangedMapping
        {
            get
            {
                return changedMapping;
            }
        }

        /// <summary>
        /// Set instruction of mapping
        /// </summary>
        public string Instruction
        {
            private get { return null; }
            set             
            {
                this.instructionTextBlock.Text = value;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cmapList"></param>
        public MappingPicker(List<MappingView> cmapList)
        {
            InitializeComponent();

            this.cmapList = cmapList;

            // Clone the list to cache
            this.cmapList.ForEach((item) =>
                {
                    this.cache.Add(new MappingView(item));
                });
        }
        #endregion

        #region Event handler
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            // Make a diff of what have been changed
            if(this.cache.Count != 0) {
                for (int i = 0; i < this.cache.Count; ++i)
                {
                    // Due to TwoWay mapping, cmapList is the result after user's selection.
                    // We only look for ALL/NONE mappings. If user change the mapping to ALL/NONE, we should do the job for users
                    // Ignore PARTIAL. We can do nothing about it.
                    if (this.cmapList[i].Mapped == true && this.cache[i].Mapped != true)
                    {
                        changedMapping.Add(this.cmapList[i].Id, true);
                    }
                    else if (this.cmapList[i].Mapped == false && this.cache[i].Mapped != false) {
                        changedMapping.Add(this.cmapList[i].Id, false);
                    }
                }
            }

            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LayoutRoot.DataContext = this.cmapList;
        }
        #endregion

    }
}

