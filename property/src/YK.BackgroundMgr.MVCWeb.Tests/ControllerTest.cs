using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YK.BackgroundMgr.MVCWeb.Controllers;
using YK.Framework.MVCWeb.Controllers;
using YK.BackgroundMgr.PresentationService;
using YK.BackgroundMgr.Common;
using YK.BackgroundMgr.CompositeAppService;
using YK.BackgroundMgr.MVCWeb.ZNMS;

namespace YK.BackgroundMgr.MVCWeb.Tests
{
    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        public void ChargeSubjectInsert()
        {
            LoginController loginController = new LoginController();
          //  loginController.Login("Kuangqifu", "yk1234");

            Console.WriteLine();
        }

        [TestMethod]
        public void HomeTest()
        {
            HomeController homeController = new HomeController();
            homeController.Index();

            Console.WriteLine();
        }

        [TestMethod]
        public void Test()
        {
            RegistService();
            TestController testController = new TestController();
            testController.GetAsynDeptTreeTest("maosiyue", 0);

            Console.WriteLine();
        }

        [TestMethod]
        public void GetKeysByOwnerId()
        {
            ZNMSService servic = new ZNMSService();
            var result = servic.GetKeysByOwnerId("58104c2c-717b-45b2-a392-5bc36bab8edf");

            Console.WriteLine();
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        private void RegistService()
        {
            PresentationServiceHelper.Register<ICookieService>(new CookieService());
            PresentationServiceHelper.Register<ICacheService>(new CacheService());
            PresentationServiceHelper.Register<ISessionService>(new SessionService());
            PresentationServiceHelper.Register<ITemplateService>(new TemplateService());
            PresentationServiceHelper.Register<IPropertyService>(new PropertyAppService());
        }
    }
}
