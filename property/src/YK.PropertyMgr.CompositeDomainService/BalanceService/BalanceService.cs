using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using Microsoft.Practices.Unity;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;

namespace YK.PropertyMgr.CompositeDomainService
{
    /// <summary>
    /// 余额处理服务
    /// </summary>
    public class BalanceService : IBalanceService
    {
        #region 单例

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static BalanceService Instance { get { return SingletonInstance; } }
        private static readonly BalanceService SingletonInstance = new BalanceService();

        #endregion

        #region IBalanceService 接口部分

        /// <summary>
        /// 余额缴费
        /// </summary>
        /// <param name="ChargBill">缴费账单ID集合</param>
        /// <param name="Amount">缴费金额</param>
        /// <param name="PayType">支付方式</param>
        /// <param name="Remark">备注</param>
        /// <returns>处理结果</returns>
        public ResultModel BalancePayment(string[] BillIDs, decimal Amount, PayTypeEnum PayType, string Remark, int Operator, string OperatorName)
        {
            //throw new NotImplementedException();
            return new ResultModel(){ IsSuccess = false, ErrorCode = "800", Msg = "暂不支持预存费缴费"};
        }


        /// <summary>
        /// 余额充值
        /// 指用户自助充值 和 后台充值（APP 或 自助缴费 非小区手动交预缴费）
        /// </summary>
        /// <param name="BalanceInfo">充值信息</param>
        /// <param name="PayType">支付方式</param>
        /// <param name="Remark">备注</param>
        /// <param name="IsInitBalance">true:余额初始化 false:余额充值</param>
        /// <returns>处理结果</returns>
        public ResultModel BalanceRecharge(BalanceInfo BalanceInfo, PayTypeEnum PayType, string Remark, int Operator, string OperatorName, bool IsInitBalance = false)
        {
            if (BalanceInfo.Amount == 0 && !IsInitBalance)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "810", Msg = "充值金额不能为零" };
            }
            if (BalanceInfo.Amount <= 0 && IsInitBalance)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "810", Msg = "余额初始化金额不能小于等于零" };
            }
            try
            {
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    //生成预存缴费账单
                    ChargBill chargBill = BillCommonService.Instance.GeneratePrepaymentBill(propertyMgrUnitOfWork, 
                        BalanceInfo.ComDeptId, BalanceInfo.HouseDeptId, BalanceInfo.ResourcesId, BalanceInfo.Amount
                        ,BillStatusEnum.Paid, "生成余额充值预付款账单", BalanceInfo.ChargeSubjectId);

                    //生成单据
                    Receipt receipt = BillCommonService.Instance.GenerateReceipt(propertyMgrUnitOfWork, "", 0, BillCommonService.SystemOperatorName, ReceiptStatusEnum.Paid,null);
                    //账单明细部分
                    IList<ChargBill> chargBillList = new List<ChargBill>();
                    chargBillList.Add(chargBill);
                    Dictionary<string, decimal> amountD = new Dictionary<string, decimal>();
                    amountD.Add(chargBill.Id, BalanceInfo.Amount);
                    //生成缴费记录
                    ChargeRecord chargeRecord = BillCommonService.Instance.GenerateChargeRecord(propertyMgrUnitOfWork, chargBillList
                        , (IsInitBalance ? ChargeTypeEnum.InitBalance : ChargeTypeEnum.DailyCharge), PayType, receipt.Id
                        , BalanceInfo.Amount, Remark
                        , amountD
                        , null, Operator, OperatorName);

                    //确定收费项目
                    if (!string.IsNullOrEmpty(BalanceInfo.ChargeSubjectName))
                    {
                        var sbj = propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll()
                            .Where(r => r.Name == BalanceInfo.ChargeSubjectName 
                            && r.ComDeptId == BalanceInfo.ComDeptId
                            && r.IsDel == false).FirstOrDefault();
                        if (sbj != null)
                        {
                            BalanceInfo.ChargeSubjectId = sbj.Id.Value;
                        }
                    }

                    //存入余额
                    ResultModel result = BillCommonService.Instance.SubjectBalanceAdd(propertyMgrUnitOfWork
                        , chargBill, BalanceInfo.Amount, PayType
                        , chargeRecord.Id, BalanceInfo.Remark, BalanceTypeEnum.Recharge, BalanceInfo.ChargeSubjectId);
                    if (!result.IsSuccess)
                    {
                        return result;
                    }
                    //余额初始化 生成交款记录
                    if (IsInitBalance)
                    {
                        IList<ChargeRecord> chargeRecordList = new List<ChargeRecord>();
                        chargeRecordList.Add(chargeRecord);
                        BillCommonService.Instance.GenerateBillPaymentTask(propertyMgrUnitOfWork, chargeRecordList, PaymentTaskStatusEnum.Audited, "余额初始化交款",Operator,OperatorName,DateTime.Now, IsInitBalance);
                    }
                    //提交事务
                    propertyMgrUnitOfWork.Commit();
                    return new ResultModel() { IsSuccess = true, ErrorCode = "0", Data = result.Data, Msg = IsInitBalance?"余额初始化成功": "余额充值成功" };
                }
            }
            catch (Exception ex)
            {
                //记录日志
                LogProperty.WriteLoginToFile(string.Format("[余额充值]Code:900 HouseDeptId:{0} Amount:{1} IsInitBalance:{2} ErrorMsg:{3}", BalanceInfo.HouseDeptId, BalanceInfo.Amount, IsInitBalance,ex.Message), "BalanceService", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = IsInitBalance? "余额初始化失败" : "余额充值失败" };
            }
        }

        /// <summary>
        /// 余额初始化导入
        /// 如果存在 跳过并返回
        /// </summary>
        /// <param name="BalanceInfoList">导入信息列表</param>
        /// <returns>处理结果 和 未初始化的导入信息</returns>
        public ResultModel BalanceInitialization(IList<BalanceInfo> BalanceInfoList, int Operator, string OperatorName)
        {
            if (BalanceInfoList.Count() < 1)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "970", Msg = "导入数据不能为空" };
            }
            int?[] houseDeptIds = BalanceInfoList.Select(b => (int?)b.HouseDeptId).ToArray();
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //已有账户
                //IList<PrepayAccount> eAccountList = propertyMgrUnitOfWork.PrepayAccountRepository
                //    .GetAll().Where(p => houseDeptIds.Contains(p.HouseDeptId)).ToList();
                //int[] eArray = eAccountList.Select(e => e.HouseDeptId.Value).ToArray();
                //错误集合返回
                //IList<BalanceInfo> ebList = BalanceInfoList.Where(b => eArray.Contains(b.HouseDeptId)).ToList();
                //IList<BalanceInfo> ebList = BalanceInfoList.Where(b => eAccountList.Any(e => e.HouseDeptId == b.HouseDeptId && e.ChargeSubjectID == b.ChargeSubjectId)).ToList();
                //foreach (BalanceInfo eItem in ebList)
                //{
                //    eItem.ErrorMsg = "该账户已初始化";
                //}
                //不存在的初始化
                //IList<BalanceInfo> nbList = BalanceInfoList.Where(b => !eArray.Contains(b.HouseDeptId)).ToList();
                //IList<BalanceInfo> nbList = BalanceInfoList.Where(b => !eAccountList.Any(e => e.HouseDeptId == b.HouseDeptId && e.ChargeSubjectID == b.ChargeSubjectId)).ToList();
                IList<BalanceInfo> ebList = new List<BalanceInfo>();
                foreach (var nItem in BalanceInfoList)
                {
                    //余额初始化 改为内部划账 2017-3-7
                    ResultModel result = BalanceRecharge(nItem, PayTypeEnum.InternalDebit, "余额初始化",Operator, OperatorName, true);
                    if (!result.IsSuccess)
                    {
                        nItem.ErrorMsg = result.Msg;
                        ebList.Add(nItem);
                    }
                }
                //判断错误集合count是否大于0
                if (ebList.Count() > 0)
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "980", Msg = "余额导入出现错误", Data = ebList };
                }
                return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "余额导入成功" };
            }
        }

        private ChargBill GenerateBalanceTransferBill(IPropertyMgrUnitOfWork pUnitWork, BalanceInfo balanceInfo
            , string remark, int Operator, string OperatorName, ref ChargeRecord chargeRecord)
        {
            //生成预存缴费账单
            ChargBill chargBill = BillCommonService.Instance.GeneratePrepaymentBill(pUnitWork,
                balanceInfo.ComDeptId, balanceInfo.HouseDeptId, balanceInfo.ResourcesId, balanceInfo.Amount
                , BillStatusEnum.Paid, remark, balanceInfo.ChargeSubjectId);

            //生成单据
            Receipt receipt = BillCommonService.Instance.GenerateReceipt(pUnitWork, "", 0, BillCommonService.SystemOperatorName, ReceiptStatusEnum.Paid, null);
            //账单明细部分
            IList<ChargBill> chargBillList = new List<ChargBill>();
            chargBillList.Add(chargBill);
            Dictionary<string, decimal> amountD = new Dictionary<string, decimal>();
            amountD.Add(chargBill.Id, balanceInfo.Amount);

            //生成缴费记录
            chargeRecord = BillCommonService.Instance.GenerateChargeRecord(pUnitWork, chargBillList
                , ChargeTypeEnum.BalanceTransfer, PayTypeEnum.InternalDebit, receipt.Id
                , balanceInfo.Amount, remark
                , amountD
                , null, Operator, OperatorName);

            return chargBill;
        }

        /// <summary>
        /// 余额转移
        /// </summary>
        /// <param name="preAccountCostTransfer"></param>
        /// <returns></returns>
        public ResultModel BalanceTransfer(PreAccountCostTransferDTO transfer)
        {
            using (var pUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //初始化信息
                var account = pUnitWork.PrepayAccountRepository.GetByKey(transfer.PrepayAccountId);
                BalanceInfo outInfo = new BalanceInfo()
                {
                    ComDeptId = account.CommDeptID.Value,
                    HouseDeptId = account.HouseDeptId.Value,
                    ResourcesId = account.HouseDeptId.Value,
                    ChargeSubjectId = account.ChargeSubjectID.Value,
                    Amount = account.Balance.Value
                };
                BalanceInfo inInfo = new BalanceInfo()
                {
                    ComDeptId = account.CommDeptID.Value,
                    HouseDeptId = account.HouseDeptId.Value,
                    ResourcesId = account.HouseDeptId.Value,
                    ChargeSubjectId = transfer.ChargeSubjectId.Value,
                    Amount = account.Balance.Value
                };

                //1、生成账单
                ChargeRecord record = new ChargeRecord();
                ChargBill bill = GenerateBalanceTransferBill(pUnitWork, inInfo, "[预存转移]", transfer.Operator, transfer.OperatorName, ref record);
                string desc = bill.ResourcesName +"  "+ transfer.OutChargeSubjectName + " 转入 " + transfer.InChargeSubjectName + "  " + outInfo.Amount.ToString();
                bill.Remark = desc;
                //2、转出账户
                //ChargeRecord outRecord = new ChargeRecord();
                //ChargBill outBill = GenerateBalanceTransferBill(pUnitWork, outInfo, "[预存转移-转出]", transfer.Operator, transfer.OperatorName, ref outRecord);
                var outresult = BillCommonService.Instance.SubjectBalanceAdd(pUnitWork, bill, outInfo.Amount, PayTypeEnum.InternalDebit,
                    record.Id, "[余额转移-转出]", BalanceTypeEnum.Payment, outInfo.ChargeSubjectId);
                if (!outresult.IsSuccess)
                {
                    return outresult;
                }
                //3、转入账户
                var inresult = BillCommonService.Instance.SubjectBalanceAdd(pUnitWork, bill, inInfo.Amount, PayTypeEnum.InternalDebit,
                    record.Id, "[余额转移-转入]", BalanceTypeEnum.Recharge, inInfo.ChargeSubjectId);
                if (!inresult.IsSuccess)
                {
                    return inresult;
                }
                //4、写日志
                PrepayAccountLog log = new PrepayAccountLog();
                log.ComDeptId = bill.ComDeptId;
                log.HouseDeptId = bill.HouseDeptId;
                log.ResourcesId = bill.ResourcesId;
                log.PrepayAccountId = transfer.PrepayAccountId;
                log.Operator = transfer.OperatorName;
                log.OperatorId = transfer.Operator;
                log.Desc = desc;
                log.OperationTime = DateTime.Now;
                log.Remark = transfer.Remark;
                pUnitWork.PrepayAccountLogRepository.Add(log);
                //5、提交
                pUnitWork.Commit();
                return new ResultModel() { IsSuccess = true, Msg = "预存账户转移成功", ErrorCode = "0" };
            }
        }

        #endregion
    }
}
