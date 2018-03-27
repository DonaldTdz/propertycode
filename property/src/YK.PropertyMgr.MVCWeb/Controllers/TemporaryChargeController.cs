using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.ApplicationDTO.Resources;
using YK.PropertyMgr.ApplicationService;

namespace YK.PropertyMgr.MVCWeb.Controllers
{
    public class TemporaryChargeController : BaseController
    {
        #region Index
        public ActionResult Index(int? DeptId, EDeptType? DeptType, string DeptName, bool IsCarPark = false)
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
                DeptId = DeptId.HasValue ? DeptId.Value : 0,
                DeptType = DeptType,
                DeptName = DeptName
            };
            int billCount;
            chargeBillListData.ChargeBillList = chargBillAppService.GetTempChargBillDTOList(chargeBillListData.ChargBillSearch, out billCount);
            chargeBillListData.PageModel.AmountShouldTotal = chargeBillListData.ChargeBillList.Sum(c => c.AmountShould);
            chargeBillListData.PageModel.AmountShouldAllTotal = chargeBillListData.PageModel.AmountShouldTotal;
            chargeBillListData.PageModel.ReceivedAmountTotal = Math.Ceiling(chargeBillListData.PageModel.AmountShouldTotal);
            chargeBillListData.PageModel.PayTypeId = PayTypeEnum.Cash.GetHashCode();
            chargeBillListData.IsCarPark = IsCarPark;
            chargeBillListData.PageModel.IsSmallToPrepay = true;//!IsCarPark;
            chargeBillListData.ChargeSubjectList = DeptId==null?new List<ChargeSubjectDTO>():chargBillAppService.GetHouseSubjectList(DeptId.Value, IsCarPark, DeptType, true).ToList();
            return View(chargeBillListData);
        }

        #endregion

        #region 查询账单信息信息

        /// <summary>
        /// 查询日常收费账单
        /// </summary>
        /// <param name="queryParams">查询参数</param>
        /// <returns>返回查询结果</returns>
        [HttpPost]
        public ActionResult GetTempChargList(ChargBillSearchDTO search)
        {
            ChargeBillListData billData = new ChargeBillListData();
            int outCount = 0;
            ChargBillAppService chargeRecordAppService = new ChargBillAppService();
            billData.ChargeBillList = chargeRecordAppService.GetTempChargBillDTOList(search, out outCount);
            billData.PageModel = new DailyChargBillDTO() { Remark = StaticResourceHelper.ReceiptRemark };
            billData.PageModel.AmountShouldTotal = billData.ChargeBillList.Sum(c => c.AmountShould);
            billData.PageModel.ReceivedAmountTotal = Math.Ceiling(billData.PageModel.AmountShouldTotal);
            billData.PageModel.AmountShouldAllTotal = billData.PageModel.AmountShouldTotal;
            billData.PageModel.PayTypeId = PayTypeEnum.Cash.GetHashCode();
            billData.PageModel.IsSmallToPrepay = true; //!search.IsCarPark;
            billData.ChargeSubjectList = chargeRecordAppService.GetHouseSubjectList(search.DeptId, search.IsCarPark, search.DeptType, true).ToList();
            return Json(billData);
        }

        #endregion

        #region 临时收费

        /// <summary>
        /// 临时缴费
        /// </summary>
        [HttpPost]
        public ActionResult PayTempChargBill(DailyChargBillDTO SaveModel)
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
            ResultModel resultModel = chargBillAppService.PayBillsTemporary(SaveModel, (int)CurrentAdminUser.Id, CurrentAdminUser.RealName);
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

        #region 窗口视图处理
        /// <summary>
        /// 新增临时账单信息
        /// </summary>
        /// <param name="HouseDeptId">房屋DEPTID</param>
        /// <returns></returns>
        public ActionResult TempChargeViewAdd(int HouseDeptId,int RefType)
        {
            TempChargeDTO tempChargeTDO = new TempChargeDTO();
            return CreateView("Add", tempChargeTDO, HouseDeptId, RefType);
        }

        private ActionResult CreateView(string viewType, TempChargeDTO model, int HouseDeptId,int RefType)
        {
            int ComDeptId = 0;
            List<ChargeSubjectDTO> subjectList = new List<ChargeSubjectDTO>();
            ChargeSubjectAppService subjectAppService = new ChargeSubjectAppService();
            model.ResourcesId = HouseDeptId; 
            switch (RefType)
            {
                case (int)EDeptType.FangWu:
                    ComDeptId = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityDeptIdByHouseDeptId(HouseDeptId);
                    subjectList = subjectAppService.GetChargeSubjectsByComDeptId(ComDeptId).Where(o => o.BillPeriod == (int)BillPeriodEnum.Once && o.SubjectType != (int)SubjectTypeEnum.SystemPreset && o.IsDel == false && o.SubjectType != (int)SubjectTypeEnum.Foreig).ToList();
                    model.RefType = (int)ReourceTypeEnum.House;
    
                    break;
                case (int)EDeptType.CheWei:
                    ComDeptId = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityDeptIdByCarPortId(HouseDeptId);
                    subjectList = subjectAppService.GetChargeSubjectsByComDeptId(ComDeptId).Where(o => o.BillPeriod == (int)BillPeriodEnum.Once && o.SubjectType == (int)SubjectTypeEnum.ParkingSpace && o.IsDel == false && o.SubjectType != (int)SubjectTypeEnum.Foreig).ToList();
                    model.RefType =(int) ReourceTypeEnum.CarPark;
                
                    break;


            }



            
             
            TempChargeViewData tempChargeViewData = new TempChargeViewData();
            tempChargeViewData.ViewType = viewType;
            tempChargeViewData.PageViewModel = model;
            tempChargeViewData.SubjectList = subjectList;
            return View("TempChargeView", tempChargeViewData);
        }
        [HttpPost]
        public ActionResult AddTemporaryChargeBill(TempChargeDTO model)
        {
             ChargBillAppService billAppService = new ChargBillAppService();
        
            ResultModel resModel = new ResultModel();
            if (model.RefType == (int)ReourceTypeEnum.House)
            {
                resModel = billAppService.GenerateTemporaryBill(model.ComVillageDeptId, model.HouseDeptId, model.SubjectId, model.BeginDate, model.EndDate, model.Money, "一次性收费");
            }
            else if(model.RefType == (int)ReourceTypeEnum.CarPark)
            {
                CarParkAppService carParkAppService = new CarParkAppService();
                int HouseDeptId = carParkAppService.GetHouseDeptIdByCarPort(model.ResourcesId);
                resModel = billAppService.GenerateTemporaryBill(model.ComVillageDeptId, HouseDeptId, model.ResourcesId, model.SubjectId, model.BeginDate, model.EndDate, model.Money, "一次性收费", model.RefType);
            }
            return Json(resModel);
        }

        public class TempChargeViewData
        {
            public string ViewType { get; set; }

            public TempChargeDTO PageViewModel { get; set; }

            public List<ChargeSubjectDTO> SubjectList { get; set; }
        }

        #endregion


        #region 计算科目的金额
        [HttpPost]
        public ActionResult ComputeChargeSubjectAmount(DailyChargBillDTO Model)
        {
            if (Model.ChargeSubjectId <= 0)
            {
                return Json(new ResultModel() { IsSuccess = false, Msg = "未选择科目" });
            }

            if (Model.HouseDeptId <= 0)
            {
                return Json(new ResultModel() { IsSuccess = false, Msg = "未选择资源" });
            }
            

            ChargBillAppService chargBillAppService = new ChargBillAppService();
            var Money = chargBillAppService.ComputeChargeSubjectAmount(Model.ChargeSubjectId, Model.HouseDeptId,Model.RefType);

            return Json(new ResultModel() { IsSuccess = true, Data= Money });
        }
        #endregion


    }
}