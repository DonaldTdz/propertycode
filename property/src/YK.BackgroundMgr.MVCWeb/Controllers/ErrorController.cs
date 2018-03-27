using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.PresentationService;

namespace YK.BackgroundMgr.MVCWeb
{
    public class ErrorController : Controller
    {
        public ActionResult PageNotFind()
        {
            return View();
        }

        public ActionResult PageError()
        {
            var sessionService = PresentationServiceHelper.LookUp<ISessionService>();
            ViewBag.ErrorMessage = sessionService.GetSession<string>("FrameworkError");
            return View();
        }
    }
}
