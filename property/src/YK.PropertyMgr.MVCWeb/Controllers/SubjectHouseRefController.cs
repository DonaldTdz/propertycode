using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class SubjectHouseRefController : Controller
    {
        // GET: SubjectHouseRef
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InsertSubjectHouseRef(SubjectHouseRefDTO subjectHouseRefDTO)
        {
            SubjectHouseRefAppService subjectHouseRefAppService = new SubjectHouseRefAppService();
            subjectHouseRefAppService.InsertSubjectHouseRefCus(subjectHouseRefDTO);
            return View();
        }
        public ActionResult UpdateSubjectHouseRef(SubjectHouseRefDTO subjectHouseRefDTO)
        {
            SubjectHouseRefAppService subjectHouseRefAppService = new SubjectHouseRefAppService();
            subjectHouseRefAppService.UpdateSubjectHouseRefCus(subjectHouseRefDTO);
            return View();
        }
    }
}