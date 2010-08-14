using System;
using System.IO.IsolatedStorage;

namespace HappyDog.SL
{
    public static class ConfigurationSettings
    {
        #region Constants
        private const string LAST_USER_NAME_KEY = "LastUserName";
        #endregion

        #region Public Properties
        public static string LastUserName
        {
            get
            {
                string value = "";

                if (IsolatedStorageSettings.ApplicationSettings.Contains(LAST_USER_NAME_KEY))
                    value = IsolatedStorageSettings.ApplicationSettings[LAST_USER_NAME_KEY].ToString();

                return value;
            }
            set
            {
                IsolatedStorageSettings.ApplicationSettings[LAST_USER_NAME_KEY] = value;
            }
        }
        #endregion
    }
}
