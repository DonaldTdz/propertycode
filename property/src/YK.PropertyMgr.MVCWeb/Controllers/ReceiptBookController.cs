using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.PresentationService;
using YK.FrameworkTools.ExcelService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.CompositeDomainService;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class ReceiptBookController : BaseController
    {


        // GET: ReceiptBook
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ReciptBookManagerList()
        {
            ReceiptBookListData receiptBookListData = new ReceiptBookListData();
            ReceiptBookAppService _ReceiptBookAppService = new ReceiptBookAppService();
            receiptBookListData.TemplateModels = _ReceiptBookAppService.GetReceiptBookListTemplate();
            return View(receiptBookListData);

        }

        public ActionResult ReciptBookHistoryList()
        {
            ReceiptBookListData receiptBookListData = new ReceiptBookListData();
            ReceiptBookHistoryAppService _ReceiptBookHistoryAppService = new ReceiptBookHistoryAppService();
            receiptBookListData.TemplateModels = _ReceiptBookHistoryAppService.GetReceiptBookHistoryListTemplate();
            return View(receiptBookListData);

        }

        public ActionResult ReceiptBookModifly()
        {
           
            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
            ReceiptBookViewData ReceiptBookViewData = new ReceiptBookViewData();
            ReceiptBookViewData.Language = Language;
            List<DictionaryModel> listReceiptBookType = propertyService.GetDictionaryModels("ReceiptBookType");
            ReceiptBookViewData.ReceiptBookTypeList = listReceiptBookType;
            ReceiptBookViewData.ReceiptBookModify = new ReceiptBookModifyDTO();
            return View(ReceiptBookViewData);
        }

        public ActionResult ReceiptBookDetailShowView(int Id)
        {
            ReceiptBookDetailListData receiptBookDetailListData = new ReceiptBookDetailListData();
            ReceiptBookDetailAppService _ReceiptBookDetailAppService = new ReceiptBookDetailAppService();
            receiptBookDetailListData.TemplateModels = _ReceiptBookDetailAppService.GetReceiptBookDetailShowListTemplate();
            receiptBookDetailListData.ReceiptBookId = Id;
            return View(receiptBookDetailListData);
        }




        [HttpPost]
        public ActionResult GetReciptBookList(ReceiptBookSearchDTO search)
        {
            int outCount;

            ReceiptBookAppService _ReceiptBookAppService = new ReceiptBookAppService();

            var dataList = _ReceiptBookAppService.GetReceiptBookDTOList(search, out outCount);
            var jdata = new
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };
            return Json(jdata);
        }

        [HttpPost]
        public ActionResult GetReceiptBookHistoryList(ReceiptBookSearchDTO search)
        {
            int outCount;

            ReceiptBookHistoryAppService _ReceiptBookHistoryAppService = new ReceiptBookHistoryAppService();
            var dataList = _ReceiptBookHistoryAppService.GetRReceiptBookHistoryDTOList(search, out outCount);
            var jdata = new
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };
            return Json(jdata);
        }


        public ActionResult ReceiptBookViewAdd()
        {
            ReceiptBookAppService _ReceiptBookAppService = new ReceiptBookAppService();
            ReceiptBookDTO ReceiptBook = new ReceiptBookDTO() { Id=0};

            return CreateReceiptBookView("Add", ReceiptBook);
        }

        public ActionResult ReceiptBookViewEdit(int receiptBookId)
        {
            ReceiptBookAppService _ReceiptBookAppService = new ReceiptBookAppService();
            var ReceiptBook=  _ReceiptBookAppService.GetReceiptBookByKey(receiptBookId);
            ReceiptBook.ReceiptBookTypeStr = ReceiptBook.ReceiptBookType.ToString();
            ReceiptBook.StatusStr = ReceiptBook.Status.ToString();
            return CreateReceiptBookView("Edit", ReceiptBook);

        }

        private ActionResult CreateReceiptBookView(string viewType, ReceiptBookDTO ReceiptBook)
        {
            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();


            ReceiptBookViewData ReceiptBookViewData = new ReceiptBookViewData();
            ReceiptBookViewData.ViewType = viewType;
            ReceiptBookViewData.Language = Language;
            List<DictionaryModel> listReceiptBookType = propertyService.GetDictionaryModels("ReceiptBookType");
            List<DictionaryModel> listReceiptBookTypeStatus = propertyService.GetDictionaryModels("ReceiptBookTypeStatus");
        
            ReceiptBookViewData.ReceiptBookTypeList = listReceiptBookType;
            ReceiptBookViewData.ReceiptBookStatusList = listReceiptBookTypeStatus;
            ReceiptBookViewData.ReceiptBook = ReceiptBook;
            return View("ReceiptBookView", ReceiptBookViewData);
        }

        [HttpPost]
        public ActionResult AddReceiptBook(ReceiptBookDTO ReceiptBook)
        {
            ReceiptBookAppService _ReceiptBookAppService = new ReceiptBookAppService();
            var ResultAuthentication = BillCommonService.Instance.AuthenticationReBookWizard(ReceiptBook, 1);
            if (!ResultAuthentication.IsSuccess)
            {
                return CreateResultView(new ReturnResult()
                {
                    IsSuccess = ResultAuthentication.IsSuccess,
                    Msg = ResultAuthentication.Msg
                });
            }
            var res = _ReceiptBookAppService.InsertReceiptBookDTO(ReceiptBook,CurrentAdminUser.RealName);
            return CreateResultView(res);
        }
        [HttpPost]
        public ActionResult IsReceiptBookStatusPrompt(int RceciptType, int ComDeptId, int ReceiptBookId)
        {
            ReceiptBookAppService _ReceiptBookAppService = new ReceiptBookAppService();
      
            var IsPrompt= _ReceiptBookAppService.IsReceiptBookStatusPrompt(RceciptType, ComDeptId, ReceiptBookId);
            var jResult = new JsonResult();
            jResult.Data = new ActionResultModel() { IsSuccess = IsPrompt };
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jResult;
        }


        





        [HttpPost]
        public ActionResult EditReceiptBook(ReceiptBookDTO ReceiptBook)
        {
            ReceiptBookAppService _ReceiptBookAppService = new ReceiptBookAppService();

            //验证当前票据号是否符合规律
           


            var ResultAuthentication = BillCommonService.Instance.AuthenticationReBookWizard(ReceiptBook, 1);



            if (!ResultAuthentication.IsSuccess)
            {
                return CreateResultView(new ReturnResult() {
                     IsSuccess = ResultAuthentication.IsSuccess,
                      Msg = ResultAuthentication.Msg
                });
            }

            var res= _ReceiptBookAppService.EditReceiptBookDTO(ReceiptBook,CurrentAdminUser.RealName);
            
            return CreateResultView(res);
        }

        [HttpPost]
        public ActionResult ModifyReceiptBook(ReceiptBookModifyDTO ReceiptBook)
        {
            ReceiptBookAppService _ReceiptBookAppService = new ReceiptBookAppService();

            ReceiptBook.ModifyDate = DateTime.Now;
            ReceiptBook.ModifyOperator = CurrentAdminUser.RealName;
            
            var res = _ReceiptBookAppService.RoutineTestingModifyReceiptBookDTO(ReceiptBook,CurrentAdminUser.RealName);

            return CreateResultView(res);
        }


        private ActionResult CreateResultView(ReturnResult res)
        {
            var jResult = new JsonResult();
            if (res.Data == null)
            {
                res.Data = "1";
            }
            jResult.Data = new ActionResultModel() { DataInfo = res.Msg, IsSuccess = res.IsSuccess, ActionInfo=res.Data.ToString() };
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jResult;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReciptBookDetailShowList(ReceiptBookDetailSeachDTO search)
        {
            int outCount;

            ReceiptBookDetailAppService _ReceiptBookDetailAppService = new ReceiptBookDetailAppService();

            var dataList = _ReceiptBookDetailAppService.GetReceiptBookDetailShowList(search, out outCount);
            var jdata = new
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };
            return Json(jdata);
        }
        /// <summary>
        /// 票据本详细导出
        /// </summary>
        /// <param name="search"></param>
        public void GetReciptBookDetailShowExportData(ReceiptBookDetailSeachDTO search)
        {
            int outCount=0;

            ReceiptBookDetailAppService _ReceiptBookDetailAppService = new ReceiptBookDetailAppService();
         
            var ExportModel = _ReceiptBookDetailAppService.GetReceiptBookDetailShowList(search, out outCount, true);

            var TemplateModels = _ReceiptBookDetailAppService.GetReceiptBookDetailShowListTemplate();
            var exprotResult = ExcelHelper.Export<ReceiptBookDetailShowDTO>(ExportModel, TemplateModels);

            ExportExcel("票据本详细" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }

    }

    public class ReceiptBookListData
    {


        public IEnumerable<TemplateModel> TemplateModels { get; set; }
        public string Language { get; set; }

    }

    public class ReceiptBookDetailListData
    {

        public IEnumerable<TemplateModel> TemplateModels { get; set; }
        public string Language { get; set; }
        public int? ReceiptBookId { get; set; }
    }


    public class ReceiptBookViewData : ReceiptBookListData
    {
        public string ViewType { get; set; }
         
        public ReceiptBookDTO ReceiptBook { get; set; }

        public ReceiptBookModifyDTO ReceiptBookModify { get; set; }
        /// <summary>
        ///收费周期 
        /// </summary>
        public List<DictionaryModel> ReceiptBookTypeList { get; set; }
        public List<DictionaryModel> ReceiptBookStatusList { get; set; }
    }

}