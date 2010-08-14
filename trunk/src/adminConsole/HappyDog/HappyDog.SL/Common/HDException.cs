using System;

namespace HappyDog.SL.Common
{
    /// <summary>
    /// A common class for Happydog frontend exceptions
    /// </summary>
    public class HDException: Exception
    {
        public HDException() 
        {
            if (Globals.IsDebugOn)
            {
                Logger.Instance.Error(this);
            }
        }
        public HDException(string message) : base(message) 
        {
            if (Globals.IsDebugOn)
            {
                Logger.Instance.Error(message, this);
            }
        }
        public HDException(string message, Exception innerException) : base(message, innerException) 
        {
            if (Globals.IsDebugOn)
            {
                Logger.Instance.Error(message, innerException);
            }
        }
    }
}
