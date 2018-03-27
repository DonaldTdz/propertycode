using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class NotificeConfigController : BaseController
    {
        // GET: NotificeConfig
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotificeConfigIndex(int DeptId)
        {
            //NotificeConfigAppService service = new NotificeConfigAppService();
            //var fristDeptConfig = service.GetNotificeConfigByComDeptId(DeptId);
            return View();
        }

        [HttpPost]
        public ActionResult GetNotificeConfig(int deptId, string deptName)
        {
            NotificeConfigAppService service = new NotificeConfigAppService();
            var config = service.GetNotificeConfigByComDeptId(deptId, deptName);
            return Json(new { data = config });
        }

        [HttpPost]
        public ActionResult SaveNotice(NotificeConfigDTO inputData)
        {
            inputData.Operator = CurrentAdminUser.Id.Value;
            inputData.OperatorName = CurrentAdminUser.RealName;
            NotificeConfigAppService service = new NotificeConfigAppService();
            var result = service.SaveNotice(inputData);
            return Json(result);
        }
    }
}