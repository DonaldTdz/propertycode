using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.ApplicationMapper;
using YK.BackgroundMgr.Crosscuting;
using YK.BackgroundMgr.PresentationService;
using YK.FrameworkTools.ExcelService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.ApplicationService.Service;
using YK.PropertyMgr.CompositeAppService;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class ReportController : BaseController
    {



        private List<DeptInfo> ComDeptList;

        public ActionResult ReportArrearsContainerIndex()
        {
            return View("ReportArrearsContainerIndex");
        }

        // GET: Report
        public ActionResult ArrearsChargeSubjectListIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            reportListData.TemplateModels = reportsTemplateAppService.GetArrearsChargeSubjectListTemplate();
            return View("ReportArrearsChargeSubjectList", reportListData);
        }


        public ActionResult ArrearsCommunityListIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            reportListData.TemplateModels = reportsTemplateAppService.GetArrearsCommunityListTemplate();
            return View("ReportArrearsCommunityList", reportListData);
        }

        public ActionResult ArrearsDetailListIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            reportListData.TemplateModels = reportsTemplateAppService.GetArrearsDetailListTemplate();
            return View("ReportArrearsDetailList", reportListData);
        }


        public ActionResult CollectionsCommunityListIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            reportListData.TemplateModels = reportsTemplateAppService.GetCollectionsCommunityListTemplate();
            return View("ReportCollectionsCommunityList", reportListData);
        }

        public ActionResult ReportCollectionsContainerIndex()
        {
            return View("ReportCollectionsContainerIndex");
        }

        public ActionResult CollectionsChargeSubjectListIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            reportListData.TemplateModels = reportsTemplateAppService.GetCollectionsChargeSubjectListTemplate();
            return View("ReportCollectionsChargeSubjectList", reportListData);
        }

        public ActionResult CollectionsDetailListIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            reportListData.TemplateModels = reportsTemplateAppService.GetCollectionsDetailListTemplate();
            return View("ReportCollectionsDetailList", reportListData);
        }

        /// <summary>
        /// 小区报表跳转页
        /// </summary>
        /// <returns></returns>
        public ActionResult CommunityReportIndex()
        {
            return View("CommunityReportIndex");
        }

        /// <summary>
        /// 应收实收对比表
        /// </summary>
        /// <returns></returns>
        public ActionResult ArrearsCollComparisonChart()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            return View("ArrearsCollComparisonChart", reportListData);
        }




        /// <summary>
        /// 报表--应收报表---科目
        /// </summary>
        [HttpPost]
        public ActionResult GetArrearsChargeSubjectList(ReportSearchDTO search)
        {
            DeptAppService deptService = new DeptAppService();
            int outCount = 0;
            search.ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            IList<ReportTableDTO> dataList = ReportAppService.ArrearsChargeSubjectReport(search, out outCount);
            SearchResultData<ReportTableDTO> queryResult = new SearchResultData<ReportTableDTO>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };

            return Json(queryResult);
        }


        /// <summary>
        /// 报表--应收报表---小区
        /// </summary>
        [HttpPost]
        public ActionResult GetArrearsCommunityList(ReportSearchDTO search)
        {
            DeptAppService deptService = new DeptAppService();
            int outCount = 0;
            search.ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            IList<ReportTableDTO> dataList = ReportAppService.ArrearsCommunityReport(search, out outCount);
            SearchResultData<ReportTableDTO> queryResult = new SearchResultData<ReportTableDTO>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };

            return Json(queryResult);
        }


        /// <summary>
        /// 报表--应收报表---明细
        /// </summary>
        [HttpPost]
        public ActionResult GetArrearsDetailList(ReportSearchDTO search)
        {
            DeptAppService deptService = new DeptAppService();
            int outCount = 0;
            search.ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            IList<ReportTableDTO> dataList = ReportAppService.ArrearsDetailReport(search, out outCount);
            SearchResultData<ReportTableDTO> queryResult = new SearchResultData<ReportTableDTO>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };

            return Json(queryResult);
        }


        /// <summary>
        /// 报表--实收报表---科目
        /// </summary>
        [HttpPost]
        public ActionResult GetCollectionsChargeSubjectList(ReportSearchDTO search)
        {
            DeptAppService deptService = new DeptAppService();
            int outCount = 0;
            search.ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            IList<ReportTableDTO> dataList = ReportAppService.CollectionsChargeSubjectReport(search, out outCount);
            SearchResultData<ReportTableDTO> queryResult = new SearchResultData<ReportTableDTO>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };

            return Json(queryResult);
        }


        /// <summary>
        /// 报表--实收报表---小区
        /// </summary>
        [HttpPost]
        public ActionResult GetCollectionsCommunityList(ReportSearchDTO search)
        {
            DeptAppService deptService = new DeptAppService();
            int outCount = 0;
            search.ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            IList<ReportTableDTO> dataList = ReportAppService.CollectionsCommunityReport(search, out outCount);
            SearchResultData<ReportTableDTO> queryResult = new SearchResultData<ReportTableDTO>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };

            return Json(queryResult);
        }

        /// <summary>
        /// 报表--实收报表---明细
        /// </summary>
        [HttpPost]
        public ActionResult GetCollectionsDetailList(ReportSearchDTO search)
        {
            DeptAppService deptService = new DeptAppService();
            int outCount = 0;
            search.ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            IList<ChargeRecordDTO> dataList = ReportAppService.CollectionsDetailReport(search, out outCount);
            SearchResultData<ChargeRecordDTO> queryResult = new SearchResultData<ChargeRecordDTO>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };

            return Json(queryResult);
        }


        /// <summary>
        /// 报表--应收报表---科目
        /// </summary>
        [HttpPost]
        public ActionResult GetArrearsCollComparisonChart(int ComDeptId)
        {
            var model = ReportAppService.GetArrearsCollComparisoCharts(ComDeptId);

            List<EchartsModel> list = new List<EchartsModel>();
            list.Add(new EchartsModel {  name= "实缴金额", value=model.RececiveTotal.Value});
            list.Add(new EchartsModel { name = "未缴金额", value = model.UnPaidAmountTotal });
            list.Add(new EchartsModel { name = "减免金额", value = model.ReliefAmountTotal });
      
            model.MoneyList = list.ToArray();
            return Json(model);
        }

        #region 实收合计
        [HttpGet]
        public JsonResult GetCollectionsTotal(ReportSearchDTO search)
        {
            DeptAppService deptService = new DeptAppService();
 
            search.ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);

         decimal money     = ReportAppService.GetCollectionsTotaMoney(search);

            var returnValue = new ResultModel
            {
                IsSuccess = true,
                Data = new
                {
                     
                    AmountTotal = money.ToString()
                }
            };

            return Json(returnValue, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region 综合报表


        #region Tab页面
        public ActionResult IntegratedReportContainerIndex()
        {
            return View("IntegratedReportContainerIndex");
        }


        #endregion

        #region 科目
        #region 页面-科目
        public ActionResult IntegratedReportByChargeSubject()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            if (ComDeptList.Count() > 0)
            {
                reportListData.DefaultDeptId = ComDeptList.First().Id;
            }
            return View("IntegratedReportByChargeSubject", reportListData);
        }
        #endregion

        #region 数据

        [HttpPost]
        public ActionResult GetIntegratedReportByChargeSubjectList(ReportSearchDTO search)
        {
            ReportAppService service = new ReportAppService();
            IList<ReportTableDTO> dataList = service.GetIntegratedReportChargeSubjectList(search);
            return Json(dataList);
        }

        public void IntegratedReportByChargeSubjectExportData(ReportSearchDTO search)
        {
            ReportAppService service = new ReportAppService();
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            IList<ReportTableDTO> dataList = service.GetIntegratedReportChargeSubjectList(search);
            var tmodules = reportsTemplateAppService.GetIntegratedReportChargeSubjectTemplate();
            var exprotResult = ExcelHelper.Export<ReportTableDTO>(dataList, tmodules);
            ExportExcel("综合报表收费项目汇总表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }



        #endregion



        
        #endregion

        #region 房间明细
        #region 页面-科目
        public ActionResult IntegratedReportByHouse()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);  
            if (ComDeptList.Count() > 0)
            {
                reportListData.DefaultDeptId = ComDeptList.First().Id;
            }
            reportListData.ReportDeptinfo = ComDeptList;
            reportListData.TemplateModels = reportsTemplateAppService.GetIntegratedReportByHouseTemplate();
            return View("IntegratedReportByHouse", reportListData);
        }
        #endregion

        #region 数据

        [HttpPost]
        public ActionResult GetIntegratedReportByHousetList(ReportSearchDTO search)
        {
            
           
            int outCount = 0;
            ReportAppService service = new ReportAppService();
            IList<ReportTableDTO> dataList = service.GetIntegratedReportHouseDetaillList(search, out outCount);

           

            SearchResultData<ReportTableDTO> queryResult = new SearchResultData<ReportTableDTO>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };

            return Json(queryResult);
        }

        #endregion

        

        #region 导出综合报表房屋数据

        public void ExportntegratedReportByHouse(ReportSearchDTO search)
        {
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            int outCount = 0;
            ReportAppService service = new ReportAppService();
            IList<ReportTableDTO> dataList = service.GetIntegratedReportHouseDetaillList(search, out outCount,true);
            var userOwnerTemplateModules = TemplateModelsMapper.ChangeTemplateModelToDTOs(reportsTemplateAppService.GetIntegratedReportByHouseTemplate());
            var exprotResult = ExcelHelper.Export<ReportTableDTO>(dataList, userOwnerTemplateModules);
            ExportExcel("综合报表房屋明细表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }
        #endregion
        #endregion

        #endregion

        #region 正在研发视图

        public ActionResult DevelopingView()
        {
            return View();
        }

        #endregion

        #region 综合报表

        public ActionResult IntegratedReportIndex()
        {
            return View();
        }

        #endregion

        #region 收款日报表

        public ActionResult DayReportIndex()
        {
            return View();
        }
        /// <summary>
        /// 汇总表
        /// </summary>
        /// <returns></returns>
        public ActionResult DayReportContainerIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            if (ComDeptList.Count() > 0)
            {
                reportListData.DefaultDeptId = ComDeptList.First().Id;
            }
            
            //reportListData.TemplateModels = reportsTemplateAppService.GetDayReportTemplate();
            return View(reportListData);
        }

        [HttpPost]
        public ActionResult GetDayReportList(ReportDaySearchDTO search)
        {
            //int outCount = 0;
            //ReportsTemplateAppService service = new ReportsTemplateAppService();
            //IList<ReportDayDTO> dataList = service.GetDayReportDataList(search, out outCount);
            //SearchResultData<ReportDayDTO> queryResult = new SearchResultData<ReportDayDTO>()
            //{
            //    draw = search.Draw,
            //    recordsFiltered = outCount,
            //    recordsTotal = outCount,
            //    data = dataList
            //};

            //return Json(queryResult);
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            IList<ReportDayDTO> dataList = service.GetDayReportDataList(search);
            return Json(dataList);
        }

        public void DayReportExportData(ReportDaySearchDTO search)
        {
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            IList<ReportDayDTO> dataList = service.GetDayReportDataList(search);
            string NMonth = search.ChargeDate.HasValue ? search.ChargeDate.Value.Month.ToString() : "N";
            var tmodules = service.GetDayReportTemplate(NMonth);
            var exprotResult = ExcelHelper.Export<ReportDayDTO>(dataList, tmodules);
            ExportExcel("收款日报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }

        /// <summary>
        /// 明细表
        /// </summary>
        /// <returns></returns>
        public ActionResult DayReportDetailIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            reportListData.TemplateModels = reportsTemplateAppService.GetDayMonthReportDetailTemplate();
            if (ComDeptList.Count() > 0)
            {
                reportListData.DefaultDeptId = ComDeptList.First().Id;
            }

            return View(reportListData);
        }

        [HttpPost]
        public ActionResult GetDayReportDetailList(ReportDaySearchDTO search)
        {
            int outCount = 0;
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            IList<BillDetailInfo> dataList = service.GetDayDetailReportDataList(search, out outCount);
            SearchResultData<BillDetailInfo> queryResult = new SearchResultData<BillDetailInfo>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };

            return Json(queryResult);
        }

        public void GetDayDetailReportExportData(ReportDaySearchDTO search)
        {
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            IList<BillDetailInfo> dataList = service.GetDayDetailReportExportData(search);
            var tmodules = service.GetDayMonthReportDetailTemplate(); ;
            var exprotResult = ExcelHelper.Export<BillDetailInfo>(dataList, tmodules);
            ExportExcel("收款日报表明细" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }

        #endregion

        #region 收款月报表

        public ActionResult MonthReportIndex()
        {
            return View();
        }

        public ActionResult MonthReportContainerIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            if (ComDeptList.Count() > 0)
            {
                reportListData.DefaultDeptId = ComDeptList.First().Id;
            }
            
            return View(reportListData);
        }

        [HttpPost]
        public ActionResult GetMonthReportList(ReportDaySearchDTO search)
        {
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            IList<ReportMonthDTO> dataList = service.GetMonthReportDataList(search);
            return Json(dataList);
        }

        public void MonthReportExportData(ReportDaySearchDTO search)
        {
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            IList<ReportMonthDTO> dataList = service.GetMonthReportDataList(search);
            var showColumns = service.GetMonthReportColumns();
            var exprotResult = ExcelHelper.ExportMultiTitle<ReportMonthDTO>(dataList, showColumns);
            ExportExcel("收款月报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }

        public ActionResult MonthReportDetailIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            reportListData.TemplateModels = reportsTemplateAppService.GetDayMonthReportDetailTemplate();
            if (ComDeptList.Count() > 0)
            {
                reportListData.DefaultDeptId = ComDeptList.First().Id;
            }

            return View(reportListData);
        }

        [HttpPost]
        public ActionResult GetMonthReportDetailList(ReportDaySearchDTO search)
        {
            int outCount = 0;
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            IList<BillDetailInfo> dataList = service.GetMonthDetailReportDataList(search, out outCount);
            SearchResultData<BillDetailInfo> queryResult = new SearchResultData<BillDetailInfo>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };

            return Json(queryResult);
        }

        public void GetMonthDetailReportExportData(ReportDaySearchDTO search)
        {
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            IList<BillDetailInfo> dataList = service.GetMonthDetailReportExportData(search);
            var tmodules = service.GetDayMonthReportDetailTemplate(); ;
            var exprotResult = ExcelHelper.Export<BillDetailInfo>(dataList, tmodules);
            ExportExcel("收款月报表明细" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }

        
        [HttpGet]
        public ActionResult GetCommunityLouyuDeptList(string ComDeptId)
        {
            List<ZTreeNodeModel> list = new List<ZTreeNodeModel>();
            DeptAppService deptService = new DeptAppService();
            list = deptService.GetBuildsByComDeptId(Convert.ToInt32(ComDeptId)).Select(o => new ZTreeNodeModel
            {
                id = o.Id.ToString(),
                text = o.Name,
                name =o.Name,
                isParent=false,
                pId=0,
            }).ToList();
            if (list == null || !(list.Count > 0))
            {
                list = new List<ZTreeNodeModel>();
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 预交费报表

        public ActionResult PrepayReportIndex()
        {
            return View("DevelopingView");
        }

        #endregion

        #region 一次性收费报表

        public ActionResult OnePaymentReportIndex()
        {
            return View("DevelopingView");
        }

        #endregion

        #region 优惠明细表
        public class PayDisInfRepListData
        {
            public string Language { get; set; }
            public IEnumerable<TemplateModel> TemplateModels { get; set; }

        }
        public ActionResult PaymentDiscountInfoReportIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            PaymentDiscountInfoAppService pdiSvc = new PaymentDiscountInfoAppService();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            if (ComDeptList.Count() > 0)
            {
                reportListData.DefaultDeptId = ComDeptList.First().Id;
            }
            reportListData.Language = Language;
            reportListData.TemplateModels = pdiSvc.GetPayDisInfRepListTemplate();
            return View(reportListData);
        }
        [HttpPost]
        public ActionResult GetPaymentDiscountInfoReportList(ReportPayDisInfSearchDTO search)
        {
            int outCount;
            decimal outSum;
            PaymentDiscountInfoAppService pdiSvc = new PaymentDiscountInfoAppService();
            var dataList = pdiSvc.GetPaymentDiscountInfoReport(search, out outCount, out outSum);
            //SearchResultData<ReportPayDisInf> queryResult = new SearchResultData<ReportPayDisInf>()
            //{
            //    draw = search.Draw,
            //    recordsFiltered = outCount,
            //    recordsTotal = outCount,
            //    data = dataList
            //};
            var jdata = new {
                otherData = new { totalAmount = outSum } ,
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };
            return Json(jdata);
        }
        public void PaymentDiscountInfoReportExportData(ReportPayDisInfSearchDTO search)
        {
            PaymentDiscountInfoAppService pdiSvc = new PaymentDiscountInfoAppService();
            var ExportModel = pdiSvc.PayDisInfRepExport(search);
            
            var exprotResult = ExcelHelper.TableToWorkbookNotError(ExportModel.TemPlateList, ExportModel.ExportData);
            ExportExcel("优惠明细表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }

        #endregion

        #region 三表费用报表

        public ActionResult MeterReportIndex()
        {
            return View();
        }
        public ActionResult MeterReportContainerIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            if (ComDeptList.Count() > 0)
            {
                reportListData.DefaultDeptId = ComDeptList.First().Id;
            }
            return View(reportListData);
        }
        public ActionResult GetMeterReportList(ReportMeterSearchDTO search)
        {

            int outCount = 0;
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            var returnModel = service.GetMeterReportDataList(search, out outCount);
            string returnModelstr = JsonHelper.JsonSerializerByNewtonsoft(returnModel);

            return Json(returnModelstr);
        }

        public ActionResult GetMeterReportListNew(ReportMeterSearchDTO search)
        {
            int outCount = 0;
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            var returnModel = service.GetMeterReportDataList(search, out outCount);
            returnModel.ReportMeterDTOList.Add(returnModel.ReportArrearsSum);
            var HeadList = returnModel.ReportHeadList.Select(o => new { name = o.Name }).ToArray();
            var aaData = returnModel.ReportMeterDTOList.Select(o => o.RowDataList.Select(a => a.Text)).ToArray();
            var result = new
            {
                thead = HeadList,
                data = new
                {
                    aaData = aaData,
                    iTotalDisplayRecords = outCount,
                    iTotalRecords = outCount,
                    draw = search.Draw++ 
                }
            };
            return Json(result);
        }

        public void MeterReportExportData(ReportMeterSearchDTO search)
        {
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            var ExportModel = service.MeterReportExport(search);
            var exprotResult = ExcelHelper.TableToWorkbookNotError(ExportModel.TemPlateList, ExportModel.ExportData);
            ExportExcel("三表收费报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }
        #endregion

        #region 三表费用明细报表

        public ActionResult MeterDetailReportContainerIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            reportListData.TemplateModels = service.GetMeterDetailReportTemplateModels();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            if (ComDeptList.Count() > 0)
            {
                reportListData.DefaultDeptId = ComDeptList.First().Id;

            }
            return View("MeterDetailReportContainerIndex", reportListData);
        }
        public ActionResult GetMeterDetailReportList(ReportMeterSearchDTO search)
        {
            int outCount;

            ReportAppService _ReportAppService = new ReportAppService();

            var dataList = _ReportAppService.GetMeterReportDetailList(search, out outCount);
            var jdata = new
            {

                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };
            return Json(jdata);
        }
        public void GetMeterDetailReportExportData(ReportMeterSearchDTO search)
        {
            int outCount;

            ReportAppService _ReportAppService = new ReportAppService();
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            var ExportModel = _ReportAppService.GetMeterReportDetailList(search, out outCount, true);
            var TemplateModels = service.GetMeterDetailReportTemplateModels();
            var exprotResult = ExcelHelper.Export<ReportMeterDetailDTO>(ExportModel, TemplateModels);

            ExportExcel("三表收费明细报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }
        #endregion

        #region 对外收费报表     
        public ActionResult ExternalchargeIndex()
        {
            return View();
        }
        public ActionResult ExternalchargeContainerIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            if (ComDeptList.Count() > 0)
            {
                reportListData.DefaultDeptId = ComDeptList.First().Id;
            }
            return View(reportListData);
        }
        public ActionResult GetExternalchargeReportList(ReportExternalchargeSearchDTO search)
        {
            int outCount = 0;
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            var returnModel = service.GetExternalchargeReportDataList(search, out outCount);
            returnModel.ReportExternalchargeDTOList.Add(returnModel.ReportArrearsSum);
            var HeadList = returnModel.ReportHeadList.Select(o => new { name = o.Name }).ToArray();
            var aaData = returnModel.ReportExternalchargeDTOList.Select(o => o.RowDataList.Select(a => a.Text)).ToArray();
            var result = new
            {
                thead = HeadList,
                data = new
                {
                    aaData = aaData,
                    iTotalDisplayRecords = outCount,
                    iTotalRecords = outCount,
                    draw = search.Draw++
                }
            };
            return Json(result);
        }
        public void ExternalchargeReportExportData(ReportExternalchargeSearchDTO search)
        {
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            var ExportModel = service.ExternalchargeReportExport(search);
            var exprotResult = ExcelHelper.TableToWorkbookNotError(ExportModel.TemPlateList, ExportModel.ExportData);
            ExportExcel("对外收费报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }
        #endregion

        #region 对外收费明细报表
        public ActionResult ExternalchargeDetailContainerIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            reportListData.TemplateModels = service.GetExternalchargeDetailReportTemplateModels();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            if (ComDeptList.Count() > 0)
            {
                reportListData.DefaultDeptId = ComDeptList.First().Id;

            }
            return View("ExternalchargeDetailContainerIndex", reportListData);
        }
        public ActionResult GetExternalchargeDetailReportList(ReportExternalchargeSearchDTO search)
        {
            int outCount;

            ReportAppService _ReportAppService = new ReportAppService();

            var dataList = _ReportAppService.GetExternalchargeReportDetailList(search, out outCount);
           
            var jdata = new
            {

                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };
            return Json(jdata);
        }
        public void GetExternalchargeDetailReportExportData(ReportExternalchargeSearchDTO search)
        {
            int outCount;
            ReportAppService _ReportAppService = new ReportAppService();
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            var ExportModel = _ReportAppService.GetExternalchargeReportDetailList(search, out outCount, true);
            var TemplateModels = service.GetExternalchargeDetailReportTemplateModels();
            var exprotResult = ExcelHelper.Export<ReportExternalchargeDetailDTO>(ExportModel, TemplateModels);

            ExportExcel("对外收费收费明细报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }
        #endregion

        #region 欠费报表-2.4版本

        public ActionResult ArrearsReportContainerIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService reportsTemplateAppService = new ReportsTemplateAppService();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            if (ComDeptList.Count() > 0)
            {
                reportListData.DefaultDeptId = ComDeptList.First().Id;
            }
            return View(reportListData);
        }
        [HttpPost]
        public ActionResult GetArrearsReportList(ReportArrearsSearchDTO search)
        {

            int outCount = 0;
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            var returnModel = service.GetArrearsReportDataList(search, out outCount);
            string returnModelstr = JsonHelper.JsonSerializerByNewtonsoft(returnModel);

          return  Json(returnModelstr);
            
        }

        [HttpPost]
        public ActionResult GetArrearsReportListNew(ReportArrearsSearchDTO search)
        {
            int outCount = 0;
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            var returnModel = service.GetArrearsReportDataList(search, out outCount);
            returnModel.ReportArrearsDTOList.Add(returnModel.ReportArrearsSum);
            var HeadList =returnModel.ReportHeadList.Select(o=>new {name=o.Name}).ToArray();
            var aaData = returnModel.ReportArrearsDTOList.Select(o => o.RowDataList.Select(a => a.Text)).ToArray();
            var result = new
            {
                thead = HeadList,
                data = new
                {
                    aaData = aaData,
                    iTotalDisplayRecords = outCount,
                    iTotalRecords = outCount,
                    draw = search.Draw++
                }
             };
            return Json(result);
        }


        public void ArrearsReportExportData(ReportArrearsSearchDTO search)
        {
            ReportsTemplateAppService service = new ReportsTemplateAppService();
            var ExportModel = service.ArrearsReportExport(search);
            
            
            var exprotResult = ExcelHelper.TableToWorkbookNotError(ExportModel.TemPlateList,ExportModel.ExportData);
            ExportExcel("欠费报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }




        #endregion

        #region 预交费明细表 2.5版本
        public ActionResult PrePaymentDetailReportContainerIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService _ReportsTemplateAppService = new ReportsTemplateAppService();
            reportListData.TemplateModels = _ReportsTemplateAppService.GetPrePaymentDetalReportTemplate();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            if (ComDeptList.Count() > 0)
            {
                reportListData.DefaultDeptId = ComDeptList.First().Id;
                //获取
                 ChargeSubjectAppService _ChargeSubjectAppService = new ChargeSubjectAppService();
                var ChargeSubjectList = new List<ChargeSubjectDTO>();
                 

                ChargeSubjectDTO AllChckModel = new ChargeSubjectDTO()
                {
                    Name = "所有科目",
                    Id = -1
                };
                ChargeSubjectList.Add(AllChckModel);

                ChargeSubjectList.AddRange(_ChargeSubjectAppService.GetChargeSubjectsByComDeptId(reportListData.DefaultDeptId.Value));
                reportListData.DefaultChargeSubjectId = -1;
                reportListData.ReportChargeSubjectInfo = ChargeSubjectList;
            }
            return View("PrePaymentDetailReportIndex", reportListData);
        }


        [HttpPost]
        public ActionResult GetReportComChargeSubjectList(int ComDeptId)
        {
            ChargeSubjectAppService _ChargeSubjectAppService = new ChargeSubjectAppService();
            var ChargeSubjectList = new List<ChargeSubjectDTO>();
            ChargeSubjectDTO AllChckModel = new ChargeSubjectDTO()
            {
                Name = "所有科目",
                Id = -1
            };
            ChargeSubjectList.Add(AllChckModel);
            ChargeSubjectList.AddRange(_ChargeSubjectAppService.GetChargeSubjectsByComDeptId(ComDeptId));
            return Json(ChargeSubjectList);
        }


        [HttpPost]
        public ActionResult GetPrePaymentDetailReportList(PrePaymentDetailSearchDTO search)
        {
            int outCount;
            decimal outSum;
            ReportAppService _ReportAppService = new ReportAppService();
 
            var dataList = _ReportAppService.GetPrePaymentDetailReportList(search, out outCount, out outSum);
            var jdata = new
            {
                otherData = new { totalAmount = outSum },
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };
            return Json(jdata);
        }


        public void PrePaymentDetailReportExportData(PrePaymentDetailSearchDTO search)
        {
            int outCount;
            decimal outSum;
            ReportAppService _ReportAppService = new ReportAppService();
            ReportsTemplateAppService _ReportsTemplateAppService = new ReportsTemplateAppService();
            var ExportModel = _ReportAppService.GetPrePaymentDetailReportList(search, out outCount, out outSum,true);
            var TemplateModels = _ReportsTemplateAppService.GetPrePaymentDetalReportTemplate();
            var exprotResult = ExcelHelper.Export<PrePaymentDetailReportDTO>(ExportModel, TemplateModels);

            ExportExcel("预收明细报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }




        #endregion

        #region 预交费抵扣明细表
        public ActionResult PrePaymentdeductionDetetailReport()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService _ReportsTemplateAppService = new ReportsTemplateAppService();
            reportListData.TemplateModels = _ReportsTemplateAppService.GetPrePaymentdeductionDetetailReportTemplateModels();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            if (ComDeptList.Count() > 0)
            {
                reportListData.DefaultDeptId = ComDeptList.First().Id;
                //获取
                ChargeSubjectAppService _ChargeSubjectAppService = new ChargeSubjectAppService();
                var ChargeSubjectList = new List<ChargeSubjectDTO>();


                ChargeSubjectDTO AllChckModel = new ChargeSubjectDTO()
                {
                    Name = "所有科目",
                    Id = -1
                };
                ChargeSubjectList.Add(AllChckModel);

                ChargeSubjectList.AddRange(_ChargeSubjectAppService.GetChargeSubjectsByComDeptId(reportListData.DefaultDeptId.Value));
                reportListData.DefaultChargeSubjectId = -1;
                reportListData.ReportChargeSubjectInfo = ChargeSubjectList;
            }
            return View("PrePaymentdeductionDetetailReport", reportListData);
        }

        public ActionResult GetPrePaymentdeductionDetetailReportList(PrePaymentdeductionDetailSearchDTO search)
        {
            int outCount;
            ReportAppService _ReportAppService = new ReportAppService();
            var dataList = _ReportAppService.GetPrePaymentdeductionDetailReportList(search, out outCount);
            var jdata = new
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };
            return Json(jdata);
        }

        public void PrePaymentdeductionDetailReportExportData(PrePaymentdeductionDetailSearchDTO search)
        {
            int outCount;
            ReportAppService _ReportAppService = new ReportAppService();
            ReportsTemplateAppService _ReportsTemplateAppService = new ReportsTemplateAppService();
            var ExportModel = _ReportAppService.GetPrePaymentdeductionDetailReportList(search, out outCount, true);
            var TemplateModels = _ReportsTemplateAppService.GetPrePaymentdeductionDetetailReportTemplateModels();
            var exprotResult = ExcelHelper.Export<PrePaymentdeductionDetailReportDTO>(ExportModel, TemplateModels);

            ExportExcel("预交费抵扣明细报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }

        #endregion

        #region 欠费明细报表 -2.6版本



        public ActionResult ArrearsReportIndex()
        {
            return View();
        }

        public ActionResult ArrearsDetailReportContainerIndex()
        {
            DeptAppService deptService = new DeptAppService();
            ReportListData reportListData = new ReportListData();
            ReportsTemplateAppService _ReportsTemplateAppService = new ReportsTemplateAppService();
            reportListData.TemplateModels = _ReportsTemplateAppService.GetArrearsDetailReportDataList();
            ComDeptList = deptService.GetComDeptList(CurrentAdminUser.UserName);
            reportListData.ReportDeptinfo = ComDeptList;
            if (ComDeptList.Count() > 0)
            {
                reportListData.DefaultDeptId = ComDeptList.First().Id;
               
            }
            return View("ArrearsDetailReportContainerIndex", reportListData);
        }

        [HttpPost]
        public ActionResult GetArrearsDetailReportList(ReportArrearsSearchDTO search)
        {
            int outCount;
         
            ReportAppService _ReportAppService = new ReportAppService();

            var dataList = _ReportAppService.GetArrearsReportDetailList(search, out outCount);
            var jdata = new
            {
                
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };
            return Json(jdata);
        }

        public void GetArrearsDetailReportExportData(ReportArrearsSearchDTO search)
        {
            int outCount;
        
            ReportAppService _ReportAppService = new ReportAppService();
            ReportsTemplateAppService _ReportsTemplateAppService = new ReportsTemplateAppService();
            var ExportModel = _ReportAppService.GetArrearsReportDetailList(search, out outCount, true);
            var TemplateModels = _ReportsTemplateAppService.GetArrearsDetailReportDataList();
            var exprotResult = ExcelHelper.Export<ReportArrearsDetailDTO>(ExportModel, TemplateModels);

            ExportExcel("欠费明细报表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls", exprotResult.SaveToStream().ToArray());
        }
        #endregion

        #region 临时收费报表 -2.9版本
        public ActionResult TemporaryChargesReportIndex()
        {

            return View();
        }



        public ActionResult TemporaryChargesReportContainerIndex()
        {
          
            return View();
        }
        #endregion


    }

    public class EchartsModel 
    {
       public string name { get; set; }
        public decimal value { get; set; }
    }

    public class ArrearsReportData
    {
        public DataTable ReportData { get; set; }

        public IEnumerable<TemplateModel> TemplateModelList { get; set; }

        public string SumStr { get; set; }

    }



    public class ReportListData
    {
        public string Language { get; set; }
   
        public IEnumerable<TemplateModel> TemplateModels { get; set; }

        public List<DeptInfo> ReportDeptinfo { get; set; }

        public int? DefaultDeptId { get; set; }

        public int? DefaultChargeSubjectId { get; set; }

        public List<ChargeSubjectDTO> ReportChargeSubjectInfo { get; set; }
    }
}