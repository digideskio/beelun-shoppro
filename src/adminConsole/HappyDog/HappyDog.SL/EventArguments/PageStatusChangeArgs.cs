using System;

namespace HappyDog.SL.EventArguments
{
    public class PageStatusChangeArgs : EventArgs
    {
        public enum eStatuses
        {
            Loading,
            Active,
            RetrievingData,
            Closed
        }

        public eStatuses Status { get; set; }

        public PageStatusChangeArgs(eStatuses status)
        {
            Status = status;
        }
    }
}
