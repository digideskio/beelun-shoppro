using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Xml.Linq;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Diagnostics;

using HappyDog.SL.Resources;

namespace HappyDog.SL.Common
{
    /// <summary>
    /// Read a txt file from resource
    /// </summary>
    public class TextResFileReader
    {
        const string TextFilePath = @"Txt/";
        private static volatile TextResFileReader instance;
        private static object syncRoot = new object();

        #region public methods
        /// <summary>
        /// Get instance
        /// </summary>
        public static TextResFileReader Instance
        {
            get
            {
                if (null == instance)
                {
                    lock (syncRoot)
                    {
                        if (null == instance)
                        {
                            instance = new TextResFileReader();
                        }
                    }
                }
                return instance;
            }
        }

        public string Read(string fileName)
        {
            // http://compilewith.net/2009/03/wpf-unit-testing-trouble-with-pack-uris.html
            if (!UriParser.IsKnownScheme("pack"))
            {
                UriParser.Register(new GenericUriParser
                    (GenericUriParserOptions.GenericAuthority), "pack", -1);
            }

            string ret = null;

            // TODO: fix myProjectNameSpace
            StreamResourceInfo sri = App.GetResourceStream(new Uri(TextFilePath + fileName, UriKind.Relative));
            using (TextReader tr = new StreamReader(sri.Stream))
            {
                ret = tr.ReadToEnd();
            }

            return ret;
        }
        #endregion
    }
}
