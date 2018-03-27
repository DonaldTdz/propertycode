using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using YK.BackgroundMgr.DomainInterface;
using Microsoft.Practices.Unity;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.CompositeDomainService.GenerateBillService;
using System.Collections;
using System.Data;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.Settlement.BusinessComponent;
using YK.Settlement.DBBusinessComponent;
using System.Configuration;
using YK.PropertyMgr.CompositeDomainService.Model;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationMapper;

namespace YK.PropertyMgr.CompositeDomainService
{
    public class PaymentService : IPayment
    {
        #region 单例

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static PaymentService Instance { get { return SingletonInstance; } }
        private static readonly PaymentService SingletonInstance = new PaymentService();

        #endregion

        #region 私有方法和属性        

        #region 单个账单付款

        private void SingleBillPayment(IPropertyMgrUnitOfWork pmUnitWork, ChargBill bill, PayTypeEnum payType, string chargeRecordId, bool isNew)
        {
            //更改账单状态
            bill.Status = BillStatusEnum.Paid.GetHashCode();
            //应收金额 = 计费金额+滞纳金-已交金额-减免金额
            decimal amount = Math.Round(bill.BillAmount.Value + bill.PenaltyAmount.Value - bill.ReceivedAmount.Value - bill.ReliefAmount.Value, 2);
            bill.ReceivedAmount = (bill.ReceivedAmount + amount);//已收金额
            bill.UpdateTime = DateTime.Now;
            //如果没有收费项目 先获取
            if (bill.ChargeSubject == null)
            {
                bill.ChargeSubject = pmUnitWork.ChargeSubjectRepository.GetByKey(bill.ChargeSubjectId);
            }

            if (isNew)
            {
                //新增账单
                pmUnitWork.ChargBillRepository.Add(bill);
                BillCommonService.Instance.GenerateChargeSubjectSna(pmUnitWork, bill, bill.ChargeSubject);
            }
            else
            {
                //修改账单
                pmUnitWork.ChargBillRepository.Update(bill);
            }

            //如果收费项目时预存费 需要更新余额
            if (bill.ChargeSubject.SubjectType == SubjectTypeEnum.SystemPreset.GetHashCode())
            {
                bill.PreChargeSubjectId = bill.PreChargeSubjectId ?? 0;
                string preRemark = "";
                if (bill.PreChargeSubjectId == 0)
                {
                    preRemark = string.Format("所有收费项目 预存{0}个月", bill.Months);
                }
                else
                {
                    var sobj = pmUnitWork.ChargeSubjectRepository.GetByKey(bill.PreChargeSubjectId);
                    if (sobj != null && sobj.IsDel == false)
                    {
                        preRemark = string.Format("{0} 预存{1}个月", sobj.Name, bill.Months);
                    }
                    else//如果收费项目删除或不存在 存入所有收费项目
                    {
                        preRemark = string.Format("所有收费项目 预存{0}个月", bill.Months);
                    }
                }
                //存入余额
                ResultModel result = BillCommonService.Instance.SubjectBalanceAdd(pmUnitWork,
                    bill, amount, payType, chargeRecordId, preRemark, BalanceTypeEnum.Recharge, bill.PreChargeSubjectId.Value);
                //不成功抛出异常
                if (!result.IsSuccess)
                {
                    throw new CompositeException("用户预存-预存余额失败 " + result.Msg);
                }
            }
        }

        public ResultModel SingleRevokeCharge(string chargeRecordId, int Operator, string OperatorName)
        {
            try
            {/*
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    //判断是否交款

                    var chargeRecord = propertyMgrUnitOfWork.ChargeRecordRepository
                        .GetAll()
                        .Where(c => c.Id == chargeRecordId)
                        .FirstOrDefault();
                    //逻辑删除费用记录
                    chargeRecord.IsDel = true;
                    chargeRecord.Operator = Operator;
                    chargeRecord.OperatorName = OperatorName;
                    chargeRecord.UpdateTime = DateTime.Now;
                    propertyMgrUnitOfWork.ChargeRecordRepository.Update(chargeRecord);
                    //撤销单据
                    var receipt = propertyMgrUnitOfWork.ReceiptRepository
                        .GetAll()
                        .Where(r => r.Id == chargeRecord.ReceiptId)
                        .FirstOrDefault();
                    var receiptStatus = (ReceiptStatusEnum)receipt.Status;
                    //已退款，撤销单据
                    if (receiptStatus == ReceiptStatusEnum.Refunded)
                    {
                        receipt.Status = ReceiptStatusEnum.Paid.GetHashCode();
                    }
                    //否则 删除单据
                    else
                    {
                        receipt.IsDel = true;
                    }
                    receipt.Operator = Operator;
                    receipt.OperatorName = OperatorName;
                    receipt.UpdateTime = DateTime.Now;
                    propertyMgrUnitOfWork.ReceiptRepository.Update(receipt);
                    //回滚账单状态
                    var chargeBill = propertyMgrUnitOfWork.ChargBillRepository
                        .GetAll()
                        .Where(c => c.Id == chargeRecord.ChargeBillId)
                        .FirstOrDefault();
                    //如果撤销退款 账单变已付
                    if (receiptStatus == ReceiptStatusEnum.Refunded)
                    {
                        chargeBill.Status = BillStatusEnum.Paid.GetHashCode();
                    }
                    //否则账单 变未付款
                    else
                    {
                        chargeBill.Status = BillStatusEnum.NoPayment.GetHashCode();
                    }
                    chargeBill.UpdateTime = DateTime.Now;
                    propertyMgrUnitOfWork.ChargBillRepository.Update(chargeBill);
                    //回滚账户余额
                    if (receiptStatus == ReceiptStatusEnum.Refunded)
                    {
                        ResultModel result = BillCommonService.Instance.BalanceAdd(propertyMgrUnitOfWork, chargeBill, chargeRecord.Amount, PayTypeEnum.InternalTransfer, chargeRecordId, "撤销退款扣除", BalanceTypeEnum.Payment);
                        if (!result.IsSuccess)
                        {
                            return result;
                        }
                    }
                    else
                    {
                        ResultModel result = BillCommonService.Instance.BalanceAdd(propertyMgrUnitOfWork, chargeBill, chargeRecord.Amount, PayTypeEnum.InternalTransfer, chargeRecordId, "撤销账单退款", BalanceTypeEnum.Recharge);
                        if (!result.IsSuccess)
                        {
                            return result;
                        }
                    }
                }*/
                return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "撤销收费成功" };
            }
            catch (Exception ex)
            {
                //记录日志
                LogProperty.WriteLoginToFile(string.Format("[撤销收费]Code:900  ChargeRecordID:{0} ErrorMsg:{1}", chargeRecordId, ex.Message), "PaymentService", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "撤销收费失败", Data = chargeRecordId };
            }
        }

        #endregion

        #region 生成找零预存账单

        /// <summary>
        /// 生成找零预存账单
        /// </summary>
        /// <param name="propertyMgrUnitOfWork">工作单元</param>
        /// <param name="comDeptId">小区ID</param>
        /// <param name="amount">预缴费</param>
        private ChargBill GenerateChangePrepaymentBill(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, int comDeptId,
            int houseDeptId, int resourcesId, decimal amount, PayTypeEnum payType, string chargeRecordId,
            int Operator, string OperatorName, int PreChargeSubjectId)
        {
            string preremark = string.Empty;
            if (PreChargeSubjectId == 0)
            {
                preremark = "找零预存到全部收费项目";
            }
            else
            {
                var subject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(PreChargeSubjectId);
                if (subject != null)
                {
                    preremark = "找零预存到" + subject.Name;
                }
                else
                {
                    preremark = "找零预存到全部收费项目";
                }
            }
            //生成找零预存缴费账单
            ChargBill chargBill = BillCommonService.Instance.GeneratePrepaymentBill(propertyMgrUnitOfWork, comDeptId,
                houseDeptId, resourcesId, amount, BillStatusEnum.Paid, preremark, PreChargeSubjectId);
            //存入余额
            ResultModel result = BillCommonService.Instance.SubjectBalanceAdd(propertyMgrUnitOfWork, chargBill, amount
                , payType, chargeRecordId, preremark, BalanceTypeEnum.Recharge, PreChargeSubjectId);
            //不成功抛出异常
            if (!result.IsSuccess)
            {
                throw new CompositeException("找零预存-预存余额失败 " + result.Msg);
            }
            return chargBill;
        }

        #endregion

        #region 多账单缴费

        private bool CheckFutureBill(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, IList<ChargBill> billList, IList<ChargBill> onlyFutureBillList)
        {
            DateTime currentMonthEnd = DateTime.Today;//(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)).AddMonths(1).AddDays(-1);
            var futureList = billList.Where(b => b.BeginDate > currentMonthEnd).ToList();
            if (futureList.Count() > 0)
            {
                var fmaxList = (from f in futureList
                                group new { f.ChargeSubjectId, f.ResourcesId, f.Id, f.EndDate } by new { f.ChargeSubjectId, f.ResourcesId } into g
                                select new
                                {
                                    g.Key.ChargeSubjectId,
                                    g.Key.ResourcesId,
                                    MaxEndDate = g.Max(f => f.EndDate)
                                }).ToList();
                foreach (var fitem in fmaxList)
                {
                    DateTime endDay = currentMonthEnd;
                    var chargeSubject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(fitem.ChargeSubjectId);
                    if (chargeSubject.BillPeriod == (int)BillPeriodEnum.MonthlyCharge)
                    {
                        endDay = (new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)).AddMonths(1).AddDays(-1);
                    }
                    //先检查要页面update的bill list
                    if (onlyFutureBillList != null && onlyFutureBillList
                        .Any(c => c.ChargeSubjectId == fitem.ChargeSubjectId//收费项目
                        && c.ResourcesId == fitem.ResourcesId               //资源
                        && c.BeginDate > endDay                             //针对未来
                        && c.Status == (int)BillStatusEnum.NoPayment        //状态 未付款
                        && c.EndDate < fitem.MaxEndDate))                   //所有小于将要缴费的最大值
                    {
                        return false;
                    }

                    string[] fids = futureList.Where(f => f.ChargeSubjectId == fitem.ChargeSubjectId)
                       .Select(f => f.Id).ToArray();
                    //再检查数据库
                    if (propertyMgrUnitOfWork.ChargBillRepository.GetAll()
                        .Any(c => c.ChargeSubjectId == fitem.ChargeSubjectId//收费项目
                        && c.ResourcesId == fitem.ResourcesId              //资源
                        && c.IsDel == false                                 //未删除
                        && !fids.Contains(c.Id)                             //排除将要缴费的
                        && c.BeginDate > endDay                             //针对未来
                        && c.Status == (int)BillStatusEnum.NoPayment        //状态 未付款
                        && c.EndDate < fitem.MaxEndDate))                   //所有小于将要缴费的最大值
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 验证支付信息
        /// </summary>
        private ResultModel PaymentValidation(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, PaymentModel model, List<ChargBill> billList)
        {
            var amount = model.Amount + (model.IsPreDeductible ? model.PreDeductibleAmount : 0);
            if (amount <= 0)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "801", Msg = "付款金额或抵扣金额不能小于等于零" };
            }
            //if (model.PayType == 0)
            //{
            //    return new ResultModel() { IsSuccess = false, ErrorCode = "809", Msg = "请选择付款方式" };
            //}
            if (model.ChargeType == ChargeTypeEnum.Refund)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "803", Msg = "不支持退款缴费" };
            }
            if (model.ChargeType == ChargeTypeEnum.InitBalance)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "804", Msg = "不支持余额初始化" };
            }
            if (model.DiscountInfo != null)
            {
                model.DiscountInfo.DiscountAmount = model.DiscountInfo.DiscountAmount ?? 0;
                if (model.DiscountInfo.DiscountAmount < 0)
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "830", Msg = "优惠金额不能小于0" };
                }

                //验证优惠券金额是否被篡改 2017-04-12
                HttpClientService service = new HttpClientService();
                var usercouponp = service.GetUserCouponById(model.DiscountInfo.UserId, int.Parse(model.DiscountInfo.RefId));
                if (usercouponp == null)
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "840", Msg = "不存在此优惠券" };
                }
                if (usercouponp.CouponsMoney != (double)model.DiscountInfo.DiscountAmount)
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "850", Msg = "优惠金额被篡改" };
                }
            }

            //只需要缴费部分账单
            if (model.BillIDs != null)
            {
                var dataList = propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(c => model.BillIDs.Contains(c.Id)).ToList();
                billList.AddRange(dataList);
            }
            //新增并缴费部分账单
            if (model.NewBillList != null)
            {
                billList.AddRange(model.NewBillList);
            }
            else
            {
                model.NewBillList = new List<ChargBill>();
            }
            //修改并缴费部分账单
            if (model.UpdateBillList != null)
            {
                billList.AddRange(model.UpdateBillList);
            }

            if (billList.Count() == 0)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "805", Msg = "付款账单ID有误" };
            }

            if (!billList.First().IsDevPay.Value && billList.Where(b => b.HouseDeptId.HasValue && b.HouseDeptId != 0).Select(b => b.HouseDeptId).Distinct().Count() > 1)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "806", Msg = "非开发商付款账单不支持多房屋合并缴费" };
            }
            //需要支付的金额 = 计费金额+滞纳金-已交金额-减免金额
            decimal billAmount = Math.Round(billList.Sum(b => b.BillAmount.Value + b.PenaltyAmount.Value - b.ReceivedAmount.Value - b.ReliefAmount.Value), 2);
            if (billAmount > amount)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "808", Msg = "付款金额或抵扣金额不足" };
            }
            //未来的周期性账单只能从该收费项目开始时间最早的账单收费 修改：2017-01-10
            if (!CheckFutureBill(propertyMgrUnitOfWork, billList, model.OnlyFutureBillList))
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "809", Msg = "未来的周期性账单只能从该收费项目开始时间最早的账单收费" };
            }

            //找零
            decimal changeAmount = Math.Round(amount - billAmount, 2);
            //找零转存预付款
            if (model.IsChangeStore && changeAmount > 0)
            {
                //如果存在开发商代缴账单
                if (billList.Where(b => b.IsDevPay == true).Count() > 0)
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "810", Msg = "开发商代缴账单不支持零钱转存预付款" };
                }
                //没有绑定房屋的资源不能预存 2017-3-16
                if (billList.Any(b => !b.HouseDeptId.HasValue || b.HouseDeptId.Value == 0))
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "820", Msg = "没有绑定房屋的资源不能预存" };
                }
            }
            //记录日志
            //LogProperty.WriteLoginToFile(string.Format("[多账单缴费]Code:0 Bills ID:{0} Amount:{1}", string.Join(",", BillIDs), Amount.ToString()), "PaymentService", FileLogType.Info);
            return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "验证成功" };
        }

        /// <summary>
        /// 处理日常收费 只有更新和新增账单（主要拆分账单） 和 新增缴费账单（添加预存费账单）
        /// </summary>
        private void DailyPaymentBillAddUpdate(IPropertyMgrUnitOfWork pmUnitWork, PaymentModel model, List<ChargBill> onlyUpdateBillList, List<ChargBill> onlyNewList)
        {
            //未来改变和新增账单
            model.OnlyFutureBillList = new List<ChargBill>();
            if (model.NewBillList != null)
            {
                //添加快照
                foreach (var item in model.NewBillList)
                {
                    item.CreateTime = DateTime.Now;
                    item.UpdateTime = DateTime.Now;
                    //item.ChargeSubject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(item.ChargeSubjectId);
                }
            }

            if (onlyUpdateBillList != null)
            {
                //只有更新
                foreach (var item in onlyUpdateBillList)
                {
                    item.UpdateTime = DateTime.Now;
                    pmUnitWork.ChargBillRepository.Update(item);
                }
                model.OnlyFutureBillList.AddRange(onlyUpdateBillList);
            }

            if (onlyNewList != null)
            {
                //只有新增
                foreach (var item in onlyNewList)
                {
                    item.CreateTime = DateTime.Now;
                    item.UpdateTime = DateTime.Now;
                    pmUnitWork.ChargBillRepository.Add(item);
                    item.ChargeSubject = pmUnitWork.ChargeSubjectRepository.GetByKey(item.ChargeSubjectId);
                    //生成快照
                    BillCommonService.Instance.GenerateChargeSubjectSna(pmUnitWork, item, item.ChargeSubject);
                }
                model.OnlyFutureBillList.AddRange(onlyNewList.Where(o => o.ChargeSubject.SubjectType != (int)SubjectTypeEnum.SystemPreset));
            }
        }

        /// <summary>
        /// 多账单缴费
        /// </summary>
        private ResultModel BillsPayment(IPropertyMgrUnitOfWork pmUnitWork, PaymentModel model, List<ChargBill> billList)
        {
            //需要支付的金额 = 计费金额+滞纳金-已交金额-减免金额
            decimal billAmount = Math.Round(billList.Sum(b => b.BillAmount.Value + b.PenaltyAmount.Value - b.ReceivedAmount.Value - b.ReliefAmount.Value), 2);

            //生成费用记录Id
            string chargeRecordId = Guid.NewGuid().ToString();
            //费用明细
            Dictionary<string, decimal> amountD = new Dictionary<string, decimal>();
            //循环处理账单
            var updateBill = billList.Where(b => !model.NewBillList.Any(n => n.Id == b.Id));
            foreach (var item in updateBill)
            {
                amountD.Add(item.Id, Math.Round(item.BillAmount.Value + item.PenaltyAmount.Value - item.ReceivedAmount.Value - item.ReliefAmount.Value, 2));
                SingleBillPayment(pmUnitWork, item, model.PayType, chargeRecordId, false);
            }
            var newBill = billList.Where(b => model.NewBillList.Any(n => n.Id == b.Id));//add v2.9 从需要出来的账单查找新增账单
            foreach (var item in newBill)
            {
                amountD.Add(item.Id, Math.Round(item.BillAmount.Value + item.PenaltyAmount.Value - item.ReceivedAmount.Value - item.ReliefAmount.Value, 2));
                SingleBillPayment(pmUnitWork, item, model.PayType, chargeRecordId, true);
            }

            //找零
            decimal changeAmount = Math.Round(model.Amount - billAmount, 2);
            //找零转存预付款
            if (model.IsChangeStore && changeAmount > 0)
            {
                //生成预付账单
                var pChargBill = GenerateChangePrepaymentBill(pmUnitWork, billList.First().ComDeptId.Value
                    , billList.First().HouseDeptId.Value, billList.First().ResourcesId.Value, changeAmount
                    , model.PayType, chargeRecordId, model.Operator, model.OperatorName, model.SmallToPrepaySubjectId);
                billList.Add(pChargBill);//添加到账单列表
                amountD.Add(pChargBill.Id, changeAmount);
            }

            model.ReceiptNo = model.ReceiptNo ?? string.Empty;
            //string ReceiptNum = model.ReceiptNo;//string.Empty;
            bool IsReceiptbook = false;
            //修改bug #2342 基于物业唯一 当为空时 不需要验证
            bool IsReceiptBookFinsh = false;
            string ReceiptBookFinshmsg = string.Empty;
            //如果需要打印票据号 就需要验证 或 生成票据号
            if (model.IsPrintReceipt.HasValue && model.IsPrintReceipt.Value)
            {
                //是否存在票据本
                if (BillCommonService.Instance.CheckHasReceiptBook(pmUnitWork, billList.First().ComDeptId.Value))
                {
                    try
                    {
                        //如果票据号不等于空 验证
                        if (!string.IsNullOrEmpty(model.ReceiptNo))
                        {
                            var comDeptId = billList.First().ComDeptId.Value;
                            var receiptBook = BillCommonService.Instance.GetCurrentReceiptBook(pmUnitWork, comDeptId);
                            string msg = string.Empty;
                            //验证格式
                            if (!BillCommonService.Instance.VerificationReceiptNumFormat(model.ReceiptNo, receiptBook, ref msg))
                            {
                                return new ResultModel() { IsSuccess = false, ErrorCode = "720", Msg = msg };
                            }
                            //验证重复
                            if (CheckReceiptBookNumRepeat(model.ReceiptNo, comDeptId, ""))
                            {
                                return new ResultModel() { IsSuccess = false, ErrorCode = "710", Msg = "票据号已被使用，请修改后再提交" };
                            }
                        }
                        else //票据号等于空 系统自动生成
                        {
                            model.ReceiptNo = BillCommonService.Instance.GenerateReceiptBookNumber(pmUnitWork, billList.First().ComDeptId.Value);
                            int i = 1;
                            while (CheckReceiptBookNumRepeat(model.ReceiptNo, billList.First().ComDeptId.Value, ""))
                            {
                                model.ReceiptNo = BillCommonService.Instance.GenerateReceiptBookNumber(pmUnitWork, billList.First().ComDeptId.Value, i);
                                i++;
                                //最多重复生成10次
                                if (i == 10)
                                {
                                    break;
                                }
                            }
                            if (i == 10)
                            {
                                throw new Exception("票据号生成失败，请联系管理员,或者补打收据");//new ResultModel() { IsSuccess = false, ErrorCode = "820", Msg = "票据号生成失败，请联系管理员" };
                            }
                        }
                        IsReceiptbook = true;

                    }
                    catch (Exception ReceiptEx)
                    {
                        IsReceiptBookFinsh = true;
                        ReceiptBookFinshmsg = ReceiptEx.Message;
                        model.ReceiptNo = "";
                    }
                }
                else
                {
                    IsReceiptBookFinsh = true;
                    ReceiptBookFinshmsg = "当前票据号已使用完，请重新设置票据号后再进行补打";
                    model.ReceiptNo = "";
                }
            }
            else
            {
                model.ReceiptNo = string.Empty;
            }

            //生成单据
            Receipt recpit = BillCommonService.Instance.GenerateReceipt(pmUnitWork, model.ReceiptNo, model.Operator, model.OperatorName, ReceiptStatusEnum.Paid, model.Remark);
            //找零不转预付=账单金额，找零转预付=收取金额
            decimal recordAmount = billAmount;
            if (model.IsChangeStore)
            {
                recordAmount = model.Amount;
            }
            //生成费用记录
            ChargeRecord chargeRecord = BillCommonService.Instance.GenerateChargeRecord(pmUnitWork, billList
                , model.ChargeType, model.PayType, recpit.Id, recordAmount, "", amountD, chargeRecordId,
                model.Operator, model.OperatorName, model.CustomerName, model.IsOnline, null, model.DiscountInfo == null ? 0 : model.DiscountInfo.DiscountAmount.Value);

            if (IsReceiptbook && IsReceiptBookFinsh == false)
            {
                BillCommonService.Instance.GenerateReceiptBookDetail(pmUnitWork, billList.First().ComDeptId.Value, chargeRecord, recpit);
            }

            //保存优惠信息
            if (model.DiscountInfo != null)
            {
                model.DiscountInfo.Id = Guid.NewGuid().ToString();
                model.DiscountInfo.ChargeRecordId = chargeRecordId;
                model.DiscountInfo.CreateTime = DateTime.Now;
                model.DiscountInfo.UpdateTime = DateTime.Now;
                model.DiscountInfo.CustomerName = string.IsNullOrEmpty(model.CustomerName) ? chargeRecord.CustomerName : model.CustomerName;
                model.DiscountInfo.Operator = model.Operator;
                model.DiscountInfo.OperatorName = model.OperatorName;
                model.DiscountInfo.Status = (int)DiscountStatusEnum.Used;
                model.DiscountInfo.IsDel = false;
                pmUnitWork.PaymentDiscountInfoRepository.Add(model.DiscountInfo);
            }

            return new ResultModel()
            {
                IsSuccess = true,
                Msg = (IsReceiptBookFinsh == false ? "" : "," + ReceiptBookFinshmsg),
                Data = chargeRecord.Id,
                ErrorCode = (IsReceiptBookFinsh == false ? "0" : "-5")
            };
        }

        private ResultModel BillsPreDeductible(IPropertyMgrUnitOfWork pmUnitWork, PaymentModel model, List<ChargBill> deducitonBillList)
        {
            //收费记录Id
            string chargeRecordId = Guid.NewGuid().ToString();
            //收费记录引用的账单
            List<ChargBill> preDeductionBillList = new List<ChargBill>();

            //排除预存费 先抵扣账单开始日最小的
            var orderbyBillList = deducitonBillList.Where(d => d.RefType != (int)SubjectTypeEnum.SystemPreset).OrderBy(d => d.BeginDate).ToList();

            //抵扣结果
            PrepayDeductionResult result;
            //抵扣结果列表
            List<PrepayDeductionResult> presultList = new List<PrepayDeductionResult>();

            //按照 账单开始日最小的 循环抵扣
            foreach (var bill in orderbyBillList)
            {
                //抵扣金额
                decimal deductionAmount = bill.BillAmount.Value + bill.PenaltyAmount.Value - bill.ReceivedAmount.Value - bill.ReliefAmount.Value;

                //已交金额 += 抵扣金额
                result = BillCommonService.Instance.AutomaticDeduction(pmUnitWork, bill, deductionAmount, chargeRecordId);

                //如果抵扣金额为0 就不需要更新账单和生成费用流水
                if (result.TotalDeductionAmount == 0)
                {
                    continue;
                }
                bill.ReceivedAmount += result.TotalDeductionAmount;
                if (bill.BillAmount == bill.ReceivedAmount)//如果已交完，更新账单状态
                {
                    bill.Status = BillStatusEnum.Paid.GetHashCode();
                }
                else
                {
                    bill.Status = BillStatusEnum.NoPayment.GetHashCode();
                }
                preDeductionBillList.Add(bill);
                //新增账单
                if (model.NewBillList.Any(b => b.Id == bill.Id))
                {
                    //如果没有收费项目 先获取
                    if (bill.ChargeSubject == null)
                    {
                        bill.ChargeSubject = pmUnitWork.ChargeSubjectRepository.GetByKey(bill.ChargeSubjectId);
                    }
                    pmUnitWork.ChargBillRepository.Add(bill);
                    BillCommonService.Instance.GenerateChargeSubjectSna(pmUnitWork, bill, bill.ChargeSubject);
                    //预存抵扣完,将账单移除 不参与缴费
                    if (bill.Status == (int)BillStatusEnum.Paid)
                    {
                        deducitonBillList.Remove(bill);
                    }
                    else//未抵扣完，移除新增列表 可修改
                    {
                        model.NewBillList.Remove(bill);
                    }
                }
                else //修改账单
                {
                    //预存抵扣完,将账单移除 不参与缴费
                    if (bill.Status == (int)BillStatusEnum.Paid)
                    {
                        pmUnitWork.ChargBillRepository.Update(bill);
                        deducitonBillList.Remove(bill);
                    }
                }

                presultList.Add(result);
            }
            //合并收费记录
            if (preDeductionBillList.Count() > 0)
            {
                //账单划账明细
                var billDictionary = presultList.ToDictionary(key => key.ChargeBillId, value => value.TotalDeductionAmount);
                BillCommonService.Instance.GenerateChargeRecordByBillList(pmUnitWork, preDeductionBillList, chargeRecordId, billDictionary, "日常收费预存抵扣", model.Operator, model.OperatorName);
            }

            //验证抵扣金额 和 页面输入金额是否一致
            var totalDeductionAmount = presultList.Sum(p => p.TotalDeductionAmount);
            if (model.PreDeductibleAmount != totalDeductionAmount)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "790", Msg = "抵扣预存金额已发生改变，请刷新页面后重试" };
            }

            return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "" };
        }

        #endregion

        #endregion

        #region IPayment 接口实现

        /// <summary>
        /// 多账单缴费
        /// </summary>
        /// <param name="BillIDs">账单IDs</param>
        /// <param name="Amount">缴费金额</param>
        /// <param name="IsReceiptNum">是否需要票据号</param>
        /// <param name="PayType">付款方式</param>
        /// <param name="ChargeType">收费类型</param>
        /// <param name="IsChangeStore">是否找零预存</param>
        /// <returns>处理结果</returns>
        public ResultModel BillsPayment(PaymentModel model)
        {
            try
            {
                using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    var billList = new List<ChargBill>();
                    //公共验证 并获取需要缴费的账单
                    var result = PaymentValidation(pmUnitWork, model, billList);
                    if (!result.IsSuccess)
                    {
                        return result;
                    }

                    //是否打印票据配置 取默认配置的值 v2.9 2017-9-18
                    if (!model.IsPrintReceipt.HasValue)
                    {
                        var config = BillCommonService.Instance.GetCommunityConfig(pmUnitWork, billList.First().ComDeptId);
                        model.IsPrintReceipt = config.IsDefaultPrintReceipt;
                    }

                    result = BillsPayment(pmUnitWork, model, billList);
                    if (result.IsSuccess)
                    {
                        pmUnitWork.Commit();
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                //记录日志
                LogProperty.WriteLoginToFile(string.Format("[多账单缴费]Code:900 Bills ID:{0} Amount:{1} ErrorMsg:{2}", string.Join(",", model.BillIDs), model.Amount.ToString(), ex.Message), "PaymentService", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "付款失败" };
            }
        }

        /// <summary>
        /// 日常收费处理
        /// </summary>
        public ResultModel BillsDailyPayment(PaymentModel model, List<ChargBill> onlyUpdateBillList, List<ChargBill> onlyNewList)
        {
            try
            {
                using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    var billList = new List<ChargBill>();
                    //1.公共验证 并获取需要缴费的账单
                    var result = PaymentValidation(pmUnitWork, model, billList);
                    if (!result.IsSuccess)
                    {
                        return result;
                    }

                    //2.处理只新增和更新账单
                    DailyPaymentBillAddUpdate(pmUnitWork, model, onlyUpdateBillList, onlyNewList);
                    //3.先处理预存抵扣
                    if (model.IsPreDeductible && model.PreDeductibleAmount > 0)
                    {
                        result = BillsPreDeductible(pmUnitWork, model, billList);
                        if (!result.IsSuccess)
                        {
                            return result;
                        }
                    }
                    //如没抵扣完 才进行缴费处理
                    if (billList.Count() > 0)
                    {
                        //4.缴费
                        result = BillsPayment(pmUnitWork, model, billList);
                    }
                   
                    if (result.IsSuccess)
                    {
                        //提交
                        pmUnitWork.Commit();
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                //记录日志
                LogProperty.WriteLoginToFile(string.Format("[日常收费]Code:900 Bills ID:{0} Amount:{1} ErrorMsg:{2}", "多账单缴费", model.Amount.ToString(), ex.Message), "PaymentService", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "付款失败" };
            }
        }

        /// <summary>
        /// 生成交款记录
        /// </summary>
        /// <param name="ChargeRecordList">费用记录</param>
        /// <param name="remark">备注</param>
        /// <returns>处理结果</returns>
        public ResultModel GenerateBillPaymentTask(string[] ChargeRecordIds, string Remark, int Operator, string OperatorName, DateTime PaymentDate)
        {
            if (ChargeRecordIds.Count() < 1)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "801", Msg = "没有账单可以交款" };
            }
            try
            {
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    IList<ChargeRecord> ChargeRecordList = propertyMgrUnitOfWork.ChargeRecordRepository
                        .GetAll()
                        .Where(c => ChargeRecordIds.Contains(c.Id))
                        .ToList();
                    BillCommonService.Instance.GenerateBillPaymentTask(propertyMgrUnitOfWork, ChargeRecordList, PaymentTaskStatusEnum.Submitted, Remark, Operator, OperatorName, PaymentDate);
                    //提交
                    propertyMgrUnitOfWork.Commit();
                    //记录日志
                    //LogProperty.WriteLoginToFile(string.Format("[生成交款记录]Code:0 ChargeRecord Ids:{0} Amount:{1}", string.Join(",", ChargeRecordIds)), "PaymentService", FileLogType.Info);
                    return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "生成交款记录成功" };
                }
            }
            catch (Exception ex)
            {
                //记录日志
                LogProperty.WriteLoginToFile(string.Format("[生成交款记录]Code:900  ChargeRecord Ids:{0} ErrorMsg:{1}", string.Join(",", ChargeRecordIds), ex.Message), "PaymentService", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "生成交款记录失败" };
            }
        }

        /// <summary>
        /// 弃审接口
        /// </summary>
        /// <param name="PaymentTaskId">交款Id</param>
        /// <returns></returns>
        public ResultModel PaymentTasksAbandonRviewed(int PaymentTaskId, int Operator, string OperatorName)
        {

            try
            {
                if (PaymentTaskId < 0)
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "801", Msg = "请选择交款记录" };
                }
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    PaymentTasks paymentTasks = propertyMgrUnitOfWork.PaymentTasksRepository.GetByKey(PaymentTaskId);
                    paymentTasks.Status = (int)PaymentTaskStatusEnum.Back;
                    paymentTasks.Operator = Operator;
                    paymentTasks.OperatorName = OperatorName;
                    paymentTasks.UpdateTime = DateTime.Now;

                    propertyMgrUnitOfWork.PaymentTasksRepository.Update(paymentTasks);
                    propertyMgrUnitOfWork.Commit();
                    return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "弃审交款记录成功" };
                }
            }
            catch (Exception ex)
            {
                //记录日志
                LogProperty.WriteLoginToFile(string.Format("[交款弃审]Code:900  PaymentTaskId:{0} ErrorMsg:{1}", PaymentTaskId.ToString(), ex.Message), "PaymentService", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "弃审交款记录失败" };
            }
        }

        /// <summary>
        /// 撤销交款
        /// </summary>
        /// <param name="PaymentTaskId"></param>
        /// <param name="Operator"></param>
        /// <param name="OperatorName"></param>
        /// <returns></returns>
        public ResultModel PaymentTasksDelete(int PaymentTaskId, int Operator, string OperatorName)
        {
            try
            {
                if (PaymentTaskId < 0)
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "801", Msg = "请选择交款记录" };
                }
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    PaymentTasks paymentTasks = propertyMgrUnitOfWork.PaymentTasksRepository.GetByKey(PaymentTaskId);
                    paymentTasks.IsDel = true;
                    paymentTasks.Operator = Operator;
                    paymentTasks.OperatorName = OperatorName;
                    paymentTasks.UpdateTime = DateTime.Now;
                    propertyMgrUnitOfWork.PaymentTasksRepository.Update(paymentTasks);

                    List<PaymentTaskDetail> list = propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll().Where(o => o.PaymentTaskID == PaymentTaskId).ToList();

                    foreach (PaymentTaskDetail paymentTaskDetail in list)
                    {
                        paymentTaskDetail.IsDel = true;
                        paymentTaskDetail.UpdateTime = DateTime.Now;
                        propertyMgrUnitOfWork.PaymentTaskDetailRepository.Update(paymentTaskDetail);
                    }
                    propertyMgrUnitOfWork.Commit();
                    return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "撤销交款成功" };
                }
            }
            catch (Exception ex)
            {
                //记录日志
                LogProperty.WriteLoginToFile(string.Format("[撤销交款]Code:900  PaymentTaskId:{0} ErrorMsg:{1}", PaymentTaskId.ToString(), ex.Message), "PaymentService", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "撤销交款失败" };
            }
        }


        /// <summary>
        /// 交款审核
        /// </summary>
        /// <param name="PaymentTaskId"></param>
        /// <param name="Operator"></param>
        /// <param name="OperatorName"></param>
        /// <returns></returns>
        public ResultModel PaymentTasksRviewed(int PaymentTaskId, int Operator, string OperatorName, string CheckRemark)
        {
            try
            {
                if (PaymentTaskId < 0)
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "801", Msg = "请选择交款记录" };
                }
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    PaymentTasks paymentTasks = propertyMgrUnitOfWork.PaymentTasksRepository.GetByKey(PaymentTaskId);
                    paymentTasks.Status = (int)PaymentTaskStatusEnum.Audited;
                    paymentTasks.ReviewerName = OperatorName;
                    paymentTasks.ReviewerId = Operator;
                    paymentTasks.ReviewDate = DateTime.Now;
                    paymentTasks.Operator = Operator;
                    paymentTasks.OperatorName = OperatorName;
                    paymentTasks.UpdateTime = DateTime.Now;
                    paymentTasks.CheckRemark = CheckRemark;

                    propertyMgrUnitOfWork.PaymentTasksRepository.Update(paymentTasks);
                    propertyMgrUnitOfWork.Commit();
                    return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "交款审核成功" };
                }
            }
            catch (Exception ex)
            {
                //记录日志
                LogProperty.WriteLoginToFile(string.Format("[交款审核]Code:900  PaymentTaskId:{0} ErrorMsg:{1}", PaymentTaskId.ToString(), ex.Message), "PaymentService", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "交款审核失败" };
            }
        }

        /// <summary>
        /// 撤销审核
        /// </summary>
        /// <param name="PaymentTaskId"></param>
        /// <param name="Operator"></param>
        /// <param name="OperatorName"></param>
        /// <returns></returns>
        public ResultModel PaymentTasksRevokeRviewed(int PaymentTaskId, int Operator, string OperatorName, string CheckRemark)
        {
            try
            {
                if (PaymentTaskId < 0)
                {
                    return new ResultModel() { IsSuccess = false, ErrorCode = "801", Msg = "请选择交款记录" };
                }
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    PaymentTasks paymentTasks = propertyMgrUnitOfWork.PaymentTasksRepository.GetByKey(PaymentTaskId);
                    paymentTasks.Status = (int)PaymentTaskStatusEnum.Submitted;
                    paymentTasks.ReviewerName = null;
                    paymentTasks.ReviewerId = null;
                    paymentTasks.ReviewDate = null;
                    paymentTasks.Operator = Operator;
                    paymentTasks.OperatorName = OperatorName;
                    paymentTasks.UpdateTime = DateTime.Now;
                    paymentTasks.CheckRemark = CheckRemark;
                    propertyMgrUnitOfWork.PaymentTasksRepository.Update(paymentTasks);
                    propertyMgrUnitOfWork.Commit();
                    return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "撤销审核成功" };
                }


            }
            catch (Exception ex)
            {
                //记录日志
                LogProperty.WriteLoginToFile(string.Format("[撤销审核]Code:900  PaymentTaskId:{0} ErrorMsg:{1}", PaymentTaskId.ToString(), ex.Message), "PaymentService", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "撤销审核失败" };
            }
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="RefundRecord">退款记录</param>
        /// <param name="Operator">操作人</param>
        /// <returns>处理结果</returns>
        public ResultModel Refund(RefundRecord RefundRecord, int Operator, string OperatorName)
        {
            //验证
            if (string.IsNullOrEmpty(RefundRecord.Customer))
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "710", Msg = "客户姓名不能为空" };
            }
            if (string.IsNullOrEmpty(RefundRecord.Reason))
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "720", Msg = "退款原因不能为空" };
            }
            if (RefundRecord.PayType == PayTypeEnum.InternalTransfer.GetHashCode())
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "730", Msg = "退款不支持预存抵扣" };
            }

            try
            {
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    //1.生成退款记录
                    RefundRecord.CreateTime = DateTime.Now;
                    RefundRecord.IsDel = false;
                    RefundRecord.Operator = Operator;
                    RefundRecord.OperatorName = OperatorName;
                    RefundRecord.UpdateTime = DateTime.Now;
                    RefundRecord.RefundTime = DateTime.Now;
                    //退款被引用的费用记录
                    var refChargeRecord = propertyMgrUnitOfWork.ChargeRecordRepository
                        .GetAll()
                        .Where(c => c.Id == RefundRecord.RefChargeRecordId)
                        .FirstOrDefault();

                    //2.更改单据状态 为已退款
                    var receipt = propertyMgrUnitOfWork.ReceiptRepository
                        .GetAll()
                        .Where(r => r.Id == refChargeRecord.ReceiptId)
                        .FirstOrDefault();

                    if (receipt.Status == ReceiptStatusEnum.Refunded.GetHashCode())
                    {
                        return new ResultModel() { IsSuccess = false, ErrorCode = "740", Msg = "已退款项不能重复退款" };
                    }

                    receipt.Status = ReceiptStatusEnum.Refunded.GetHashCode();
                    receipt.UpdateTime = DateTime.Now;
                    receipt.Operator = Operator;
                    receipt.OperatorName = OperatorName;
                    propertyMgrUnitOfWork.ReceiptRepository.Update(receipt);
                    //关系表查出费用关联的账单
                    Dictionary<string, decimal> amountD = propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository
                        .GetAll()
                        .Where(b => b.ChargeRecordId == refChargeRecord.Id)
                        //.Select(c => c.ChargeBillId)
                        .ToList()
                        .ToDictionary(key => key.ChargeBillId, vaule => (vaule.Amount.HasValue ? vaule.Amount.Value : 0));
                    //string[] chargeBillIds = propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository
                    //    .GetAll()
                    //    .Where(b => b.ChargeRecordId == refChargeRecord.Id)
                    //    .Select(c => c.ChargeBillId)
                    //    .ToArray();
                    string[] chargeBillIds = amountD.Keys.ToArray();
                    //账单列表
                    IList<ChargBill> chargeBillList = propertyMgrUnitOfWork.ChargBillRepository
                        .GetAll()
                        .Where(c => chargeBillIds.Contains(c.Id))
                        .ToList();

                    //3.生成退款费用记录
                    var chargeRecord = BillCommonService.Instance.GenerateChargeRecord(propertyMgrUnitOfWork, chargeBillList
                        , ChargeTypeEnum.Refund, (PayTypeEnum)RefundRecord.PayType, receipt.Id
                        , refChargeRecord.Amount.Value, "", amountD
                        , null, Operator, OperatorName, "", false, null, refChargeRecord.DiscountAmount.Value);

                    //更改退款记录的客户为退款记录客户
                    chargeRecord.CustomerName = RefundRecord.Customer;
                    chargeRecord.PayMthodId = RefundRecord.PayType;
                    //propertyMgrUnitOfWork.ChargeRecordRepository.Update(chargeRecord);

                    //设置退款记录ID 并添加
                    RefundRecord.ChargeRecordId = chargeRecord.Id;
                    RefundRecord.ReceiptID = refChargeRecord.ReceiptId;
                    propertyMgrUnitOfWork.RefundRecordRepository.Add(RefundRecord);
                    //4.更改账单状态为 未付款
                    foreach (var billItem in chargeBillList)
                    {
                        //如果是临时收费 状态更改为已退费
                        if (refChargeRecord.ChargeType == (int)ChargeTypeEnum.TemporaryCharge || refChargeRecord.ChargeType == (int)ChargeTypeEnum.ForeignCharge)
                        {
                            billItem.Status = BillStatusEnum.Refunded.GetHashCode();
                        }
                        else
                        {
                            //预存款 状态更改为已退费
                            if (billItem.ChargeSubject.SubjectType == (int)SubjectTypeEnum.SystemPreset)
                            {
                                billItem.Status = BillStatusEnum.Refunded.GetHashCode();
                            }
                            else
                            {
                                billItem.Status = BillStatusEnum.NoPayment.GetHashCode();
                            }
                        }

                        billItem.ReceivedAmount = (billItem.ReceivedAmount - amountD[billItem.Id]);//#bug 2363 退款后已交金额要减去票据支付金额
                        billItem.UpdateTime = DateTime.Now;
                        propertyMgrUnitOfWork.ChargBillRepository.Update(billItem);
                        /* 注释代码 2017-9-6
                        //5.如果是预存款，需要扣除余额
                        if (billItem.RefType == (int)SubjectTypeEnum.SystemPreset)
                        {
                            //处理不同收费项目预存账户
                            //var preentity = (from p in propertyMgrUnitOfWork.PrepayAccountRepository.GetAll()
                            //                 where p.PrepayAccountItems.Any(pd => pd.ChargeRecordId == refChargeRecord.Id)
                            //                 select p).FirstOrDefault();

                            //多收费项目预存费要分开退款 2017-09-06
                            var preList = (from p in propertyMgrUnitOfWork.PrepayAccountRepository.GetAll()
                                           where p.PrepayAccountItems.Any(pd => pd.ChargeRecordId == refChargeRecord.Id)
                                           && p.ChargeSubjectID == billItem.PreChargeSubjectId //根据收费项目账户退款
                                           select p).ToList();
                            if (preList.Count() > 0)
                            {
                                foreach (var preentity in preList)
                                {
                                    preentity.ChargeSubjectID = preentity.ChargeSubjectID ?? 0;
                                    ResultModel result = BillCommonService.Instance.SubjectBalanceAdd(propertyMgrUnitOfWork,
                                    billItem, amountD[billItem.Id], (PayTypeEnum)RefundRecord.PayType,
                                    chargeRecord.Id, "预存费退款扣除", BalanceTypeEnum.Payment, preentity.ChargeSubjectID.Value);

                                    if (!result.IsSuccess)
                                    {
                                        if (result.ErrorCode == "810")
                                        {
                                            string subName = "全部收费项目";
                                            //查找收费项目账户
                                            if (preentity.ChargeSubjectID != 0)
                                            {
                                                var chargeSubject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(preentity.ChargeSubjectID);
                                                if (chargeSubject != null)
                                                {
                                                    subName = chargeSubject.Name;
                                                }
                                            }
                                            result.Msg = subName + "账户退款预存费销账余额不足";
                                        }

                                        return result;
                                    }

                                }
                            }
                        }*/
                    }

                    //5. 预存费销账
                    //多收费项目预存费要分开退款 2017-09-06
                    var preList = (from p in propertyMgrUnitOfWork.PrepayAccountDetailRepository.GetAll()
                                   where p.ChargeRecordId == refChargeRecord.Id
                                   && p.ProductionAmount > 0 //排除本身是缴费抵扣的 2017-9-11
                                   select p).ToList();
                    //判断账单是否包含预存费 2017-9-11
                    if (preList.Count() > 0 && chargeBillList.Any(b => b.ChargeSubject.SubjectType == (int)SubjectTypeEnum.SystemPreset))
                    {
                        ChargBill bill = new ChargBill();
                        bill.Description = "预存费";
                        bill.ComDeptId = refChargeRecord.ComDeptId;
                        bill.HouseDeptId = refChargeRecord.HouseDeptId;
                        foreach (var preentity in preList)
                        {
                            var chargeSubjectID = preentity.PrepayAccount.ChargeSubjectID ?? 0;
                            ResultModel result = BillCommonService.Instance.SubjectBalanceAdd(propertyMgrUnitOfWork,
                            bill, preentity.ProductionAmount.Value, (PayTypeEnum)RefundRecord.PayType,
                            chargeRecord.Id, "预存费退款销账扣除", BalanceTypeEnum.Payment, chargeSubjectID);

                            if (!result.IsSuccess)
                            {
                                if (result.ErrorCode == "810")
                                {
                                    string subName = "全部收费项目";
                                    //查找收费项目账户
                                    if (chargeSubjectID != 0)
                                    {
                                        var chargeSubject = propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(chargeSubjectID);
                                        if (chargeSubject != null)
                                        {
                                            subName = chargeSubject.Name;
                                        }
                                    }
                                    result.Msg = subName + "账户退款预存费销账余额不足";
                                }

                                return result;
                            }
                        }
                    }

                    bool couponReturn = true;
                    //如果使用优惠券，需要退回优惠券
                    if (refChargeRecord.DiscountAmount > 0)
                    {
                        var couponList = propertyMgrUnitOfWork.PaymentDiscountInfoRepository.GetAll()
                            .Where(p => p.ChargeRecordId == refChargeRecord.Id
                            && p.IsDel == false && p.DiscountType == (int)DiscountTypeEnum.Coupon).ToList();
                        HttpClientService service = new HttpClientService();
                        foreach (var item in couponList)
                        {
                            bool bo = service.ChangeUserCoupon(item.UserId, int.Parse(item.RefId), "unused");
                            if (!bo)
                            {
                                item.Status = (int)DiscountStatusEnum.ReturnException;
                                couponReturn = false;
                            }
                            else
                            {
                                item.Status = (int)DiscountStatusEnum.Returned;
                            }
                            //更新优惠券使用状态
                            propertyMgrUnitOfWork.PaymentDiscountInfoRepository.Update(item);
                        }
                    }
                    //提交
                    propertyMgrUnitOfWork.Commit();
                    if (!couponReturn)
                    {
                        return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "退款成功,但优惠券回退失败，请联系管理员" };
                    }
                    return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "退款成功" };
                }
            }
            catch (Exception ex)
            {
                //记录日志
                LogProperty.WriteLoginToFile(string.Format("[退款]Code:900  ReceiptID:{0} ErrorMsg:{1}", RefundRecord.ReceiptID, ex.Message), "PaymentService", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "退款失败" };
            }
        }

        /// <summary>
        /// 撤销收费
        /// 删除收费
        /// </summary>
        /// <param name="ChargeRecordList">费用记录</param>
        /// <param name="Operator">操作人</param>
        /// <returns>处理结果</returns>
        public ResultModel RevokeCharge(string[] ChargeRecordIds, int Operator, string OperatorName)
        {
            IList<string> errorRecordIds = new List<string>();
            ResultModel result;
            foreach (var item in ChargeRecordIds)
            {
                result = SingleRevokeCharge(item, Operator, OperatorName);
                if (!result.IsSuccess)
                {
                    errorRecordIds.Add(result.Data.ToString());
                }
            }
            if (errorRecordIds.Count() > 0)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "700", Msg = "存在撤销收费失败", Data = errorRecordIds };
            }
            return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "撤销收费成功" };
        }

        public bool IsSubmitted(string ChargeRecordId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var count = propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll().Where(p => p.IsDel == false && p.ChargeRecordId == ChargeRecordId).Count();
                if (count > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public bool CheckReceiptNumRepeat(string ReceiptNum, int ComDeptId, string ReceiptId)
        {
            int?[] comDeptIds = DomainInterfaceHelper
                .LookUp<IPropertyDomainService>().GetComDeptIdsByComDeptId(ComDeptId);

            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //创建
                if (string.IsNullOrEmpty(ReceiptId))
                {
                    var count = (from r in propertyMgrUnitOfWork.ReceiptRepository.GetAll()
                                 join c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(r => comDeptIds.Contains(r.ComDeptId))
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
                                 .Where(r => comDeptIds.Contains(r.ComDeptId))
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
        /// 计算月预交费
        /// </summary>
        public decimal? CalculationMonthPrePayment(int HouseDeptId, out bool IsDevPay)
        {
            int ComDeptId = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityDeptIdByHouseDeptId(HouseDeptId);
            //获取计算数量，如房屋面积、停车位面积
            IList<CalculateProperty> hmpList = CalculatePropertyHelper.GetHouseCalculateProperty(ComDeptId, HouseDeptId);
            IList<CalculateProperty> pmpList = CalculatePropertyHelper.GetParkingSpaceCalculateProperty(ComDeptId, hmpList, HouseDeptId);
            decimal? calculationAmout = 0;
            BillDateRange dr = new BillDateRange();
            dr.BeginDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dr.EndDate = dr.BeginDate.AddMonths(1).AddDays(-1);
            IsDevPay = false;
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //查询满足预交费条件的收费项目
                var preSubjects = (from c in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll()
                                   join r in propertyMgrUnitOfWork.SubjectHouseRefRepository.GetAll()
                                   on c.Id equals r.ChargeSubjecId
                                   where r.HouseDeptId == HouseDeptId && r.IsDel == false
                                   where c.ComDeptId == ComDeptId
                                   && (c.BillPeriod == (int)BillPeriodEnum.DailyCharge || c.BillPeriod == (int)BillPeriodEnum.MonthlyCharge)//必须是周期性收费项目
                                   && c.SubjectType != (int)SubjectTypeEnum.Meter//收费类型要不等于三表
                                   && c.IsDel == false
                                   select new { c, r }).ToDictionary(key => key.c, value => value.r);
                string qstr = string.Empty;
                foreach (var sub in preSubjects)
                {
                    if (sub.Key.SubjectType == (int)SubjectTypeEnum.House)
                    {
                        //房屋最多只有一套
                        if (hmpList.Count() > 0)
                        {
                            calculationAmout += BillCommonService.Instance.CalculateAmount(sub.Key, hmpList.First(), ref qstr);
                        }
                    }
                    else if (sub.Key.SubjectType == (int)SubjectTypeEnum.ParkingSpace)
                    {
                        //车位会有多个 找出绑定到当前收费项目的车位
                        foreach (var pitem in pmpList.Where(p => p.ResourcesId == sub.Value.ResourcesId))
                        {
                            calculationAmout += BillCommonService.Instance.CalculateAmount(sub.Key, pitem, ref qstr);
                        }
                    }
                    else//其它收费
                    {
                        CalculateProperty property = new CalculateProperty();
                        property.ComDeptId = ComDeptId;
                        property.HouseDeptID = HouseDeptId;
                        property.ResourcesId = HouseDeptId;
                        calculationAmout += BillCommonService.Instance.CalculateAmount(sub.Key, property, ref qstr);
                    }
                    if (!IsDevPay)
                    {
                        IsDevPay = BillCommonService.Instance.GetIsDevPay(sub.Value, dr);
                    }
                }
            }
            return calculationAmout;
        }

        /// <summary>
        /// 计算房屋每月预交款
        /// </summary>
        /// <param name="HouseDeptId">房屋ID</param>
        /// <param name="SubjectId">收费项目ID 为0表示所有</param>
        /// <returns></returns>
        public decimal? CalculationMonthPrePayment(int HouseDeptId, int SubjectId)
        {
            int ComDeptId = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityDeptIdByHouseDeptId(HouseDeptId);
            //获取计算数量，如房屋面积、停车位面积
            IList<CalculateProperty> hmpList = CalculatePropertyHelper.GetHouseCalculateProperty(ComDeptId, HouseDeptId);
            IList<CalculateProperty> pmpList = CalculatePropertyHelper.GetParkingSpaceCalculateProperty(ComDeptId, hmpList, HouseDeptId);
            decimal? calculationAmout = 0;
            BillDateRange dr = new BillDateRange();
            dr.BeginDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dr.EndDate = dr.BeginDate.AddMonths(1).AddDays(-1);
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //查询满足预交费条件的收费项目
                //var preSubjects = (from c in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll()
                //                   join r in propertyMgrUnitOfWork.SubjectHouseRefRepository.GetAll()
                //                   on c.Id equals r.ChargeSubjecId
                //                   where r.HouseDeptId == HouseDeptId && r.IsDel == false
                //                   where c.ComDeptId == ComDeptId
                //                   && (c.BillPeriod == (int)BillPeriodEnum.DailyCharge 
                //                   || c.BillPeriod == (int)BillPeriodEnum.MonthlyCharge 
                //                   || c.BillPeriod == (int)BillPeriodEnum.MeterCharge)//必须是周期性收费项目
                //                   && (SubjectId == 0 || c.Id == SubjectId)
                //                   && c.IsDel == false
                //                   select new { c, r }).ToDictionary(key => key.c, value => value.c.ChargeSubjectHouseRefItems);
                var preSubjects = from c in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll()
                                  where c.ChargeSubjectHouseRefItems.Any(h => h.HouseDeptId == HouseDeptId && h.IsDel == false)
                                  && c.ComDeptId == ComDeptId
                                  && c.IsDel == false
                                  && (c.BillPeriod == (int)BillPeriodEnum.DailyCharge
                                       || c.BillPeriod == (int)BillPeriodEnum.MonthlyCharge
                                       || c.BillPeriod == (int)BillPeriodEnum.MeterCharge)//必须是周期性收费项目
                                  && (SubjectId == 0 || c.Id == SubjectId)
                                  select c;
                string qstr = string.Empty;
                foreach (var sub in preSubjects)
                {
                    if (sub.SubjectType == (int)SubjectTypeEnum.House)
                    {
                        //房屋最多只有一套
                        if (hmpList.Count() > 0)
                        {
                            calculationAmout += BillCommonService.Instance.CalculateAmount(sub, hmpList.First(), ref qstr);
                        }
                    }
                    else if (sub.SubjectType == (int)SubjectTypeEnum.ParkingSpace)
                    {
                        var rids = sub.ChargeSubjectHouseRefItems.Where(r => r.IsDel == false).Select(v => v.ResourcesId).ToArray();
                        //车位会有多个 找出绑定到当前收费项目的车位
                        foreach (var pitem in pmpList.Where(p => rids.Contains(p.ResourcesId)).ToList())
                        {
                            calculationAmout += BillCommonService.Instance.CalculateAmount(sub, pitem, ref qstr);
                        }
                    }
                    else if (sub.SubjectType == (int)SubjectTypeEnum.Meter)
                    {
                        //三表会有多个 但三表目前没法计算
                    }
                    else//其它收费
                    {
                        CalculateProperty property = new CalculateProperty();
                        property.ComDeptId = ComDeptId;
                        property.HouseDeptID = HouseDeptId;
                        property.ResourcesId = HouseDeptId;
                        calculationAmout += BillCommonService.Instance.CalculateAmount(sub, property, ref qstr);
                    }
                }
            }
            return calculationAmout;
        }

        public IList<SubjectMonthPrePaymentDTO> CalculationMonthPrePaymentList(int HouseDeptId, bool NeedAll)
        {
            int ComDeptId = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityDeptIdByHouseDeptId(HouseDeptId);
            //获取计算数量，如房屋面积、停车位面积
            IList<CalculateProperty> hmpList = CalculatePropertyHelper.GetHouseCalculateProperty(ComDeptId, HouseDeptId);
            IList<CalculateProperty> pmpList = CalculatePropertyHelper.GetParkingSpaceCalculateProperty(ComDeptId, hmpList, HouseDeptId);
            BillDateRange dr = new BillDateRange();
            dr.BeginDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dr.EndDate = dr.BeginDate.AddMonths(1).AddDays(-1);
            IList<SubjectMonthPrePaymentDTO> sList = new List<SubjectMonthPrePaymentDTO>();
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var preSubjects = (from c in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll()
                                   where c.ChargeSubjectHouseRefItems.Any(h => h.HouseDeptId == HouseDeptId && h.IsDel == false)
                                   && c.ComDeptId == ComDeptId
                                   && c.IsDel == false
                                   && (c.BillPeriod == (int)BillPeriodEnum.DailyCharge
                                        || c.BillPeriod == (int)BillPeriodEnum.MonthlyCharge
                                        || c.BillPeriod == (int)BillPeriodEnum.MeterCharge)//必须是周期性收费项目
                                   select c).ToList();
                //公区表部分 2017-9-6
                var hdeptIdStr = "," + HouseDeptId.ToString() + ",";
                var publicMeter = (from sb in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll()
                                   from hr in sb.ChargeSubjectHouseRefItems
                                   join m in propertyMgrUnitOfWork.MeterRepository.GetAll()
                                   on hr.ResourcesId equals m.Id
                                   where sb.IsDel == false
                                   && sb.BillPeriod == (int)BillPeriodEnum.MeterCharge
                                   && hr.IsDel == false
                                   && m.IsEnabled == true
                                   && m.IsPublicArea == true
                                   && ("," + m.PublicAreaHouseDeptIDs + ",").Contains(hdeptIdStr)
                                   select sb).ToList();
                string qstr = string.Empty;
                preSubjects.AddRange(publicMeter);
                var subjectList = preSubjects.Distinct().ToList();

                foreach (var sub in preSubjects)
                {
                    SubjectMonthPrePaymentDTO entity = new SubjectMonthPrePaymentDTO();
                    entity.SubjectId = sub.Id;
                    entity.SubjectName = sub.Name;
                    entity.BillPeriod = sub.BillPeriod;
                    if (sub.SubjectType == (int)SubjectTypeEnum.House)
                    {
                        //房屋最多只有一套
                        if (hmpList.Count() > 0)
                        {
                            entity.PreAmount = BillCommonService.Instance.CalculateAmount(sub, hmpList.First(), ref qstr);
                        }
                    }
                    else if (sub.SubjectType == (int)SubjectTypeEnum.ParkingSpace)
                    {
                        var rids = sub.ChargeSubjectHouseRefItems.Where(r => r.IsDel == false).Select(v => v.ResourcesId).ToArray();
                        decimal amount = 0;
                        //车位会有多个 找出绑定到当前收费项目的车位
                        foreach (var pitem in pmpList.Where(p => rids.Contains(p.ResourcesId)).ToList())
                        {
                            amount += BillCommonService.Instance.CalculateAmount(sub, pitem, ref qstr);
                        }
                        if (amount > 0)
                        {
                            entity.PreAmount = amount;
                        }
                    }
                    else if (sub.SubjectType == (int)SubjectTypeEnum.Meter)
                    {
                        //三表会有多个 但三表目前没法计算
                        entity.PreAmount = 0; //三表默认为0
                    }
                    else//其它收费
                    {
                        CalculateProperty property = new CalculateProperty();
                        property.ComDeptId = ComDeptId;
                        property.HouseDeptID = HouseDeptId;
                        property.ResourcesId = HouseDeptId;
                        entity.PreAmount = BillCommonService.Instance.CalculateAmount(sub, property, ref qstr);
                    }

                    //如果没有值表示无法计算 或 没绑定 要排除 2017-8-31
                    if (entity.PreAmount.HasValue)
                    {
                        sList.Add(entity);
                    }
                }
            }
            //是否添加全部收费项目
            if (NeedAll)
            {
                sList.Add(new SubjectMonthPrePaymentDTO() { SubjectId = 0, SubjectName = "全部收费项目", PreAmount = sList.Sum(s => s.PreAmount) });
            }

            return sList;
        }

        public decimal CalculationBillsAmount(string[] BillIds)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var billList = propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(c => BillIds.Contains(c.Id)).ToList();
                //需要支付的金额 = 计费金额+滞纳金-已交金额-减免金额
                decimal billAmount = Math.Round(billList.Sum(b => b.BillAmount.Value + b.PenaltyAmount.Value - b.ReceivedAmount.Value - b.ReliefAmount.Value), 2);
                return billAmount;
            }
        }

        public List<ReportTableDTO> CalculationAllPrepaymentByComDeptId(int ComDeptId, DateTime BeginDate, DateTime EndDate, DateTime PayDate)
        {
            try
            {
                PayDate = PayDate.AddDays(1).AddMilliseconds(-1);
                EndDate = EndDate.AddDays(1).AddMilliseconds(-1);
                //1房间
                //获取绑定的房间
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    //小区房间列表
                    var HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptListByCommunityDeptId(ComDeptId.ToString());
                    //取出该小区的绑定数据
                    var SubjectHouseRefList_Query = propertyMgrUnitOfWork.SubjectHouseRefRepository.GetAll();
                    //取得起始日之后符合条件的绑定数据
                    var SubjectHouseRefList = (from c in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll()
                                               join r in SubjectHouseRefList_Query on c.Id equals r.ChargeSubjecId
                                               where c.BeginDate <= EndDate
                                                && (c.BillPeriod == (int)BillPeriodEnum.DailyCharge || c.BillPeriod == (int)BillPeriodEnum.MonthlyCharge)//必须是周期性收费项目
                                                && c.SubjectType != (int)SubjectTypeEnum.Meter//收费类型要不等于三表
                                                && c.IsDel == false
                                                && r.IsDel == false
                                                && c.ComDeptId == ComDeptId
                                               select r).ToList();
                    //取得收费项目列表
                    //相隔 计算月份
                    int year = EndDate.Year - BeginDate.Year;
                    int month = EndDate.Month - BeginDate.Month;
                    int Monthtotal = year * 12 + month + 1;
                    var ChargeSubjectList = propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll().Where(o => o.ComDeptId == ComDeptId
                                                           && o.BeginDate <= EndDate
                                                           && o.BillPeriod == (int)BillPeriodEnum.DailyCharge || o.BillPeriod == (int)BillPeriodEnum.MonthlyCharge
                                                           && o.SubjectType != (int)SubjectTypeEnum.Meter).ToList();

                    //选出只有绑定项目的小区列表---
                    HouseDeptList = (from h in HouseDeptList
                                     join r in SubjectHouseRefList on h.Id equals r.HouseDeptId
                                     group new { h } by new { h } into b
                                     select new DeptInfo
                                     {
                                         Id = b.Key.h.Id,
                                         Code = b.Key.h.Code,
                                         DeptType = b.Key.h.DeptType,
                                         Name = b.Key.h.Name,
                                         Order = b.Key.h.Order,
                                         PId = b.Key.h.PId,
                                         Remark = b.Key.h.Remark

                                     }).ToList();

                    //房间Id集合
                    int?[] HouseDeptIds = HouseDeptList.Select(o => o.Id).ToArray();
                    //科目Id集合
                    int?[] ChargeSubjectIds = ChargeSubjectList.Select(o => o.Id).ToArray();
                    //账单集合
                    var ChargeBillList = (from b in propertyMgrUnitOfWork.ChargBillRepository.GetAll()
                                          join c in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on b.ChargeSubjectId equals c.Id
                                          where b.BeginDate >= BeginDate && b.EndDate <= EndDate && b.BeginDate >= c.BeginDate
                                                     && ChargeSubjectIds.Contains(b.ChargeSubjectId)
                                                     && HouseDeptIds.Contains(b.HouseDeptId)
                                          select b
                                          ).ToList();

                    Dictionary<int?, decimal?> savelist = new Dictionary<int?, decimal?>();
                    IList<CalculateProperty> hmpList = CalculatePropertyHelper.GetHouseCalculateProperty(ComDeptId);
                    IList<CalculateProperty> pmpList = CalculatePropertyHelper.GetParkingSpaceCalculateProperty(ComDeptId, hmpList);
                    foreach (var houseDept in HouseDeptList)
                    {
                        BillDateRange dr = new BillDateRange();
                        dr.BeginDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                        dr.EndDate = dr.BeginDate.AddMonths(1).AddDays(-1);

                        var preSubjects = (from c in ChargeSubjectList
                                           join r in SubjectHouseRefList
                                           on c.Id equals r.ChargeSubjecId
                                           where r.HouseDeptId == houseDept.Id.Value && r.IsDel == false
                                           where c.ComDeptId == ComDeptId
                                           && (c.BillPeriod == (int)BillPeriodEnum.DailyCharge || c.BillPeriod == (int)BillPeriodEnum.MonthlyCharge)//必须是周期性收费项目
                                           && c.SubjectType != (int)SubjectTypeEnum.Meter//收费类型要不等于三表
                                           && c.SubjectType != (int)SubjectTypeEnum.SystemPreset//不等于系统
                                               && c.SubjectType != (int)SubjectTypeEnum.Other//不等于一次性费用
                                           && c.IsDel == false
                                           && r.IsDel == false
                                           select new { c, r }).ToList();

                        string qstr = string.Empty;
                        foreach (var sub in preSubjects)
                        {
                            decimal HouseSubjectMoney = 0;
                            decimal ParkSubjectMoney = 0;
                            List<ChargBill> ChargBillList = ChargeBillList.Where(o => o.ChargeSubjectId == sub.c.Id && o.HouseDeptId == houseDept.Id).ToList();

                            if (sub.c.SubjectType == (int)SubjectTypeEnum.House)
                            {
                                if (hmpList.Where(o => o.HouseDeptID == houseDept.Id.Value).ToList().Count() > 0)
                                {
                                    if (ChargBillList.Count > 0)
                                    {
                                        HouseSubjectMoney = BillCommonService.Instance.CalculateAmount(sub.c, hmpList.Where(o => o.HouseDeptID == houseDept.Id.Value).ToList().First(), ref qstr);
                                        var billMoney = CountAmountByTimeSpan(ChargBillList, EndDate, HouseSubjectMoney);
                                        HouseSubjectMoney = billMoney;
                                    }
                                    else
                                    {
                                        if (BeginDate < sub.c.BeginDate.Value)
                                        {
                                            year = EndDate.Year - sub.c.BeginDate.Value.Year;
                                            month = EndDate.Month - sub.c.BeginDate.Value.Month;
                                            Monthtotal = year * 12 + month + 1;
                                        }
                                        HouseSubjectMoney = BillCommonService.Instance.CalculateAmount(sub.c, hmpList.Where(o => o.HouseDeptID == houseDept.Id.Value).ToList().First(), ref qstr) * Monthtotal;
                                    }

                                }
                            }
                            else if (sub.c.SubjectType == (int)SubjectTypeEnum.ParkingSpace)
                            {

                                //车位会有多个 找出绑定到当前收费项目的车位
                                foreach (var pitem in pmpList.Where(p => p.ResourcesId == sub.r.ResourcesId))
                                {
                                    if (ChargBillList.Count > 0)
                                    {

                                        var parkMoney = BillCommonService.Instance.CalculateAmount(sub.c, pitem, ref qstr);
                                        var billMoney = CountAmountByTimeSpan(ChargBillList, EndDate, parkMoney);
                                        ParkSubjectMoney += billMoney;
                                    }
                                    else
                                    {
                                        if (BeginDate < sub.c.BeginDate.Value)
                                        {
                                            year = EndDate.Year - sub.c.BeginDate.Value.Year;
                                            month = EndDate.Month - sub.c.BeginDate.Value.Month;
                                            Monthtotal = year * 12 + month + 1;
                                        }
                                        ParkSubjectMoney += BillCommonService.Instance.CalculateAmount(sub.c, pitem, ref qstr) * Monthtotal;
                                    }
                                }
                            }

                            if (savelist.ContainsKey(sub.c.Id.Value))
                            {
                                savelist[sub.c.Id.Value] = (decimal)savelist[sub.c.Id.Value] + HouseSubjectMoney;
                                savelist[sub.c.Id.Value] = (decimal)savelist[sub.c.Id.Value] + ParkSubjectMoney;
                            }
                            else
                            {
                                savelist.Add(sub.c.Id.Value, HouseSubjectMoney + ParkSubjectMoney);
                            }
                        }//   foreach (var sub in preSubjects)

                    }// foreach (var houseDept in HouseDeptList)
                    var abc = savelist;

                    //哈希表转换成DT
                    return ExChangeList(savelist, BeginDate, EndDate, PayDate, ChargeSubjectList);

                }// using
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[综合报表]Code:900  ComDeptId:{0} ErrorMsg:{1}", ComDeptId, ex.Message), "PaymentService", FileLogType.Exception);
                throw ex;
            }
        }


        private List<ReportTableDTO> ExChangeList(Dictionary<int?, decimal?> savelist, DateTime BeginDate, DateTime EndDate, DateTime payDate, List<ChargeSubject> ChargeSubjectList)
        {
            int?[] chargesubjectIds = savelist.Keys.ToArray();
            //查询对应的值组合成List
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var paydatalist = (from r in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll()
                                   join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on r.Id equals m.ChargeRecordId
                                   join c in propertyMgrUnitOfWork.ChargBillRepository.GetAll() on m.ChargeBillId equals c.Id
                                   join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll().Where(o => chargesubjectIds.Contains(o.Id)) on c.ChargeSubjectId equals s.Id
                                   where c.BeginDate >= BeginDate && c.EndDate <= EndDate && r.PayDate <= payDate && c.BeginDate.Value.Year >= s.BeginDate.Value.Year && c.BeginDate.Value.Month >= s.BeginDate.Value.Month
                                   && (s.BillPeriod == (int)BillPeriodEnum.DailyCharge || s.BillPeriod == (int)BillPeriodEnum.MonthlyCharge)//必须是周期性收费项目
                                   && s.SubjectType != (int)SubjectTypeEnum.Meter//收费类型要不等于三表
                                   && s.SubjectType != (int)SubjectTypeEnum.SystemPreset//不等于系统
                                   && s.SubjectType != (int)SubjectTypeEnum.Other//不等于一次性费用
                                   select new ReportTableDTO()
                                   {
                                       ChargeSubjectId = s.Id,
                                       ChargeSubjectName = s.Name,
                                       RececiveTotal = r.ChargeType == (int)ChargeTypeEnum.Refund ? -m.Amount.Value : m.Amount.Value

                                   }).ToList();

                var data = (from r in paydatalist
                            group new { r.ChargeSubjectId, r.ChargeSubjectName, r.RececiveTotal } by new { r.ChargeSubjectId, r.ChargeSubjectName } into b
                            select new ReportTableDTO
                            {
                                ChargeSubjectId = b.Key.ChargeSubjectId,
                                ChargeSubjectName = b.Key.ChargeSubjectName,
                                RececiveTotal = b.Sum(c => c.RececiveTotal)

                            }
                            ).ToList();

                List<ReportTableDTO> reportList = new List<ReportTableDTO>();
                foreach (var obj in savelist)
                {
                    var dataobj = data.Find(o => o.ChargeSubjectId == obj.Key.Value);
                    if (dataobj != null)
                    {
                        dataobj.ReliefAmountTotal = 0;
                        dataobj.TotalRecAmount = obj.Value; ;
                        dataobj.UnPaidAmountTotal = dataobj.TotalRecAmount.Value - dataobj.RececiveTotal.Value - dataobj.ReliefAmountTotal;
                        if (dataobj.TotalRecAmount.Value - dataobj.ReliefAmountTotal > 0)
                            dataobj.PayRate = Math.Round(((dataobj.RececiveTotal.Value / dataobj.TotalRecAmount.Value - dataobj.ReliefAmountTotal) * 100), 2).ToString() + "%";
                        else
                            dataobj.PayRate = "0%";
                        reportList.Add(dataobj);
                    }
                    else
                    {
                        ReportTableDTO reportTableDTO = new ReportTableDTO();
                        reportTableDTO.ChargeSubjectName = ChargeSubjectList.Where(o => o.Id == obj.Key.Value).First().Name;
                        reportTableDTO.ReliefAmountTotal = 0;
                        reportTableDTO.TotalRecAmount = obj.Value.Value;
                        reportTableDTO.UnPaidAmountTotal = reportTableDTO.TotalRecAmount.Value - reportTableDTO.RececiveTotal.Value - reportTableDTO.ReliefAmountTotal;
                        if (reportTableDTO.TotalRecAmount.Value - reportTableDTO.ReliefAmountTotal > 0)
                            reportTableDTO.PayRate = Math.Round(((reportTableDTO.RececiveTotal.Value / reportTableDTO.TotalRecAmount.Value - reportTableDTO.ReliefAmountTotal) * 100), 2).ToString() + "%";
                        else
                            reportTableDTO.PayRate = "0%";
                        reportList.Add(reportTableDTO);
                    }
                }

                return reportList;
            }

        }

        public List<ReportTableDTO> CalculationAllPrepaymenHousetByComDeptId(int ComDeptId, DateTime BeginDate, DateTime EndDate, DateTime PayDate, string ResouceNo, int TuitionStatus, int PageIndex, int PageSize, out int totalCount, bool isHouse)
        {
            var rerunDTOList = CalculationAllPrepaymentByComDeptId_New(ComDeptId, BeginDate, EndDate, PayDate, ResouceNo, isHouse);
            rerunDTOList = (from h in rerunDTOList
                            group new { h.HouseDeptId, h.OwnerUserName, h.ResourceName, h.TotalRecAmount, h.ReliefAmountTotal, h.UnPaidAmountTotal, h.RececiveTotal } by new { h.HouseDeptId, h.OwnerUserName, h.ResourceName } into b
                            select new ReportTableDTO
                            {
                                HouseDeptId = b.Key.HouseDeptId,
                                OwnerUserName = b.Key.OwnerUserName,
                                ResourceName = b.Key.ResourceName,
                                RececiveTotal = b.Sum(c => c.RececiveTotal),
                                TotalRecAmount = b.Sum(c => c.TotalRecAmount),
                                ReliefAmountTotal = b.Sum(c => c.ReliefAmountTotal),
                                UnPaidAmountTotal = b.Sum(c => c.UnPaidAmountTotal)
                            }
                           ).ToList();

            foreach (var count in rerunDTOList)
            {
                count.UnPaidAmountTotal = count.TotalRecAmount.Value - count.RececiveTotal.Value - count.ReliefAmountTotal;
                if (count.TotalRecAmount.Value - count.ReliefAmountTotal > 0)
                    count.PayRate = Math.Round(((count.RececiveTotal.Value / count.TotalRecAmount.Value - count.ReliefAmountTotal) * 100), 2).ToString() + "%";
                else
                    count.PayRate = "0%";
            }

            rerunDTOList.Sort(Factory.Comparer);
            if (TuitionStatus == 1)
            {//已交清
                totalCount = rerunDTOList.Where(o => o.UnPaidAmountTotal <= 0).Count();
                rerunDTOList = rerunDTOList.Where(o => o.UnPaidAmountTotal <= 0).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList(); ;
            }
            else if (TuitionStatus == 2)
            {//欠费
                totalCount = rerunDTOList.Where(o => o.UnPaidAmountTotal > 0).Count();
                rerunDTOList = rerunDTOList.Where(o => o.UnPaidAmountTotal > 0).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList(); ;
            }
            else
            {//全部
                totalCount = rerunDTOList.Count();
                rerunDTOList = rerunDTOList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList(); ;
            }
            return rerunDTOList;
        }


        public List<ReportTableDTO> CalculationAllPrepaymenHousetByComDeptIdTotal(int ComDeptId, DateTime BeginDate, DateTime EndDate, DateTime PayDate, string ResouceNo, int TuitionStatus, bool isHouse)
        {
            var rerunDTOList = CalculationAllPrepaymentByComDeptId_New(ComDeptId, BeginDate, EndDate, PayDate, ResouceNo, isHouse);
            rerunDTOList = (from h in rerunDTOList
                            group new { h.HouseDeptId, h.ResourceName, h.TotalRecAmount, h.ReliefAmountTotal, h.UnPaidAmountTotal, h.RececiveTotal } by new { h.HouseDeptId, h.ResourceName } into b
                            select new ReportTableDTO
                            {
                                HouseDeptId = b.Key.HouseDeptId,
                                ResourceName = b.Key.ResourceName,
                                RececiveTotal = b.Sum(c => c.RececiveTotal),
                                TotalRecAmount = b.Sum(c => c.TotalRecAmount),
                                ReliefAmountTotal = b.Sum(c => c.ReliefAmountTotal),
                                UnPaidAmountTotal = b.Sum(c => c.UnPaidAmountTotal)
                            }
                           ).ToList();

            foreach (var count in rerunDTOList)
            {
                count.UnPaidAmountTotal = count.TotalRecAmount.Value - count.RececiveTotal.Value - count.ReliefAmountTotal;
                if (count.TotalRecAmount.Value - count.ReliefAmountTotal > 0)
                    count.PayRate = Math.Round(((count.RececiveTotal.Value / count.TotalRecAmount.Value - count.ReliefAmountTotal) * 100), 2).ToString() + "%";
                else
                    count.PayRate = "0%";
            }

            if (TuitionStatus == 1)
            {//已交清

                rerunDTOList = rerunDTOList.Where(o => o.UnPaidAmountTotal <= 0).ToList(); ;
            }
            else if (TuitionStatus == 2)
            {//欠费

                rerunDTOList = rerunDTOList.Where(o => o.UnPaidAmountTotal > 0).ToList(); ;
            }
            else
            {//全部

                rerunDTOList = rerunDTOList.ToList(); ;
            }
            return rerunDTOList;
        }

        public List<ReportTableDTO> CalculationAllPrepaymentByComDeptId_New(int ComDeptId, DateTime BeginDate, DateTime EndDate, DateTime PayDate, string DoorNo = "", bool isHouse = false)
        {
            try
            {
                PayDate = PayDate.AddDays(1).AddMilliseconds(-1);
                EndDate = EndDate.AddDays(1).AddMilliseconds(-1);
                var houseDeptList = GetIntegratedReportHouseDept(ComDeptId, DoorNo, isHouse);
                //      houseDeptList.Sort(Factory.Comparer);
                //在该时间段里面生成的账单
                var charglist = GetTimeSpanChargBill(ComDeptId, BeginDate, EndDate, houseDeptList);

                //本段时间内的绑定关系
                var subjectHouseRefList = GetTimeSpanSubjectHouseRef(ComDeptId, BeginDate, EndDate);

                //本段时间内的科目
                var chargeSubjectList = GetTimeSpanChargeSubject(ComDeptId, EndDate);

                //该小区的房间-筛选符合条件的房间
                var chargeRecordRetableList = GetTimeSpanChargeRecord2ReTableDTO(BeginDate, EndDate, PayDate, houseDeptList);

                //循环房间 进行预算
                IList<CalculateProperty> hmpList = CalculatePropertyHelper.GetHouseCalculateProperty(ComDeptId);
                IList<CalculateProperty> pmpList = CalculatePropertyHelper.GetParkingSpaceCalculateProperty(ComDeptId, hmpList);
                List<ReportTableDTO> returnReportList = new List<ReportTableDTO>();
                foreach (var house in houseDeptList)
                {
                    //取出该房间下面存在的账单和绑定关系
                    var houseChargList = charglist.Where(o => o.HouseDeptId == house.Id).ToList();
                    //获取
                    var preSubjects = (from c in chargeSubjectList
                                       join r in subjectHouseRefList
                                       on c.Id equals r.ChargeSubjecId
                                       where r.HouseDeptId == house.Id.Value && r.IsDel == false
                                       where c.ComDeptId == ComDeptId
                                       && (c.BillPeriod == (int)BillPeriodEnum.DailyCharge || c.BillPeriod == (int)BillPeriodEnum.MonthlyCharge)//必须是周期性收费项目
                                       && c.SubjectType != (int)SubjectTypeEnum.Meter//收费类型要不等于三表
                                       && c.SubjectType != (int)SubjectTypeEnum.SystemPreset//不等于系统
                                           && c.SubjectType != (int)SubjectTypeEnum.Other//不等于一次性费用
                                       && c.IsDel == false
                                       && r.IsDel == false
                                       select new CountChargesubjectSubref { chargeSubject = c, subjectHouseRef = r }).ToList();

                    returnReportList.AddRange(CountIntegratedReportReportTableDTO(ComDeptId, preSubjects, houseChargList, BeginDate, EndDate, hmpList, pmpList));
                }

                var ChargListReDTO = charglist.Select(o => new ReportTableDTO()
                {
                    HouseDeptId = o.HouseDeptId,
                    ChargeSubjectId = o.ChargeSubjectId,
                    TotalRecAmount = o.BillAmount,
                    ReliefAmountTotal = o.ReliefAmount.Value
                }).ToList();
                returnReportList.AddRange(ChargListReDTO);
                returnReportList.AddRange(chargeRecordRetableList);
                foreach (var entity in returnReportList)
                {
                    var subject = chargeSubjectList.Where(o => o.Id == entity.ChargeSubjectId).FirstOrDefault();
                    if (subject == null)
                    {
                        entity.ChargeSubjectName = "";
                    }
                    else
                    {
                        entity.ChargeSubjectName = chargeSubjectList.Where(o => o.Id == entity.ChargeSubjectId).First().Name;
                    }
                    var house = houseDeptList.Where(o => o.Id == entity.HouseDeptId).FirstOrDefault();
                    if (house != null)
                    {
                        entity.ResourceName = house.Name;
                        entity.OwnerUserName = house.OwnerUserName;
                    }
                }
                return returnReportList;
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[综合报表]Code:900  ComDeptId:{0} ErrorMsg:{1}", ComDeptId, ex.Message), "PaymentService", FileLogType.Exception);
                throw ex;
            }
        }

        /// <summary>
        /// 获取时间段内的已经生成账单数据
        /// </summary>
        /// <returns></returns>
        private List<ChargBill> GetTimeSpanChargBill(int ComDeptId, DateTime BeginDate, DateTime EndDate, List<DeptInfo> houseDeptList)
        {
            try
            {
                int?[] houseDeptListidstr = houseDeptList.Select(o => o.Id).ToArray();
                //获取账单生成数据
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    var ChargeBillList = (from b in propertyMgrUnitOfWork.ChargBillRepository.GetAll()
                                          join c in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on b.ChargeSubjectId equals c.Id
                                          where b.BeginDate >= BeginDate && b.EndDate <= EndDate
                                                       && (c.BillPeriod == (int)BillPeriodEnum.DailyCharge || c.BillPeriod == (int)BillPeriodEnum.MonthlyCharge)//必须是周期性收费项目
                                                && c.SubjectType != (int)SubjectTypeEnum.Meter//收费类型要不等于三表
                                                && c.SubjectType != (int)SubjectTypeEnum.SystemPreset//不等于系统
                                                    && c.SubjectType != (int)SubjectTypeEnum.Other//不等于一次性费用
                                                    && b.ComDeptId == ComDeptId
                                                    && b.IsDel == false
                                                    && houseDeptListidstr.Contains(b.HouseDeptId)
                                          select b
                                   ).ToList();

                    return ChargeBillList;
                }
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[综合报表]Code:900  ComDeptId:{0} ErrorMsg:{1}", ComDeptId, ex.Message), "PaymentService", FileLogType.Exception);
                throw ex;
            }
        }


        /// <summary>
        /// 时段范围内的绑定关系数据
        /// </summary>
        /// <param name="ComDeptId"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        private List<SubjectHouseRef> GetTimeSpanSubjectHouseRef(int ComDeptId, DateTime BeginDate, DateTime EndDate)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var SubjectHouseRefList = (from c in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll()
                                           join r in propertyMgrUnitOfWork.SubjectHouseRefRepository.GetAll() on c.Id equals r.ChargeSubjecId
                                           where c.BeginDate <= EndDate
                                            && (c.BillPeriod == (int)BillPeriodEnum.DailyCharge || c.BillPeriod == (int)BillPeriodEnum.MonthlyCharge)//必须是周期性收费项目
                                            && c.SubjectType != (int)SubjectTypeEnum.Meter//收费类型要不等于三表
                                            && c.SubjectType != (int)SubjectTypeEnum.SystemPreset//不等于系统费用
                                            && c.SubjectType != (int)SubjectTypeEnum.Other//不等于一次性费用
                                            && c.IsDel == false
                                            && r.IsDel == false
                                            && c.ComDeptId == ComDeptId
                                           select r).ToList();
                return SubjectHouseRefList;
            }
        }

        /// <summary>
        /// 时间段范围内科目数据
        /// </summary>
        /// <param name="ComDeptId"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        private List<ChargeSubject> GetTimeSpanChargeSubject(int ComDeptId, DateTime EndDate)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var ChargeSubjectList = propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll().Where(o => o.ComDeptId == ComDeptId
                                               && o.BillPeriod == (int)BillPeriodEnum.DailyCharge || o.BillPeriod == (int)BillPeriodEnum.MonthlyCharge
                                               && o.SubjectType != (int)SubjectTypeEnum.Meter).ToList();
                return ChargeSubjectList;
            }

        }


        /// <summary>
        /// 获取该次查询中需要引用的报表
        /// </summary>
        /// <param name="ComDeptId"></param>
        /// <param name="chargBillList"></param>
        /// <param name="subjectHouseRefList"></param>
        /// <param name="DoorNo"></param>
        /// <returns></returns>
        private List<DeptInfo> GetIntegratedReportHouseDept(int ComDeptId, string DoorNo = "", bool isHouse = false)
        {
            //var HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptListByCommunityDeptId(ComDeptId.ToString());
            //2017-3-9 增加业主姓名
            if (isHouse)
            {
                var HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptAndOwnerListByComDeptId(ComDeptId.ToString());
                if (DoorNo != "")
                {
                    HouseDeptList = HouseDeptList.Where(o => o.Name.Contains(DoorNo)).ToList();
                }
                return HouseDeptList;
            }
            else
            {
                var HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptListByCommunityDeptId(ComDeptId.ToString());
                if (DoorNo != "")
                {
                    HouseDeptList = HouseDeptList.Where(o => o.Name.Contains(DoorNo)).ToList();
                }
                return HouseDeptList;
            }

        }

        /// <summary>
        /// 获取时间范围内的金额
        /// </summary>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="payDate"></param>
        /// <param name="HouseList"></param>
        /// <returns></returns>
        private List<ReportTableDTO> GetTimeSpanChargeRecord2ReTableDTO(DateTime BeginDate, DateTime EndDate, DateTime payDate, List<DeptInfo> HouseList)
        {
            int?[] HouseIds = HouseList.Select(o => o.Id).ToArray();
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var paydatalist = (from r in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll()
                                   join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on r.Id equals m.ChargeRecordId
                                   join c in propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(o => HouseIds.Contains(o.HouseDeptId)) on m.ChargeBillId equals c.Id
                                   join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on c.ChargeSubjectId equals s.Id
                                   where c.BeginDate >= BeginDate && c.EndDate <= EndDate && r.PayDate <= payDate
                                  && (s.BillPeriod == (int)BillPeriodEnum.DailyCharge || s.BillPeriod == (int)BillPeriodEnum.MonthlyCharge)//必须是周期性收费项目
                                  && s.SubjectType != (int)SubjectTypeEnum.Meter//收费类型要不等于三表
                                  && s.SubjectType != (int)SubjectTypeEnum.SystemPreset//不等于系统
                                  && s.SubjectType != (int)SubjectTypeEnum.Other//不等于一次性费用
                                  && r.IsDel == false
                                   select new ReportTableDTO()
                                   {
                                       HouseDeptId = m.HouseDeptId.Value,
                                       ChargeSubjectId = s.Id,
                                       RececiveTotal = r.ChargeType == (int)ChargeTypeEnum.Refund ? -m.Amount.Value : m.Amount.Value

                                   }).ToList();
                return paydatalist;
            }

        }

        private List<ReportTableDTO> CountIntegratedReportReportTableDTO(int ComDeptId, IList<CountChargesubjectSubref> cslist, List<ChargBill> houseChargList, DateTime BeginDate, DateTime EndDate, IList<CalculateProperty> hmpList, IList<CalculateProperty> pmpList)
        {
            string qstr = string.Empty;
            decimal money = 0;
            List<ReportTableDTO> BudgetRetableList = new List<ReportTableDTO>();
            foreach (var sub in cslist)
            {
                List<ChargBill> ChargBillList = houseChargList.Where(o => o.ChargeSubjectId == sub.chargeSubject.Id && o.HouseDeptId == sub.subjectHouseRef.HouseDeptId).ToList();
                if (sub.chargeSubject.SubjectType == (int)SubjectTypeEnum.House)
                {
                    if (hmpList.Where(o => o.HouseDeptID == sub.subjectHouseRef.HouseDeptId).ToList().Count() > 0)
                    {
                        money = CountAmountByTimeSpan(ChargBillList, BeginDate, EndDate, sub.chargeSubject, hmpList.Where(o => o.HouseDeptID == sub.subjectHouseRef.HouseDeptId).ToList().First());
                    }
                }
                else if (sub.chargeSubject.SubjectType == (int)SubjectTypeEnum.ParkingSpace)
                {
                    foreach (var pitem in pmpList.Where(p => p.ResourcesId == sub.subjectHouseRef.ResourcesId))
                    {
                        money = CountAmountByTimeSpan(ChargBillList, BeginDate, EndDate, sub.chargeSubject, pitem);
                    }
                }

                if (money > 0)
                {
                    ReportTableDTO reportTableDTO = new ReportTableDTO();
                    reportTableDTO.HouseDeptId = sub.subjectHouseRef.HouseDeptId;
                    reportTableDTO.ChargeSubjectId = sub.chargeSubject.Id;
                    reportTableDTO.TotalRecAmount = money;
                    BudgetRetableList.Add(reportTableDTO);
                }
            }

            List<ReportTableDTO> retrunList = new List<ReportTableDTO>();
            retrunList.AddRange(BudgetRetableList);
            return retrunList;
        }


        /// <summary>
        /// 计算时间内的数据
        /// </summary>
        /// <param name="chargBillis"></param>
        /// <param name="beginDate"></param>
        /// <param name="enddate"></param>
        /// <param name="chargeSubject"></param>
        /// <param name="calculateProperty"></param>
        /// <returns></returns>
        private decimal CountAmountByTimeSpan(List<ChargBill> chargBillis, DateTime beginDate, DateTime enddate, ChargeSubject chargeSubject, CalculateProperty calculateProperty)
        {
            decimal Money = 0;
            if (calculateProperty.HouseStatus.HasValue)
            {
                if (((int)calculateProperty.HouseStatus.Value == (int)HouseStatusEnum.Unsold) && calculateProperty.UnsoldIsBindDeveloper.HasValue && calculateProperty.UnsoldIsBindDeveloper.Value == false)
                {//未售房并且小区没有绑定房间开发商收费
                    return Money;
                }
            }
            else
            {
                return Money;
            }
            if (chargBillis != null && chargBillis.Count > 0)
            {//有账单
                var chargBillisEnd = chargBillis.OrderByDescending(o => o.EndDate).ToList().First();
                if (chargBillisEnd.BeginDate < enddate && chargBillisEnd.EndDate > beginDate)
                {//在查询范围内

                    return CountAmountByTimeSpan_CB(chargeSubject, beginDate, enddate, calculateProperty, chargBillisEnd);
                }
                else
                {
                    return CountAmountByTimeSpan_CB(chargeSubject, beginDate, enddate, calculateProperty);
                }
            }
            else
            {//没账单
                return CountAmountByTimeSpan_CB(chargeSubject, beginDate, enddate, calculateProperty);
            }
        }

        private decimal CountAmountByTimeSpan_CB(ChargeSubject chargeSubject, DateTime beginDate, DateTime endDate, CalculateProperty calculateProperty, ChargBill chargbill = null)
        {
            int year = 0;
            int month = 0;
            int Monthtotal = 0;
            string qstr = string.Empty;
            if (chargeSubject.BeginDate <= endDate)
            {//在开始时间之内
                //生成当月记录
                if (chargbill != null)
                {
                    //首先看是否生成
                    if (chargbill.EndDate > chargeSubject.BeginDate)
                    {//账单结束时间大于收费项目开始时间
                        //上个月最后一天
                        DateTime MonthofFirstDay = (DateTime.Now.AddDays(-DateTime.Now.Day + 1).AddMilliseconds(-1));
                        if (chargbill.EndDate > MonthofFirstDay)
                        {
                            //就按照账单的结束时间来生成账单
                            year = endDate.Year - chargbill.EndDate.Value.Year;
                            month = endDate.Month - chargbill.EndDate.Value.Month;
                            Monthtotal = year * 12 + month;
                        }
                        else
                        {
                            //按照当前月来吧
                            year = endDate.Year - chargbill.EndDate.Value.Year;
                            month = endDate.Month - chargbill.EndDate.Value.Month;
                            Monthtotal = year * 12 + month;
                        }
                    }
                }
                else
                {//从当月开始生成记录

                    //当月时间和开始时间
                    if (DateTime.Now < beginDate)
                    {
                        year = endDate.Year - beginDate.Year;
                        month = endDate.Month - beginDate.Month;
                        Monthtotal = year * 12 + month + 1;
                    }
                    else
                    {
                        year = endDate.Year - DateTime.Now.Year;
                        month = endDate.Month - DateTime.Now.Month;
                        Monthtotal = year * 12 + month + 1;
                    }
                }

                return BillCommonService.Instance.CalculateAmount(chargeSubject, calculateProperty, ref qstr) * Monthtotal;
            }
            else
            {
                return 0;
            }

        }

        private decimal CountAmountByTimeSpan(List<ChargBill> chargBillis, DateTime enddate, decimal MonthAmount)
        {
            //先按照时间进行排序
            chargBillis = chargBillis.OrderByDescending(o => o.EndDate).ToList();
            var billmoney = chargBillis.Sum(o => o.BillAmount);
            //取出最大的进行后面的筛选
            ChargBill chargBillEnd = chargBillis[0];
            //剩余时间进行计算月份
            int year = enddate.Year - chargBillEnd.EndDate.Value.Year;
            int month = enddate.Month - chargBillEnd.EndDate.Value.Month;
            int Monthtotal = year * 12 + month;
            return billmoney.Value + (MonthAmount * Monthtotal);
        }

        private class CountChargesubjectSubref
        {
            public SubjectHouseRef subjectHouseRef { get; set; }
            public ChargeSubject chargeSubject { get; set; }
        }

        #region 生成预结算
        /// <summary>
        /// 结算
        /// </summary>
        /// <param name="communityIdentity">小区ID</param>
        /// <param name="occurMoney"> 发生金额</param>
        /// <param name="originId">缴费单号</param>
        /// <param name="type">结算的入口（1.APP推送的结算，2.后台推送的结算）</param>
        public ResultModel SettleAccountingPay(string chargeRecordId, int type)
        {
            string settlementIsOpen = ConfigurationManager.AppSettings["SettlementIsOpen"];
            if (!Convert.ToBoolean(settlementIsOpen.ToUpper()))
            {
                return new ResultModel() { IsSuccess = false, Msg = "未开启预结算、无法生成预结算" };
            }

            ChargeRecord chargeRecord = new ChargeRecord();
            BusinessCallClient preSettlementClient = new BusinessCallClient();

            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                chargeRecord = propertyMgrUnitOfWork.ChargeRecordRepository.GetByKey(chargeRecordId);
                if (type == (int)SettleAccountWayEnum.APPWAY)
                {
                    if (chargeRecord.AccountingStatus == (int)AccountingStatusEnum.NotApplicable)
                    {
                        chargeRecord.AccountingStatus = (int)AccountingStatusEnum.NoSettlement;
                        propertyMgrUnitOfWork.ChargeRecordRepository.Update(chargeRecord);
                    }
                }
                propertyMgrUnitOfWork.Commit();
            }
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                try
                {
                    if (chargeRecord.AccountingStatus == (int)AccountingStatusEnum.NoSettlement)
                    {
                        var result = preSettlementClient.CreatePropertyPreSettlement(new PrePropertySettlementMaterial()
                        {
                            BusinessTime = Convert.ToDateTime(chargeRecord.PayDate),
                            OccurMoney = Convert.ToDecimal(chargeRecord.Amount),
                            CommunityIdentity = chargeRecord.ComDeptId.ToString(),
                            OriginId = chargeRecord.SerialNumber
                        });
                        chargeRecord.AccountingStatus = result.IsSuccess ? (int)AccountingStatusEnum.BeenSettled : (int)AccountingStatusEnum.NoSettlement;
                        propertyMgrUnitOfWork.ChargeRecordRepository.Update(chargeRecord);
                        bool isSuccess = propertyMgrUnitOfWork.Commit();
                        if (isSuccess)
                        {
                            return new ResultModel() { IsSuccess = true, Msg = "生成预结算成功" };
                        }
                        else
                        {
                            return new ResultModel() { IsSuccess = false, Msg = "生成预结算失败" };
                        }
                    }
                    else
                    {
                        return new ResultModel() { IsSuccess = false, Msg = "当前记录" + (chargeRecord.AccountingStatus == (int)AccountingStatusEnum.BeenSettled ? "已生成预结算" : "不适用") };
                    }
                }
                catch (Exception ex)
                {
                    chargeRecord.AccountingStatus = (int)AccountingStatusEnum.NoSettlement;
                    propertyMgrUnitOfWork.ChargeRecordRepository.Update(chargeRecord);
                    propertyMgrUnitOfWork.Commit();

                    LogProperty.WriteLoginToFile(string.Format("[生成预结算] chargeRecordId:{0} ErrorMsg:{1}", chargeRecordId, ex.Message), "SettleAccountingPay", FileLogType.Exception);
                    return new ResultModel() { IsSuccess = false, Msg = "预结算失败" };
                }
            }
        }

        #endregion

        #region APP 接口 

        /// <summary>
        /// app 缴费验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AppBillsPaymentCheck(PaymentModel model)
        {
            try
            {
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    var billList = new List<ChargBill>();
                    //公共验证
                    return PaymentValidation(propertyMgrUnitOfWork, model, billList);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                LogProperty.WriteLoginToFile(string.Format("[APP缴费验证]Code:900 Bills ID:{0} Amount:{1} ErrorMsg:{2}", string.Join(",", model.BillIDs), model.Amount.ToString(), ex.Message), "PaymentService", FileLogType.Exception);
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "验证失败" };
            }
        }

        /// <summary>
        /// 获取缴费明细
        /// </summary>
        /// <param name="chargeRecord"></param>
        /// <returns></returns>
        public AppChargeRecordDetail GetChargeRecordDetail(string chargeRecordId)
        {
            AppChargeRecordDetail detail = new AppChargeRecordDetail();
            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
            var payList = propertyService.GetDictionaryModels(PropertyEnumType.PayType.ToString());
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var chargeRecord = pmUnitWork.ChargeRecordRepository.GetByKey(chargeRecordId);
                if (chargeRecord != null)
                {
                    detail.Amount = chargeRecord.Amount;
                    detail.DiscountAmount = chargeRecord.DiscountAmount;
                    if (detail.DiscountAmount > 0)
                    {
                        //目前只有优惠券
                        detail.DiscountTypeName = "优惠券";
                        //var discount = pmUnitWork.PaymentDiscountInfoRepository.GetAll()
                        //                .Where(p => p.ChargeRecordId == chargeRecord.Id)
                        //                .FirstOrDefault();
                        //if (discount != null)
                        //{
                        //    detail.DiscountTypeName = (DiscountTypeEnum)discount.DiscountType == DiscountTypeEnum.Coupon ? "优惠券" : "";
                        //}
                    }
                    detail.PaymentDate = chargeRecord.PayDate.Value.ToString("yyyy-MM-dd HH:mm");
                    var paytype = payList.Where(p => p.Code == chargeRecord.PayMthodId.ToString()).FirstOrDefault();
                    detail.PaymentTypeName = paytype == null ? "" : paytype.CnName + "支付";

                    //账单明细
                    var billList = (from b in pmUnitWork.ChargBillRepository.GetAll()
                                    join m in pmUnitWork.ChargeBillRecordMatchingRepository.GetAll()
                                    on b.Id equals m.ChargeBillId
                                    where m.ChargeRecordId == chargeRecord.Id
                                    select new
                                    {
                                        SubjectName = b.ChargeSubject.Name,
                                        b.BeginDate,
                                        b.EndDate,
                                        b.RefType,
                                        BillId = b.Id,
                                        m.Amount,
                                        b.Remark
                                    }).ToList();
                    detail.BillDetailList = billList
                        .Select(b => new AppBillDetail()
                        {
                            Amount = b.Amount,
                            SubjectName = b.SubjectName,
                            Desc = (b.RefType == (int)SubjectTypeEnum.Meter ?
                                                GetMeterBillDesc(pmUnitWork, b.BillId)
                                                : GetBillDesc(b.BeginDate, b.EndDate, b.Remark))
                        }).ToList();
                }
            }
            return detail;
        }

        /// <summary>
        /// 获取三表账单描述
        /// </summary>
        /// <param name="billId"></param>
        /// <returns></returns>
        private string GetMeterBillDesc(IPropertyMgrUnitOfWork pmUnitWork, string billId)
        {
            string meterDesc = string.Empty;
            var meterRead = pmUnitWork.MeterReadRecordRepository.GetAll().Where(b => b.BillID.Contains(billId)).FirstOrDefault();
            if (meterRead != null)
            {
                meterDesc = "读数(";
                //求上一条
                var previous = pmUnitWork.MeterReadRecordRepository
                                .GetAll()
                                .Where(b => b.MeterId == meterRead.MeterId
                                && b.ReadDate < meterRead.ReadDate)
                                .OrderByDescending(b => b.ReadDate)
                                .FirstOrDefault();
                if (previous != null)
                {
                    meterDesc += (previous.MeterValue.ToString() + "至");
                }
                meterDesc += (meterRead.MeterValue.ToString() + ")");
            }
            return meterDesc;
        }

        private string GetBillDesc(DateTime? BeginDate, DateTime? EndDate, string Remark)
        {
            string meterDesc = string.Empty;
            if (BeginDate.HasValue)
            {
                meterDesc += BeginDate.Value.ToString("yyyy-MM-dd");
            }
            if (EndDate.HasValue)
            {
                meterDesc += ("至" + EndDate.Value.ToString("yyyy-MM-dd"));
            }
            //如果没有值 显示 备注
            if (string.IsNullOrEmpty(meterDesc))
            {
                meterDesc = Remark;
            }
            return meterDesc;
        }

        /// <summary>
        /// 生成预存费列表
        /// </summary>
        /// <param name="subjectCost"></param>
        /// <returns></returns>
        private List<ChargBill> GeneratePreBillList(AppSubjectPreCost subjectCost)
        {
            List<ChargBill> billList = new List<ChargBill>();
            if (subjectCost.SubjectIds == null || subjectCost.SubjectIds.Count() <= 0)
            {
                return billList;
            }
            var monthDataList = CalculationMonthPrePaymentList(subjectCost.HouseDeptId, false);
            //求房屋资源名
            string resourcesName = string.Empty;
            var deptInfo = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoById(subjectCost.HouseDeptId.ToString());
            if (deptInfo != null)
            {
                resourcesName = deptInfo.Name;
            }

            //循环收费项目生产预交费账单
            foreach (var subjectId in subjectCost.SubjectIds)
            {
                var monthData = monthDataList.Where(m => m.SubjectId == subjectId).FirstOrDefault();
                if (monthData != null)
                {
                    var remark = string.Format("{0} 预存{1}个月 ", monthData.SubjectName, subjectCost.Month.Value);
                    var bill = BillCommonService.Instance.GenerateTempPrepaymentBill(
                        subjectCost.ComDeptId, subjectCost.HouseDeptId, subjectCost.HouseDeptId, resourcesName,
                        monthData.PreAmount.Value * subjectCost.Month.Value, BillStatusEnum.NoPayment, remark, null, null
                    );
                    //指定预存项目的Id
                    bill.PreChargeSubjectId = subjectId;
                    billList.Add(bill);
                }
            }
            return billList;
        }

        private ChargeSubject GetSubjectById(int? subjectId)
        {
            using (var pMUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return pMUnitWork.ChargeSubjectRepository.GetByKey(subjectId);
            }
        }

        /// <summary>
        /// 生成用户手动输入金额账单
        /// </summary>
        /// <param name="subjectCost"></param>
        /// <returns></returns>
        private List<ChargBill> GenerateManualPreBillList(AppSubjectPreCost subjectCost)
        {
            List<ChargBill> billList = new List<ChargBill>();
            //求房屋资源名
            string resourcesName = string.Empty;
            var deptInfo = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoById(subjectCost.HouseDeptId.ToString());
            if (deptInfo != null)
            {
                resourcesName = deptInfo.Name;
            }

            //循环收费项目生产预交费账单
            foreach (var item in subjectCost.ManualSubjectPreCostList)
            {
                var subject = GetSubjectById(item.SubjectId);
                if (subject != null && item.PreCost > 0)
                {
                    var remark = string.Format("{0} 预存金额{1} ", subject.Name, item.PreCost);
                    var bill = BillCommonService.Instance.GenerateTempPrepaymentBill(
                        subjectCost.ComDeptId, subjectCost.HouseDeptId, subjectCost.HouseDeptId, resourcesName,
                        item.PreCost.Value, BillStatusEnum.NoPayment, remark, null, null
                    );
                    //指定预存项目的Id
                    bill.PreChargeSubjectId = subject.Id;
                    billList.Add(bill);
                }
            }
            return billList;
        }

        /// <summary>
        /// 预交费检查
        /// </summary>
        /// <param name="subjectCost"></param>
        /// <returns></returns>
        public ResultModel SubjectPreCostCheck(AppSubjectPreCost subjectCost)
        {
            //生成预交费账单
            var billList = GeneratePreBillList(subjectCost);
            //获取用户手动输入金额 添加：2017-07-07 tdz
            if (subjectCost.ManualSubjectPreCostList.Count() > 0)
            {
                var manualBillList = GenerateManualPreBillList(subjectCost);
                billList.AddRange(manualBillList);
            }

            //缴费金额
            decimal? amount = billList.Sum(b => b.BillAmount);

            //支付金额 = 预存月数 * 收费项目合计金额 - 优惠金额
            var payAmonut = amount - (subjectCost.DiscountInfo == null ? 0 : subjectCost.DiscountInfo.DiscountAmount);
            if (payAmonut < 0)
            {
                payAmonut = 0;
            }

            //支付金额 - 用户手动输入金额 与实际需支付金额不一致
            if (subjectCost.PaymentAmount != payAmonut)
            {
                return new ResultModel()
                {
                    ErrorCode = "709",
                    IsSuccess = false,
                    Msg = "支付金额与实际需支付金额不一致"
                };
            }
            PaymentModel model = new PaymentModel();
            model.NewBillList = billList;
            model.IsOnline = true;
            model.Operator = -1;
            model.OperatorName = "逸社区APP";
            model.PayType = subjectCost.PayType;
            model.Remark = "缴预存费";
            model.ChargeType = ChargeTypeEnum.DailyCharge;
            model.Amount = amount.Value;
            model.CustomerName = subjectCost.CustomerName;
            if (subjectCost.DiscountInfo != null)
            {
                model.DiscountInfo = PaymentDiscountInfoMappers.ChangeDTOToPaymentDiscountInfoNew(subjectCost.DiscountInfo);
            }
            ResultModel result = new ResultModel();
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var bList = new List<ChargBill>();
                //公共验证
                result = PaymentValidation(pmUnitWork, model, bList);
            }
            return result;
        }

        /// <summary>
        /// 保存预交费
        /// </summary>
        /// <param name="subjectCost"></param>
        /// <returns></returns>
        public ResultModel SaveSubjectPreCost(AppSubjectPreCost subjectCost, string OperatorName)
        {
            //生成预交费账单
            var billList = GeneratePreBillList(subjectCost);
            //获取用户手动输入金额 添加：2017-07-07 tdz
            if (subjectCost.ManualSubjectPreCostList.Count() > 0)
            {
                var manualBillList = GenerateManualPreBillList(subjectCost);
                billList.AddRange(manualBillList);
            }

            //缴费金额
            decimal? amount = billList.Sum(b => b.BillAmount);

            //支付金额与实际需支付金额不一致
            if (subjectCost.PaymentAmount != amount)
            {
                return new ResultModel()
                {
                    ErrorCode = "709",
                    IsSuccess = false,
                    Msg = "支付金额与实际需支付金额不一致"
                };
            }

            PaymentModel model = new PaymentModel();
            model.NewBillList = billList;
            model.IsOnline = true;
            model.Operator = -1;
            model.OperatorName = OperatorName;//"逸社区APP";
            model.PayType = subjectCost.PayType;
            model.Remark = "缴预存费";
            model.ChargeType = ChargeTypeEnum.DailyCharge;
            model.Amount = amount.Value;
            model.CustomerName = subjectCost.CustomerName;
            model.IsPrintReceipt = false;
            if (subjectCost.DiscountInfo != null)
            {
                model.DiscountInfo = PaymentDiscountInfoMappers.ChangeDTOToPaymentDiscountInfoNew(subjectCost.DiscountInfo);
            }

            var result = BillsPayment(model);
            return result;
        }

        public List<AppChargeRecord> GetChargeHistoryRecordList(int? houseDeptId, int pageIndex, int pageSize)
        {
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //时间倒序展示用户使用app的缴费记录 修改bug #4643 2017-7-10
                var query = pmUnitWork.ChargeRecordRepository.GetAll().Where(c => c.HouseDeptId == houseDeptId && c.IsOnline == true);
                var dataList = query.OrderByDescending(q => q.PayDate)
                                    .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return dataList.Select(d => new AppChargeRecord()
                {
                    Id = d.Id,
                    PaymentDate = d.PayDate.Value.ToString("yyyy-MM-dd HH:mm"),
                    Amount = d.Amount,
                    Desc = GetChargeBillDesc(pmUnitWork, d.Id)
                }).ToList();
            }
        }

        private string GetChargeBillDesc(IPropertyMgrUnitOfWork pmUnitWork, string chargeRecordId)
        {
            var descList = (from m in pmUnitWork.ChargeBillRecordMatchingRepository.GetAll()
                            join b in pmUnitWork.ChargBillRepository.GetAll()
                            on m.ChargeBillId equals b.Id
                            where m.ChargeRecordId == chargeRecordId
                            select b.Description).Distinct().ToList();
            return string.Join("、", descList);
        }

        #endregion

        #endregion

        #region 生成票据号

        public string GenerateReceiptBookNumberByCommDeptId(int commDeptId)
        {
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return BillCommonService.Instance.GenerateReceiptBookNumber(pmUnitWork, commDeptId);
            }
        }

        public string GenerateReceiptBookNumberByHouseDeptId(int houseDeptId)
        {
            var commDeptId = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetCommunityDeptIdByHouseDeptId(houseDeptId);
            return GenerateReceiptBookNumberByCommDeptId(commDeptId);
        }

        #endregion

        #region 终端统一支付日志

        public void CreateClientPaymentLog(string numericalNumber, ClientPayOrder payOrder)
        {
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                ClientPaymentLog log = new ClientPaymentLog();
                log.NumericalNumber = numericalNumber;
                log.OrderNum = payOrder.OrderNum;
                log.PayState = 0;//待支付
                log.OrderData = JsonHelper.JsonSerializerByNewtonsoft(payOrder);
                log.CreateTime = DateTime.Now;
                log.Desc = "待支付";
                pmUnitWork.ClientPaymentLogRepository.Add(log);
                pmUnitWork.Commit();
            }
        }

        public void CallBackUpdateClientPaymentLog(string numericalNumber, string ChargeRecordId, int? payState, int? payType, string desc)
        {
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                ClientPaymentLog log = pmUnitWork.ClientPaymentLogRepository.GetAll().Where(c => c.NumericalNumber == numericalNumber).FirstOrDefault();
                log.ChargeRecordId = ChargeRecordId;
                log.PayState = payState;
                log.PayType = payType;
                log.CallBackTime = DateTime.Now;
                log.Desc = desc;
                pmUnitWork.ClientPaymentLogRepository.Update(log);
                pmUnitWork.Commit();
            }
        }

        public ClientPaymentLog GetClientPayLogByNumericalNumber(string numericalNumber)
        {
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var payOrder = pmUnitWork.ClientPaymentLogRepository.GetAll().Where(c => c.NumericalNumber == numericalNumber).FirstOrDefault();
                return payOrder;
            }
        }

        public int? GetClientPayOrderState(string numericalNumber)
        {
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var payOrder = pmUnitWork.ClientPaymentLogRepository.GetAll().Where(c => c.NumericalNumber == numericalNumber).FirstOrDefault();
                if (payOrder == null)
                {
                    return -2;
                }
                return payOrder.PayState;
            }
        }

        /// <summary>
        /// 终端用户取消支付 或 跳转到其它页面
        /// </summary>
        /// <param name="numericalNumber"></param>
        public void ClientLeavePay(string numericalNumber)
        {
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var payOrder = pmUnitWork.ClientPaymentLogRepository.GetAll().Where(c => c.NumericalNumber == numericalNumber).FirstOrDefault();
                //PayState == 0 待支付状态 才可以更新
                if (payOrder != null && payOrder.PayState == 0)
                {
                    payOrder.PayState = 6;
                    payOrder.Desc = "用户离开页面";
                    payOrder.CallBackTime = DateTime.Now;
                    pmUnitWork.ClientPaymentLogRepository.Update(payOrder);
                    pmUnitWork.Commit();
                }
            }
        }

        #endregion

        #region 排序方法
        class Factory : IComparer<ReportTableDTO>
        {
            private Factory() { }
            public static IComparer<ReportTableDTO> Comparer
            {
                get { return new Factory(); }
            }
            public int Compare(ReportTableDTO x, ReportTableDTO y)
            {
                try
                {

                    return x.ResourceName.Length == y.ResourceName.Length ? x.ResourceName.CompareTo(y.ResourceName) : x.ResourceName.Length - y.ResourceName.Length;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

    }
}
