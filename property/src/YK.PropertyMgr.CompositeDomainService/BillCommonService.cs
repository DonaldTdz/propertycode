using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using Microsoft.Practices.Unity;
using YK.PropertyMgr.ApplicationDTO;
using YK.BackgroundMgr.DomainInterface;
using YK.PropertyMgr.CompositeDomainService.GenerateBillService.Interface;
using YK.PropertyMgr.CompositeDomainService.GenerateBillService;
using System.Data;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO.Resources;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;
using System.Linq.Expressions;

namespace YK.PropertyMgr.CompositeDomainService
{
    /// <summary>
    /// 公共处理服务 供其它服务调用
    /// </summary>
    public class BillCommonService
    {
        #region 单例

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static BillCommonService Instance { get { return SingletonInstance; } }
        private static readonly BillCommonService SingletonInstance = new BillCommonService();

        #endregion

        #region 属性

        //private static string ReceiptRemark = "请您妥善保存交款收据，如是押金类或代收费项目，票据遗失恕不退付";

        #endregion

        #region 静态资源

        public static string SystemOperatorName = "系统生成";

        public OwnerInfo GetOwnerInfoByHosueDeptID(int houseDeptId, int DeptTypeId = (int)EDeptType.FangWu)
        {
            switch (DeptTypeId)
            {
                case (int)EDeptType.FangWu:
                    var result = DomainInterfaceHelper
                   .LookUp<IPropertyDomainService>()
                   .GetUserOwnerMasterByHouseDeptId(houseDeptId);
                    if (result != null)
                    {
                        return new OwnerInfo() { Id = result.Id.ToString(), DoorNo = result.DoorNo, HouseDeptId = houseDeptId, UserName = result.UserName, Phone = result.BindingPhonerNumber };
                    }
                    break;

                case (int)EDeptType.CheWei:
                    var resultCar = DomainInterfaceHelper
                   .LookUp<IPropertyDomainService>()
                   .GetUserOwnerMasterByCarPortId(houseDeptId);
                    if (resultCar != null)
                    {
                        return new OwnerInfo() { Id = resultCar.Id.ToString(), DoorNo = resultCar.DoorNo, HouseDeptId = houseDeptId, UserName = resultCar.UserName, Phone = resultCar.BindingPhonerNumber };
                    }
                    break;
            }


            return null;
        }

        public static string GetDeptNosByHouseDeptIds(int?[] houseDeptIds)
        {
            return DomainInterfaceHelper
               .LookUp<IPropertyDomainService>()
               .GetDeptNosByHouseDeptIds(houseDeptIds);
        }

        #endregion

        #region 收费流水

        public string GetSerialNumber(string prefix)
        {
            IdWorker idWorker = new IdWorker(0, 0);
            long id = idWorker.nextId();
            return prefix + id.ToString();
        }

        /// <summary>
        /// 生成收费流水
        /// </summary>
        /// <param name="pmUnitWork">工作单元</param>
        /// <param name="chargBill">账单</param>
        /// <param name="chargeType">费用类型</param>
        /// <param name="payType">支付类型</param>
        /// <param name="receiptNum">票据ID</param>
        /// <param name="chargeRecordId">收费流水ID</param>
        /// <param name="Operator">操作者用户ID</param>
        /// <param name="amount">流水金额</param>
        public ChargeRecord GenerateChargeRecord(IPropertyMgrUnitOfWork pmUnitWork
            , IList<ChargBill> chargBillList
            , ChargeTypeEnum chargeType
            , PayTypeEnum payType
            , string receiptId
            , decimal amount
            , string remark
            , Dictionary<string, decimal> amountDetail
            , string chargeRecordId = null
            , int Operator = 0
            , string OperatorName = ""
            , string CustomerName = ""
            , bool IsOnline = false
            , DateTime? payDate = null
            , decimal discountAmount = 0)//优惠金额

        {
            //如果计费金额为0，不需要生成流水
            if (amount == 0)
            {
                throw new CompositeException("生成账单费用记录金额不能为零"); ;
            }
            if (string.IsNullOrEmpty(chargeRecordId))
            {
                chargeRecordId = Guid.NewGuid().ToString();
            }
            //如果优惠金额大于 费用金额 则 优惠金额 = 费用金额 2017-4-26
            if (discountAmount > amount)
            {
                discountAmount = amount;
            }
            //退款为负
            if (chargeType == ChargeTypeEnum.Refund)
            {
                amount = amount * -1;
                discountAmount = discountAmount * -1;
            }
            ChargBill chargBillFirst = chargBillList.First();
            ChargeRecord chargeRecord = new ChargeRecord();
            chargeRecord.Amount = amount;
            chargeRecord.DiscountAmount = discountAmount;
            //chargeRecord.ChargeBillId = chargBill.Id;
            //chargeRecord.ChargeSubjectId = chargBill.ChargeSubjectId;
            chargeRecord.ChargeType = chargeType.GetHashCode();
            //chargeRecord.ComDeptId = chargBill.ComDeptId;
            chargeRecord.ComDeptId = chargBillFirst.ComDeptId;
            chargeRecord.CreateTime = DateTime.Now;
            //chargeRecord.Description = chargBill.Description;
            chargeRecord.Description = "";// chargBill.Description;
            chargeRecord.Id = chargeRecordId;
            chargeRecord.IsDel = false;
            chargeRecord.Operator = Operator;
            chargeRecord.OperatorName = OperatorName;
            chargeRecord.PayDate = payDate ?? DateTime.Now;
            chargeRecord.PayMthodId = payType.GetHashCode();
            chargeRecord.ReceiptId = receiptId;
            chargeRecord.Remark = remark;
            //结算状态默认不适用 修改:2017-01-12
            chargeRecord.AccountingStatus = (int)AccountingStatusEnum.NotApplicable;
            //添加线上线下区分 2017-02-27
            chargeRecord.IsOnline = IsOnline;

            //开发商代缴 修改2016-10-18
            if (chargBillFirst.IsDevPay.Value)
            {
                chargeRecord.HouseDeptId = null;
                chargeRecord.CustomerId = null;
                chargeRecord.CustomerName = "开发商";
                //处理多个房屋显示记录
                int?[] HouseDeptIds = chargBillList.Select(c => c.HouseDeptId).Distinct().ToArray();
                string DoorNos = string.Empty;
                chargeRecord.HouseDeptNos = GetDeptNosByHouseDeptIds(HouseDeptIds);
            }
            //非开发商
            else
            {

                chargeRecord.HouseDeptId = chargBillFirst.HouseDeptId == null ? 0 : chargBillFirst.HouseDeptId.Value;
                //没有客户ID 接口获取
                if (string.IsNullOrEmpty(CustomerName))
                {
                    OwnerInfo ownerInfo = GetOwnerInfoByHosueDeptID(chargeRecord.HouseDeptId.Value);
                    if (ownerInfo != null)
                    {
                        chargeRecord.CustomerId = ownerInfo.Id;
                        chargeRecord.CustomerName = ownerInfo.UserName;
                        //chargeRecord.HouseDeptNos = ownerInfo.DoorNo;
                    }
                }
                else
                {
                    chargeRecord.CustomerId = null;
                    chargeRecord.CustomerName = CustomerName;
                }

                chargeRecord.HouseDeptNos = GetDeptNosByHouseDeptIds(new int?[] { chargeRecord.HouseDeptId.Value });
            }

            //生成规则:S+收费项目ID7位+资源类型Value+资源ID补足7位 + 账单开始时间
            string ChargeSubjectId = chargBillFirst.ChargeSubjectId.ToString();
            string ResourcesId = chargBillFirst.ResourcesId.ToString();
            DateTime bDate = DateTime.Today;
            if (chargBillFirst.BeginDate.HasValue)
            {
                bDate = chargBillFirst.BeginDate.Value;
            }
            chargeRecord.SerialNumber = "S"
                + (ChargeSubjectId.Length < 5 ? ChargeSubjectId.PadLeft(5, '0') : ChargeSubjectId)
                + chargBillFirst.RefType.ToString()
                + (ResourcesId.Length < 7 ? ResourcesId.PadLeft(7, '0') : ResourcesId)
                + bDate.ToString("yyMMdd")
                + DateTime.Now.ToString("HHmmss");
            chargeRecord.Status = ChargeStatusEnum.Normal.GetHashCode();
            chargeRecord.UpdateTime = DateTime.Now;
            //添加资源名称 修改:2017-01-13
            chargeRecord.ResourcesNames = string.Join(",", chargBillList.Where(c => !string.IsNullOrEmpty(c.ResourcesName)).Select(c => c.ResourcesName).Distinct().ToArray());
            pmUnitWork.ChargeRecordRepository.Add(chargeRecord);

            //保存关系
            foreach (var item in chargBillList)
            {
                ChargeBillRecordMatching brm = new ChargeBillRecordMatching();
                brm.ChargeBillId = item.Id;
                brm.ChargeRecordId = chargeRecord.Id;
                brm.Amount = amountDetail[item.Id];
                brm.HouseDeptId = item.HouseDeptId;
                brm.ResourcesId = item.ResourcesId;
                pmUnitWork.ChargeBillRecordMatchingRepository.Add(brm);
            }
            return chargeRecord;
        }

        /// <summary>
        /// 生成账单收费流水 对单账单
        /// </summary>
        /// <param name="pmUnitWork">工作单元</param>
        /// <param name="chargBill">账单</param>
        /// <param name="ChargeRecordId">收费流水ID</param>
        public ChargeRecord GenerateChargeRecordByBill(IPropertyMgrUnitOfWork pmUnitWork
            , ChargBill chargBill
            , string ChargeRecordId
            , decimal? deductionAmount
            , string Remark
            , int Operator = 0, string OperatorName = "系统生成")//操作人Id 操作人名
        {
            //生成单据
            var receipt = GenerateReceipt(pmUnitWork, "", Operator, OperatorName, ReceiptStatusEnum.Paid, null);
            IList<ChargBill> chargBillList = new List<ChargBill>();
            chargBillList.Add(chargBill);
            Dictionary<string, decimal> amountD = new Dictionary<string, decimal>();
            amountD.Add(chargBill.Id, deductionAmount.Value);
            //生成流水
            return GenerateChargeRecord(pmUnitWork,
                    chargBillList,
                    ChargeTypeEnum.DailyCharge,         //日常收费
                    PayTypeEnum.InternalTransfer,       //内部转账
                    receipt.Id,                         //预付款划转默认收据号为空
                    deductionAmount.Value,              //预存划账金额 抵扣金额
                    Remark,
                    amountD,
                    ChargeRecordId,                     //费用记录ID
                    Operator,
                    OperatorName
                );
        }

        /// <summary>
        /// 生成账单收费流水 对多账单
        /// </summary>
        /// <param name="pmUnitWork">工作单元</param>
        /// <param name="chargBill">账单</param>
        /// <param name="ChargeRecordId">收费流水ID</param>
        public ChargeRecord GenerateChargeRecordByBillList(IPropertyMgrUnitOfWork pmUnitWork
            , IList<ChargBill> chargBillList
            , string ChargeRecordId
            , Dictionary<string, decimal> amountD
            , string Remark
            , int Operator = 0, string OperatorName = "系统生成")//操作人Id 操作人名
        {
            //生成单据
            var receipt = GenerateReceipt(pmUnitWork, "", Operator, OperatorName, ReceiptStatusEnum.Paid, null);
            //Dictionary<string, decimal> amountD = chargBillList.ToDictionary(key => key.Id, value => value.ReceivedAmount.Value);
            var deductionAmount = amountD.Sum(a => a.Value);
            //生成流水
            return GenerateChargeRecord(pmUnitWork,
                    chargBillList,
                    ChargeTypeEnum.DailyCharge,         //日常收费
                    PayTypeEnum.InternalTransfer,       //内部转账
                    receipt.Id,                         //预付款划转默认收据号为空
                    deductionAmount,                    //划账抵扣金额合计
                    Remark,
                    amountD,
                    ChargeRecordId,                     //费用记录ID
                    Operator,
                    OperatorName
                );
        }

        #endregion

        #region 生成票据

        /// <summary>
        /// 生成票据
        /// </summary>
        /// <param name="propertyMgrUnitOfWork">工作单元</param>
        /// <param name="number">票据号</param>
        /// <param name="chargeRecordId">流水ID</param>
        /// <param name="Operator">操作人</param>
        /// <param name="OperatorName">操作人名</param>
        /// <returns>票据</returns>
        public Receipt GenerateReceipt(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, string number
            , int Operator, string OperatorName, ReceiptStatusEnum receiptStatus, string remark)
        {




            Receipt receipt = new Receipt();
            receipt.Id = Guid.NewGuid().ToString();
            //receipt.ChargeRecordId = chargeRecordId;
            receipt.CreateTime = DateTime.Now;
            receipt.IsDel = false;
            receipt.Number = number;
            receipt.Operator = Operator;
            receipt.OperatorName = OperatorName;
            receipt.Status = receiptStatus.GetHashCode();
            receipt.UpdateTime = DateTime.Now;
            if (string.IsNullOrEmpty(remark))
            {
                remark = StaticResourceHelper.ReceiptRemark;
            }
            receipt.Remark = remark;
            propertyMgrUnitOfWork.ReceiptRepository.Add(receipt);

            return receipt;
        }

        #endregion

        #region 预交收费

        /// <summary>
        /// 根据小区ID获取预交费项目
        /// 如果不存在 会新增
        /// </summary>
        /// <param name="ComDeptId">小区ID</param>
        /// <returns>预交费收费项目</returns>
        public ChargeSubject GetPrepaymentSubjectByComDeptId(int ComDeptId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var pSubject = propertyMgrUnitOfWork.ChargeSubjectRepository
                    .GetAll()
                    .Where(p => p.ComDeptId == ComDeptId && p.SubjectType == EnumHelper.SubjectType.SystemPreset)
                    .FirstOrDefault();

                if (pSubject != null)
                {
                    return pSubject;
                }

                ChargeSubject subject = new ChargeSubject();
                subject.BeginDate = DateTime.Today;
                subject.BillDay = 20;
                subject.BillPeriod = BillPeriodEnum.Once.GetHashCode();
                subject.ChargeFormula = "";
                subject.ChargeFormulaShow = "";
                subject.Code = "PRE" + ComDeptId.ToString();
                subject.ComDeptId = ComDeptId;
                subject.CreateTime = DateTime.Now;
                subject.IsDel = false;
                subject.IsOnline = true;
                subject.Name = "预存费";
                subject.Operator = 0;
                subject.PenaltyRate = 0;
                subject.Price = 1;
                subject.Remark = "";
                subject.SubjectType = SubjectTypeEnum.SystemPreset.GetHashCode();
                subject.UpdateTime = DateTime.Now;
                propertyMgrUnitOfWork.ChargeSubjectRepository.Add(subject);
                propertyMgrUnitOfWork.Commit();
                return subject;
            }
        }

        /// <summary>
        /// 生成预付款账单
        /// </summary>
        /// <param name="propertyMgrUnitOfWork">工作单元</param>
        /// <param name="comDeptId">小区ID</param>
        /// <param name="houseDeptId">房屋ID</param>
        /// <param name="resourcesId">资源ID</param>
        /// <param name="amount">预付款金额</param>
        /// <param name="billStatus">账单状态</param>
        /// <param name="remark">备注</param>
        /// <returns>账单</returns>
        public ChargBill GeneratePrepaymentBill(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, int comDeptId, int houseDeptId
            , int resourcesId, decimal amount, BillStatusEnum billStatus, string remark, int PreChargeSubjectId)
        {
            //获取收费项目
            ChargeSubject subject = GetPrepaymentSubjectByComDeptId(comDeptId);
            //生成账单
            ChargBill chargBill = new ChargBill();
            chargBill.BeginDate = DateTime.Today;
            chargBill.EndDate = DateTime.Today;
            chargBill.BillAmount = amount;
            chargBill.ChargeSubjectId = subject.Id;
            chargBill.ComDeptId = comDeptId;
            chargBill.CreateTime = DateTime.Now;
            chargBill.Description = subject.Name;
            chargBill.HouseDeptId = houseDeptId;
            chargBill.ResourcesId = resourcesId;
            CalculateProperty hmp = CalculatePropertyHelper.GetHouseCalculateProperty(comDeptId, houseDeptId).FirstOrDefault();
            //添加资源名
            if (hmp != null)
            {
                chargBill.ResourcesName = hmp.ResourcesName;
                // add fixed bug #5433 2017-8-31
                chargBill.HouseDoorNo = hmp.HouseDoorNo;
            }

            chargBill.Id = Guid.NewGuid().ToString();
            chargBill.IsDel = false;
            chargBill.IsDevPay = false;//开发商不支持预缴
            chargBill.PenaltyAmount = 0;
            chargBill.Quantity = string.Empty;
            chargBill.ReceivedAmount = (billStatus == BillStatusEnum.NoPayment ? 0 : amount);
            chargBill.RefType = subject.SubjectType;
            chargBill.ReliefAmount = 0;
            chargBill.Status = billStatus.GetHashCode();
            chargBill.UpdateTime = DateTime.Now;
            chargBill.Remark = remark;
            //修改bug4558 2017-7-4 tdz
            chargBill.PreChargeSubjectId = PreChargeSubjectId;


            propertyMgrUnitOfWork.ChargBillRepository.Add(chargBill);
            //生成快照
            GenerateChargeSubjectSna(propertyMgrUnitOfWork, chargBill, subject);
            return chargBill;
        }

        /// <summary>
        /// 生成预付款账单
        /// </summary>
        /// <param name="comDeptId">小区ID</param>
        /// <param name="houseDeptId">房屋ID</param>
        /// <param name="resourcesId">资源ID</param>
        /// <param name="amount">预付款金额</param>
        /// <param name="billStatus">账单状态</param>
        /// <param name="remark">备注</param>
        /// <returns>账单</returns>
        public ChargBill GenerateTempPrepaymentBill(int comDeptId, int houseDeptId, int resourcesId, string resourcesName
            , decimal amount, BillStatusEnum billStatus, string remark, DateTime? BeginDate, DateTime? EndDate)
        {
            //获取收费项目
            ChargeSubject subject = GetPrepaymentSubjectByComDeptId(comDeptId);
            //生成账单
            ChargBill chargBill = new ChargBill();
            //如果没有至或为0就为当天
            //if (!months.HasValue || months == 0)
            //{
            //    chargBill.BeginDate = null;// DateTime.Today;
            //    chargBill.EndDate = null;// DateTime.Today;
            //}
            //else
            //{
            //    DateTime nextMonthDate = DateTime.Now.AddMonths(1);
            //    chargBill.BeginDate = new DateTime(nextMonthDate.Year, nextMonthDate.Month, 1);
            //    int month = (int)months;
            //    int day = (int)(30 * (months - month)) - 1;
            //    chargBill.EndDate = chargBill.BeginDate;
            //    if (month > 0)
            //    {
            //        chargBill.EndDate = chargBill.EndDate.Value.AddMonths(month);
            //    }
            //    if (day > 0)
            //    {
            //        //如果是二月
            //        if (chargBill.EndDate.Value.Month == 2 && day > 28)
            //        {
            //            day = 28;
            //        }
            //        chargBill.EndDate = chargBill.EndDate.Value.AddDays(day);
            //    }
            //}

            // add fixed bug #5433 2017-8-31
            CalculateProperty hmp = CalculatePropertyHelper.GetHouseCalculateProperty(comDeptId, houseDeptId).FirstOrDefault();
            //添加资源名
            if (hmp != null)
            {
                //chargBill.ResourcesName = hmp.ResourcesName;
                chargBill.HouseDoorNo = hmp.HouseDoorNo;
            }
            chargBill.BeginDate = BeginDate;
            chargBill.EndDate = EndDate;

            chargBill.BillAmount = amount;
            chargBill.ChargeSubjectId = subject.Id;
            chargBill.ComDeptId = comDeptId;
            chargBill.CreateTime = DateTime.Now;
            chargBill.Description = subject.Name;
            chargBill.HouseDeptId = houseDeptId;
            chargBill.ResourcesId = resourcesId;
            chargBill.ResourcesName = resourcesName;
            chargBill.Id = Guid.NewGuid().ToString();
            chargBill.IsDel = false;
            chargBill.IsDevPay = false;//开发商不支持预缴
            chargBill.PenaltyAmount = 0;
            chargBill.Quantity = string.Empty;
            chargBill.ReceivedAmount = (billStatus == BillStatusEnum.NoPayment ? 0 : amount);
            chargBill.RefType = subject.SubjectType;
            chargBill.ReliefAmount = 0;
            chargBill.Status = billStatus.GetHashCode();
            chargBill.UpdateTime = DateTime.Now;
            chargBill.Remark = remark;

            //修改bug4558 2017-7-4 tdz
            //chargBill.PreChargeSubjectId = PreChargeSubjectId;

            return chargBill;
        }
        #endregion

        #region 临时收费

        public ChargBill GenerateTemporaryBill(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, ChargeSubject subject, int comDeptId, int houseDeptId, int resourcesId
            , string resourcesName, DateTime? BeginDateTime, DateTime? EndDateTime, decimal amount, BillStatusEnum billStatus, string remark)
        {
            //生成账单
            ChargBill chargBill = new ChargBill();
            chargBill.BeginDate = BeginDateTime;
            chargBill.EndDate = EndDateTime;
            chargBill.ChargeSubjectId = subject.Id;
            chargBill.ComDeptId = comDeptId;
            chargBill.CreateTime = DateTime.Now;
            chargBill.Description = subject.Name;
            chargBill.HouseDeptId = houseDeptId;
            chargBill.ResourcesId = resourcesId;
            chargBill.ResourcesName = resourcesName;
            chargBill.HouseDoorNo = resourcesName;
            chargBill.Id = Guid.NewGuid().ToString();
            chargBill.IsDel = false;
            chargBill.IsDevPay = false;
            chargBill.PenaltyAmount = 0;
            //CalculateProperty calculateProperty = CalculatePropertyHelper.GetCalculateProperty(subject.ComDeptId.Value, houseDeptId, (SubjectTypeEnum)subject.SubjectType);
            //decimal quantity = 1;
            //if (calculateProperty != null && calculateProperty.Properties != null && calculateProperty.Properties.Count() > 0)
            //{
            //    if (!string.IsNullOrEmpty(calculateProperty.Properties.First().Value))
            //    {
            //        quantity = decimal.Parse(calculateProperty.Properties.First().Value);
            //    }
            //}
            //临时收费目前只针对一次性收费，且金额由前台人员输入，不需要获取属性计算（计算涉及问题，同一账号下有多个车位，多个三表）
            CalculateProperty calculateProperty = new CalculateProperty()
            {
                ComDeptId = comDeptId,
                HouseDeptID = houseDeptId,
                ResourcesId = resourcesId
            };
            chargBill.Quantity = string.Empty;
            chargBill.BillAmount = amount; //Math.Round(quantity * subject.Price,2);
            chargBill.ReceivedAmount = (billStatus == BillStatusEnum.NoPayment ? 0 : amount);
            chargBill.RefType = subject.SubjectType;
            chargBill.ReliefAmount = 0;
            chargBill.Status = billStatus.GetHashCode();
            chargBill.UpdateTime = DateTime.Now;
            chargBill.Remark = remark;
            propertyMgrUnitOfWork.ChargBillRepository.Add(chargBill);
            //生成快照
            GenerateChargeSubjectSna(propertyMgrUnitOfWork, chargBill, subject);
            return chargBill;
        }

        public ChargBill GenerateTempTemporaryBill(ChargeSubject subject, int comDeptId, int houseDeptId, int resourcesId
            , string resourcesName, DateTime? BeginDateTime, DateTime? EndDateTime, decimal amount, BillStatusEnum billStatus, string remark)
        {
            //生成账单
            ChargBill chargBill = new ChargBill();
            chargBill.BeginDate = BeginDateTime;
            chargBill.EndDate = EndDateTime;
            chargBill.ChargeSubjectId = subject.Id;
            chargBill.ComDeptId = comDeptId;
            chargBill.CreateTime = DateTime.Now;
            chargBill.Description = subject.Name;
            chargBill.HouseDeptId = houseDeptId;
            chargBill.ResourcesId = resourcesId;
            //添加： 2017-01-13
            chargBill.ResourcesName = resourcesName;
            //添加： 2017-06-29
            chargBill.HouseDoorNo = resourcesName;
            chargBill.Id = Guid.NewGuid().ToString();
            chargBill.IsDel = false;
            chargBill.IsDevPay = false;
            chargBill.PenaltyAmount = 0;
            chargBill.Quantity = string.Empty;
            chargBill.BillAmount = amount;
            chargBill.ReceivedAmount = (billStatus == BillStatusEnum.NoPayment ? 0 : amount);
            chargBill.RefType = subject.SubjectType;
            chargBill.ReliefAmount = 0;
            chargBill.Status = billStatus.GetHashCode();
            chargBill.UpdateTime = DateTime.Now;
            chargBill.Remark = remark;
            //生成快照
            //GenerateChargeSubjectSna(propertyMgrUnitOfWork, chargBill, subject);
            return chargBill;
        }

        #endregion

        #region 计费金额

        //public decimal CalculateAmount(ChargeSubject chargeSubject,int houseDeptId)
        //{
        //    SubjectTypeEnum sType = (SubjectTypeEnum)chargeSubject.SubjectType;
        //    CalculateProperty calculateProperty = CalculatePropertyHelper.GetCalculateProperty(chargeSubject.ComDeptId.Value, houseDeptId, sType);
        //    return CalculateAmount(chargeSubject, calculateProperty);
        //}

        /// <summary>
        /// 计费金额
        /// </summary>
        /// <param name="chargeSubject">收费项目</param>
        /// <param name="calculateProperty">计费属性</param>
        /// <returns>计费金额</returns>
        public decimal CalculateAmount(ChargeSubject chargeSubject, ICalculateProperty calculateProperty, ref string quantity)
        {
            if (calculateProperty == null)
            {
                return 0;
            }
            string resultFormula = chargeSubject.ChargeFormula;
            //替换单价
            resultFormula = resultFormula.Replace(CalculatePropertyHelper.GetChargeFormulaProperty(ChargeFormulaEnum.Price), chargeSubject.Price.ToString());
            //替换计算对象属性值
            if (calculateProperty.Properties != null)
            {
                foreach (var item in calculateProperty.Properties)
                {
                    resultFormula = resultFormula.Replace(CalculatePropertyHelper.GetChargeFormulaProperty(item.Key), item.Value);
                }
            }

            //替换掉多余的逗号
            resultFormula = resultFormula.Replace(",", "");
            quantity = resultFormula;
            try
            {
                DataTable dt = new DataTable();
                //公式计算
                decimal result = Math.Round(decimal.Parse(dt.Compute(resultFormula, "false").ToString()), 2);
                return result;
            }
            catch (Exception ex)
            {
                //记录错误日志
                LogProperty.WriteLoginToFile(string.Format("[计费金额]Code:702 ChargeSubjectId:{0} Formula:{1} ErrorMsg:{2}", chargeSubject.Id, resultFormula, ex.Message), "GenerateChargBill", FileLogType.Exception);
                return 0;
            }
        }

        #endregion

        #region 余额账户

        /// <summary>
        /// 余额账户生成 修改 对于所有收费项目
        /// </summary>
        /// <param name="houseDeptId">账户关联房屋ID</param>
        /// <param name="initAmount">初始化金额</param>
        /// <returns>余额账户</returns>
        private PrepayAccount GeneratePrepayAccount(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, int houseDeptId, int commDeptID, decimal initAmount, BalanceTypeEnum balanceType, PayTypeEnum payType, string description, string chargeRecordId, string remark)
        {
            return GenerateSubjectPrepayAccount(propertyMgrUnitOfWork, houseDeptId, commDeptID, initAmount, balanceType, payType, description, chargeRecordId, remark, 0);
        }

        /// <summary>
        /// 余额账户生成 添加 生成收费项目余额账户
        /// </summary>
        /// <param name="houseDeptId">账户关联房屋ID</param>
        /// <param name="initAmount">初始化金额</param>
        /// <param name="SubjectId">收费项目Id</param>
        /// <returns>余额账户</returns>
        private PrepayAccount GenerateSubjectPrepayAccount(IPropertyMgrUnitOfWork propertyMgrUnitOfWork,
            int houseDeptId, int commDeptID, decimal initAmount, BalanceTypeEnum balanceType,
            PayTypeEnum payType, string description, string chargeRecordId, string remark, int SubjectId)
        {
            PrepayAccount prepayAccount = new PrepayAccount();
            prepayAccount.Balance = 0;
            prepayAccount.CreateTime = DateTime.Now;
            prepayAccount.HouseDeptId = houseDeptId;
            prepayAccount.CommDeptID = commDeptID;
            prepayAccount.ChargeSubjectID = SubjectId;
            prepayAccount.IsDel = false;
            prepayAccount.Operator = 0;
            prepayAccount.Remark = remark;
            prepayAccount.UpdateTime = DateTime.Now;
            using (var pUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                pUnitWork.PrepayAccountRepository.Add(prepayAccount);
                pUnitWork.Commit();
            }
            if (initAmount > 0)
            {
                //生成费用明细
                GeneratePrepayAccountDetail(propertyMgrUnitOfWork, prepayAccount, initAmount, balanceType, payType, description, chargeRecordId, remark);
                prepayAccount.Balance = initAmount;
                prepayAccount.UpdateTime = DateTime.Now;
                propertyMgrUnitOfWork.PrepayAccountRepository.Update(prepayAccount);
            }
            return prepayAccount;
        }

        /// <summary>
        /// 生成余额明细
        /// </summary>
        /// <param name="propertyMgrUnitOfWork">工作单元</param>
        /// <param name="prepayAccount">账户</param>
        /// <param name="amount">金额</param>
        /// <param name="balanceType">余额类型</param>
        private PrepayAccountDetail GeneratePrepayAccountDetail(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, PrepayAccount prepayAccount, decimal amount, BalanceTypeEnum balanceType, PayTypeEnum payType, string description, string chargeRecordId, string remark)
        {
            //记录划账流水
            PrepayAccountDetail prepayAccountDetail = new PrepayAccountDetail();
            //缴费 为负
            if (balanceType == BalanceTypeEnum.Payment)
            {
                prepayAccountDetail.BeginningBalance = prepayAccount.Balance;//期初
                prepayAccountDetail.EndingBalance = Math.Round(prepayAccount.Balance.Value - amount, 2);//期末
                prepayAccountDetail.ProductionAmount = amount * -1;//发生金额 支出为负
            }
            //充值 为正
            else
            {
                prepayAccountDetail.BeginningBalance = prepayAccount.Balance;//期初
                prepayAccountDetail.EndingBalance = Math.Round(prepayAccount.Balance.Value + amount, 2);//期末
                prepayAccountDetail.ProductionAmount = amount;//发生金额
            }
            prepayAccountDetail.PayTypeId = payType.GetHashCode();
            prepayAccountDetail.Operator = 0;
            prepayAccountDetail.IsDel = false;
            prepayAccountDetail.Description = description;
            prepayAccountDetail.CreateTime = DateTime.Now;
            prepayAccountDetail.ChargeRecordId = chargeRecordId;
            prepayAccountDetail.Remark = remark;
            prepayAccountDetail.UpdateTime = DateTime.Now;
            //prepayAccountDetail.PrepayAccountId = prepayAccount.Id.Value;
            prepayAccountDetail.PrepayAccount = prepayAccount;
            propertyMgrUnitOfWork.PrepayAccountDetailRepository.Add(prepayAccountDetail);
            return prepayAccountDetail;
        }

        /// <summary>
        /// 全部收费项目账户
        /// 缴费 或 充值
        /// </summary>
        /// <param name="propertyMgrUnitOfWork">工作单元</param>
        /// <param name="chargBill">账单</param>
        /// <param name="amount">金额</param>
        /// <param name="payType">支付方式</param>
        /// <param name="chargeRecordId">费用流水ID</param>
        /// <param name="remark">备注</param>
        /// <param name="balanceType">余额类型</param>
        /// <returns>错误信息</returns>
        public ResultModel BalanceAdd(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, ChargBill chargBill, decimal amount, PayTypeEnum payType, string chargeRecordId, string remark, BalanceTypeEnum balanceType)
        {
            return SubjectBalanceAdd(propertyMgrUnitOfWork, chargBill, amount, payType, chargeRecordId, remark, balanceType, 0);
        }

        /// <summary>
        /// 余额缴费充值 添加收费项目余额充值 2017-4-10
        /// </summary>
        /// <returns>错误信息</returns>
        public ResultModel SubjectBalanceAdd(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, ChargBill chargBill
            , decimal amount, PayTypeEnum payType, string chargeRecordId, string remark, BalanceTypeEnum balanceType
            , int PreChargeSubjectId)
        {
            if (amount <= 0)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "800", Msg = (balanceType == BalanceTypeEnum.Payment ? "缴费" : "充值") + "金额不能小于等于零" };
            }
            if (string.IsNullOrEmpty(chargeRecordId))
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "801", Msg = "收费流水ID不能为空" };
            }
            if (chargBill == null)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "802", Msg = "账单不能为NULL" };
            }
            PrepayAccount prepayAccount = propertyMgrUnitOfWork
                .PrepayAccountRepository
                .GetAll()
                .Where(p => p.HouseDeptId == chargBill.HouseDeptId
                && p.CommDeptID == chargBill.ComDeptId
                && p.ChargeSubjectID == PreChargeSubjectId //添加预存到收费项目 2017-04-10
                && p.IsDel == false)
                .FirstOrDefault();
            //如果不存在预存账户 生成一个余额账户
            if (prepayAccount == null)
            {
                //只有充值 和 余额初始化才创建账户
                if (balanceType == BalanceTypeEnum.Recharge)
                {
                    prepayAccount = GenerateSubjectPrepayAccount(propertyMgrUnitOfWork,
                        chargBill.HouseDeptId.Value, chargBill.ComDeptId.Value,
                        amount, balanceType, payType, chargBill.Description, chargeRecordId
                        , remark, PreChargeSubjectId);
                    return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "充值成功", Data = prepayAccount.Balance };
                }
                else
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "805", Msg = "余额账户未充值或未初始化" };
                }
            }
            //缴费 为负
            if (balanceType == BalanceTypeEnum.Payment)
            {
                if (prepayAccount.Balance < amount)
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "810", Msg = "余额不足", Data = prepayAccount.Balance }; //余额不足
                }
            }
            var prepayAccountDetail = GeneratePrepayAccountDetail(propertyMgrUnitOfWork, prepayAccount, amount, balanceType, payType, chargBill.Description, chargeRecordId, remark);

            prepayAccount.Balance = prepayAccountDetail.EndingBalance;//最新余额 等于 流水期末
            prepayAccount.UpdateTime = DateTime.Now;
            propertyMgrUnitOfWork.PrepayAccountRepository.Update(prepayAccount);

            //余额为0时 预存催缴
            if (prepayAccount.Balance == 0)
            {
                //SendNotification(chargBill.ComDeptId.Value, chargBill.HouseDeptId.Value);
            }

            return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = (balanceType == BalanceTypeEnum.Payment ? "缴费" : "充值") + "成功", Data = prepayAccount.Balance };
        }

        #endregion

        #region 预存催缴

        /// <summary>
        /// 预存催缴
        /// </summary>
        /// <param name="HouseDeptId">收费账户 HouseDeptId</param>
        public void SendNotification(int ComDeptId, int HouseDeptId)
        {
            //【XXX物业】您好，您的物业费已于本月到期，请及时续交费用！";

            var houseInfo = DomainInterfaceHelper
           .LookUp<IPropertyDomainService>()
           .GetHouseInfo(ComDeptId, HouseDeptId, true);

            MessageInfo msgInfo = new MessageInfo()
            {
                Title = "缴费提醒",
                Content = string.Format("【{0}】您好，您的物业费已于本月到期，请及时续交费用！", houseInfo.PropertyName),
                ActionUrl = "" //注：需要跳转到物业APP缴费页面
            };

            Task.Run(() =>
            {
                HttpClientService service = new HttpClientService();
                var user = GetOwnerInfoByHosueDeptID(HouseDeptId);
                if (user != null && !string.IsNullOrEmpty(user.Phone))
                {
                    try
                    {
                        service.SendPushWithJson(new string[] { user.Phone }, msgInfo, ComDeptId.ToString());
                    }
                    catch (Exception ex)
                    {
                        LogProperty.WriteLoginToFile(string.Format("[预存催缴]HouseDeptId:{0} Error:{1}", HouseDeptId.ToString(), ex.Message), "JPushMsg", FileLogType.Exception);
                    }
                }
            });

        }

        #endregion

        #region 项目快照

        /// <summary>
        /// 生成账单的 收费项目快照
        /// </summary>
        /// <param name="propertyMgrUnitOfWork">工作单元</param>
        /// <param name="chargBill">账单</param>
        /// <param name="chargeSubject">收费项目</param>
        public void GenerateChargeSubjectSna(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, ChargBill chargBill, ChargeSubject chargeSubject)
        {
            ChargeSubjectSna chargeSubjectSna = new ChargeSubjectSna()
            {
                BillDay = chargeSubject.BillDay,
                ChargeBillId = chargBill.Id,//获取账单ID
                ChargeFormula = chargeSubject.ChargeFormula,
                ChargeFormulaShow = chargeSubject.ChargeFormulaShow,
                ComDeptId = chargeSubject.ComDeptId,
                CreateTime = DateTime.Now,
                IsOnline = chargeSubject.IsOnline,
                Operator = chargeSubject.Operator,
                PenaltyRate = chargeSubject.PenaltyRate,
                Price = chargeSubject.Price,
                Remark = chargeSubject.Remark,
                UpdateTime = DateTime.Now
            };
            //如果是预存到收费项目 单价快照要获取 预存收费项目的单价 add by donald 2017-06-14
            if (chargBill.RefType == (int)SubjectTypeEnum.SystemPreset
                && chargBill.PreChargeSubjectId.HasValue
                && chargBill.PreChargeSubjectId != 0)
            {
                var sobj = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(chargBill.PreChargeSubjectId);
                if (sobj != null && sobj.IsDel == false)
                {
                    chargeSubjectSna.Price = sobj.Price;
                }
            }
            propertyMgrUnitOfWork.ChargeSubjectSnaRepository.Add(chargeSubjectSna);
        }

        #endregion

        #region 交款记录

        /// <summary>
        /// 生成当账单交款记录
        /// </summary>
        /// <param name="propertyMgrUnitOfWork">工作单元</param>
        /// <param name="chargeRecordList">账单</param>
        /// <param name="paymentTaskStatus">交款状态</param>
        /// <param name="remark">备注</param>
        /// <param name="isInitBalance">是否是余额初始化</param>
        public void GenerateBillPaymentTask(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, IList<ChargeRecord> chargeRecordList, PaymentTaskStatusEnum paymentTaskStatus, string remark, int Operator, string OperatorName, DateTime PaymentDate, bool isInitBalance = false)
        {
            if (chargeRecordList.Count() < 1)
            {
                return;
            }
            PaymentTasks paymentTask = new PaymentTasks();
            paymentTask.PaymentDate = PaymentDate;
            paymentTask.IsDel = false;
            paymentTask.Money = chargeRecordList.Sum(c => c.Amount);
            paymentTask.Operator = Operator;
            paymentTask.OperatorName = OperatorName;
            paymentTask.ComDeptId = chargeRecordList.First().ComDeptId;
            //提交申请
            if (paymentTaskStatus == PaymentTaskStatusEnum.Submitted)
            {
                paymentTask.ApplicantId = Operator;
                paymentTask.ApplicantName = OperatorName;
            }
            //已审核
            if (paymentTaskStatus == PaymentTaskStatusEnum.Audited)
            {
                paymentTask.ApplicantId = Operator;
                paymentTask.ApplicantName = OperatorName;
                paymentTask.ReviewerId = Operator;
                paymentTask.ReviewerName = OperatorName;
                paymentTask.ReviewDate = DateTime.Now;

            }

            paymentTask.Remark = remark;
            paymentTask.Status = paymentTaskStatus.GetHashCode();

            paymentTask.CreateTime = DateTime.Now;
            paymentTask.UpdateTime = DateTime.Now;
            paymentTask.IsInitBalance = isInitBalance;
            paymentTask.PaymentTaskItems = new List<PaymentTaskDetail>();
            foreach (var item in chargeRecordList)
            {
                PaymentTaskDetail taskDetail = new PaymentTaskDetail();
                taskDetail.PaymentTasks = paymentTask;
                taskDetail.IsDel = false;
                taskDetail.UpdateTime = DateTime.Now;
                taskDetail.ChargeRecordId = item.Id;
                taskDetail.CreateTime = DateTime.Now;
                paymentTask.PaymentTaskItems.Add(taskDetail);
            }
            paymentTask.Code = CreatePaymentTaskCode(paymentTask);
            propertyMgrUnitOfWork.PaymentTasksRepository.Add(paymentTask);
        }

        #endregion

        #region 账单拆分

        /// <summary>
        /// 拆分账单
        /// </summary>
        /// <param name="bill">被拆分的账单</param>
        /// <param name="splitDate">拆分日期</param>
        /// <param name="remark">备注</param>
        /// <returns>返回拆分后的新账单</returns>
        public ChargBill SplitBill(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, ChargBill bill, DateTime splitDate, string remark, int Operator, string OperatorName)
        {
            //新账单
            ChargBill newBill = new ChargBill();
            newBill.RefType = bill.RefType;
            newBill.BeginDate = splitDate.AddDays(1);
            newBill.EndDate = bill.EndDate;
            newBill.ChargeSubjectId = bill.ChargeSubjectId;
            newBill.ComDeptId = bill.ComDeptId;
            newBill.CreateTime = DateTime.Now;
            newBill.Description = bill.Description;
            newBill.HouseDeptId = bill.HouseDeptId;
            newBill.Id = Guid.NewGuid().ToString();
            newBill.IsDel = false;
            newBill.IsDevPay = bill.IsDevPay;
            newBill.PenaltyAmount = 0;//滞纳金当前计算为零
            newBill.ReliefAmount = 0;//减免金额为0
            newBill.Remark = remark;
            newBill.ResourcesId = bill.ResourcesId;
            newBill.ResourcesName = bill.ResourcesName;
            //添加：2017-06-29
            newBill.HouseDoorNo = bill.HouseDoorNo;
            newBill.Status = BillStatusEnum.NoPayment.GetHashCode();
            newBill.UpdateTime = DateTime.Now;

            decimal monthDay = (bill.EndDate.Value - bill.BeginDate.Value).Days + 1;
            decimal splitDay = (splitDate - bill.BeginDate.Value).Days + 1;
            //int newDay = monthDay - splitDay;
            //账单金额 数量 已收金额
            decimal amount = bill.BillAmount.Value;
            //decimal quantity = bill.Quantity.Value;
            decimal receivedAmount = bill.ReceivedAmount.Value;
            //原来账单处理
            bill.EndDate = splitDate;
            //原计费金额/该月总天数*计费天数
            bill.BillAmount = Math.Round(amount / monthDay * splitDay, 2);
            //bill.Quantity = Math.Round(quantity / monthDay * splitDay, 2);
            if (receivedAmount < bill.BillAmount)
            {
                bill.ReceivedAmount = receivedAmount;
            }
            else
            {
                bill.ReceivedAmount = bill.BillAmount;
                bill.Status = BillStatusEnum.Paid.GetHashCode();
            }
            bill.ReliefAmount = 0;//都不享受减免金额
            newBill.Quantity = bill.Quantity;
            newBill.BillAmount = amount - bill.BillAmount;
            decimal newRAmount = receivedAmount - bill.BillAmount.Value;
            newBill.ReceivedAmount = (newRAmount > 0 ? newRAmount : 0);
            bill.UpdateTime = DateTime.Now;

            propertyMgrUnitOfWork.ChargBillRepository.Update(bill);
            propertyMgrUnitOfWork.ChargBillRepository.Add(newBill);

            //是否需要更新收费记录
            if (newBill.ReceivedAmount > 0)
            {
                var chargeRecordM = propertyMgrUnitOfWork
                    .ChargeBillRecordMatchingRepository
                    .GetAll()
                    .Where(c => c.ChargeBillId == bill.Id)
                    .FirstOrDefault();
                if (chargeRecordM != null)
                {
                    ChargeBillRecordMatching newMath = new ChargeBillRecordMatching();
                    newMath.ChargeBillId = newBill.Id;
                    newMath.ChargeRecordId = chargeRecordM.ChargeRecordId;
                    propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.Add(newMath);
                }
            }
            //生成快照
            GenerateChargeSubjectSna(propertyMgrUnitOfWork, newBill, bill.ChargeSubject);
            return newBill;
        }

        /// <summary>
        /// 拆分账单
        /// </summary>
        /// <param name="bill">被拆分的账单</param>
        /// <param name="splitDate">拆分日期</param>
        /// <param name="remark">备注</param>
        /// <returns>返回拆分后的新账单</returns>
        public ChargBill SplitTempBill(ChargBill bill, DateTime splitDate, string remark, int Operator, string OperatorName)
        {
            //新账单
            ChargBill newBill = new ChargBill();
            newBill.RefType = bill.RefType;
            newBill.BeginDate = splitDate.AddDays(1);
            newBill.EndDate = bill.EndDate;
            newBill.ChargeSubjectId = bill.ChargeSubjectId;
            newBill.ComDeptId = bill.ComDeptId;
            newBill.CreateTime = DateTime.Now;
            newBill.Description = bill.Description;
            newBill.HouseDeptId = bill.HouseDeptId;
            newBill.Id = Guid.NewGuid().ToString();
            newBill.IsDel = false;
            newBill.IsDevPay = bill.IsDevPay;
            newBill.PenaltyAmount = 0;//滞纳金当前计算为零
            newBill.ReliefAmount = 0;//减免金额为0
            //bill.Remark = bill.Remark ?? "";
            //newBill.Remark = bill.Remark + remark;
            newBill.Remark = remark;
            newBill.ResourcesId = bill.ResourcesId;
            //添加 时间：2017-01-13
            newBill.ResourcesName = bill.ResourcesName;
            //添加：2017-06-29
            newBill.HouseDoorNo = bill.HouseDoorNo;
            newBill.Status = BillStatusEnum.NoPayment.GetHashCode();
            newBill.UpdateTime = DateTime.Now;
            //newBill.ChargeSubject = bill.ChargeSubject;

            decimal monthDay = (bill.EndDate.Value - bill.BeginDate.Value).Days + 1;
            decimal splitDay = (splitDate - bill.BeginDate.Value).Days + 1;
            //int newDay = monthDay - splitDay;
            //账单金额 数量 已收金额
            decimal amount = bill.BillAmount.Value;
            //decimal quantity = bill.Quantity.Value;
            decimal receivedAmount = bill.ReceivedAmount.Value;
            //原来账单处理
            bill.EndDate = splitDate;
            //原计费金额/该月总天数*计费天数
            bill.BillAmount = Math.Round(amount / monthDay * splitDay, 2);
            //bill.Quantity = Math.Round(quantity / monthDay * splitDay, 2);
            if (receivedAmount < bill.BillAmount)
            {
                bill.ReceivedAmount = receivedAmount;
            }
            else
            {
                bill.ReceivedAmount = bill.BillAmount;
                bill.Status = BillStatusEnum.Paid.GetHashCode();
            }
            bill.ReliefAmount = 0;//都不享受减免金额
            newBill.Quantity = bill.Quantity;
            newBill.BillAmount = amount - bill.BillAmount;
            decimal newRAmount = receivedAmount - bill.BillAmount.Value;
            newBill.ReceivedAmount = (newRAmount > 0 ? newRAmount : 0);
            bill.UpdateTime = DateTime.Now;
            return newBill;
        }

        #endregion

        #region 开发商代缴

        public bool GetIsDevPay(SubjectHouseRef subjectHouseItem, BillDateRange dItem)
        {
            //只要设置开发商代缴，在账单的开始结束时间范围内，先标识为开发商代缴
            if (subjectHouseItem.IsDevPay.HasValue && subjectHouseItem.IsDevPay.Value &&
                //账单开始日期 <= 开发商开始日期 <= 账单结束日期
                ((dItem.BeginDate <= subjectHouseItem.DevBeginDate && subjectHouseItem.DevBeginDate <= dItem.EndDate)
                //账单开始日期 <= 开发商结束日期 <= 账单结束日期
                || (dItem.BeginDate <= subjectHouseItem.DevEndDate && subjectHouseItem.DevEndDate <= dItem.EndDate)
                //开发商开始日期 <= 账单开始日期 && 账单结束日期 <= 开发商结束日期
                || (subjectHouseItem.DevBeginDate <= dItem.BeginDate && dItem.EndDate <= subjectHouseItem.DevEndDate)
                ))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 获取账单开始日

        /// <summary>
        /// 自动生成 只能生成当月的账单 修改：2016-11-21
        /// 手动生成 需要生成连续账单 修改：2016-12-16
        /// </summary>
        public DateTime? GetBillBeginDate(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, ChargeSubject chargeSubject, SubjectHouseRef subjectHouseItem, bool IsManual)
        {
            //bug #4611, 改变房屋关系的资源 需要通过资源Id的最后账单日 2017-7-10
            Condition<ChargBill> condition = new Condition<ChargBill>(c => c.ChargeSubjectId == chargeSubject.Id
                    //&& c.HouseDeptId == subjectHouseItem.HouseDeptId
                    && c.ResourcesId == subjectHouseItem.ResourcesId
                    && c.IsDel == false);
            if (chargeSubject.SubjectType == (int)SubjectTypeEnum.House)
            {
                condition = condition & new Condition<ChargBill>(c => c.HouseDeptId == subjectHouseItem.HouseDeptId);
            }

            //查询上次生成账单
            ChargBill lastBill = propertyMgrUnitOfWork
                .ChargBillRepository
                .GetAll()
                .Where(condition.ExpressionBody)
                    .OrderByDescending(c => c.EndDate)
                    .FirstOrDefault();

            //获取房屋开始计费时间，如果没有 获取 最新绑定时间 修改：2017-08-18 v2.8
            var houseBillBeginDate = subjectHouseItem.BeginDateBill.HasValue ? subjectHouseItem.BeginDateBill.Value : subjectHouseItem.CreateTime.Value;
            var lastBindDate = new DateTime(houseBillBeginDate.Year, houseBillBeginDate.Month, houseBillBeginDate.Day);//.AddDays(1);

            //手动需要连续生成
            //开始时间分别取该收费项目在各个资源已生成账单的截止时间和该收费项目绑定时间，取两者中时间靠后那个为准 修改：2016-12-16
            if (IsManual)
            {
                DateTime billBeginDate;
                if (lastBill == null)
                {
                    //如果没有生成过账单，直接取收费项目开始时间
                    billBeginDate = chargeSubject.BeginDate.Value;
                }
                else
                {
                    //取上次收费项目结束时间 + 1天
                    //收费周期是每月计费 则开始时间是下个月1号（对于导入不规则日期的账单） 修改：2017-7-5
                    if (chargeSubject.BillPeriod == (int)BillPeriodEnum.MonthlyCharge)
                    {
                        var edate = new DateTime(lastBill.EndDate.Value.Year, lastBill.EndDate.Value.Month, 1);//本月1号
                        billBeginDate = edate.AddMonths(1);//下个月1号
                    }
                    else
                    {
                        billBeginDate = lastBill.EndDate.Value.AddDays(1);
                    }

                    //如收费项目时间更晚 取 收费项目时间（主要存在导入账单） fixed bug #5432 2017-8-31
                    if (chargeSubject.BeginDate.Value > billBeginDate)
                    {
                        billBeginDate = chargeSubject.BeginDate.Value;
                    }
                }
                //如果绑定时间更晚 开始时间为绑定时间
                if (lastBindDate > billBeginDate)
                {
                    billBeginDate = lastBindDate;
                }

                return billBeginDate;
            }
            //自动只需要生成本月
            else
            {
                //账单开始日期为本月1号
                DateTime billBeginDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                //如果账单开始日 还未到 账单生成日 跳过
                if (billBeginDate < chargeSubject.BeginDate)
                {
                    //如果是同月
                    if (billBeginDate.Year == chargeSubject.BeginDate.Value.Year
                        && billBeginDate.Month == chargeSubject.BeginDate.Value.Month)
                    {
                        billBeginDate = chargeSubject.BeginDate.Value;
                    }
                    //账单还未开始计费
                    else
                    {
                        return null;
                    }
                }
                //如果绑定时间更晚 开始时间为绑定时间
                if (lastBindDate > billBeginDate)
                {
                    billBeginDate = lastBindDate;
                }

                //账单有重叠 不能生成
                if (lastBill != null)
                {
                    var lastBillEndDate = lastBill.EndDate;
                    //收费周期是每月计费 对于导入的 账单日期不规则账单处理 2017-7-5
                    if (chargeSubject.BillPeriod == (int)BillPeriodEnum.MonthlyCharge)
                    {
                        var tDate = new DateTime(lastBillEndDate.Value.Year, lastBillEndDate.Value.Month, 1);//本月1号
                        lastBillEndDate = tDate.AddMonths(1).AddDays(-1);//获得本月最后一天
                    }

                    if (billBeginDate <= lastBillEndDate)
                    {
                        return null;
                    }
                }
                return billBeginDate;
            }
        }

        #endregion

        #region 票据号流水号生成
        public TicketSerialNumber CreateTicketSerialNumber(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, int ComDeptId)
        {
            TicketSerialNumber ticketserialnumber = new TicketSerialNumber();



            ticketserialnumber = propertyMgrUnitOfWork.TicketSerialNumberRepository.GetAll().Where(o => o.CommunityId == ComDeptId).FirstOrDefault();

            if (ticketserialnumber == null)
            {

                TicketSerialNumber FirstTSN = new TicketSerialNumber()
                {
                    CommunityId = ComDeptId,
                    SerialValue = 0,
                    CompleteSerialValue = "000000000000000"

                };



                using (var propertyMgrUnitOfWork_New = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    propertyMgrUnitOfWork_New.TicketSerialNumberRepository.Add(FirstTSN);
                    propertyMgrUnitOfWork_New.Commit();
                }
                ticketserialnumber = propertyMgrUnitOfWork.TicketSerialNumberRepository.GetAll().Where(o => o.CommunityId == ComDeptId).First();

            }


            //获取值+1
            int Tickvalue = ticketserialnumber.SerialValue.Value + 1;
            int Digit = Tickvalue.ToString().Length;
            string SerialValueStr = string.Empty;
            string ComDeptStr = string.Empty;
            if (Digit == 8)
            {//等于8位
                SerialValueStr = Digit.ToString();


            }
            else if (Digit < 8)
            {
                //小于8位补齐
                SerialValueStr = Tickvalue.ToString().PadLeft(8, '0');
            }
            else if (Digit > 8)
            {
                throw new Exception("票据号已经超过最大限度");
            }

            Digit = ComDeptId.ToString().Length;
            if (Digit == 7)
            {//等于7位
                ComDeptStr = ComDeptId.ToString();


            }
            else if (Digit < 7)
            {
                //小于7位补齐
                ComDeptStr = ComDeptId.ToString().PadLeft(7, '0');
            }
            else if (Digit > 7)
            {
                throw new Exception("小区编号已经超过最大限度，请通知管理员");
            }


            ticketserialnumber.CompleteSerialValue = ComDeptStr + SerialValueStr;
            ticketserialnumber.SerialValue = Tickvalue;



            return ticketserialnumber;
        }

        public string CreateTicketSerialNumberTest(int CommunityId, int SerialValue)
        {
            int Tickvalue = SerialValue + 1;
            int Digit = Tickvalue.ToString().Length;
            string SerialValueStr = string.Empty;
            string ComDeptStr = string.Empty;
            if (Digit == 8)
            {//等于8位
                SerialValueStr = Digit.ToString();


            }
            else if (Digit < 8)
            {
                //小于8位补齐
                SerialValueStr = Tickvalue.ToString().PadLeft(8, '0');
            }
            else if (Digit > 8)
            {
                throw new Exception("票据号已经超过最大限度");
            }

            Digit = CommunityId.ToString().Length;
            if (Digit == 7)
            {//等于7位
                ComDeptStr = CommunityId.ToString();


            }
            else if (Digit < 7)
            {
                //小于7位补齐
                ComDeptStr = CommunityId.ToString().PadLeft(7, '0');
            }
            else if (Digit > 7)
            {
                throw new Exception("小区编号已经超过最大限度，请通知管理员");
            }




            return ComDeptStr + SerialValueStr;
        }




        #endregion

        #region 票据本票据号生成

        /// <summary>
        /// 获取最新的票据本
        /// </summary>
        /// <param name="propertyMgrUnitOfWork"></param>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        public ReceiptBook GetCurrentReceiptBook(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, int ComDeptId)
        {
            return propertyMgrUnitOfWork.ReceiptBookRepository.GetAll().Where(o => o.IsDel == false && o.DeptId == ComDeptId && o.Status == (int)ReceiptBookStatusEnum.Enabled &&
                                                                      o.ReceiptBookType == (int)ReceiptBookTypeEnum.ChargeBill).FirstOrDefault();
        }

        public bool CheckHasReceiptBook(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, int ComDeptId)
        {
            var ReceiptModel = GetCurrentReceiptBook(propertyMgrUnitOfWork, ComDeptId);
            if (ReceiptModel != null && ReceiptModel.Id > 0)
            {
                return true;
            }
            return false;

        }

        /// <summary>
        /// 生成票据
        /// </summary>
        /// <param name="receiptbook"></param>
        /// <returns></returns>
        public string GenerateReceiptBookNumber(IPropertyMgrUnitOfWork pmUnitWork, int ComDeptId, int IncrementalNumber = 0)
        {
            var receiptbook = GetCurrentReceiptBook(pmUnitWork, ComDeptId);
            return CreateNowReceiptBookNumber(receiptbook, IncrementalNumber);

        }

        /// <summary>
        /// 生成票据
        /// </summary>
        /// <param name="receiptbook"></param>
        /// <returns></returns>
        public string CreateNowReceiptBookNumber(ReceiptBook receiptbook, int IncrementalNumber)
        {
            //判断票据号是否用完
            //最后的票据号+当前票据号进行比对
            var EndReceiptNumber = CreateReceiptBookNumber(receiptbook, receiptbook.EndCode);

            if (receiptbook.CurrentReceiptNum == EndReceiptNumber && CheckReceiptBookNumRepeat(receiptbook.CurrentReceiptNum, receiptbook.DeptId.Value, ""))
            {
                throw new Exception("当前票据号已使用完，请重新设置票据号后再进行补打");

            }

            int NowReceiptCode = 0;
            if (string.IsNullOrEmpty(receiptbook.CurrentReceiptNum))
            {
                NowReceiptCode = receiptbook.BeginCode.Value + IncrementalNumber;
                if (NowReceiptCode > receiptbook.EndCode.Value && receiptbook.BeginCode != receiptbook.EndCode)
                {
                    throw new Exception("当前票据号已使用完，请重新设置票据号后再进行票据补打");
                }
            }
            else
            {
                //获取当前票据号的数字位
                if (CheckReceiptBookNumRepeat(receiptbook.CurrentReceiptNum, receiptbook.DeptId.Value, ""))
                {
                    NowReceiptCode = ReceiptNumberToIntNumber(receiptbook) + IncrementalNumber;
                }
                else
                {
                    int CurrentNumber = InterceptionReceiptBookCurrentNumber(ReceiptBookMappers.ChangeReceiptBookToDTO(receiptbook), receiptbook.CurrentReceiptNum);
                    NowReceiptCode = CurrentNumber + 1;
                }

                if (NowReceiptCode > receiptbook.EndCode.Value)
                {
                    throw new Exception("当前票据号已使用完，请重新设置票据号后再进行票据补打");
                }
            }

            return CreateReceiptBookNumber(receiptbook, NowReceiptCode);
        }

        public bool CheckReceiptBookNumRepeat(string ReceiptNum, int ComDeptId, string ReceiptId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //创建
                if (string.IsNullOrEmpty(ReceiptId))
                {
                    var count = (from r in propertyMgrUnitOfWork.ReceiptRepository.GetAll()
                                 join c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(r => r.ComDeptId == ComDeptId)
                                 on r.Id equals c.ReceiptId
                                 where r.Number == ReceiptNum

                                 select r.Id).Count();
                    if (count > 0)
                    {
                        return true;
                    }
                    return false;
                }
                //修改
                else
                {
                    var count = (from r in propertyMgrUnitOfWork.ReceiptRepository.GetAll().Where(r => r.Id != ReceiptId)
                                 join c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll()
                                 .Where(r => r.ComDeptId == ComDeptId)
                                 on r.Id equals c.ReceiptId
                                 where r.Number == ReceiptNum
                                 select r.Id).Count();
                    if (count > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// /获取当前票据号的数字位
        /// </summary>
        /// <param name="receiptbook"></param>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public int ReceiptNumberToIntNumber(ReceiptBook receiptbook)
        {

            string str1 = receiptbook.CurrentReceiptNum.Remove(0, receiptbook.Prefix.Length);
            int SuffixInt = Convert.ToInt32(str1);
            return SuffixInt;
        }

        /// <summary>
        /// /获取自写票据号的数字位
        /// </summary>
        /// <param name="receiptbook"></param>
        /// <param name="ReceiptNumber"></param>
        /// <returns></returns>
        public int ReceiptNumberToIntNumber(ReceiptBook receiptbook, string ReceiptNumber)
        {
            string str1 = ReceiptNumber.Remove(0, receiptbook.Prefix.Length);
            int SuffixInt = Convert.ToInt32(str1);
            return SuffixInt;
        }


        /// <summary>
        /// 生成指定票据号
        /// </summary>
        /// <param name="receiptbook"></param>
        /// <returns></returns>
        public string CreateReceiptBookNumber(ReceiptBook receiptbook, int? Num)
        {
            var CurrentCode = Num;
            string CurrentCodestr = CurrentCode.Value.ToString().PadLeft(receiptbook.Suffix.Value, '0');
            return receiptbook.Prefix + CurrentCodestr;

        }


        /// <summary>
        /// 票据本冗余数据进行处理
        /// </summary>
        /// <param name="receiptbook"></param>
        /// <returns></returns>
        public void ReceiptBookIncrease(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, ReceiptBook receiptbook, string ReceiptNum, decimal Amount)
        {
            receiptbook.UsedNumber += 1;//已用数量加1
            receiptbook.UpdateTime = DateTime.Now;//修改时间

            //
            var currenNumber = ReceiptNumberToIntNumber(receiptbook, ReceiptNum);


            receiptbook.CurrentReceiptNum = CreateReceiptBookNumber(receiptbook, currenNumber);
            if (receiptbook.ReceiptAmount == null)
                receiptbook.ReceiptAmount = Amount;
            else
                receiptbook.ReceiptAmount += Amount;
            propertyMgrUnitOfWork.ReceiptBookRepository.Update(receiptbook);
        }

        /// <summary>
        /// 验证票据号格式
        /// </summary>
        public bool VerificationReceiptNumFormat(string Number, ReceiptBook receiptbook, ref string msg)
        {
            if (receiptbook.BeginCode > receiptbook.EndCode)
            {
                msg = "起号不能大于止号";
                return false;
            }

            //1.验证前缀
            var poPrefixstr = Number.Substring(0, receiptbook.Prefix.Length);
            if (poPrefixstr != receiptbook.Prefix)
            {//前缀验证不通过
                msg = "前缀验证不通过";
                return false;
            }
            //2.验证后缀位数
            string str1 = Number.Remove(0, receiptbook.Prefix.Length);
            if (receiptbook.Suffix.Value != str1.Length)
            {
                msg = "后缀位数验证不通过";
                return false;
            }
            //验证后缀是否在范围内
            try
            {
                int SuffixInt = Convert.ToInt32(str1);
                if (SuffixInt < receiptbook.BeginCode || SuffixInt > receiptbook.EndCode)
                {
                    msg = "当前票据不在范围内";
                    return false;
                }
            }
            catch (Exception)
            {
                msg = "票据位：" + str1 + "不合法，请输入正确的格式";
                return false;
            }
            return true;
        }

        #endregion

        #region 交款单号生成
        private string CreatePaymentTaskCode(PaymentTasks paymentTasks)
        {

            string paymentTasksCode = string.Empty;

            paymentTasksCode = paymentTasks.ComDeptId.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss");

            return paymentTasksCode;
        }
        #endregion

        #region 获取收费信息（房间、车位、三表 的信息）
        public ChargeBillInformationDTO GetChargeBillInformationDTOByResourceId(int HouseDeptId, int DeptTypeId)
        {
            switch (DeptTypeId)
            {
                case (int)EDeptType.FangWu:

                    return GetChargeBillInformationDTOByHouseDeptId(HouseDeptId);
                case (int)EDeptType.CheWei:
                    return GetChargeBillInformationDTOByCarParkId(HouseDeptId);


                default:
                    return new ChargeBillInformationDTO();
            }

        }


        private ChargeBillInformationDTO GetChargeBillInformationDTOByHouseDeptId(int HouseDeptId)
        {
            ChargeBillInformationDTO model = new ChargeBillInformationDTO();
            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
            var HouseDept = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoById(HouseDeptId.ToString());
            var ComDeptId = Convert.ToInt32(HouseDept.Code.Split('.')[2]);
            //房间
            var HouseIinfo = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseInfo(ComDeptId, HouseDeptId);
            List<DictionaryModel> listHouseStatusType = propertyService.GetDictionaryModels("HouseState");
            string HouseStatusStr = HouseIinfo.HouseStatus.ToString();
            var type = listHouseStatusType.Where(o => o.Code == HouseStatusStr).FirstOrDefault();
            if (type != null)
            { HouseStatusStr = type.CnName; }
            else
            { HouseStatusStr = ""; }
            HouseIinfo.HouseStatusStr = HouseStatusStr;
            model.houseInfo = HouseIinfo;

            if (HouseIinfo != null && HouseIinfo.Id > 0)
                model.IsShowHouse = true;
            else
                model.IsShowHouse = false;


            //停车场
            var CarParkInfo = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetParkingSpaceInfo(ComDeptId, HouseDeptId);
            model.parkingSpaceInfo = CarParkInfo;
            if (CarParkInfo != null && CarParkInfo.Count > 0)
                model.IsShowCarPark = true;
            else
                model.IsShowCarPark = false;

            List<DictionaryModel> listReceiptBookType = propertyService.GetDictionaryModels("MeterType");
            var meterlist = MeterMappers.ChangeMeterToDTOs(GetMeterList(HouseDeptId));

            foreach (var obj in meterlist)
            {
                string MeterTypestr = obj.MeterType.ToString();
                var typeModel = listReceiptBookType.Where(o => o.Code == MeterTypestr).FirstOrDefault();
                if (typeModel != null)
                    obj.MeterTypeStr = typeModel.CnName;
                else
                    obj.MeterTypeStr = "";

                obj.ReadDateStr = obj.ReadDate.Value.ToString("yyyy-MM-dd");
            }

            //三表
            model.meter = meterlist;

            if (meterlist != null && meterlist.Count > 0)
                model.IsShowMeter = true;
            else
                model.IsShowMeter = false;


            if (model.meter.Count <= 0)
            {
                model.meter.Add(new MeterDTO());
            }

            if (model.parkingSpaceInfo.Count <= 0)
            {
                model.parkingSpaceInfo.Add(new BackgroundMgr.ApplicationDTO.ApplicationDTO.ParkingSpaceInfo());
            }



            return model;

        }

        private ChargeBillInformationDTO GetChargeBillInformationDTOByCarParkId(int CarParkId)
        {

            ChargeBillInformationDTO model = new ChargeBillInformationDTO();
            var CarParkInfo = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetParkingSpaceListByResourcesId(CarParkId);
            model.houseInfo = new BackgroundMgr.ApplicationDTO.ApplicationDTO.HouseInfo();
            model.parkingSpaceInfo = CarParkInfo;
            model.meter = (new List<MeterDTO>());
            model.meter.Add(new MeterDTO());
            model.IsShowMeter = false;
            model.IsShowCarPark = true;
            model.IsShowHouse = false;
            return model;
        }


        /// <summary>
        /// 返回该房间的全部三表
        /// </summary>
        /// <param name="houseDeptId"></param>

        /// <returns></returns>
        public List<Meter> GetMeterList(int houseDeptId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {

                return propertyMgrUnitOfWork.MeterRepository.GetAll().Where(o => o.HouseDeptID == houseDeptId).ToList();
            }
        }

        #endregion

        #region 票据本相关
        /// <summary>
        /// 票据验证向导程序
        /// </summary>
        /// <param name="ModelDTO"></param>
        /// <param name="AuthenticationType"></param>
        /// <returns></returns>
        public ResultModel AuthenticationReBookWizard(ReceiptBookDTO ModelDTO, int AuthenticationType)
        {
            switch (AuthenticationType)
            {
                case 1:
                    //新增和修改验证
                    return AuthenticationReceiptBookInsertOrUpdate(ModelDTO);

                default:
                    return new ResultModel() { IsSuccess = false, Msg = "没有合适的验证方法，请联系管理员" };

            }

        }


        #region 票据验证相关
        private ResultModel AuthenticationReceiptBookInsertOrUpdate(ReceiptBookDTO ModelDTO)
        {
            //Step1 验证基本格式
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var result = AuthenticationReceiptBookCurrenNumberFormat(ModelDTO, propertyMgrUnitOfWork);
                if (!result.IsSuccess)
                {
                    return new ResultModel() { IsSuccess = false, Msg = result.Msg };

                }

            }
            return new ResultModel() { IsSuccess = true, Msg = "验证成功" };
        }

        /// <summary>
        /// 当前票据号格式验证
        /// </summary>
        /// <param name="ModelDTO"></param>
        /// <returns></returns>
        private ResultModel AuthenticationReceiptBookCurrenNumberFormat(ReceiptBookDTO ModelDTO, IPropertyMgrUnitOfWork propertyMgrUnitOfWork)
        {
            //验证起号和止号的错误
            if (ModelDTO.BeginCode > ModelDTO.EndCode)
            {
                return new ResultModel()
                {
                    IsSuccess = false,
                    Msg = "起号不能大于止号"
                };
            }
            string ReceiptCurrentNumber = ModelDTO.ReceiptCurrentNumberView;


            //1.验证前缀
            ReceiptCurrentNumber = ReceiptCurrentNumber.Substring(0, ModelDTO.Prefix.Length);
            if (ReceiptCurrentNumber != ModelDTO.Prefix)
            {//前缀验证不通过

                return new ResultModel()
                {
                    IsSuccess = false,
                    Msg = "前缀验证不通过"
                };

            }
            //2.验证后缀位数
            ReceiptCurrentNumber = ModelDTO.ReceiptCurrentNumberView.Remove(0, ModelDTO.Prefix.Length);
            if (ModelDTO.Suffix.Value != ReceiptCurrentNumber.Length)
            {
                return new ResultModel()
                {
                    IsSuccess = false,
                    Msg = "后缀位数验证不通过"
                };

            }
            //验证票据编号


            if (AuthenticationReceiptBookInterval(ModelDTO, propertyMgrUnitOfWork))
            {
                return new ResultModel()
                {
                    IsSuccess = false,
                    Msg = "票据号范围不能有冲突，请检查前缀和起号、止号"
                };
            }

            int ReceiptNumInt = 0;
            string ReceiptMessage = string.Empty;
            if (IsNumberic(ReceiptCurrentNumber, out ReceiptMessage, out ReceiptNumInt))
            {
                if (ReceiptNumInt < ModelDTO.BeginCode || ReceiptNumInt > ModelDTO.EndCode)
                {

                    return new ResultModel()
                    {
                        IsSuccess = false,
                        Msg = "当前票据必须在起号和止号范围内"
                    };


                }
            }
            else
            {
                return new ResultModel()
                {

                    IsSuccess = false,
                    Msg = "验证成功"
                };
            }


            //如果Id大于0.则验证当前票据号是否小于已有票据号
            if (ModelDTO.Id != null && ModelDTO.Id > 0)
            {
                if (!AuthenticationReceiptBookNumberLessThanDataBase(ModelDTO, propertyMgrUnitOfWork))
                {

                    return new ResultModel()
                    {
                        IsSuccess = false,
                        Msg = "当前票据号小于已有票据"
                    };
                }



            }



            return new ResultModel() { IsSuccess = true, Msg = "验证成功" };
        }

        /// <summary>
        /// 验证是否是数字
        /// </summary>
        /// <param name="ReceiptBookNumber"></param>
        /// <param name="Message"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool IsNumberic(string ReceiptBookNumber, out string Message, out int result)
        {
            try
            {

                result = Convert.ToInt32(ReceiptBookNumber);
                Message = "";
                return true;
            }
            catch (Exception ex)
            {
                Message = "票据位：" + ReceiptBookNumber + "格式错误，请输入正确格式";
                result = 0;
                return false;
            }
        }

        /// <summary>
        /// 验证当前票据号是否小于已有票据 
        /// </summary>
        /// <returns></returns>
        private bool AuthenticationReceiptBookNumberLessThanDataBase(ReceiptBookDTO ModelDTO, IPropertyMgrUnitOfWork propertyMgrUnitOfWork)
        {


            var query = (from r in propertyMgrUnitOfWork.ReceiptBookDetailRepository.GetAll().Where(o => o.ReceiptBookId == ModelDTO.Id)
                         select r).OrderByDescending(o => o.Number).FirstOrDefault();

            if (query != null && query.Id > 0)
            {
                //判断字符串
                var IntDataBase = InterceptionReceiptBookCurrentNumber(ModelDTO, query.Number);
                var IntNow = InterceptionReceiptBookCurrentNumber(ModelDTO, ModelDTO.ReceiptCurrentNumberView);
                if (IntNow == ModelDTO.EndCode.Value)
                {//已经使用完毕
                    return true;
                }
                if (IntNow <= IntDataBase)
                {
                    return false;

                }
                return true;
            }
            return true;
        }

        /// <summary>
        /// 截取当前票据的数字位置
        /// </summary>
        /// <param name="ModelDTO"></param>
        /// <returns></returns>
        public int InterceptionReceiptBookCurrentNumber(ReceiptBookDTO ModelDTO, string ReceiptNumber)
        {

            //取消前缀
            return Convert.ToInt32(ReceiptNumber.Remove(0, ModelDTO.Prefix.Length));

        }


        //根据条件获取票据本列表
        private IList<ReceiptBook> GetReceiptBookList(Expression<Func<ReceiptBook, bool>> predicate)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ReceiptBookRepository.GetAll().Where(predicate).ToList();
            }
        }
        #endregion

        #region 当前票据号的处理
        public string HandleCurrendReceiptBookNumber(ReceiptBookDTO ModelDTO)
        {
            //所有的只要起号等于当前票据号 那么票据就是从头开始

            //如果起号等于止号

            var CurrentInt = InterceptionReceiptBookCurrentNumber(ModelDTO, ModelDTO.ReceiptCurrentNumberView) - 1;  //当前显示票据号数字位
            var CurrentIntA = 0;
            if (string.IsNullOrEmpty(ModelDTO.CurrentReceiptNum))
            {
                if (ModelDTO.BeginCode == ModelDTO.EndCode)
                {
                    return "";
                }
                else if (ModelDTO.BeginCode > CurrentInt)
                    return "";
                else
                    CurrentIntA = ModelDTO.BeginCode.Value;
            }
            else
            {
                CurrentIntA = InterceptionReceiptBookCurrentNumber(ModelDTO, ModelDTO.CurrentReceiptNum);
            }

            if (CurrentIntA == ModelDTO.EndCode)
            {
                return CreateReceiptBookNumber(ModelDTO, (CurrentIntA));
            }
            if (CurrentInt < 0)
            {
                return "";
            }

            //生成票据号
            return CreateReceiptBookNumber(ModelDTO, (CurrentInt));
        }


        public string CreateReceiptBookNumber(ReceiptBookDTO receiptbook, int? Num)
        {
            var CurrentCode = Num;
            string CurrentCodestr = CurrentCode.Value.ToString().PadLeft(receiptbook.Suffix.Value, '0');
            return receiptbook.Prefix + CurrentCodestr;

        }


        /// <returns></returns>
        private bool AuthenticationReceiptBookInterval(ReceiptBookDTO receiptbook, IPropertyMgrUnitOfWork propertyMgrUnitOfWork)
        {

            if (receiptbook.Id == null || receiptbook.Id == 0)
                receiptbook.Id = 0;

            var query = propertyMgrUnitOfWork.ReceiptBookRepository.GetAll().Where
                (o => o.Prefix == receiptbook.Prefix && o.Suffix == receiptbook.Suffix && o.DeptId == receiptbook.DeptId && o.Id != receiptbook.Id)
                .Where(o => (o.BeginCode == receiptbook.BeginCode) || (o.BeginCode == receiptbook.EndCode) || (o.EndCode == receiptbook.EndCode) || (o.EndCode == receiptbook.BeginCode) || (o.BeginCode < receiptbook.BeginCode && receiptbook.EndCode < o.EndCode) || (receiptbook.EndCode > o.BeginCode && receiptbook.EndCode < o.EndCode) || (o.EndCode > receiptbook.BeginCode && o.EndCode < receiptbook.EndCode));

            if (query.Count() > 0)
            {
                return true;
            }
            return false;
        }
        #endregion


        #region 验证该票据是否已经被交款
        public ResultModel AuthenticationReBookHasPaymentTasks(string OldReceiptNumber, string NewReceiptNumber)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                ResultModel ResultMoldel = new ResultModel() { IsSuccess = true };


                var query = (from d in propertyMgrUnitOfWork.ReceiptBookDetailRepository.GetAll()
                             join r in propertyMgrUnitOfWork.ReceiptRepository.GetAll() on d.ReceiptId equals r.Id
                             join c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll() on r.Id equals c.ReceiptId
                             join p in propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll() on c.Id equals p.ChargeRecordId
                             join t in propertyMgrUnitOfWork.PaymentTasksRepository.GetAll() on p.PaymentTaskID equals t.Id
                             where d.IsDel == false && r.IsDel == false && c.IsDel == false && p.IsDel == false
                             where d.Number == OldReceiptNumber
                             select t
                                 );

                string ErrorStr = "票据号";
                bool IsError = false;
                if (query.Count() > 0)
                {
                    IsError = true;
                    ErrorStr += OldReceiptNumber;
                    //ResultMoldel.IsSuccess = false;
                    //ResultMoldel.Msg += "票据号：" + OldReceiptNumber + "为已交款票据，不能修改票据号";
                }
                query = (from d in propertyMgrUnitOfWork.ReceiptBookDetailRepository.GetAll()
                         join r in propertyMgrUnitOfWork.ReceiptRepository.GetAll() on d.ReceiptId equals r.Id
                         join c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll() on r.Id equals c.ReceiptId
                         join p in propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll() on c.Id equals p.ChargeRecordId
                         join t in propertyMgrUnitOfWork.PaymentTasksRepository.GetAll() on p.PaymentTaskID equals t.Id
                         where d.IsDel == false && r.IsDel == false && c.IsDel == false && p.IsDel == false
                         where d.Number == NewReceiptNumber
                         select t
                               );

                if (query.Count() > 0)
                {
                    IsError = true;
                    ErrorStr += "  " + NewReceiptNumber;

                    //ResultMoldel.IsSuccess = false;
                    //ResultMoldel.Msg += "    票据号：" + NewReceiptNumber + "为已交款票据，不能修改票据号";
                }
                if (IsError)
                {
                    ResultMoldel.IsSuccess = false;
                    ResultMoldel.Msg = ErrorStr + "为已交款票据，不能修改票据号";
                }

                return ResultMoldel;


            }
        }
        #endregion

        #region 生成票据明细
        public void GenerateReceiptBookDetail(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, int ComDeptId, ChargeRecord chargerecord, Receipt receipt)
        {
            var receiptbook = GetCurrentReceiptBook(propertyMgrUnitOfWork, ComDeptId);
            ReceiptBookIncrease(propertyMgrUnitOfWork, receiptbook, receipt.Number, chargerecord.Amount.Value);
            ReceiptBookDetail receiptBookDetail = new ReceiptBookDetail()
            {
                IsDel = false,
                OperatorName = "",
                ReceiptAmount = chargerecord.Amount,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Number = receipt.Number,
                ReceiptBookId = receiptbook.Id,
                ReceiptId = receipt.Id

            };
            propertyMgrUnitOfWork.ReceiptBookDetailRepository.Add(receiptBookDetail);

        }
        #endregion

        #region 已生成票据修改（包含票据号互换）
        public void ReceiptBookModifyNumber(ReceiptBookModifyDTO receiptbookmodifyDTO, ReceiptBook receiptbook, string OperatorName, int oldReceiptBookId = 0)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                ReceiptBookHistory receiptbookhistory = new ReceiptBookHistory()
                {
                    DeptId = receiptbook.DeptId,
                    CreateTime = DateTime.Now,
                    IsDel = false,
                    ReceiptBookHistoryType = (int)ReceiptBookHistoryTypeEnum.ReceiptModify,
                    OperatorName = OperatorName,
                    UpdateTime = DateTime.Now
                };
                if (receiptbookmodifyDTO.IsExChange)
                {
                    //取出数据

                    ProcessReceiptBookModifyNumber(oldReceiptBookId, receiptbook.Id, receiptbookmodifyDTO, propertyMgrUnitOfWork, receiptbook, OperatorName);
                    receiptbookhistory.OperatorContent = "票据号" + receiptbookmodifyDTO.OldReceiptNum + "和 " + receiptbookmodifyDTO.NewReceiptNum + "互换票据编号。（" + receiptbookmodifyDTO.Remark + "）";

                }
                else
                {//纯更换
                 //更换
                    if (receiptbookmodifyDTO.IsCurrent)
                    {//新票最近票据

                        var NewInt = InterceptionReceiptBookCurrentNumber(ReceiptBookMappers.ChangeReceiptBookToDTO(receiptbook), receiptbookmodifyDTO.NewReceiptNum);
                        var CurrentView = CreateReceiptBookNumber(ReceiptBookMappers.ChangeReceiptBookToDTO(receiptbook), NewInt + 1);
                        ProcessReceiptBookModifyNumber(oldReceiptBookId, receiptbook.Id, receiptbookmodifyDTO, propertyMgrUnitOfWork, receiptbook, OperatorName);
                        receiptbookhistory.OperatorContent = "票据号" + receiptbookmodifyDTO.OldReceiptNum + "修改为 " + receiptbookmodifyDTO.NewReceiptNum + ",当前票据号修改为：" + CurrentView + "。 （" + receiptbookmodifyDTO.Remark + "）";


                    }
                    else
                    {
                        //需要修改
                        ProcessReceiptBookModifyNumber(oldReceiptBookId, receiptbook.Id, receiptbookmodifyDTO, propertyMgrUnitOfWork, receiptbook, OperatorName);
                        receiptbookhistory.OperatorContent = "票据号" + receiptbookmodifyDTO.OldReceiptNum + "修改为 " + receiptbookmodifyDTO.NewReceiptNum + "。（" + receiptbookmodifyDTO.Remark + "）";
                    }


                }
                propertyMgrUnitOfWork.ReceiptBookHistoryRepository.Add(receiptbookhistory);
                propertyMgrUnitOfWork.Commit();

            }


        }

        private void ProcessReceiptBookModifyNumber(int? OldReceiptBookId, int? NewReciptBookId, ReceiptBookModifyDTO receiptbookmodifyDTO, IPropertyMgrUnitOfWork propertyMgrUnitOfWork, ReceiptBook receiptbook, string OperatorName)
        {

            var OldReceiptNumber = propertyMgrUnitOfWork.ReceiptBookDetailRepository.GetAll().Where(o => o.IsDel == false && o.ReceiptBookId == OldReceiptBookId && o.Number == receiptbookmodifyDTO.OldReceiptNum).FirstOrDefault();
            //  OldReceiptNumber = propertyMgrUnitOfWork.ReceiptBookDetailRepository.GetByKey(OldReceiptNumber.Id);
            var NewReceiptNumber = propertyMgrUnitOfWork.ReceiptBookDetailRepository.GetAll().Where(o => o.IsDel == false && o.ReceiptBookId == receiptbook.Id && o.Number == receiptbookmodifyDTO.NewReceiptNum).FirstOrDefault();
            string IntermediateVariables = string.Empty;
            decimal? IntermediateDceimal = 0m;
            if (receiptbookmodifyDTO.IsExChange)
            {

                //首先修改ReceiptBookDetail

                IntermediateVariables = OldReceiptNumber.ReceiptId;
                OldReceiptNumber.ReceiptId = NewReceiptNumber.ReceiptId;
                NewReceiptNumber.ReceiptId = IntermediateVariables;
                IntermediateDceimal = OldReceiptNumber.ReceiptAmount;
                OldReceiptNumber.ReceiptAmount = NewReceiptNumber.ReceiptAmount;
                NewReceiptNumber.ReceiptAmount = IntermediateDceimal;
                propertyMgrUnitOfWork.ReceiptBookDetailRepository.Update(OldReceiptNumber);
                propertyMgrUnitOfWork.ReceiptBookDetailRepository.Update(NewReceiptNumber);
                //获取票据号
                var OldReceipt = propertyMgrUnitOfWork.ReceiptRepository.GetAll().Where(o => o.IsDel == false && o.Id == OldReceiptNumber.ReceiptId).FirstOrDefault();
                var NewReceipt = propertyMgrUnitOfWork.ReceiptRepository.GetAll().Where(o => o.IsDel == false && o.Id == NewReceiptNumber.ReceiptId).FirstOrDefault();
                IntermediateVariables = OldReceipt.Number;

                OldReceipt.Number = NewReceipt.Number;
                NewReceipt.Number = IntermediateVariables;
                propertyMgrUnitOfWork.ReceiptRepository.Update(OldReceipt);
                propertyMgrUnitOfWork.ReceiptRepository.Update(NewReceipt);
            }
            else
            {
                //判断新票据号是否存在

                //取出旧票据和新票据

                if (NewReceiptNumber != null && NewReceiptNumber.Id > 0)
                {
                    IntermediateVariables = OldReceiptNumber.ReceiptId;
                    OldReceiptNumber.ReceiptId = NewReceiptNumber.ReceiptId;
                    NewReceiptNumber.ReceiptId = IntermediateVariables;
                    propertyMgrUnitOfWork.ReceiptBookDetailRepository.Update(OldReceiptNumber);
                    propertyMgrUnitOfWork.ReceiptBookDetailRepository.Update(NewReceiptNumber);
                    //获取票据号
                    var OldReceipt = propertyMgrUnitOfWork.ReceiptRepository.GetAll().Where(o => o.IsDel == false && o.Id == OldReceiptNumber.ReceiptId).FirstOrDefault();
                    var NewReceipt = propertyMgrUnitOfWork.ReceiptRepository.GetAll().Where(o => o.IsDel == false && o.Id == NewReceiptNumber.ReceiptId).FirstOrDefault();

                    IntermediateVariables = OldReceipt.Number;
                    OldReceipt.Number = NewReceipt.Number;
                    NewReceipt.Number = IntermediateVariables;
                    propertyMgrUnitOfWork.ReceiptRepository.Update(OldReceipt);
                    propertyMgrUnitOfWork.ReceiptRepository.Update(NewReceipt);
                }
                else
                {
                    //否则就是生成一个新的票据进行交换
                    ReceiptBookDetail NewReceiptBookDetail = new ReceiptBookDetail()
                    {
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        IsDel = false,
                        Number = receiptbookmodifyDTO.NewReceiptNum,
                        OperatorName = OperatorName,
                        ReceiptAmount = OldReceiptNumber.ReceiptAmount,
                        ReceiptId = OldReceiptNumber.ReceiptId,
                        ReceiptBookId = NewReciptBookId

                    };
                    var OldReceipt = propertyMgrUnitOfWork.ReceiptRepository.GetAll().Where(o => o.IsDel == false && o.Id == OldReceiptNumber.ReceiptId).FirstOrDefault();
                    OldReceiptNumber.ReceiptId = "";
                    OldReceiptNumber.ReceiptAmount = 0;
                    var NewReceiptBook = propertyMgrUnitOfWork.ReceiptBookRepository.GetByKey(NewReciptBookId);
                    if (receiptbookmodifyDTO.IsCurrent)
                        NewReceiptBook.CurrentReceiptNum = receiptbookmodifyDTO.NewReceiptNum;
                    NewReceiptBook.UsedNumber += 1;

                    OldReceipt.Number = receiptbookmodifyDTO.NewReceiptNum;

                    propertyMgrUnitOfWork.ReceiptBookDetailRepository.Add(NewReceiptBookDetail);
                    propertyMgrUnitOfWork.ReceiptBookDetailRepository.Update(OldReceiptNumber);
                    propertyMgrUnitOfWork.ReceiptBookRepository.Update(NewReceiptBook);
                    propertyMgrUnitOfWork.ReceiptRepository.Update(OldReceipt);



                }








            }

        }
        #endregion


        #endregion

        #region 预存抵扣

        /// <summary>
        /// 预存抵扣
        /// 修改:2017-04-12
        /// 用户预存费余额即存在“全部收费项目”，又存在单独收费项目的预存余额时，首先抵扣收费项目对应的预存费，再抵扣“全部收费项目”
        /// </summary>
        /// <param name="pmUnitWork">工作单元</param>
        /// <param name="chargBill">账单</param>
        /// <param name="deductionAmount">应该付金额</param>
        /// <param name="ChargeRecordId">费用流水ID</param>
        /// <returns>返回已划账金额</returns>
        public PrepayDeductionResult AutomaticDeduction(IPropertyMgrUnitOfWork pmUnitWork, ChargBill chargBill, decimal deductionAmount, string ChargeRecordId)
        {
            PrepayDeductionResult pResult = new PrepayDeductionResult();
            pResult.ChargeBillId = chargBill.Id;
            pResult.HouseDeptId = chargBill.HouseDeptId;
            //1.先从费项目预存账户抵扣
            PrepayAccount subjectAccount = pmUnitWork
            .PrepayAccountRepository
            .GetAll()
            .Where(p => p.HouseDeptId == chargBill.HouseDeptId && p.IsDel == false && p.ChargeSubjectID == chargBill.ChargeSubjectId)
            .FirstOrDefault();
            decimal amount = 0;//要计算的划账金额

            if (subjectAccount != null)
            {
                if (deductionAmount < subjectAccount.Balance)
                {
                    //划账金额 = 应付金额
                    amount = deductionAmount;
                    //全部划账成功 直接返回
                    pResult.TotalDeductionAmount = amount;
                    pResult.DeductionDetailList.Add(new DeductionDetail() { SubjectId = subjectAccount.ChargeSubjectID, DeductionAmount = amount, PrepayAccountId = subjectAccount.Id });
                    //收费项目账户 全部划账
                    ResultModel result1 = SubjectBalanceAdd(pmUnitWork, chargBill,
                        deductionAmount, PayTypeEnum.InternalTransfer, ChargeRecordId, "系统自动划账",
                        BalanceTypeEnum.Payment, chargBill.ChargeSubjectId.Value);
                    //不成功抛出异常
                    if (!result1.IsSuccess)
                    {
                        throw new CompositeException("系统自动划账-收费项目账户自动划账失败 " + result1.Msg);
                    }
                    return pResult;
                }
                //收费项目账户 部分划账
                else
                {
                    if (subjectAccount.Balance != 0)
                    {
                        //应付金额剩余部分 = 应付金额 - 抵扣部分
                        deductionAmount = (deductionAmount - subjectAccount.Balance.Value);
                        //划账金额 = 抵扣金额
                        amount = subjectAccount.Balance.Value;
                        //返回值
                        pResult.TotalDeductionAmount = amount;
                        pResult.DeductionDetailList.Add(new DeductionDetail() { SubjectId = subjectAccount.ChargeSubjectID, DeductionAmount = amount, PrepayAccountId = subjectAccount.Id });
                        //划收费项目账
                        ResultModel result1 = SubjectBalanceAdd(pmUnitWork, chargBill,
                            subjectAccount.Balance.Value, PayTypeEnum.InternalTransfer, ChargeRecordId, "系统自动划账",
                            BalanceTypeEnum.Payment, chargBill.ChargeSubjectId.Value);
                        //不成功抛出异常
                        if (!result1.IsSuccess)
                        {
                            throw new CompositeException("系统自动划账-收费项目账户自动划账失败 " + result1.Msg);
                        }
                    }

                    //收费项目预存账户余额为0 通知用户 预存催缴
                    //BillCommonService.Instance.SendNotification(chargBill.ComDeptId.Value, chargBill.HouseDeptId.Value);
                }
            }
            //应付金额剩余 == 0
            if (deductionAmount == 0)
            {
                return pResult;
            }


            //2.所有收费项目预存账户抵扣
            PrepayAccount prepayAccount = pmUnitWork
                .PrepayAccountRepository
                .GetAll()
                .Where(p => p.HouseDeptId == chargBill.HouseDeptId && p.IsDel == false && p.ChargeSubjectID == 0)
                .FirstOrDefault();
            //如果不存在预存账户 返回0
            if (prepayAccount == null)
            {
                return pResult;
            }
            //如果剩余应付金额 小于账户余额
            if (deductionAmount < prepayAccount.Balance)
            {
                //划账金额 += 剩余抵扣金额
                amount += deductionAmount;
                pResult.TotalDeductionAmount = amount;
                pResult.DeductionDetailList.Add(new DeductionDetail() { SubjectId = prepayAccount.ChargeSubjectID, DeductionAmount = deductionAmount, PrepayAccountId = prepayAccount.Id });
                //划账
                ResultModel result = BalanceAdd(pmUnitWork, chargBill, deductionAmount, PayTypeEnum.InternalTransfer, ChargeRecordId, "系统自动划账", BalanceTypeEnum.Payment);
                //不成功抛出异常
                if (!result.IsSuccess)
                {
                    throw new CompositeException("系统自动划账-全部收费项目账户自动划账失败 " + result.Msg);
                }
            }
            else
            {
                if (prepayAccount.Balance != 0)
                {
                    //划账金额 += 剩余抵扣金额
                    amount += prepayAccount.Balance.Value;
                    pResult.DeductionDetailList.Add(new DeductionDetail() { SubjectId = prepayAccount.ChargeSubjectID, DeductionAmount = prepayAccount.Balance.Value, PrepayAccountId = prepayAccount.Id });
                    pResult.TotalDeductionAmount = amount;
                    //划账
                    ResultModel result = BalanceAdd(pmUnitWork, chargBill, prepayAccount.Balance.Value, PayTypeEnum.InternalTransfer, ChargeRecordId, "系统自动划账", BalanceTypeEnum.Payment);

                    //不成功抛出异常
                    if (!result.IsSuccess)
                    {
                        throw new CompositeException("系统自动划账-全部收费项目账户自动划账失败 " + result.Msg);
                    }
                }

                //全部收费项目账户余额为0 通知用户 预存催缴
                //BillCommonService.Instance.SendNotification(chargBill.ComDeptId.Value, chargBill.HouseDeptId.Value);
            }

            //返回划账金额
            return pResult;
        }

        #endregion

        #region 三表读数记录

        public void AddMeterReadRecord(IPropertyMgrUnitOfWork mrUnitOfWork, Meter meterItem, MeterReadRecord lastReadRecord, decimal meterVal, string billID)
        {
            MeterReadRecord newRecord = new MeterReadRecord();
            newRecord.IsBill = true;
            newRecord.MeterId = meterItem.Id;
            //如果读数异常 记录上一次读数
            if (meterVal == 0)
            {
                newRecord.MeterValue = lastReadRecord.MeterValue;
                newRecord.Remark = "异常抄表，本次抄表读数为：" + meterItem.MeterValue;
            }
            else
            {
                newRecord.MeterValue = meterItem.MeterValue;
            }

            newRecord.NextRefID = null;
            newRecord.ReadDate = meterItem.ReadDate;
            newRecord.BillID = billID;
            newRecord.CreateTime = DateTime.Now;
            mrUnitOfWork.MeterReadRecordRepository.Add(newRecord);
        }

        #endregion

        #region 个性化配置

        /// <summary>
        /// 获取小区配置
        /// </summary>
        public CommunityConfig GetCommunityConfig(IPropertyMgrUnitOfWork pmUnitWork, int? commDeptId)
        {
            var config = pmUnitWork.CommunityConfigRepository.GetAll().Where(o => o.ComDeptId == commDeptId).FirstOrDefault();
            if (config == null)
            {
                config = new CommunityConfig();
                config.ComDeptId = commDeptId;
                config.ConfigType = (int)ConfigTypeEnum.Common;
                config.Id = 0;
                config.IsBuilding = true;
                config.IsFloor = true;
                config.IsNumber = true;
                config.IsUnit = true;
                //添加配置 v2.9 2017-9-13
                config.IsDefaultPrintReceipt = true;
                config.IsChargeConfirm = true;
                config.IsPreAutomaticDeduction = true;
                config.IsPreMergeChargeRecord = true;
            }
            return config;
        }

        #endregion

    }
}
