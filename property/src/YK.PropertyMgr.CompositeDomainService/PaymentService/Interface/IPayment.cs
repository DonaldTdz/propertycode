using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.CompositeDomainService.Model;
using YK.PropertyMgr.DomainEntity;

namespace YK.PropertyMgr.CompositeDomainService
{
    public interface IPayment
    {
        /// <summary>
        /// 多账单缴费
        /// </summary>
        /// <param name="BillIDs">账单IDs</param>
        /// <param name="Amount">缴费金额</param>
        /// <param name="IsReceiptNum">票据号</param>
        /// <param name="PayType">付款方式</param>
        /// <param name="ChargeType">收费类型</param>
        /// <param name="IsChangeStore">是否找零预存</param>
        /// <param name="CustomerName">客户姓名</param>
        /// <returns>处理结果</returns>
        ResultModel BillsPayment(PaymentModel model);

        /// <summary>
        /// 生成账单交款记录
        /// </summary>
        /// <param name="ChargeRecordIds">费用记录Ids</param>
        /// <param name="remark">备注</param>
        /// <returns>处理结果</returns>
        ResultModel GenerateBillPaymentTask(string[] ChargeRecordIds, string Remark, int Operator, string OperatorName, DateTime PaymentDate);

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="RefundRecord">退款记录</param>
        /// <param name="Operator">操作人</param>
        /// <returns>处理结果</returns>
        ResultModel Refund(RefundRecord RefundRecord, int Operator, string OperatorName);

        /// <summary>
        /// 撤销收费
        /// </summary>
        /// <param name="ChargeRecord">费用记录</param>
        /// <param name="Operator">操作人</param>
        /// <returns>处理结果</returns>
        ResultModel RevokeCharge(string[] ChargeRecordIds, int Operator, string OperatorName);
        /// <summary>
        /// 是否已交款
        /// </summary>
        /// <param name="ChargeRecordId">费用记录ID</param>
        /// <returns>bool</returns>
        bool IsSubmitted(string ChargeRecordId);

        /// <summary>
        /// 验证票据号是否重复
        /// </summary>
        bool CheckReceiptNumRepeat(string ReceiptNum, int ComDeptId, string ReceiptId);

        /// <summary>
        /// 计算账户月预存费
        /// </summary>
        decimal? CalculationMonthPrePayment(int HouseDeptId, out bool IsDevPay);

        /// <summary>
        /// 计算账单金额
        /// </summary>
        decimal CalculationBillsAmount(string[] BillIds);

        /// <summary>
        /// 综合报表预算小区费用
        /// </summary>
        /// <param name="ComDeptId"></param>
        /// <param name="BeginDate"></param>
        /// <param name="EndDate"></param>
        List<ReportTableDTO> CalculationAllPrepaymentByComDeptId(int ComDeptId, DateTime BeginDate, DateTime EndDate, DateTime PayDate);

        /// <summary>
        /// 综合报表预算小区房间费用
        /// </summary>
        /// <param name="ComDeptId">小区ID</param>
        /// <param name="BeginDate">开始时间</param>
        /// <param name="EndDate">结束时间</param>
        /// <param name="PayDate">付款率时间</param>
        /// <param name="ResouceNo">资源编号</param>
        /// <param name="IsTuition">是否欠费</param>
        /// <param name="PageIndex">分页第几页</param>
        /// <param name="PageSize">每页多少条</param>
        /// <param name="totalCount">数据总数</param>
        /// <returns></returns>
        List<ReportTableDTO> CalculationAllPrepaymenHousetByComDeptId(int ComDeptId, DateTime BeginDate, DateTime EndDate, DateTime PayDate, string ResouceNo, int TuitionStatus, int PageIndex, int PageSize, out int totalCount, bool isHouse);


        List<ReportTableDTO> CalculationAllPrepaymenHousetByComDeptIdTotal(int ComDeptId, DateTime BeginDate, DateTime EndDate, DateTime PayDate, string ResouceNo, int TuitionStatus, bool isHouse);


        List<ReportTableDTO> CalculationAllPrepaymentByComDeptId_New(int ComDeptId, DateTime BeginDate, DateTime EndDate, DateTime PayDate, string DoorNo = "", bool isHouse = false);

        ResultModel AppBillsPaymentCheck(PaymentModel model);

        /// <summary>
        /// 获取缴费明细
        /// </summary>
        /// <param name="chargeRecord"></param>
        /// <returns></returns>
        AppChargeRecordDetail GetChargeRecordDetail(string chargeRecord);
    }
}
