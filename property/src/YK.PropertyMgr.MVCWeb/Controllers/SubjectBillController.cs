using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class SubjectBillController : Controller
    {
        public ActionResult SubjectBillIndex()
        {
            //SubjectBillPageView pageView = new SubjectBillPageView();
            //pageView.PageViewSubjectBills = JsonConvert.DeserializeObject<List<SubjectBillView>>(data);
            //return View(pageView);
            return View();
        }
        public class SubjectBillPageView
        {
            public List<SubjectBillView> PageViewSubjectBills { get; set; }
        }
        [HttpPost]
        public ActionResult GenerationBillUnbundling(string subjectIdJson, string unbundlingDtoJson)
        {
            List<int> subjectIds = JsonConvert.DeserializeObject<List<int>>(subjectIdJson).Distinct().ToList();
            List<UnbundlingDto> list = JsonConvert.DeserializeObject<List<UnbundlingDto>>(unbundlingDtoJson);
            ChargBillAppService service = new ChargBillAppService();
            var resultList = service.GenerationBillUnbundling(subjectIds, list);
            var jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jsonResult.Data = resultList;// JsonConvert.SerializeObject(resultList);
            return jsonResult;
        }
    }
}