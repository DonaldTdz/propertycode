using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.CompositeDomainService;
using YK.PropertyMgr.CompositeDomainService.Model;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.DomainService;

namespace YK.PropertyMgr.CompositeAppService
{
    public class PaymentAppService
    {
        #region 收费 日常收费 临时收费 对外收费

        /// <summary>
        /// 日常收费
        /// </summary>
        //public static ResultModel BillsDailyPayment(string[] BillIDs, IList<ChargBill> newList, IList<ChargBill> updateList
        //    , List<ChargBill> onlyUpdateBillList, List<ChargBill> onlyNewList, decimal Amount, bool IsReceiptNum
        //    , PayTypeEnum PayType, bool IsChangeStore, int Operator, string OperatorName, string Remark
        //    , int SmallToPrepaySubjectId)
        //{
        //    if (Operator == 0 && string.IsNullOrEmpty(OperatorName))
        //    {
        //        OperatorName = BillCommonService.SystemOperatorName;
        //    }
        //    //if (string.IsNullOrEmpty(ReceiptNum))
        //    //{
        //    //    return new ResultModel() { IsSuccess = false, ErrorCode = "710", Msg = "票据号不能为空" };
        //    //}

        //    PaymentModel model = new PaymentModel();
        //    model.BillIDs = BillIDs;
        //    model.NewBillList = newList;
        //    model.UpdateBillList = updateList;
        //    model.Amount = Amount;
        //    model.IsCheckReceiptNum = IsReceiptNum;
        //    model.PayType = PayType;
        //    model.IsChangeStore = IsChangeStore;
        //    model.SmallToPrepaySubjectId = SmallToPrepaySubjectId;
        //    model.ChargeType = ChargeTypeEnum.DailyCharge;
        //    model.Operator = Operator;
        //    model.OperatorName = OperatorName;
        //    model.Remark = Remark;
        //    return PaymentService.Instance.BillsDailyPayment(model, onlyUpdateBillList, onlyNewList);
        //}

        public static ResultModel BillsDailyPayment(DailyChargBillDTO SaveModel, int Operator, string OperatorName)
        {
            if (Operator == 0 && string.IsNullOrEmpty(OperatorName))
            {
                OperatorName = BillCommonService.SystemOperatorName;
            }
            IList<ChargBill> newList = new List<ChargBill>();
            IList<ChargBill> updateList = new List<ChargBill>();
            List<ChargBill> onlyUpdateList = new List<ChargBill>();
            List<ChargBill> onlyNewList = new List<ChargBill>();
            if (SaveModel.NewBillList != null)
            {
                foreach (var item in SaveModel.NewBillList)
                {
                    item.CreateTime = DateTime.Now;
                    if (item.ActionStatus == ActionStatusEnum.New)
                    {
                        if (item.IsChecked)
                        {
                            newList.Add(ChargBillMappers.ChangeDTOToChargBillNew(item));
                        }
                        else
                        {
                            onlyNewList.Add(ChargBillMappers.ChangeDTOToChargBillNew(item));
                        }
                    }
                    else if (item.ActionStatus == ActionStatusEnum.Update)
                    {
                        if (item.IsChecked)
                        {
                            updateList.Add(ChargBillMappers.ChangeDTOToChargBillNew(item));
                        }
                        else
                        {
                            onlyUpdateList.Add(ChargBillMappers.ChangeDTOToChargBillNew(item));
                        }
                    }
                }
            }
            PaymentModel model = new PaymentModel();
            model.BillIDs = SaveModel.BillIds;
            model.NewBillList = newList;
            model.UpdateBillList = updateList;
            model.Amount = SaveModel.ReceivedAmountTotal;
            model.IsCheckReceiptNum = true;
            model.PayType = (PayTypeEnum)SaveModel.PayTypeId;
            model.IsChangeStore = SaveModel.IsSmallToPrepay;
            model.SmallToPrepaySubjectId = SaveModel.SmallToPrepaySubjectId;
            model.ChargeType = ChargeTypeEnum.DailyCharge;
            model.Operator = Operator;
            model.OperatorName = OperatorName;
            model.Remark = SaveModel.Remark;
            //v2.9 添加预存抵扣 2017-9-18
            model.IsPreDeductible = SaveModel.IsPreDeductible;
            model.PreDeductibleAmount = SaveModel.PreDeductibleAmount;
            //v2.9 页面票据打印 2017-9-18
            model.IsPrintReceipt = SaveModel.IsPrintReceipt;
            model.ReceiptNo = SaveModel.ReceiptNo;
            return PaymentService.Instance.BillsDailyPayment(model, onlyUpdateList, onlyNewList);
        }

        /// <summary>
        /// 临时收费
        /// </summary>
        /// <param name="BillIDs">账单IDs</param>
        /// <param name="Amount">缴费金额</param>
        /// <param name="IsReceiptNum">票据号</param>
        /// <param name="PayType">付款方式</param>
        /// <param name="ChargeType">收费类型</param>
        /// <param name="IsChangeStore">是否找零预存</param>
        /// <returns>处理结果</returns>
        public static ResultModel BillsTemporaryPayment(string[] BillIds, IList<ChargBill> ChargBillList, decimal Amount,
            bool IsReceiptNum, PayTypeEnum PayType, bool IsChangeStore, int Operator, string OperatorName, string Remark
            , int SmallToPrepaySubjectId)
        {
            if (Operator == 0 && string.IsNullOrEmpty(OperatorName))
            {
                OperatorName = BillCommonService.SystemOperatorName;
            }
            //if (string.IsNullOrEmpty(ReceiptNum))
            //{
            //    return new ResultModel() { IsSuccess = false, ErrorCode = "710", Msg = "票据号不能为空" };
            //}
            PaymentModel model = new PaymentModel();
            model.BillIDs = BillIds;
            model.NewBillList = ChargBillList;
            model.Amount = Amount;
            model.IsCheckReceiptNum = IsReceiptNum;
            model.PayType = PayType;
            model.IsChangeStore = IsChangeStore;
            model.ChargeType = ChargeTypeEnum.TemporaryCharge;
            model.Operator = Operator;
            model.OperatorName = OperatorName;
            model.Remark = Remark;
            model.SmallToPrepaySubjectId = SmallToPrepaySubjectId;
            return PaymentService.Instance.BillsPayment(model);
        }


        /// <summary>
        /// 对外收费
        /// </summary>
        /// <param name="BillIDs">账单IDs</param>
        /// <param name="Amount">缴费金额</param>
        /// <param name="IsReceiptNum">票据号</param>
        /// <param name="PayType">付款方式</param>
        /// <param name="ChargeType">收费类型</param>
        /// <param name="IsChangeStore">是否找零预存</param>
        /// <returns>处理结果</returns>
        public static ResultModel BillsForeigBillPayment(string[] BillIds, IList<ChargBill> ChargBillList, decimal Amount,
            bool IsReceiptNum, PayTypeEnum PayType, bool IsChangeStore, int Operator, string OperatorName, string Remark,
            string CustomerName)
        {
            if (Operator == 0 && string.IsNullOrEmpty(OperatorName))
            {
                OperatorName = BillCommonService.SystemOperatorName;
            }
            //if (string.IsNullOrEmpty(ReceiptNum))
            //{
            //    return new ResultModel() { IsSuccess = false, ErrorCode = "710", Msg = "票据号不能为空" };
            //}
            PaymentModel model = new PaymentModel();
            model.BillIDs = BillIds;
            model.NewBillList = ChargBillList;
            model.Amount = Amount;
            model.IsCheckReceiptNum = IsReceiptNum;
            model.PayType = PayType;
            model.IsChangeStore = IsChangeStore;
            model.ChargeType = ChargeTypeEnum.ForeignCharge;
            model.Operator = Operator;
            model.OperatorName = OperatorName;
            model.Remark = Remark;
            model.CustomerName = CustomerName;
            return PaymentService.Instance.BillsPayment(model);
        }



        #endregion

        #region 生成交款记录

        /// <summary>
        /// 生成交款记录
        /// </summary>
        /// <param name="chargBillList">账单</param>
        /// <param name="remark">备注</param>
        /// <returns>处理结果</returns>
        public static ResultModel GenerateBillPaymentTask(string[] ChargeRecordIds, string Remark, int Operator, string OperatorName, DateTime PaymentDate)
        {
            if (Operator == 0 && string.IsNullOrEmpty(OperatorName))
            {
                OperatorName = BillCommonService.SystemOperatorName;
            }
            return PaymentService.Instance.GenerateBillPaymentTask(ChargeRecordIds, Remark, Operator, OperatorName, PaymentDate);
        }

        #endregion

        #region 弃审

        public static ResultModel PaymentTasksAbandonRviewed(int PaymentTaskId, int Operator, string OperatorName)
        {
            if (Operator == 0 && string.IsNullOrEmpty(OperatorName))
            {
                OperatorName = BillCommonService.SystemOperatorName;
            }
            return PaymentService.Instance.PaymentTasksAbandonRviewed(PaymentTaskId, Operator, OperatorName);
        }
        #endregion

        #region 审核
        public static ResultModel PaymentTasksRviewed(int PaymentTaskId, int Operator, string OperatorName, string CheckRemark)
        {
            if (Operator == 0 && string.IsNullOrEmpty(OperatorName))
            {
                OperatorName = BillCommonService.SystemOperatorName;
            }
            return PaymentService.Instance.PaymentTasksRviewed(PaymentTaskId, Operator, OperatorName, CheckRemark);
        }
        #endregion

        #region 撤销交款
        public static ResultModel PaymentTasksDelete(int PaymentTaskId, int Operator, string OperatorName)
        {
            if (Operator == 0 && string.IsNullOrEmpty(OperatorName))
            {
                OperatorName = BillCommonService.SystemOperatorName;
            }
            return PaymentService.Instance.PaymentTasksDelete(PaymentTaskId, Operator, OperatorName);
        }

        #endregion

        #region 撤销审核

        public static ResultModel PaymentTasksRevokeRviewed(int PaymentTaskId, int Operator, string OperatorName, string CheckRemark)
        {
            if (Operator == 0 && string.IsNullOrEmpty(OperatorName))
            {
                OperatorName = BillCommonService.SystemOperatorName;
            }
            return PaymentService.Instance.PaymentTasksRevokeRviewed(PaymentTaskId, Operator, OperatorName, CheckRemark);
        }
        #endregion

        #region 退款

        public static ResultModel Refund(RefundRecordDTO RefundRecord, int Operator, string OperatorName)
        {
            var refundRecord = RefundRecordMappers.ChangeDTOToRefundRecordNew(RefundRecord);
            return PaymentService.Instance.Refund(refundRecord, Operator, OperatorName);
        }

        #endregion

        #region 是否交款

        public static bool IsSubmitted(string ChargeRecordId)
        {
            return PaymentService.Instance.IsSubmitted(ChargeRecordId);
        }

        #endregion

        #region 票据号

        /// <summary>
        /// 检查票据号是否重复
        /// </summary>
        public static bool CheckReceiptNumRepeat(string ReceiptNum, int ComDeptId, string ReceiptId)
        {
            return PaymentService.Instance.CheckReceiptNumRepeat(ReceiptNum, ComDeptId, ReceiptId);
        }

        public static string GenerateReceiptBookNumber(int DeptId, EDeptType DeptType)
        {
            try
            {
                //小区
                if (DeptType == EDeptType.XiaoQu)
                {
                    return PaymentService.Instance.GenerateReceiptBookNumberByCommDeptId(DeptId);
                }
                else if (DeptType == EDeptType.FangWu)
                {
                    return PaymentService.Instance.GenerateReceiptBookNumberByHouseDeptId(DeptId);
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }

        }

        #endregion

        #region 计算月预交费

        public static decimal? CalculationMonthPrePayment(int HouseDeptId, out bool IsDepPay)
        {
            return PaymentService.Instance.CalculationMonthPrePayment(HouseDeptId, out IsDepPay);
        }

        public static decimal? CalculationMonthPrePayment(int HouseDeptId, int SubjectId)
        {
            return PaymentService.Instance.CalculationMonthPrePayment(HouseDeptId, SubjectId);
        }

        public static IList<SubjectMonthPrePaymentDTO> CalculationMonthPrePaymentList(int HouseDeptId)
        {
            return PaymentService.Instance.CalculationMonthPrePaymentList(HouseDeptId, true);
        }

        public static IList<SubjectMonthPrePaymentDTO> CalculationMonthPrePaymentList(int HouseDeptId, bool NeedAll)
        {
            return PaymentService.Instance.CalculationMonthPrePaymentList(HouseDeptId, NeedAll);
        }

        #endregion

        #region App 微信 平板

        #region 预交费 及 预交费接口 20170613

        public static ResultModel CalculationMonthPrePayment(int? HouseDeptId)
        {
            if (!HouseDeptId.HasValue || HouseDeptId == 0)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "710", Msg = "参数有误" };
            }
            bool IsDepPay;
            decimal? amount = PaymentService.Instance.CalculationMonthPrePayment(HouseDeptId.Value, out IsDepPay);
            return new ResultModel()
            {
                IsSuccess = true,
                ErrorCode = "0",
                Msg = "计算月预交费成功",
                Data = new { MonthPreAmount = amount, IsDepPay = IsDepPay }
            };
        }

        /// <summary>
        /// 检查预交费
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static ResultModel SubjectPreCostCheck(AppSubjectPreCost parameter)
        {
            return PaymentService.Instance.SubjectPreCostCheck(parameter);
        }

        /// <summary>
        /// 预交费
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static ResultModel SaveSubjectPreCostPost(AppSubjectPreCost parameter, string OperatorName)
        {
            //1.验证
            //var result = PaymentService.Instance.SubjectPreCostCheck(parameter);
            //if (!result.IsSuccess)
            //{
            //    return result;
            //}

            //2.生成预存费账单和缴费
            var result = PaymentService.Instance.SaveSubjectPreCost(parameter, OperatorName);

            //3.结算
            if (result.IsSuccess)
            {
                Task.Run(() =>
                {
                    PaymentService.Instance.SettleAccountingPay(result.Data.ToString(), (int)SettleAccountWayEnum.APPWAY);
                });
            }

            return result;
        }

        #endregion

        #region 自助缴费

        /// <summary>
        /// 自助缴费
        /// </summary>
        /// <param name="BillIDs">账单IDs</param>
        /// <param name="Amount">缴费金额</param>
        /// <param name="PayType">付款方式</param>
        /// <param name="ChargeType">收费类型</param>
        /// <returns>处理结果</returns>
        public static ResultModel BillsAppPayment(string[] BillIDs, decimal Amount, PayTypeEnum PayType, int Operator,
            string OperatorName, string CustomerName, PaymentDiscountInfoDTO DiscountInfo)
        {
            switch (PayType)
            {
                case PayTypeEnum.InternalTransfer:
                    {
                        return new ResultModel() { IsSuccess = false, ErrorCode = "710", Msg = "暂不支持内部转存自助缴费" };
                    }
                case PayTypeEnum.Cash:
                    {
                        return new ResultModel() { IsSuccess = false, ErrorCode = "720", Msg = "暂不支持现金自助缴费" };
                    }
                case PayTypeEnum.BankCard:
                    {
                        return new ResultModel() { IsSuccess = false, ErrorCode = "730", Msg = "暂不支持银行卡自助缴费" };
                    }
            }
            //添加一卡通缴费
            if (PayType != PayTypeEnum.WeChat && PayType != PayTypeEnum.Alipay && PayType != PayTypeEnum.Wallet && PayType != PayTypeEnum.OneNetcom)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "740", Msg = "支付类型不存在" };
            }

            if (Amount == -1)
            {
                Amount = PaymentService.Instance.CalculationBillsAmount(BillIDs);
            }

            PaymentModel model = new PaymentModel();
            model.BillIDs = BillIDs;
            model.Amount = Amount;
            model.PayType = PayType;
            model.ChargeType = ChargeTypeEnum.DailyCharge;
            model.Operator = Operator;
            model.OperatorName = OperatorName;
            model.CustomerName = CustomerName;
            model.IsCheckReceiptNum = false;
            model.IsChangeStore = false;
            model.Remark = "线上支付";
            model.IsOnline = true;
            model.IsPrintReceipt = false;
            if (DiscountInfo != null)
            {
                model.DiscountInfo = PaymentDiscountInfoMappers.ChangeDTOToPaymentDiscountInfoNew(DiscountInfo);
                //model.DiscountInfo.CustomerName = CustomerName;
                //model.Operator = Operator;
                //model.OperatorName = OperatorName;
            }

            var result = PaymentService.Instance.BillsPayment(model);
            if (result.IsSuccess)
            {
                Task.Run(() =>
                {
                    PaymentService.Instance.SettleAccountingPay(result.Data.ToString(), (int)SettleAccountWayEnum.APPWAY);
                });
            }
            return result;
        }


        #endregion

        #region 生成预结算
        /// <summary>
        /// 费用结算
        /// </summary>
        /// <param name="chargeRecordId">记录ID</param>
        public static ResultModel SettleAccount(string chargeRecordId)
        {
            return PaymentService.Instance.SettleAccountingPay(chargeRecordId, (int)SettleAccountWayEnum.BACKSTAGE);
        }
        #endregion

        #region APP缴费验证

        public static ResultModel AppBillsPaymentCheck(string[] BillIDs, decimal Amount, PayTypeEnum PayType, PaymentDiscountInfoDTO DiscountInfo)
        {
            switch (PayType)
            {
                case PayTypeEnum.InternalTransfer:
                    {
                        return new ResultModel() { IsSuccess = false, ErrorCode = "710", Msg = "暂不支持内部转存自助缴费" };
                    }
                case PayTypeEnum.Cash:
                    {
                        return new ResultModel() { IsSuccess = false, ErrorCode = "720", Msg = "暂不支持现金自助缴费" };
                    }
                case PayTypeEnum.BankCard:
                    {
                        return new ResultModel() { IsSuccess = false, ErrorCode = "730", Msg = "暂不支持银行卡自助缴费" };
                    }
            }
            //添加一卡通缴费
            if (PayType != PayTypeEnum.WeChat && PayType != PayTypeEnum.Alipay && PayType != PayTypeEnum.Wallet && PayType != PayTypeEnum.OneNetcom)
            {
                return new ResultModel() { IsSuccess = false, ErrorCode = "740", Msg = "支付类型不存在" };
            }

            if (Amount == -1)
            {
                Amount = PaymentService.Instance.CalculationBillsAmount(BillIDs);
            }

            PaymentModel model = new PaymentModel();
            model.BillIDs = BillIDs;
            model.Amount = Amount;
            model.PayType = PayType;
            model.ChargeType = ChargeTypeEnum.DailyCharge;
            model.IsCheckReceiptNum = false;
            model.IsChangeStore = false;
            model.IsOnline = true;
            if (DiscountInfo != null)
            {
                model.DiscountInfo = PaymentDiscountInfoMappers.ChangeDTOToPaymentDiscountInfoNew(DiscountInfo);
            }

            return PaymentService.Instance.AppBillsPaymentCheck(model);
        }

        #endregion

        #region APP历史缴费

        /// <summary>
        /// 费用详情
        /// </summary>
        /// <param name="chargeRecordId"></param>
        /// <returns></returns>
        public static AppChargeRecordDetail GetChargeRecordDetail(string chargeRecordId)
        {
            return PaymentService.Instance.GetChargeRecordDetail(chargeRecordId);
        }

        public static List<AppChargeRecord> GetChargeHistoryRecordList(int? houseDeptId, int pageIndex, int pageSize)
        {
            return PaymentService.Instance.GetChargeHistoryRecordList(houseDeptId, pageIndex, pageSize);
        }

        #endregion

        #endregion

        #region 缴费终端

        public static QRCode GetPayQRCodeUrl(ClientPayOrder payOrder)
        {
            return HttpClientService.GetPayQRCodeUrl(payOrder);
        }

        public static APIResultDTO CheckClientPaymentPost(ClientPayOrder payOrder)
        {

            APIResultDTO dto = new APIResultDTO();
            if (payOrder.PayType == 1)//账单
            {
                ResultModel result = AppBillsPaymentCheck(payOrder.Ids, -1, PayTypeEnum.Alipay, null);
                dto.Code = result.IsSuccess ? 0 : int.Parse(result.ErrorCode);
                dto.Message = result.Msg;
                return dto;
            }
            else//预存费
            {
                AppSubjectPreCost parameter = new AppSubjectPreCost();
                parameter.HouseDeptId = payOrder.HouseDeptId.Value;
                //手动输入 金额 系统不能计算的每月计费
                parameter.ManualSubjectPreCostList = payOrder.Subjects.Where(s => s.MonthAmount == 0).Select(s => new ManualSubjectPreCost()
                {
                    SubjectId = s.SubjectId,
                    PreCost = s.PreAmount
                }).ToList();
                parameter.PaymentAmount = payOrder.PayAmount;
                //系统生成 金额 系统能计算的每月计费
                parameter.SubjectIds = payOrder.Subjects.Where(s => s.MonthAmount != 0).Select(s => (int)s.SubjectId).ToArray();
                parameter.Month = payOrder.Month;
                var rlt = SubjectPreCostCheck(parameter);
                dto.Code = rlt.IsSuccess ? 0 : int.Parse(rlt.ErrorCode);
                dto.Message = rlt.Msg;
            }
            return dto;
        }

        public static string GetNumericalState(string numericalNumber)
        {
            //return HttpClientService.GetNumericalState(numericalNumber);
            return PaymentService.Instance.GetClientPayOrderState(numericalNumber).ToString();
        }

        public static APIResultDTO ClientPayCallBack(int? CallType, string NumericalNumber, int? state, int? PayType)
        {
            LogProperty.WriteLoginToFile(string.Format("终端支付回调 NumericalNumber：{0} CallType：{1} state：{2} PayType：{3}", NumericalNumber, CallType, state, PayType), "ClientPayCallBack", FileLogType.Info);
            try
            {
                var paylog = PaymentService.Instance.GetClientPayLogByNumericalNumber(NumericalNumber);
                if (paylog == null)
                {
                    return new APIResultDTO() { Code = 701, Message = "支付订单不存在" };
                }

                //忽略同步
                if (CallType == 3)
                {
                    return new APIResultDTO() { Code = 700, Message = "同步不处理" };
                }

                //异常处理
                if (CallType == 3)
                {
                    PaymentService.Instance.CallBackUpdateClientPaymentLog(NumericalNumber, "", -1, null, "支付异常");
                    return new APIResultDTO() { Code = 0, Message = "异常处理成功" };
                }

                if (PayType != 0 && PayType != 2)
                {
                    PaymentService.Instance.CallBackUpdateClientPaymentLog(NumericalNumber, "", 7, PayType, "支付类型不支持");
                    return new APIResultDTO() { Code = 702, Message = "支付类型不支持" };
                }

                //0 是待支付 6 是用户离开页面
                if (paylog.PayState != 0 && paylog.PayState != 6)
                {
                    PaymentService.Instance.CallBackUpdateClientPaymentLog(NumericalNumber, "", 7, PayType, "该订单不能重复支付");
                    return new APIResultDTO() { Code = 703, Message = "该订单不能重复支付" };
                }

                var ptype = (PayType == 0 ? PayTypeEnum.Alipay : PayTypeEnum.WeChat);
                var payOrder = JsonHelper.JsonDeserializeByNewtonsoft<ClientPayOrder>(paylog.OrderData);

                //支付成功
                if (state == 2)
                {
                    //账单缴费
                    if (payOrder.PayType == 1)
                    {
                        //调用账单缴费接口
                        ResultModel result = BillsAppPayment(payOrder.Ids, -1, ptype, -1, "物业终端", null, null);
                        if (result.IsSuccess)
                        {
                            PaymentService.Instance.CallBackUpdateClientPaymentLog(NumericalNumber, result.Data.ToString(), state, PayType, "支付成功");
                        }
                        else
                        {
                            //业务处理支付失败
                            PaymentService.Instance.CallBackUpdateClientPaymentLog(NumericalNumber, "", 7, PayType, result.Msg);
                        }
                        APIResultDTO dto = new APIResultDTO();
                        dto.Code = result.IsSuccess ? 0 : int.Parse(result.ErrorCode);
                        dto.Message = dto.Code == 0 ? "缴费成功" : result.Msg;
                        LogProperty.WriteLoginToFile(string.Format("终端支付回调结果 APIResultDTO：{0}", JsonHelper.JsonSerializerByNewtonsoft(dto)), "ClientPayCallBack", FileLogType.Info);
                        return dto;
                    }
                    //预存费
                    else
                    {
                        AppSubjectPreCost parameter = new AppSubjectPreCost();
                        parameter.HouseDeptId = payOrder.HouseDeptId.Value;
                        //手动输入 金额 系统不能计算的每月计费
                        parameter.ManualSubjectPreCostList = payOrder.Subjects.Where(s => s.MonthAmount == 0).Select(s => new ManualSubjectPreCost()
                        {
                            SubjectId = s.SubjectId,
                            PreCost = s.PreAmount
                        }).ToList();
                        parameter.PaymentAmount = payOrder.PayAmount;
                        //系统生成 金额 系统能计算的每月计费
                        parameter.SubjectIds = payOrder.Subjects.Where(s => s.MonthAmount != 0).Select(s => (int)s.SubjectId).ToArray();
                        parameter.Month = payOrder.Month;
                        //支付方式 
                        parameter.PayType = ptype;
                        //调用预存费接口
                        var rlt = SaveSubjectPreCostPost(parameter, "物业终端");
                        if (rlt.IsSuccess)
                        {
                            PaymentService.Instance.CallBackUpdateClientPaymentLog(NumericalNumber, rlt.Data.ToString(), state, PayType, "支付成功");
                        }
                        else
                        {
                            //业务处理支付失败
                            PaymentService.Instance.CallBackUpdateClientPaymentLog(NumericalNumber, "", 7, PayType, rlt.Msg);
                        }
                        APIResultDTO result = new APIResultDTO();
                        result.Code = rlt.IsSuccess ? 0 : int.Parse(rlt.ErrorCode);
                        result.Message = result.Code == 0 ? "预存费成功" : rlt.Msg;
                        LogProperty.WriteLoginToFile(string.Format("终端支付回调结果 APIResultDTO：{0}", JsonHelper.JsonSerializerByNewtonsoft(result)), "ClientPayCallBack", FileLogType.Info);
                        return result;
                    }
                }
                else
                {
                    PaymentService.Instance.CallBackUpdateClientPaymentLog(NumericalNumber, "", state, PayType, "支付失败");
                    return new APIResultDTO() { Code = 0, Message = "支付失败处理" };
                }
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("终端支付回调异常 NumericalNumber：{0} Exception Msg：{1} Exception：{2}", NumericalNumber, ex.Message, ex), "ClientPayCallBack", FileLogType.Exception);
                return new APIResultDTO() { Code = 901, Message = "支付回调异常" };
            }
        }

        public static APIResultDTO ClientLeavePay(string NumericalNumber)
        {
            try
            {
                PaymentService.Instance.ClientLeavePay(NumericalNumber);
                return new APIResultDTO() { Code = 0, Message = "离开页面更新成功" };
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("离开页面异常 NumericalNumber：{0} Exception Msg：{1} Exception：{2}", NumericalNumber, ex.Message, ex), "ClientCancelPay", FileLogType.Exception);
                return new APIResultDTO() { Code = 901, Message = "取消支付失败" };
            }
        }

        #endregion

        #region 综合报表（计算应收值）
        public static List<ReportTableDTO> CalculationAllPrepaymentBySubjectId(int? ComDeptId, DateTime? BeginDate, DateTime? EndDate, DateTime? PayDate)
        {

            if (ComDeptId == null || BeginDate == null || EndDate == null || PayDate == null)
            {
                return new List<ReportTableDTO>();
            }

            if (ComDeptId <= 0 || BeginDate == DateTime.MinValue || EndDate == DateTime.MinValue || PayDate == DateTime.MinValue)
            {
                return new List<ReportTableDTO>();
            }

            return PaymentService.Instance.CalculationAllPrepaymentByComDeptId(ComDeptId.Value, BeginDate.Value, EndDate.Value, PayDate.Value);
        }

        public static List<ReportTableDTO> CalculationAllPrepaymentByHouseId(int? ComDeptId, DateTime? BeginDate, DateTime? EndDate, DateTime? PayDate, string ResouceNo, int TuitionStatus, int PageIndex, int PageSize, out int totalCount, bool isHouse)
        {

            if (ComDeptId == null || BeginDate == null || EndDate == null || PayDate == null)
            {
                totalCount = 0;
                return new List<ReportTableDTO>();
            }

            if (ComDeptId <= 0 || BeginDate == DateTime.MinValue || EndDate == DateTime.MinValue || PayDate == DateTime.MinValue)
            {
                totalCount = 0;
                return new List<ReportTableDTO>();
            }

            return PaymentService.Instance.CalculationAllPrepaymenHousetByComDeptId(ComDeptId.Value, BeginDate.Value, EndDate.Value, PayDate.Value, ResouceNo, TuitionStatus, PageIndex, PageSize, out totalCount, isHouse);
        }

        public static List<ReportTableDTO> CalculationAllPrepaymentByHouseIdTotal(int? ComDeptId, DateTime? BeginDate, DateTime? EndDate, DateTime? PayDate, string ResouceNo, int TuitionStatus, bool isHouse)
        {

            if (ComDeptId == null || BeginDate == null || EndDate == null || PayDate == null)
            {

                return new List<ReportTableDTO>();
            }

            if (ComDeptId <= 0 || BeginDate == DateTime.MinValue || EndDate == DateTime.MinValue || PayDate == DateTime.MinValue)
            {

                return new List<ReportTableDTO>();
            }

            return PaymentService.Instance.CalculationAllPrepaymenHousetByComDeptIdTotal(ComDeptId.Value, BeginDate.Value, EndDate.Value, PayDate.Value, ResouceNo, TuitionStatus, isHouse);
        }




        #endregion

        #region 综合报表 （收费项目 20161130）

        public static IList<ReportTableDTO> GetChargeSubjectIntegratedReportData(ReportSearchDTO search)
        {
            IList<ReportTableDTO> dataList = PaymentService.Instance
                .CalculationAllPrepaymentByComDeptId_New(search.ComDeptId.Value
                , search.BeginDate, search.EndDate, search.Paydate, "", search.IsHouse);
            var tableData = (from d in dataList
                             group new { d.ChargeSubjectId, d.ChargeSubjectName, d.TotalRecAmount, d.RececiveTotal, d.ReliefAmountTotal, d.UnPaidAmountTotal }
                             by new { d.ChargeSubjectId, d.ChargeSubjectName } into gg
                             select new ReportTableDTO()
                             {
                                 ChargeSubjectName = gg.Key.ChargeSubjectName,
                                 RececiveTotal = gg.Sum(c => c.RececiveTotal),
                                 TotalRecAmount = gg.Sum(c => c.TotalRecAmount),
                                 ReliefAmountTotal = gg.Sum(c => c.ReliefAmountTotal)
                             }).ToList();
            foreach (var count in tableData)
            {
                count.UnPaidAmountTotal = count.TotalRecAmount.Value - count.RececiveTotal.Value - count.ReliefAmountTotal;
                if (count.TotalRecAmount.Value - count.ReliefAmountTotal > 0)
                    count.PayRate = Math.Round(((count.RececiveTotal.Value / count.TotalRecAmount.Value - count.ReliefAmountTotal) * 100), 2).ToString() + "%";
                else
                    count.PayRate = "0%";
            }

            return tableData;
        }

        #endregion

    }

}
