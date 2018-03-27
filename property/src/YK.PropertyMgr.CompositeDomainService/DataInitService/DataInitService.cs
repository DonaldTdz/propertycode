using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.RepositoryContract;
using Microsoft.Practices.Unity;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.CompositeDomainService.GenerateBillService;
using YK.BackgroundMgr.PresentationService;
using YK.BackgroundMgr.DomainInterface;
using YK.PropertyMgr.ApplicationDTO.Enums;

namespace YK.PropertyMgr.CompositeDomainService
{
    public class DataInitService : IDataInitService
    {
        #region 单例

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static DataInitService Instance { get { return SingletonInstance; } }
        private static readonly DataInitService SingletonInstance = new DataInitService();

        #endregion

        #region 导入账单信息

        public ResultModel BaseValidation(int ComDeptId, IPropertyMgrUnitOfWork propertyMgrUnitOfWork, BaseImportBillInfo info, IList<BaseImportBillInfo> BaseInfoList)
        {
            //1.基本验证信息
            if (info.Amount <= 0)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "710", Msg = "金额必须大于零" };
            }
            if (BaseInfoList.Where(a => a != info
            && a.ResourceNo == info.ResourceNo
            && a.SubjectName == info.SubjectName //当时同一个收费项目的时候 2017-2-24
            && ((info.BeginDate <= a.BeginDate && a.BeginDate <= info.EndDate)
                || (info.BeginDate <= a.EndDate && a.EndDate <= info.EndDate)
                ||
                (a.BeginDate <= info.BeginDate && info.BeginDate <= a.EndDate)
                || (a.BeginDate <= info.EndDate && info.EndDate <= a.EndDate))
                ).Count() > 0)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "720", Msg = "列表账单开始结束时间有重叠" };
            }

            //2.验证和获取收费项目信息
            var subject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll().Where(p => p.ComDeptId == ComDeptId && p.Name == info.SubjectName).FirstOrDefault();
            if (subject == null)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "730", Msg = "该小区收费项目不存在" };
            }
            //v2.7 屏蔽验证收费项目开始时间 2017-06-14
            //if (info.EndDate >= subject.BeginDate)
            //{
            //    return new ResultModel() { IsSuccess = false, ErrorCode = "740", Msg = "账单结束日必须小于收费项目计费开始日" };
            //}

            SubjectTypeEnum SubjectType = (SubjectTypeEnum)subject.SubjectType;
            if (SubjectType == SubjectTypeEnum.SystemPreset || SubjectType == SubjectTypeEnum.Other)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "750", Msg = "改收费项目不支持导入" };
            }

            //判断是否包含资源类型 如果不包含 
            if (SubjectType != SubjectTypeEnum.ParkingSpace && !SubjectResourceType[SubjectType].Contains(info.ResourceTypeName))
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "760", Msg = "收费项目与资源类型不匹配" };
            }

            if (info.ChargeType == ChargeTypeEnum.ForeignCharge && SubjectType != SubjectTypeEnum.Foreig)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "770", Msg = "收费项目与收费类型不匹配" };
            }

            //验证数据库已存在账单时间重叠 2017-2-24
            if (propertyMgrUnitOfWork.ChargBillRepository.GetAll().Any(b => b.ComDeptId == ComDeptId
            && b.ResourcesName == info.ResourceNo//资源
            && b.Description == subject.Name//收费项目名称
            && b.IsDel == false    //排除作废账单
            && ((info.BeginDate <= b.BeginDate && b.BeginDate <= info.EndDate)
                || (info.BeginDate <= b.EndDate && b.EndDate <= info.EndDate)
                ||
                (b.BeginDate <= info.BeginDate && info.BeginDate <= b.EndDate)
                || (b.BeginDate <= info.EndDate && info.EndDate <= b.EndDate)
                )
            ))
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "745", Msg = "数据库账单开始结束时间有重叠" };
            }

            CalculateProperty calculateProperty = new CalculateProperty();
            calculateProperty.ComDeptId = ComDeptId;
            //3.验证和获取资源信息
            switch (SubjectType)
            {
                case SubjectTypeEnum.House:
                    {
                        var houseInfo = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseInfoByHouseNo(ComDeptId, info.ResourceNo);
                        if (houseInfo == null)
                        {
                            return new ResultModel() { IsSuccess = false, ErrorCode = "770", Msg = "没找到" + info.ResourceTypeName + "资源" };
                        }
                        calculateProperty.HouseDeptID = houseInfo.HouseDeptID;
                        calculateProperty.ResourcesId = houseInfo.HouseDeptID.Value;
                        calculateProperty.ResourcesName = houseInfo.DoorNo;
                        //添加： 2017-06-29
                        calculateProperty.HouseDoorNo = houseInfo.DoorNo;
                    }
                    break;
                case SubjectTypeEnum.ParkingSpace:
                    {
                        var spaceInfo = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetParkingSpaceInfoBySpaceNo(ComDeptId, info.ResourceTypeName, info.ResourceNo);
                        if (spaceInfo == null)
                        {
                            return new ResultModel() { IsSuccess = false, ErrorCode = "780", Msg = "没找到" + info.ResourceTypeName + "资源" };
                        }
                        calculateProperty.HouseDeptID = spaceInfo.HouseDeptID;
                        calculateProperty.ResourcesId = spaceInfo.ParkingSpaceId.Value;
                        calculateProperty.ResourcesName = spaceInfo.CarportNum;
                        //添加： 2017-06-29
                        if (spaceInfo.HouseDeptID.HasValue)
                        {
                            var houseInfo = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseInfo(ComDeptId, spaceInfo.HouseDeptID.Value);
                            if (houseInfo != null)
                            {
                                calculateProperty.HouseDoorNo = houseInfo.DoorNo;
                            }
                        }
                        
                    }
                    break;
                case SubjectTypeEnum.Meter:
                    {
                        MeterTypeEnum meterType = (info.ResourceTypeName == "水表" ? MeterTypeEnum.WaterMeter
                            : info.ResourceTypeName == "电表" ? MeterTypeEnum.WattHourMeter : MeterTypeEnum.GasMeter);
                        var meterInfo = GetMeterInfo(ComDeptId, meterType, info.ResourceNo);
                        if (meterInfo == null)
                        {
                            return new ResultModel() { IsSuccess = false, ErrorCode = "790", Msg = "没找到" + info.ResourceTypeName + "资源" };
                        }
                        calculateProperty.HouseDeptID = meterInfo.HouseDeptID;
                        calculateProperty.ResourcesId = meterInfo.Id.Value;
                        calculateProperty.ResourcesName = meterInfo.MeterNum;
                        //添加： 2017-06-29
                        if (meterInfo.HouseDeptID.HasValue)
                        {
                            var houseInfo = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseInfo(ComDeptId, meterInfo.HouseDeptID.Value);
                            if (houseInfo != null)
                            {
                                calculateProperty.HouseDoorNo = houseInfo.DoorNo;
                            }
                        }
                    }
                    break;
                case SubjectTypeEnum.Foreig://添加对外收费2017-3-17
                    {
                        //资源与小区不匹配
                        var comInfo = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetComDeptInfoByName(info.ResourceNo);
                        if (comInfo == null || comInfo.Id != ComDeptId)
                        {
                            return new ResultModel() { IsSuccess = false, ErrorCode = "791", Msg = "对外收费小区名不存在或与收费项目不匹配" };
                        }

                        calculateProperty.HouseDeptID = 0;
                        calculateProperty.ResourcesId = comInfo.Id.Value;
                        calculateProperty.ResourcesName = comInfo.Name;
                    }
                    break;
            }
            return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "", Data = calculateProperty, PageData = subject };
        }

        /// <summary>
        /// 生成收费流水
        /// </summary>
        /// <param name="propertyMgrUnitOfWork">工作单元</param>
        /// <param name="chargBill">账单</param>
        /// <param name="ChargeRecordId">收费流水ID</param>
        private void GenerateChargeRecord(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, ChargBill chargBill, string ChargeRecordId, BaseImportBillInfo info)
        {
            //如果已划转金额为0，不需要生成流水
            if (chargBill.ReceivedAmount == 0)
            {
                return;
            }
            //生成单据
            var receipt = BillCommonService.Instance.GenerateReceipt(propertyMgrUnitOfWork, info.ReceiptNo, -1, info.OperatorName
                , (info.ChargeType == ChargeTypeEnum.Refund? ReceiptStatusEnum.Refunded : ReceiptStatusEnum.Paid), info.Remark);
            IList<ChargBill> chargBillList = new List<ChargBill>();
            chargBillList.Add(chargBill);
            Dictionary<string, decimal> amountD = new Dictionary<string, decimal>();
            amountD.Add(chargBill.Id, chargBill.ReceivedAmount.Value);
            //生成流水
            BillCommonService.Instance.GenerateChargeRecord(propertyMgrUnitOfWork,
                    chargBillList,
                    info.ChargeType,
                    info.PayType,
                    receipt.Id,                         //预付款划转默认收据号为空
                    chargBill.ReceivedAmount.Value,   //已收金额
                    info.Remark,
                    amountD,
                    ChargeRecordId,              //费用记录ID
                    -1,
                    info.OperatorName,
                    info.ChargeType == ChargeTypeEnum.ForeignCharge? info.CustomerName:"",//对外收费传入客户
                    false,
                    info.PayDate
                );
        }


        /// <summary>
        /// 生成收费项目下 单个账单
        /// </summary>
        /// <param name="propertyMgrUnitOfWork">工作单元</param>
        /// <param name="chargeSubject">收费项目</param>
        /// <param name="ditem">收费周期</param>
        /// <param name="calculateProperty">计算属性</param>
        /// <param name="subjectHouseItem">账号绑定信息</param>
        ///  <returns>账单ID</returns>
        private ChargBill GenerateSingleChargBill(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, BaseImportBillInfo info, ChargeSubject chargeSubject, CalculateProperty calculateProperty)
        {
            //账单周期
            BillPeriodEnum billPeriod = (BillPeriodEnum)chargeSubject.BillPeriod;
            //本次生成账单
            ChargBill chargBill = new ChargBill();
            chargBill.ChargeSubjectId = chargeSubject.Id;
            chargBill.CreateTime = DateTime.Now;
            chargBill.IsDel = false;
            chargBill.Description = chargeSubject.Name;
            chargBill.Id = Guid.NewGuid().ToString();
            chargBill.ComDeptId = calculateProperty.ComDeptId;
            calculateProperty.HouseDeptID = calculateProperty.HouseDeptID ?? 0;
            chargBill.HouseDeptId = calculateProperty.HouseDeptID;
            chargBill.ResourcesId = calculateProperty.ResourcesId;
            chargBill.Remark = "[导入账单]" + info.Remark;
            chargBill.RefType = chargeSubject.SubjectType;
            chargBill.UpdateTime = DateTime.Now;
            //添加result name 2017-2-23
            chargBill.ResourcesName = calculateProperty.ResourcesName;
            //添加： 2017-06-29
            chargBill.HouseDoorNo = calculateProperty.HouseDoorNo;

            //开发商代缴
            chargBill.IsDevPay = (info.IsDeveloper == "是");
            chargBill.BeginDate = info.BeginDate;
            chargBill.EndDate = info.EndDate;
            chargBill.BillAmount = info.Amount;
            chargBill.Quantity = string.Empty;
            //减免金额 目前页面控制
            chargBill.ReliefAmount = 0;
            //滞纳金额 暂时为0 未处理
            chargBill.PenaltyAmount = chargeSubject.PenaltyRate * 0;
            //生成账户流水ID
            string ChargeRecordId = Guid.NewGuid().ToString();

            if (info.ImportType == ImportDataType.HistoryCost)
            {
                chargBill.ReceivedAmount = info.Amount;
                chargBill.Status = info.ChargeType == ChargeTypeEnum.Refund? BillStatusEnum.Refunded.GetHashCode():BillStatusEnum.Paid.GetHashCode();
                //生成费用流水
                GenerateChargeRecord(propertyMgrUnitOfWork, chargBill, ChargeRecordId, info);
            }
            else
            {
                //应划款金额=计费金额+滞纳金-减免金额
                decimal deductionAmount = chargBill.BillAmount.Value + chargBill.PenaltyAmount.Value - chargBill.ReliefAmount.Value;
                chargBill.ReceivedAmount = 0;
                if (deductionAmount <= 0)
                {
                    chargBill.Status = BillStatusEnum.Paid.GetHashCode();
                }
                else
                {
                    chargBill.Status = BillStatusEnum.NoPayment.GetHashCode();
                }
            }

            //生成账单
            propertyMgrUnitOfWork.ChargBillRepository.Add(chargBill);
            //生成快照
            BillCommonService.Instance.GenerateChargeSubjectSna(propertyMgrUnitOfWork, chargBill, chargeSubject);
            return chargBill;
        }

        #endregion

        #region 欠费信息导入

        private Dictionary<SubjectTypeEnum, string[]> SubjectResourceType
        {
            get
            {
                Dictionary<SubjectTypeEnum, string[]> SubjectResourceType = new Dictionary<SubjectTypeEnum, string[]>();
                SubjectResourceType.Add(SubjectTypeEnum.House, new string[] { "房屋" });
                SubjectResourceType.Add(SubjectTypeEnum.Meter, new string[] { "水表", "电表", "气表" });
                SubjectResourceType.Add(SubjectTypeEnum.Foreig, new string[] { "小区"});
                return SubjectResourceType;
            }
        }

        private Meter GetMeterInfo(int ComDeptId, MeterTypeEnum MeterType, string MeterNum)
        {
            int?[] houseIds = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseListByCommunityDeptId(ComDeptId).Select(c => c.HouseDeptID).ToArray();
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = propertyMgrUnitOfWork.MeterRepository.GetAll().Where(m => m.MeterType == (int)MeterType
                && m.MeterNum == MeterNum && houseIds.Contains(m.HouseDeptID));
                return query.FirstOrDefault();
            }
        }

        private ResultModel ImportSingleArrearage(int ComDeptId, ArrearageInfo info, IList<ArrearageInfo> ArrearageInfoList, int Operator, string OperatorName)
        {
            try
            {
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    var result = BaseValidation(ComDeptId, propertyMgrUnitOfWork, info, ArrearageInfoList.ToList<BaseImportBillInfo>());
                    if (!result.IsSuccess)
                    {
                        return result;
                    }
                    info.ImportType = ImportDataType.Arrearage;
                    //生成账单
                    ChargBill bill = GenerateSingleChargBill(propertyMgrUnitOfWork, info, (ChargeSubject)result.PageData, (CalculateProperty)result.Data);
                    //var count = propertyMgrUnitOfWork.ChargBillRepository.GetAll()
                    //    .Where(p => p.ResourcesId == bill.ResourcesId
                    //    && p.ChargeSubjectId == bill.ChargeSubjectId
                    //    && ((info.BeginDate <= p.BeginDate && p.BeginDate <= info.EndDate)
                    //    || (info.BeginDate <= p.EndDate && p.EndDate <= info.EndDate))).Count();
                    //if (count > 0)
                    //{
                    //    return new ResultModel() { IsSuccess = false, ErrorCode = "790", Msg = "账单开始结束时间和已有账单有重叠" };
                    //}
                    propertyMgrUnitOfWork.Commit();
                    return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "导入信息成功" };
                }
            }
            catch (Exception ex)
            {
                //记录日志
                LogProperty.WriteLoginToFile(string.Format("[欠费导入]Code:900 ResourceNo:{0} Amount:{1} ErrorMsg:{2}", info.ResourceNo, info.Amount, ex.Message), "ImportSingleArrearage", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "欠费导入失败" };
            }
        }

        /// <summary>
        /// 导入欠费信息
        /// </summary>
        /// <returns>ResultModel</returns>
        public ResultModel ImportArrearage(int ComDeptId, IList<ArrearageInfo> ArrearageInfoList, int Operator, string OperatorName)
        {
            if (ArrearageInfoList.Count() < 1)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "970", Msg = "导入数据不能为空" };
            }

            IList<ArrearageInfo> errorList = new List<ArrearageInfo>();
            foreach (var item in ArrearageInfoList)
            {
                ResultModel result = ImportSingleArrearage(ComDeptId, item, ArrearageInfoList, Operator, OperatorName);
                if (!result.IsSuccess)
                {
                    item.ErrorMsg = result.Msg;
                    errorList.Add(item);
                }
            }
            if (errorList.Count() > 0)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "980", Msg = "欠费导入出现错误", Data = errorList };
            }
            return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "欠费导入成功" };
        }

        #endregion

        #region 历史收费信息导入

        public ResultModel HistoryCostValidation(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, BaseImportBillInfo info, IList<BaseImportBillInfo> BaseInfoList)
        {
            //交易时间必须小于等于当前
            if (info.PayDate > DateTime.Now)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "810", Msg = "交易时间必须小于等于当前" };
            }
            //收费类型验证
            if (info.ChargeTypeName != "日常收费"
                && info.ChargeTypeName != "临时收费"
                && info.ChargeTypeName != "退款"
                && info.ChargeTypeName != "对外收费")
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "820", Msg = "收费类型不匹配或不支持" };
            }

            //支付方式验证
            if (info.PayTypeName != "支付宝"
                && info.PayTypeName != "微信"
                && info.PayTypeName != "现金")
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "820", Msg = "支付方式不匹配或不支持" };
            }
            //票据号可以为空 不为空时验证
            if (!string.IsNullOrEmpty(info.ReceiptNo))
            {
                //单据号重复验证
                if (info.ChargeType != ChargeTypeEnum.Refund)//退款可重复
                {
                    if (BaseInfoList.Where(b => b.RowNum != info.RowNum && b.ReceiptNo == info.ReceiptNo).Count() > 0)
                    {
                        return new ResultModel() { IsSuccess = false, ErrorCode = "841", Msg = "表格票据号存在重复" };
                    }
                    var count = propertyMgrUnitOfWork.ReceiptRepository.GetAll().Where(r => r.Number == info.ReceiptNo).Count();
                    if (count > 0)
                    {
                        return new ResultModel() { IsSuccess = false, ErrorCode = "842", Msg = "数据库票据号存在重复" };
                    }
                }
                else
                {
                    if (BaseInfoList.Where(b => b.RowNum != info.RowNum && b.ReceiptNo == info.ReceiptNo).Count() > 1)
                    {
                        return new ResultModel() { IsSuccess = false, ErrorCode = "843", Msg = "表格票据号存在重复" };
                    }
                }
            }

            //对外收费 客户为必填
            if (info.ChargeType == ChargeTypeEnum.ForeignCharge && string.IsNullOrEmpty(info.CustomerName))
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "844", Msg = "对外收费客户为必填" };
            }

            return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "" };
        }

        private ResultModel ImportSingleHistoryCost(int ComDeptId, HistoryCostInfo info, IList<HistoryCostInfo> HistoryCostInfoList, int Operator, string OperatorName)
        {
            try
            {
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    var result = BaseValidation(ComDeptId, propertyMgrUnitOfWork, info, HistoryCostInfoList.ToList<BaseImportBillInfo>());
                    if (!result.IsSuccess)
                    {
                        return result;
                    }
                    var hcResult = HistoryCostValidation(propertyMgrUnitOfWork, info, HistoryCostInfoList.ToList<BaseImportBillInfo>());
                    if (!hcResult.IsSuccess)
                    {
                        return hcResult;
                    }
                    info.ImportType = ImportDataType.HistoryCost;
                    //生成账单
                    ChargBill bill = GenerateSingleChargBill(propertyMgrUnitOfWork, info, (ChargeSubject)result.PageData, (CalculateProperty)result.Data);
                    //var count = propertyMgrUnitOfWork.ChargBillRepository.GetAll()
                    //    .Where(p => p.ResourcesId == bill.ResourcesId
                    //    && p.ChargeSubjectId == bill.ChargeSubjectId
                    //    && ((info.BeginDate <= p.BeginDate && p.BeginDate <= info.EndDate)
                    //    || (info.BeginDate <= p.EndDate && p.EndDate <= info.EndDate))).Count();
                    //if (count > 0)
                    //{
                    //    return new ResultModel() { IsSuccess = false, ErrorCode = "790", Msg = "账单开始结束时间和已有账单有重叠" };
                    //}
                    propertyMgrUnitOfWork.Commit();
                    return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "导入信息成功" };
                }
            }
            catch (Exception ex)
            {
                //记录日志
                LogProperty.WriteLoginToFile(string.Format("[历史费用导入]Code:900 ResourceNo:{0} Amount:{1} ErrorMsg:{2}", info.ResourceNo, info.Amount, ex.Message), "ImportSingleHistoryCost", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "历史费用导入失败" };
            }
        }

        public ResultModel ImportHistoryCost(int ComDeptId, IList<HistoryCostInfo> HistoryCostInfoList, int Operator, string OperatorName)
        {
            if (HistoryCostInfoList.Count() < 1)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "970", Msg = "导入数据不能为空" };
            }

            IList<HistoryCostInfo> errorList = new List<HistoryCostInfo>();
            foreach (var item in HistoryCostInfoList)
            {
                ResultModel result = ImportSingleHistoryCost(ComDeptId, item, HistoryCostInfoList, Operator, OperatorName);
                if (!result.IsSuccess)
                {
                    item.ErrorMsg = result.Msg;
                    errorList.Add(item);
                }
            }
            if (errorList.Count() > 0)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "980", Msg = "历史费用导入出现错误", Data = errorList };
            }
            return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "历史费用导入成功" };
        }

        #endregion
    }
}
