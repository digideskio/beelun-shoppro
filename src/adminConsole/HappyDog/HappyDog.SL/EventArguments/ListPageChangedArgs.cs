using System;

namespace HappyDog.SL.EventArguments
{
    public class ListPageChangedArgs : EventArgs
    {
        public int PageNumber { get; set; }

        public ListPageChangedArgs(int pageNumber)
        {
            PageNumber = pageNumber;
        }
    }
}
