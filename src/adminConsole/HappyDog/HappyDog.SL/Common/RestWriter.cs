using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;

using HappyDog.SL.Resources;
using HappyDog.SL.EventArguments;
using HappyDog.SL.Common;

namespace HappyDog.SL.Common
{
    /// <summary>
    /// This class is responsible CREATE/UPDATE part of CRUD service
    /// Single threaded to avoid the trouble of multiple writer
    /// Singlton - Allow only one instance in frontend
    /// </summary>
    public class RestWriter
    {
        public delegate void CallbackDelegate(HDCallBackEventArgs args);

        private static volatile RestWriter instance;
        private static object syncRoot = new object();

        #region public methods
        /// <summary>
        /// Get instance
        /// </summary>
        public static RestWriter Instance
        {
            get
            {
                if (null == instance)
                {
                    lock (syncRoot)
                    {
                        if (null == instance)
                        {
                            instance = new RestWriter();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Upload(ASYNC version)
        /// </summary>
        /// <param name="stringToUpload"></param>
        /// <param name="cb"></param>
        public void CreateUpdateAsync(string url, string contentToUpload, CallbackDelegate cb)
        {
            // TODO: implement this
            throw new NotImplementedException();
        }

        /// <summary>
        /// Upload(SYNC version)
        /// </summary>
        /// <param name="stringToUpload"></param>
        /// <returns></returns>
        public HDCallBackEventArgs CreateUpdate(string url, string stringToUpload)
        {
            // TODO: implement this
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete(ASYNC version)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cb"></param>
        public void DeleteAsync(string url, CallbackDelegate cb)
        {
            // TODO: implement me
            throw new NotFiniteNumberException();
        }

        /// <summary>
        /// Delete(SYNC version)
        /// </summary>
        /// <param name="url"></param>
        public void Delete(string url)
        {
            // TODO: implement me
            throw new NotFiniteNumberException();
        }
        #endregion
    }
}
