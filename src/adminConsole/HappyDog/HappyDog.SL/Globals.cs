using System;
using System.Globalization;
using System.ServiceModel;
using System.Collections.Generic;
using HappyDog.SL.Data;

namespace HappyDog.SL
{
    public static class Globals
    {
        #region Public static Fields
        public static bool IsRunningOutOfBrowser = false;
        public static bool IsDebugOn = false;
        public static bool IsLoggedIn = false;
        public static string WebRootUrl = null;
        public static string ServiceRootUrl
        {
            get
            {
                return WebRootUrl + @"services/";
            }
            set { ;}
        }
        public static TimeSpan UpdatePeriod = new TimeSpan(0, 10, 0); // update every 10 min
        public static CultureInfo Culture = null;
        #endregion

        #region Private static fields
        private static Beelun.Shoppro.WSEntryManager.WSEntryManagerClient _wsEntryManager = null;
        private static BasicHttpBinding _Binding = null;
        private static BasicHttpBinding Binding
        {
            get
            {
                if (_Binding == null)
                {
                    _Binding  = new BasicHttpBinding();
                    _Binding.MaxReceivedMessageSize = 16 * 1024 * 1024;  // 16MB
                }
                return _Binding;
            }
        }            
        #endregion

        #region Public Properties
        /// <summary>
        /// Refer to: com.beelun.shoppro.service.WSEntryManager
        /// </summary>
        public static Beelun.Shoppro.WSEntryManager.WSEntryManagerClient WSClient
        {
            // TODO: We will detach event handler in the code.
            // Refer to: http://stackoverflow.com/questions/1362204/c-how-to-remove-a-lambda-event-handler
            get
            {
                if (_wsEntryManager == null)
                { 
                    _wsEntryManager = new HappyDog.SL.Beelun.Shoppro.WSEntryManager.WSEntryManagerClient(Binding, new EndpointAddress(Globals.ServiceRootUrl + "WSEntryManager"));
                }
                return _wsEntryManager;
            }

            set
            {
                _wsEntryManager = value;
            }
        }

        /// <summary>
        /// Return a new WSClient each time
        /// </summary>
        public static Beelun.Shoppro.WSEntryManager.WSEntryManagerClient NewWSClient
        {
            get
            {
                return new HappyDog.SL.Beelun.Shoppro.WSEntryManager.WSEntryManagerClient(Binding, new EndpointAddress(Globals.ServiceRootUrl + "WSEntryManager"));
            }
        }

        #endregion

        #region Public Constants
        public const int LIST_ITEMS_PER_PAGE = 20;
        public const int LIST_VIEW_MAX_COLUMN = 10;
        public const int MAX_UPLOAD_SIZE = 8 * 1024 * 1024; // 8 MB
        // public const string RelativeLogoImgUri = @"web/images/YourCompanyLogo.png";

        #endregion

        #region Category type(DON'T CHAGNE THIS)
        // TODO: map this from backend. DONT CHANGE THIS FOR NOW
        public const long ALL_CATEGORIES = (-1);
        public const long UNCATEGORIED = (-2);
        #endregion
    }
}

