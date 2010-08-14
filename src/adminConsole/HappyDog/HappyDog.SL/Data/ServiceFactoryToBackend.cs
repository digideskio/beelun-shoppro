using System;
using System.ServiceModel;
using System.Windows;

using Microsoft.Silverlight.Samples;
using HappyDog.SL.ToBackend;

namespace HappyDog.SL.Data
{
    /// <summary>
    /// A factory to create an instance of the web service call
    /// </summary>
    public static class ServiceFactoryToBackend
    {
        #region Member Variables
        private static string webServiceUrlPath = null;
        private static string rootUrlPath = null;
        private static bool? isSecure = null;
        #endregion

        #region Public Functions
        /// <summary>
        /// Instantiates an instance of the web service and configures the url
        /// and the credentials in the message header before returning the
        /// instance.
        /// </summary>
        /// <returns>An instance of the web service</returns>
        public static BackendDataServiceClient GetService()
        {
            BasicHttpSecurityMode securityMode = (IsSecure ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None);

            EndpointAddress address = new EndpointAddress(WebServiceUrlPath);
            BasicHttpMessageInspectorBinding binding = new BasicHttpMessageInspectorBinding(new HDServiceMessageInspector(), securityMode);

            BackendDataServiceClient service = new BackendDataServiceClient(binding, address);
            return service;
        }
        #endregion

        #region Public Properties
        public static bool IsSecure
        {
            get
            {
                if (isSecure == null)
                {
                    // We need to instantiate the service just to determine the
                    // security mode of the binding from the clientconfig file - 
                    // easier than opening and reading the file itself
                    BackendDataServiceClient tempClient = new BackendDataServiceClient();
                    string scheme = tempClient.Endpoint.Binding.Scheme;

                    isSecure = (scheme.ToLower() == "https");
                }

                return isSecure.Value;
            }
        }

        public static string GetRootUrlPath(string imgRelativePath)
        {
                if (rootUrlPath == null)
                {
                    // Now build up the url of the service based upon the url
                    // the application was run from and the scheme specified in
                    // the client config
                    Uri url = new Uri(Application.Current.Host.Source, imgRelativePath);
                    rootUrlPath = url.OriginalString.Replace(url.Scheme + "://", (IsSecure ? "https" : "http") + "://");
                }

                return rootUrlPath;
        }
        #endregion

        #region Private Properties
        private static string WebServiceUrlPath
        {
            get
            {
                if (webServiceUrlPath == null)
                {
                    // Now build up the url of the service based upon the url
                    // the application was run from and the scheme specified in
                    // the client config
                    Uri url = new Uri(Application.Current.Host.Source, "../BackendDataService.svc");
                    webServiceUrlPath = url.OriginalString.Replace(url.Scheme + "://", (IsSecure ? "https" : "http") + "://");
                }

                return webServiceUrlPath;
            }
        }
        #endregion

    }
}
