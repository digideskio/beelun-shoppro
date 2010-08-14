using System;
using System.ComponentModel;

namespace HappyDog.SL.CustomSort
{
    /// <summary>
    /// Refresh Event Arguments, provides indication of need for data refresh
    /// </summary>
    public class RefreshEventArgs : EventArgs
    {
        public SortDescriptionCollection SortDescriptions { get; set; }
    }
}
