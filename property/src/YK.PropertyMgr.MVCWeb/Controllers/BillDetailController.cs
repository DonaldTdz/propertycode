using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.ApplicationMapper;
using YK.BackgroundMgr.Crosscuting;
using YK.BackgroundMgr.PresentationService;
using YK.FrameworkTools.ExcelService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.ApplicationService.Service;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class BillDetailController : BaseController
    {
        // GET: BillDetail
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BillDetailList(int? DeptId, EDeptType? DeptType, string DeptName, bool SettleAccount)
        {
            BillDetailAppService service = new BillDetailAppService();
            BillDetailViewData data = new BillDetailViewData();
            //data.PayTypeList = service.GetPayTypeList().ToList();
            //data.ChargeTypeList = service.GetChargeTypeList().ToList();
            //data.BillStatusList = service.GetBillStatusList().ToList();
            data.ChargeSubjectList = service.GetChargeSubjectList(DeptId, DeptType).ToList();
            data.TemplateModels = service.GetBillDetailTemplate(SettleAccount);
            return View(data);
        }

        [HttpPost]
        public ActionResult GetBillDetailList(BillDetailSearchDTO search)
        {
            int outCount = 0;
            BillDetailAppService service = new BillDetailAppService();
            IList<BillDetailInfo> dataList = service.GetBillDetailList(search, out outCount);
            SearchResultData<BillDetailInfo> queryResult = new SearchResultData<BillDetailInfo>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };

            return Json(queryResult);
        }
      
        [HttpPost]
        public ActionResult GetSubjectList(int? DeptId, EDeptType? DeptType)
        {
            BillDetailAppService service = new BillDetailAppService();
            var subjectList = service.GetChargeSubjectList(DeptId, DeptType).ToList();
            return Json(subjectList);
        }

        public void ExportData(BillDetailSearchDTO search)
        {
            search.PageSize = int.MaxValue;
            int outCount = 0;
            BillDetailAppService service = new BillDetailAppService();
            IList<BillDetailInfo> dataList = service.GetBillDetailList(search, out outCount);
            var tmodules = TemplateModelsMapper.ChangeTemplateModelToDTOs(service.GetBillDetailTemplate(search.SettleAccount));
            var exprotResult = ExcelHelper.Export<BillDetailInfo>(dataList, tmodules);
            ExportExcel("账单详情" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }
    }

    public class BillDetailViewData
    {
        //public List<DictionaryModel> ChargeTypeList { get; set; }

        public List<object> BillStatusList { get; set; }

        //public List<DictionaryModel> PayTypeList { get; set; }

        public List<ChargeSubjectDTO> ChargeSubjectList { get; set; }

        public string Language { get; set; }

        public IEnumerable<TemplateModel> TemplateModels { get; set; }
    }
}