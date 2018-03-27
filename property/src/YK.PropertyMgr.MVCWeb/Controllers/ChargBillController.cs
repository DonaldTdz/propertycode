using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.ApplicationDTO.Resources;
using YK.PropertyMgr.ApplicationService;
using YK.PropertyMgr.ApplicationService.Service;
using YK.PropertyMgr.CompositeAppService;
using YK.PropertyMgr.CompositeDomainService;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainEntity;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class ChargBillController : BaseController
    {
        //
        // GET: /ChargBill/
        public ActionResult Index(bool IsCarPark = false)
        {
            ChargBillPageData data = new ChargBillPageData();
            data.IsCarPark = IsCarPark;
            return View(data);
        }

        //DailyChargeList
        public ActionResult DailyChargeList(int? DeptId, EDeptType? DeptType, string DeptName, bool IsCarPark = false)
        {
            ChargBillAppService chargBillAppService = new ChargBillAppService();
            var cbData = chargBillAppService.GetDailyChargList(DeptId, DeptType, DeptName, IsCarPark, Language);
            return View(cbData);
        }

        /// <summary>
        /// 查询日常收费账单
        /// </summary>
        /// <param name="queryParams">查询参数</param>
        /// <returns>返回查询结果</returns>
        [HttpPost]
        public ActionResult GetDailyChargList(ChargBillSearchDTO search)
        {
            ChargBillAppService chargeRecordAppService = new ChargBillAppService();
            var billData = chargeRecordAppService.GetDailyChargList(search);
            return Json(billData);
        }
        
        /// <summary>
        /// 日常缴费
        /// </summary>
        [HttpPost]
        public ActionResult PayChargBill(DailyChargBillDTO SaveModel)
        {
            ChargBillAppService chargBillAppService = new ChargBillAppService();
            PayTypeEnum paytypeEnum = (PayTypeEnum)SaveModel.PayTypeId;

            ResultModel resultModel = chargBillAppService.PayChargBill(SaveModel, (int)CurrentAdminUser.Id, CurrentAdminUser.RealName);
            if (resultModel.IsSuccess)
            {
                resultModel.Msg = "缴费成功"+ resultModel.Msg;
                resultModel.PageData = new DailyChargBillDTO();
            }
            else
            {
                resultModel.Msg = "缴费失败：" + resultModel.Msg;
            }
            return Json(resultModel);
        }

        #region 开发商收费

        /// <summary>
        /// 开发商代缴Index
        /// </summary>
        /// <returns></returns>
        public ActionResult DeveloperIndex()
        {
            return View();
        }

        public ActionResult DeveloperChargeList(int? DeptId, EDeptType? DeptType, string DeptName)
        {
            ChargBillAppService chargeRecordAppService = new ChargBillAppService();
            ChargeBillListData chargeBillListData = new ChargeBillListData();
            chargeBillListData.Language = Language;
            ChargBillAppService chargBillAppService = new ChargBillAppService();
            chargeBillListData.PageModel = new DailyChargBillDTO() { Remark = StaticResourceHelper.ReceiptRemark };
            chargeBillListData.PayTypeList = chargeRecordAppService.GetPayTypeList().ToList();
            chargeBillListData.ChargBillSearch = new ChargBillSearchDTO()
            {
                IsDevPay = true,
                DeptId = DeptId.HasValue ? DeptId.Value : 0,
                DeptType = DeptType,
                DeptName = DeptName
            };
            int billCount;
            chargeBillListData.ChargeBillList = chargBillAppService.GetDeveloperChargBillDTOList(chargeBillListData.ChargBillSearch, out billCount);
            chargeBillListData.PageModel.AmountShouldTotal = chargeBillListData.ChargeBillList.Where(o => o.RowType != RowTypeEnum.FatherRow).Sum(c => c.AmountShould);
            chargeBillListData.PageModel.AmountShouldAllTotal = chargeBillListData.PageModel.AmountShouldTotal;
            chargeBillListData.PageModel.ReceivedAmountTotal = Math.Ceiling(chargeBillListData.PageModel.AmountShouldTotal);
            chargeBillListData.PageModel.PayTypeId = PayTypeEnum.Cash.GetHashCode();
            chargeBillListData.PageModel.IsSmallToPrepay = false;
            return View(chargeBillListData);
        }

        /// <summary>
        /// 查询日常收费账单
        /// </summary>
        /// <param name="queryParams">查询参数</param>
        /// <returns>返回查询结果</returns>
        [HttpPost]
        public ActionResult GetDeveloperChargeList(ChargBillSearchDTO search)
        {
            ChargeBillListData billData = new ChargeBillListData();
            int outCount = 0;
            ChargBillAppService chargeRecordAppService = new ChargBillAppService();
            billData.ChargeBillList = chargeRecordAppService.GetDeveloperChargBillDTOList(search, out outCount);
            billData.PageModel = new DailyChargBillDTO() { Remark = StaticResourceHelper.ReceiptRemark };
            billData.PageModel.AmountShouldTotal = billData.ChargeBillList.Where(o => o.RowType != RowTypeEnum.FatherRow).Sum(c => c.AmountShould);
            billData.PageModel.ReceivedAmountTotal = Math.Ceiling(billData.PageModel.AmountShouldTotal);
            billData.PageModel.AmountShouldAllTotal = billData.PageModel.AmountShouldTotal;
            billData.PageModel.PayTypeId = PayTypeEnum.Cash.GetHashCode();
            billData.PageModel.IsSmallToPrepay = false;
            return Json(billData);
        }

        #endregion

        #region  对外收费
        /// <summary>
        /// 对外收费index
        /// </summary>
        /// <returns></returns>
        public ActionResult ForeigBillIndex()
        {
            return View();
        }

        public ActionResult ForeigBillChargeList(int? DeptId, EDeptType? DeptType, string DeptName)
        {
            ChargBillAppService chargeRecordAppService = new ChargBillAppService();
            ChargeBillListData chargeBillListData = new ChargeBillListData();
            chargeBillListData.Language = Language;
            ChargBillAppService chargBillAppService = new ChargBillAppService();
            chargeBillListData.PageModel = new DailyChargBillDTO() { Remark = StaticResourceHelper.ReceiptRemark };
            chargeBillListData.PayTypeList = chargeRecordAppService.GetPayTypeList().ToList();
            chargeBillListData.ChargBillSearch = new ChargBillSearchDTO()
            {
                IsDevPay = false,
                SubjectType = (int)SubjectTypeEnum.Foreig,
                DeptId = DeptId.HasValue ? DeptId.Value : 0,
                DeptType = DeptType,
                DeptName = DeptName
            };
            int billCount;
            chargeBillListData.ChargeBillList = chargBillAppService.GetForeigChargBillDTOList(chargeBillListData.ChargBillSearch, out billCount);
            chargeBillListData.PageModel.AmountShouldTotal = chargeBillListData.ChargeBillList.Where(o => o.RowType != RowTypeEnum.FatherRow).Sum(c => c.AmountShould);
            chargeBillListData.PageModel.AmountShouldAllTotal = chargeBillListData.PageModel.AmountShouldTotal;
            chargeBillListData.PageModel.ReceivedAmountTotal = Math.Ceiling(chargeBillListData.PageModel.AmountShouldTotal);
            chargeBillListData.PageModel.PayTypeId = PayTypeEnum.Cash.GetHashCode();
            chargeBillListData.PageModel.IsSmallToPrepay = false;
            return View(chargeBillListData);
        }

        /// <summary>
        /// 查询日常收费账单
        /// </summary>
        /// <param name="queryParams">查询参数</param>
        /// <returns>返回查询结果</returns>
        [HttpPost]
        public ActionResult GetForeigBillChargeList(ChargBillSearchDTO search)
        {
            ChargeBillListData billData = new ChargeBillListData();
            int outCount = 0;
            ChargBillAppService chargeRecordAppService = new ChargBillAppService();
            billData.ChargeBillList = chargeRecordAppService.GetForeigChargBillDTOList(search, out outCount);
            billData.PageModel = new DailyChargBillDTO() { Remark = StaticResourceHelper.ReceiptRemark };
            billData.PageModel.AmountShouldTotal = billData.ChargeBillList.Where(o => o.RowType != RowTypeEnum.FatherRow).Sum(c => c.AmountShould);
            billData.PageModel.ReceivedAmountTotal = Math.Ceiling(billData.PageModel.AmountShouldTotal);
            billData.PageModel.AmountShouldAllTotal = billData.PageModel.AmountShouldTotal;
            billData.PageModel.PayTypeId = PayTypeEnum.Cash.GetHashCode();
            billData.PageModel.IsSmallToPrepay = false;
            return Json(billData);
        }



        //弹出

        public ActionResult ForeigBillChargeViewAdd(int HouseDeptId)
        {
            ChargBillDTO ForeigBillChargeTDO = new ChargBillDTO();
            return CreateForeigView( ForeigBillChargeTDO, HouseDeptId);
        }

        private ActionResult CreateForeigView(ChargBillDTO model, int ComDeptId)
        {
            ChargeSubjectAppService subjectAppService = new ChargeSubjectAppService();
          
            List<ChargeSubjectDTO> subjectList = subjectAppService.GetChargeSubjectsByComDeptId(ComDeptId).Where( o=>o.SubjectType == (int)SubjectTypeEnum.Foreig && o.IsDel == false).ToList();
            ForeigBillChargeViewData ForeigBillChargeViewData = new ForeigBillChargeViewData();
 
            ForeigBillChargeViewData.PageViewModel = model;
            ForeigBillChargeViewData.SubjectList = subjectList;
            return View("ForeigBillChargeView", ForeigBillChargeViewData);
        }

        [HttpPost]
        public ActionResult AddForeigChargeBill(TempChargeDTO model)
        {
            ChargBillAppService billAppService = new ChargBillAppService();
            ResultModel resModel = billAppService.GenerateForeigChargeBill(model.ComVillageDeptId,model.SubjectId, model.BeginDate, model.EndDate, model.Money, model.Remark,model.CustomerName);

            return Json(resModel);
        }

        #region 对外收费

        /// <summary>
        /// 对外收费
        /// </summary>
        [HttpPost]
        public ActionResult PayForeigChargBill(DailyChargBillDTO SaveModel)
        {
            if (SaveModel.ReceivedAmountTotal == 0)
            {
                return Json(new ResultModel() { IsSuccess = false, Msg = "请输入缴费金额" });
            }
            if ((SaveModel.BillIds == null || SaveModel.BillIds.Length < 1)
               && (SaveModel.NewBillList == null || SaveModel.NewBillList.Count() < 1))
            {
                return Json(new ResultModel() { IsSuccess = false, Msg = "请选择账单" });
            }
            if (SaveModel.Remark.Length > 35)
            {
                return Json(new ResultModel() { IsSuccess = false, Msg = "备注长度不能超过35个字符" });
            }
            ChargBillAppService chargBillAppService = new ChargBillAppService();
            PayTypeEnum paytypeEnum = (PayTypeEnum)SaveModel.PayTypeId;
            ResultModel resultModel = chargBillAppService.BillsForeigBillPayment(SaveModel, (int)CurrentAdminUser.Id, CurrentAdminUser.RealName);
            if (resultModel.IsSuccess)
            {
                resultModel.Msg = "缴费成功"+ resultModel.Msg;
                resultModel.PageData = new DailyChargBillDTO();
            }
            else
            {
                resultModel.Msg = "缴费失败:" + resultModel.Msg;
            }
            return Json(resultModel);
        }

        #endregion

        #endregion

        #region 拆分账单

        public ActionResult SplitBillView(ChargBillDTO bill)
        {
            if (bill == null)
            {
                bill = new ChargBillDTO();
            }
            //bill.Remark = "";
            SplitBillViewData viewData = new SplitBillViewData();
            viewData.Language = this.Language;
            viewData.ChargBillInfo = bill;
            ChargBillAppService service = new ChargBillAppService();
            viewData.TemplateModels = service.GetSplitBillViewTemplate();
            return View(viewData);
        }




        [HttpPost]
        public ActionResult SplitBill(ChargBillDTO bill)
        {
            if (!bill.EndDate.HasValue)
            {
                return Json(new ResultModel() { IsSuccess = false, Msg = "账单拆分日期不能为空" });
            }
            var result = GenerateBillAppService.SplitTempBill(bill, this.CurrentAdminUser.Id.Value, this.CurrentAdminUser.RealName);
            return Json(result);
        }

        [HttpPost]
        public ActionResult CheckSplitBill(ChargBillDTO bill)
        {
            if (!bill.EndDate.HasValue)
            {
                return Json(new ResultModel() { IsSuccess = false, Msg = "账单拆分日期不能为空" });
            }
            var result = GenerateBillAppService.CheckSplitTempBill(bill);
            return Json(result);
        }

        #endregion

        #region 作废账单
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        public ActionResult DeleteBillView(List<ChargBillDTO> bill)
        {
            ChargBillDTO ViewModel = new ChargBillDTO();
            if (bill.Count() == 0)
            {
                ViewModel = new ChargBillDTO();
            }
            else
            {
                ViewModel.Remark = "";
                var ChargeSubjectNameList = bill.Select(o => o.ChargeSubjectName).Distinct().ToArray();
                var HouseNoNameList = bill.Select(o => o.HouseDoorNo).Distinct().ToArray();
                ViewModel.HouseDoorNo = string.Join(",", HouseNoNameList);
                ViewModel.Description = string.Join(",", ChargeSubjectNameList);
                ViewModel.BillAmount = bill.Sum(o => o.BillAmount);
               
            }
            
            DeleteBillViewData viewData = new DeleteBillViewData();
            viewData.Language = this.Language;
            viewData.ChargBillInfo = ViewModel;
            viewData.DelteBillList = bill;
            ChargBillAppService service = new ChargBillAppService();
            viewData.TemplateModels = service.GetDeleteBillViewTemplate();
            return View(viewData);
        }

        [HttpPost]
        public ActionResult DeleteBill(ChargBillDTO bill,List<ChargBillDTO> billList,DailyChargBillDTO deleteModel)
        {
            var result = GenerateBillAppService.DeleteBill(bill, billList,deleteModel);
            return Json(result);
        }

        [HttpPost]
        public ActionResult CheckDeleteBill(List<ChargBillDTO> bill, DailyChargBillDTO deleteModel)
        {

            var result = GenerateBillAppService.CheckDeleteBill_new(bill, deleteModel);
            return Json(result);
        }





        #endregion

        #region 刷新账单

        [HttpPost]
        public ActionResult RefreshBill(int houseDeptID)
        {
            var result = GenerateBillAppService.RefreshChargBill(houseDeptID);
            return Json(result);
        }

        #endregion

        #region 预交款

        public ActionResult PrepareAmountView(int houseDeptId, string doorNo)
        {
            PrepareAmountViewData viewData = new PrepareAmountViewData();
            viewData.Language = this.Language;
            //bool isDepPay;
            DateTime dt = DateTime.Today.AddMonths(1);
            DateTime bedate = new DateTime(dt.Year, dt.Month, 1);
            //viewData.MonthPreAmount = PaymentAppService.CalculationMonthPrePayment(houseDeptId, 0);
            //viewData.IsDepPay = isDepPay;
            viewData.MonthPrePaymentList = PaymentAppService.CalculationMonthPrePaymentList(houseDeptId);
            viewData.MonthPreAmount = viewData.MonthPrePaymentList.Where(m => m.SubjectId == 0).First().PreAmount;
            viewData.ChargBillInfo = new ChargBillDTO()
            {
                HouseDeptId = houseDeptId,
                HouseDoorNo = doorNo,
                Months = 1,
                //BeginDate = bedate,
                //EndDate = bedate.AddMonths(1).AddDays(-1),
                PreChargeSubjectId = 0,
                BillAmount = viewData.MonthPreAmount
            };
            //if (isDepPay)
            //{
            //    viewData.ChargBillInfo.Months = null;
            //    viewData.ChargBillInfo.BeginDate = null;
            //    viewData.ChargBillInfo.EndDate = null;
            //    viewData.ChargBillInfo.BillAmount = null;
            //}
            ChargBillAppService service = new ChargBillAppService();
            viewData.TemplateModels = service.GetPrepareAmountViewTemplate(houseDeptId);
            return View(viewData);
        }

        [HttpPost]
        public ActionResult PrepareAmount(ChargBillDTO bill)
        {
            var result = GenerateBillAppService.GenerateTempPrepaymentBill(bill, this.CurrentAdminUser.Id.Value, this.CurrentAdminUser.RealName);
            return Json(result);
        }

        #endregion

        #region 获取业主姓名
        [HttpPost]
        public ActionResult GetOwerName(int HouseDeptId,int DeptTypeId)
        {
            string OwerName = string.Empty;
            var ownerinfo = BillCommonService.Instance.GetOwnerInfoByHosueDeptID(HouseDeptId, DeptTypeId);
            if (ownerinfo != null)
            {
                OwerName = ownerinfo.UserName;
            }


            return Content(OwerName);
        }

        #endregion

        #region 手动生成账单
        public ActionResult ManuallyGenerateBillView(int? houseDeptId)
        {  
            ManuallyGenerateBillViewData viewData = new ManuallyGenerateBillViewData();
            ChargeSubjectAppService service = new ChargeSubjectAppService();
              
            viewData.Language = this.Language;
            viewData.ChargBillInfo = new ChargBillDTO() {  HouseDeptId= houseDeptId };
            ChargeSubjectAppService chargeSubjectAppService = new ChargeSubjectAppService();
         
            return View(viewData);
        }

        [HttpGet]
        public ActionResult GetChargeSubjectList(int? houseDeptId, string DeptName, int DeptType = (int)EDeptType.FangWu)
        {
            var jResult = new JsonResult();
            ChargeSubjectAppService service = new ChargeSubjectAppService();
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            var ChargeSubjectList = service.GetChargeSubjectTreeByDeptId(houseDeptId.Value, DeptType, DeptName);
                //service.GetCycleChargeSubjectListByHouseDeptId(houseDeptId.Value, DeptType)
                //.Select(o => new CustomTreeNodeModel
                //{
                //    id = o.Id.Value.ToString(),
                //    text = o.Name,
                //    children = new List<CustomTreeNodeModel>(),
                //    icon = "fa fa-tags" ,
                //    state =new { selected=true }
                //}).ToList();
            jResult.Data = ChargeSubjectList;
            return jResult;
        }

        [HttpPost]
        public ActionResult ManuallyGenerateBill(int houseDeptId,int DeptType, string[] IdStr, DateTime EndDate)
        {
            if (IdStr.Length == 0)
            {
                return Json(new ResultModel() { IsSuccess = false, Msg = "请选择收费项目"});
            }

            var result = GenerateBillAppService.ManualGenerationHouseBill(houseDeptId, DeptType, IdStr, EndDate);
            return Json(result);
        }

        #endregion

        #region 批量生成账单
        public ActionResult BatchGenerateBillIndex()
        {

            ManuallyGenerateBillViewData viewData = new ManuallyGenerateBillViewData();
            viewData.Language = this.Language;
            return View(viewData);
        }


         [HttpGet]
        public ActionResult GetSubjectSelectChildTree(int id, int type)
        {
            var jResult = new JsonResult();
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jResult.MaxJsonLength = int.MaxValue;
            jResult.Data = GetTree(id, type);
            return jResult;
        }

        /// <summary>
        /// 获取资源树
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<TreeNodeModel> GetTree(int id, int type)
        {
            string strFilter = string.Empty;
            ChargeSubjectDTO subjetcDTO = new ChargeSubjectDTO();
            ChargBillAppService chargBillAppService = new ChargBillAppService();
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            List<TreeNodeModel> list = new List<TreeNodeModel>();
            List<int?> listIds = new List<int?>();
            switch (type)
            {
                case (int)SubjectTypeEnum.House:/*房屋*/
                    list = chargBillAppService.GetBatchGenerateBillHouseTree(id).ToList();
                    break;

               case (int)SubjectTypeEnum.ParkingSpace:/*车位*/
                    list = chargBillAppService.GetBatchGenerateBillCarParkTree(id).ToList();
                    break;

                case (int)SubjectTypeEnum.Meter:/*三表*/
                    list = chargBillAppService.GetBatchGenerateBillMeterTree(id).ToList();
                    break;
            }

            if (list.Count() == 0)
            {
                list.Add(new TreeNodeModel
                {
                    id = "3_" + EDeptType.RootNode.GetHashCode(),
                    text = "没有资源绑定",
                    icon = "fa fa-comment-o",
                    children = new List<TreeNodeModel>(),
                    state = new { disabled = true}
                });
            }
            return list;
        }

        [HttpPost]
        public ActionResult SaveGenerateBillI(string[] Nodes,DateTime EndDate,int ChargeSubjectId)
        {
            if (Nodes.Length == 0)
            {
                return Json(new ResultModel() { IsSuccess = false, Msg = "请选择资源后提交" });
            }
            var nodelist = Nodes.Where(o => !o.Contains("_root")).ToList();
            List<int?> saveStrlist = nodelist.ConvertAll<int?>(i=>int.Parse(i));

             
            ChargeSubjectAppService _ChargeSubjectAppService = new ChargeSubjectAppService();
            ChargeSubjectDTO chargeSubjectDTO = _ChargeSubjectAppService.GetChargeSubjectByKey(ChargeSubjectId);
            var result = GenerateBillAppService.ManualBatchGenerationByComDeptId(chargeSubjectDTO.ComDeptId.Value, ChargeSubjectId, saveStrlist.ToArray(), EndDate);
            return Json(result);


        }

        #endregion

        #region 生成缴费通知单
        public ActionResult GeneratePaymentNoticeIndex()
        {
            ManuallyGenerateBillViewData viewData = new ManuallyGenerateBillViewData();
            viewData.Language = this.Language;
            return View(viewData);
        }
        [HttpGet]
        public ActionResult GetPayNotSelectChildTree(int id)
        {
            var jResult = new JsonResult();
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            string strFilter = string.Empty;
            ChargBillAppService chargBillAppService = new ChargBillAppService();
            List<CustomTreeNodeModel> list = new List<CustomTreeNodeModel>();
            list = chargBillAppService.GetGeneratePaymentNoticeHouseTree(id).ToList();

            if (list.Count() == 0)
            {
                list.Add(new CustomTreeNodeModel
                {
                    id = "3_" + EDeptType.RootNode.GetHashCode(),
                    text = "没有资源绑定",
                    icon = "fa fa-comment-o",
                    children = new List<CustomTreeNodeModel>(),
                    state = new { disabled = true }
                });
            }

            jResult.Data = list;
            return jResult;
        }



        [HttpGet]
        public ActionResult GetPayNotSelectChildTreeAysn(int id)
        {
            var jResult = new JsonResult();
            jResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jResult.MaxJsonLength = Int32.MaxValue;
            ChargBillAppService chargBillAppService = new ChargBillAppService();
            jResult.Data = chargBillAppService.GetGeneratePaymentNoticeHouseTreeAysn(id);
          
            return jResult;
        }



       [HttpPost]
        public ActionResult SavePaymentNotice(string[] Nodes, DateTime EndDate, string Remarks, int ComDeptId)
        {
            DeptAppService _DeptAppService = new DeptAppService();

            if (Nodes.Length == 0)
            {
                return Json(new ResultModel() { IsSuccess = false, Msg = "请选择资源后提交" });
            }
            List<int?> saveStrlist = new List<int?>();
            var ComNodeStrList = Nodes.Where(o => o.Contains("_Comroot")).ToList();

            if(ComNodeStrList.Count>0)
            {
                //获取该小区所有房屋

                saveStrlist= _DeptAppService.GetDeptHouseList(ComDeptId.ToString()).Select(o=>o.Id).ToList();

            }
            else
            {
                //所有非根节点勾选
                var nodelist = Nodes.Where(o => !o.Contains("_root") && !o.Contains("_Comroot")).ToList();
                saveStrlist = nodelist.ConvertAll<int?>(i => int.Parse(i));


                //根节点
                //获取楼栋根节点
                var LouyuNodeList = Nodes.Where(o => o.Contains("_root")).ToList();
                List<int?> LouyuNodeIntList = new List<int?>();
                foreach (var c in LouyuNodeList)
                {
                    LouyuNodeIntList.Add(Convert.ToInt32(c.Replace("_root", "")));
                }

                var LouyuDeptIds=  _DeptAppService.GetDeptHouseList(LouyuNodeIntList).Select(o=>o.Id).ToList();

                saveStrlist.AddRange(LouyuDeptIds);
                saveStrlist.Distinct();

            }

            int everyBillCount = 0;
            int pageReceipt = 0;
            int footSize = 0;
            if (saveStrlist.Count > 3000)
            {
               
                    return Json(new ResultModel() { IsSuccess = false, Msg = "选择资源数目超过3000条，请减少勾选数量，当前勾选数量："+saveStrlist.Count });
              
            }
            ChargBillAppService billAppService = new ChargBillAppService();
            List<PrintDataModel> dataList = billAppService.GetPrintDataModel(ComDeptId, saveStrlist, EndDate, Remarks, ref everyBillCount, ref pageReceipt, ref footSize);
            byte[] byteOutputStream = PrintHelper.GetPdfFileStream(dataList, everyBillCount, pageReceipt, footSize);
            if (byteOutputStream.Length <= 0)
                return Json(new ResultModel() { IsSuccess = false, Msg = "没有需要缴费的资源" });
            else
                TempData["PrintDataModel"] = byteOutputStream;

            return Json(new ResultModel() { IsSuccess = true, Msg = "数据获取成功" });
        }
        public ActionResult PaymentNoticePrintPDF()
        {
            
            try
            {
                byte[] byteOutputStream = TempData["PrintDataModel"] as byte[];
                Response.AddHeader("Content-Length", byteOutputStream.Length.ToString());
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "inline;FileName=out.pdf");
                Response.BinaryWrite(byteOutputStream);
                Response.OutputStream.Flush();
                Response.OutputStream.Close();
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile("PaymentNoticePrintPDF 异常：" + ex, "Controllers/ChargBillController", FileLogType.Exception);
            }

            return View();
        }

        #endregion

        #region 作废账单查询页面
        public ActionResult DeleteChargBillList()
        {
            ChargeBillListData chargeBillListData = new ChargeBillListData();
            ChargBillAppService chargBillAppService = new ChargBillAppService();
            ChargeSubjectAppService chargeSubjectAppService = new ChargeSubjectAppService();
            chargeBillListData.Language = Language;
            var templateModels = chargBillAppService.GetDeleteChargBillListTemplate();
            chargeBillListData.TemplateModels = templateModels;
            return View(chargeBillListData);
        }

        /// <summary>
        /// 查询作废账单
        /// </summary>
        [HttpPost]
        public ActionResult GetDeleteChargeBillList(ChargBillSearchDTO search)
        {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
            int outCount = 0;
            ChargBillAppService chargBillAppService = new ChargBillAppService();
            IList<ChargBillDTO> dataList = chargBillAppService.GetDeleteChargBillDTOList(search, out outCount);
            SearchResultData<ChargBillDTO> queryResult = new SearchResultData<ChargBillDTO>()
            {
                draw = search.Draw,
                recordsFiltered = outCount,
                recordsTotal = outCount,
                data = dataList
            };
            return Json(queryResult);
        }

        [HttpPost]
        public ActionResult GetComChargeSubjectList(int? ComDeptId)
        {

            if (ComDeptId.HasValue)
            {
                ChargeSubjectAppService chargeSubjectAppService = new ChargeSubjectAppService();
                List<ChargeSubjectDTO> chargesubjectlist = new List<ChargeSubjectDTO>();
                chargesubjectlist.Add(new ChargeSubjectDTO() { Id = 0, Name = "-----全选-----" });
                chargesubjectlist.AddRange(chargeSubjectAppService.GetChargeSubjectsByComDeptId(ComDeptId.Value).Where(o=>o.SubjectType!=(int)SubjectTypeEnum.SystemPreset&&o.SubjectType!=(int)SubjectTypeEnum.Foreig&&o.BillPeriod!=(int)BillPeriodEnum.Once));
                return Json(new ResultModel() { IsSuccess = true, Data = chargesubjectlist });

            }
            else
            {
                return Json(new ResultModel() { IsSuccess = true, Data=new object() });
            }
            //ResultModel resultModel = PaymentAppService.PaymentTasksRviewed((int)PageModel.Id, (int)CurrentAdminUser.Id, CurrentAdminUser.RealName, PageModel.CheckRemark);
            //return Json(resultModel);
        }


        #endregion

        #region 收费确认框 2017-9-8

        public ActionResult ConfirmView(bool isPrintReceipt, int? deptId, EDeptType? deptType)
        {
            deptId = deptId ?? 0;
            deptType = deptType ?? 0;
            string receiptNo = string.Empty;
            if (isPrintReceipt)
            {
                receiptNo = PaymentAppService.GenerateReceiptBookNumber(deptId.Value, deptType.Value);
            }
           
            return View(receiptNo);
        }

        [HttpPost]
        public ActionResult GetReceiptBookNumber(int? deptId, EDeptType? deptType)
        {
            deptId = deptId ?? 0;
            deptType = deptType ?? 0;
            string receiptNo = PaymentAppService.GenerateReceiptBookNumber(deptId.Value, deptType.Value);
            return Json(receiptNo);
        }

        #endregion


    }

    public class SplitBillViewData
    {
        public string Language { get; set; }

        public ChargBillDTO ChargBillInfo { get; set; }
        public IEnumerable<TemplateModel> TemplateModels { get; set; }
    }


    public class DeleteBillViewData
    {
        public string Language { get; set; }
        public List<ChargBillDTO> DelteBillList { get; set; }
        public ChargBillDTO ChargBillInfo { get; set; }
        public IEnumerable<TemplateModel> TemplateModels { get; set; }
    }

    public class ManuallyGenerateBillViewData
    {
        public string Language { get; set; }

        public ChargBillDTO ChargBillInfo { get; set; }

        public  IList<TreeNodeModel> ChargeSubjectList { get; set; }
    
    }

    public class PrepareAmountViewData
    {
        public string Language { get; set; }

        public ChargBillDTO ChargBillInfo { get; set; }
        public IEnumerable<TemplateModel> TemplateModels { get; set; }

        public decimal? MonthPreAmount { get; set; }

        public bool IsDepPay { get; set; }

        public IList<SubjectMonthPrePaymentDTO> MonthPrePaymentList { get; set; }
    }


    public class ForeigBillChargeViewData
    {

        public string Language { get; set; }
        public ChargBillDTO PageViewModel { get; set; }

        public List<ChargeSubjectDTO> SubjectList { get; set; }
    }

    public class ChargBillPageData
    {
        public bool IsCarPark { get; set; }
    }

}