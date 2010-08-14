using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Silverlight.Testing;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HappyDog.SL.Data;
using HappyDog.SL.Common;

namespace HappyDog.Test
{
    [TestClass]
    public class ModelProviderTest : SilverlightTest
    {
        const string userId = "123";
        const string password = "happydog";
        const string rootUrl = @"http://localhost:9080/happydog/ws/dal/";
        static ModelProvider mp = ModelProvider.Instance;
        private bool bSkipMe = true;

        [ClassInitialize]
        [Asynchronous]
        public void Init()
        {
            if (bSkipMe)
            {
                Assert.IsTrue(true);
                EnqueueTestComplete();
                return;
            }
            mp.UserId = userId;
            mp.Password = password;
            mp.AsyncHanlder += (sender, e) =>
            {
                EnqueueTestComplete();
            };
            mp.InitSchemaAsync();
        }

        [TestMethod]
        [Asynchronous]
        public void TestSetLoggedInUser()
        {
            if (bSkipMe)
            {
                Assert.IsTrue(true);
                EnqueueTestComplete();
                return;
            }
            //Assert.IsNotNull(mp.MDC.currentUser != null);
            EnqueueTestComplete();
            //RestReader md = new RestReader();
            //string[] urls = new string[1];
            //urls[0] = GetUrl("ADUser");
            //md.OnEvent += (sender, e) =>
            //    {
            //        try
            //        {
            //            switch (e.status)
            //            {
            //                case DownloadStatus.CompleteOne_Success:
            //                    //mp.UserId = userId;
            //                    //mp.SetLoggedInUser(e.data);
            //                    break;
            //                case DownloadStatus.CompleteAll_Success:
            //                    Assert.IsTrue(mp.CurrentUser.Roles.Count == 2);
            //                    EnqueueTestComplete();
            //                    break;
            //                default:
            //                    Assert.Fail("error msg:" + e.data);
            //                    EnqueueTestComplete();
            //                    break;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            Assert.Fail(ex.Message);
            //        }
            //    };
            //md.Start(urls, 3, 5, "a");
        }

        [TestMethod]
        [Asynchronous]
        public void TestGetPropertyList()
        {
            if (bSkipMe)
            {
                Assert.IsTrue(true);
                EnqueueTestComplete();
                return;
            }

            RestReader md = new RestReader();
            string[] urls = new string[1];
            urls[0] = GetUrl("hd_ui_treeview_access");
            md.OnEvent += (sender, e) =>
                {
                    try
                    {
                        switch (e.status)
                        {
                            case DownloadStatus.CompleteOne_Success:
                                //// element
                                //Dictionary<string, string> l = mp.GetPropertyList(e.data, "hd_ui_treeview_access", "name");
                                //Assert.IsTrue(l != null);
                                //Assert.IsTrue(l.Count == 4);
                                //Assert.IsTrue(l["2"] == "test2");

                                //// element.attribute
                                //Dictionary<string, string> l2 = mp.GetPropertyList(e.data, "hd_ui_treeview_access", "role.identifier");
                                //Assert.IsTrue(l2 != null);
                                //Assert.IsTrue(l2.Count == 4);
                                //Assert.IsTrue(l2["2"] == "HDTestRoleA");

                                //// negative test
                                //Dictionary<string, string> l3 = mp.GetPropertyList(e.data, "hd_ui_treeview_access", "role.identifier.id");
                                //Assert.IsTrue(l3 == null);

                                break;
                            case DownloadStatus.CompleteAll_Success:
                                EnqueueTestComplete();
                                break;
                            default:
                                Assert.Fail("error msg:" + e.data);
                                EnqueueTestComplete();
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Assert.Fail(ex.Message);
                    }

                };
            md.Start(urls, 3, 5, "a");
        }      

        #region private method
        private string GetUrl(string entityName)
        {
            return string.Format("{0}{1}?l={2}&p={3}", rootUrl, entityName, userId, password);
        }
        #endregion
    }
}