using YK.PropertyMgr.ApplicationService;
using System.Collections.Generic;
using System.Web.Mvc;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationService.Service;
using YK.PropertyMgr.DomainEntity;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class PersonalizedConfigurationController : Controller
    {
        // GET: PersonalizedConfiguration
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 根据小区ID获取小区配置
        /// </summary>
        /// <param name="CommunityDeptId"></param>
        /// <returns></returns>
        public ActionResult GetCommunityConfig(int? CommunityDeptId)
        {
            CommunityConfigAppService Service = new CommunityConfigAppService();
            var CommunityConfig = Service.GetCommunityConfig(CommunityDeptId);
            return Json(CommunityConfig);
        }
        /// <summary>
        /// 插入配置信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SaveCommunityConfig(CommunityConfigDTO model)
        {
            ReturnResult res = new ReturnResult();
            CommunityConfigAppService Service = new CommunityConfigAppService();
            res = Service.SaveData(model);
            return Json(res);
        }
        public class CommunityConfigData
        {
            public int Id { get; set; }
            public int ComDeptId { get; set; }
            public int ConfigType { get; set; }
            public bool IsBuilding { get; set; }
            public bool IsUnit { get; set; }
            public bool IsFloor { get; set; }
            public bool IsNumber { get; set; }
        }

    }
}