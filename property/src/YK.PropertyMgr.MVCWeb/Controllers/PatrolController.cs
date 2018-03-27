using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.PresentationService;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class PatrolController : BaseController
    {
        // GET: Patrol
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PatrolPlanIndex()
        {
            ViewBag.PatrolSrc = ConfigurationManager.AppSettings["PatrolPlanUrl"];
            return View("Index");
        }

        public ActionResult PatrolEquipmentIndex()
        {
            ViewBag.PatrolSrc = ConfigurationManager.AppSettings["PatrolEquipmentUrl"];
            return View("Index");
        }

        public ActionResult PatrolReportIndex()
        {
            ViewBag.PatrolSrc = ConfigurationManager.AppSettings["PatrolReportUrl"];
            return View("Index");
        }
    }
}