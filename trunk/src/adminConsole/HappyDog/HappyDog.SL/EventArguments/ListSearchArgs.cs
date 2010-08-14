using System;

namespace HappyDog.SL.EventArguments
{
    public class ListSearchArgs : EventArgs
    {
        public string SearchText { get; set; }
        public bool InSearchMode { get; set; }

        public ListSearchArgs(string searchText, bool inSearchMode)
        {
            SearchText = searchText;
            InSearchMode = inSearchMode;
        }
    }

}
