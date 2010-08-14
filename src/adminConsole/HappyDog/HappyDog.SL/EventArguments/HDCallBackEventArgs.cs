using System;

namespace HappyDog.SL.EventArguments
{
    /// <summary>
    /// Used for call back event argument
    /// In callback, you must catch all exceptions to avoid it thrown to users
    /// </summary>
    public class HDCallBackEventArgs : EventArgs
    {
        public Exception Error { get; set; }
        public object Result { get; set; }
        public object tag { get; set; }

        // Don't allow no-param constructor
        private HDCallBackEventArgs() { }

        // Successful case
        public HDCallBackEventArgs(object tag, object result)
        {
            this.Error = null;
            this.Result = result;
            this.tag = tag;
        }

        // Error case
        public HDCallBackEventArgs(object tag, Exception error, object result)
        {
            this.tag = tag;
            this.Error = error;
            this.Result = result;
        }
    }
}
