using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.ApplicationService;
using YK.BackgroundMgr.PresentationService;

namespace YK.Framework.MVCWeb.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View(ReadHomeData());
        }

        #region Private Method

        private HomeData ReadHomeData()
        {
            HomeData homeData = new HomeData();

            string userName = this.CurrentAdminUser.UserName;
            homeData.LoginUserInfo = this.CurrentAdminUser;
            //string userName = "Kuangqifu";

            SEC_ModuleAppService moduleService = new SEC_ModuleAppService();
            SEC_OperateAppService operateService = new SEC_OperateAppService();

            homeData.FrameworkName = ConfigurationManager.AppSettings["FrameworkName"];
            homeData.FrameworkVersion = ConfigurationManager.AppSettings["FrameworkVersion"];
            homeData.Language = "zh-CN";
            var lstUserModules = moduleService.GetUserModuls(userName).OrderBy(r => r.Order);
            homeData.ModuleInfos = lstUserModules;
            homeData.RootModules = homeData.ModuleInfos.Where(m => m.PId == 0);
            homeData.CurrentModule = homeData.ModuleInfos.Where(m => m.PId == 0 && m.EnName == ConfigurationManager.AppSettings["SysModuleEnName"]).FirstOrDefault();
            homeData.LeftModules = homeData.ModuleInfos.Where(m => m.PId == homeData.CurrentModule.Id);
            homeData.CurrentLeftModule = homeData.LeftModules.FirstOrDefault();
            homeData.OperateCodeAndRoleInfos = operateService.GetUserRoleAndOperates(userName);
            return homeData;
        }

        #endregion
    }

    public class HomeData
    {
        public string FrameworkName { get; set; }
        public string FrameworkVersion { get; set; }
        public string Language { get; set; }
        public IEnumerable<SEC_ModuleDTO> ModuleInfos { get; set; }
        public IEnumerable<SEC_ModuleDTO> RootModules { get; set; }
        public IEnumerable<SEC_ModuleDTO> LeftModules { get; set; }
        public SEC_ModuleDTO CurrentLeftModule { get; set; }
        public SEC_ModuleDTO CurrentModule { get; set; }
        public SEC_AdminUserDTO LoginUserInfo { get; set; }
        public IEnumerable<OperateCodeAndRoleInfo> OperateCodeAndRoleInfos { get; set; }
    }
}
