using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.Crosscuting;
using YK.BackgroundMgr.PresentationService;
using YK.FrameworkTools.ExcelService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.ApplicationService.Service;


namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class PrepayAccountController : BaseController
    {

        public ActionResult Index()
        {
            PrepayAccountListData prepayAccountListData = new PrepayAccountListData();
            prepayAccountListData.Language = Language;
            var templateModels = GetTemplateModels();
            prepayAccountListData.TemplateModels = templateModels;
            return View("PrepayAccountList", prepayAccountListData);
        }

        [HttpPost]
        public ActionResult ImportPrepayAccount()
        {
            string filePath = "";
            try
            {
                PrepayAccountAppService subjectService = new PrepayAccountAppService();


                var uploadFile = System.Web.HttpContext.Current.Request.Files["fileToUpload"];
                var deptId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Form["PrepayAccountdeptId"]);
                if (uploadFile == null || uploadFile.ContentLength == 0)
                {
                    PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode("请选择需要导入的余额数据文件", System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                    return View("UploadFileResult");
                }

                var fileName = Path.GetFileName(uploadFile.FileName);
                 //获取文件扩展名
                string aLastName = fileName.Substring(fileName.LastIndexOf(".") + 1);
                if (aLastName != "xlsx" && aLastName != "xls")
                {
                    PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode("文件格式不对，正确的文件后缀为.xlsx，请下载模板重试", System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                    return View("UploadFileResult");
                }
                var serverName = string.Format("PrepayAccount_{0}{1}", Guid.NewGuid(), fileName);
                var root = System.Web.HttpContext.Current.Server.MapPath("/");
                string fileDir = Path.Combine(root, "UploadFile");
                if (!Directory.Exists(fileDir))
                {
                    Directory.CreateDirectory(fileDir);
                }
                IEnumerable<TemplateModel> templateModels = GetExcelTemplateModels();

                filePath = Path.Combine(fileDir, serverName);
                uploadFile.SaveAs(filePath);

                ImportResult rm = subjectService.ImportPrepayAccounts(filePath, templateModels, deptId, (int)CurrentAdminUser.Id, CurrentAdminUser.RealName);
                if (rm.IsSuccess)
                {
                    PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode("导入余额成功！", System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                }
                else
                {
                    PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode(rm.ErrorMsg, System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                    if (rm.ErrorTable != null)
                    {
                        var importErrorResult = ExcelHelper.TableToWorkbook(templateModels, rm.ErrorTable);
                        ExportExcel_NewFirefox("导入余额数据结果统计.xls", importErrorResult.SaveToStream().ToArray());
                    }
                }

                System.IO.File.Delete(filePath);
                return View("UploadFileResult");

            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                ViewBag.UploadResult = ex.Message;
                PresentationServiceHelper.LookUp<ICookieService>().SetCookie("ImportResultMsg", HttpUtility.UrlEncode("模板不匹配，请下载模板重试", System.Text.Encoding.UTF8), DateTime.Now.AddHours(5));
                return View("UploadFileResult");
            }

        }
        /// <summary>
        /// 弹窗账户缴费信息（视图构建）
        /// </summary>
        /// <param name="HouseDeptId"></param>
        /// <returns></returns>
        public ActionResult PrepayAccountView(int? HouseDeptId)
        {
            HouseDeptId = HouseDeptId ?? 0;
            BillChargeRecordViewData billChargeRecordViewData = new BillChargeRecordViewData();
            billChargeRecordViewData.Language = this.Language;
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            billChargeRecordViewData.TemplateModels = chargeRecordAppService.GetBillChargeRecordViewTemplate();
            billChargeRecordViewData.BalanceAmount = chargeRecordAppService.GetBalanceAmountByHouseDeptId(HouseDeptId.Value, " ");
            return View(billChargeRecordViewData);
        }

        #region Private Method


        /// <summary>
        /// 弹窗账户缴费信息试（查询）
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetBillChargeRecordViewList(BillChargeRecordSearchDTO search)
        {
            int outCount = 0;
            ChargeRecordAppService chargeRecordAppService = new ChargeRecordAppService();
            if (search.HouseDeptId.HasValue)
            {
                search.DeptId = Convert.ToInt32(search.HouseDeptId);
                search.DeptType = EDeptType.FangWu;
            }
            IList<BillChargeRecord> dataList = chargeRecordAppService.GetBillChargeRecordList(search, out outCount);
            SearchResultData<BillChargeRecord> queryResult = new SearchResultData<BillChargeRecord>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };
            return Json(queryResult);
        }
        #endregion
        private IEnumerable<TemplateModel> GetTemplateModels()
        {
            return PresentationServiceHelper.LookUp<ITemplateService>().GetTemplateModels("PropertyMgrTemplate.xml", "PrepayAccountShow", true);
        }
        private IEnumerable<TemplateModel> GetExcelTemplateModels()
        {
            return PresentationServiceHelper.LookUp<ITemplateService>().GetTemplateModels("PropertyMgrTemplate.xml", "PrepayAccountExcel", true);
        }
        /// <summary>
        /// 主页面查询用户信息
        /// </summary>
        /// <param name="search">查询参数</param>
        /// <returns>返回查询结果</returns>
        [HttpPost]
        public ActionResult GetPrepayAccountShow(PrepayAccountShowDTO search)
        {
            try
            {
                //string search = queryParams.CustomSearch;
                int count = 0;
                //int pageIndex = 0;
                PrepayAccountAppService chargeSubAppService = new PrepayAccountAppService();
                //PrepayAccountShowDTO model = chargeSubAppService.GetSearchParms(queryParams, out pageIndex);
                //pageIndex = (queryParams.Start / 10) + 1;
                IEnumerable<PrepayAccountShowDTO> list = chargeSubAppService.Paging(search.PageStart, search.PageSize, search, "CreateTime", out count);
                SearchResultData<PrepayAccountShowDTO> queryResult = new SearchResultData<PrepayAccountShowDTO>()
                {
                    draw = search.Draw,
                    recordsFiltered = count,
                    recordsTotal = count,
                    data = list
                };
                return Json(queryResult);
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }

    public class PrepayAccountListData
    {
        public string Language { get; set; }
        public IEnumerable<TemplateModel> TemplateModels { get; set; }
    }
    public class PrepayAccountViewData : PrepayAccountListData
    {
        public string ViewType { get; set; }

        public PrepayAccountDTO PrepayAccount { get; set; }
    }

}