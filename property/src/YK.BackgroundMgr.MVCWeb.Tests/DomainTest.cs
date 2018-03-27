using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YK.BackgroundMgr.MVCWeb.Controllers;
using YK.Framework.MVCWeb.Controllers;
using YK.BackgroundMgr.PresentationService;
using YK.BackgroundMgr.Common;
using YK.BackgroundMgr.CompositeAppService;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.DomainCompositeService;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;

namespace YK.BackgroundMgr.MVCWeb.Tests
{
    [TestClass]
    public class DomainTest
    {
        [TestMethod]
        public void GetHouseInfosByCommunityDeptIdTest()
        {
            RegistService();
            var result = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseListByCommunityDeptId(14437);
            var info = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseInfo(14437, 14568);
            Console.WriteLine(result.Count);
        }

        [TestMethod]
        public void GetParkingSpaceListByCommunityDeptId()
        {
            RegistService();
            var result = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetParkingSpaceListByCommunityDeptId(14439);
            Console.WriteLine(result.Count);
        }

        [TestMethod]
        public void GetAsynParkingSpaceTree()
        {
            RegistService();
            var result = new PropertyAppService().GetAsynParkingSpaceTree("4D2EBFAB-D458-4EB4-B3AA-4025F4215C93");
            Console.WriteLine(result.Count);
        }

        [TestMethod]
        public void GetAsynParkingTree()
        {
            RegistService();
            var result = new PropertyAppService().GetAsynParkingTree(14439);
            Console.WriteLine(result.Count);
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

            RegistDomainService();
        }

        /// <summary>
        /// 领域层服务
        /// </summary>
        private void RegistDomainService()
        {
            DomainInterfaceHelper.Register<IPropertyDomainService>(new PropertyDomainService());
        }
    }
}
