using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;

namespace YK.PropertyMgr.CompositeDomainService
{
    public interface IBalanceService
    {
        /// <summary>
        /// 余额缴费
        /// </summary>
        /// <param name="ChargBill">缴费账单ID集合</param>
        /// <param name="Amount">缴费金额</param>
        /// <param name="PayType">支付方式</param>
        /// <param name="Remark">备注</param>
        /// <returns>处理结果</returns>
        ResultModel BalancePayment(string[] BillIDs, decimal Amount, PayTypeEnum PayType, string Remark, int Operator, string OperatorName);

        /// <summary>
        /// 余额充值
        /// </summary>
        /// <param name="BalanceInfo">充值信息</param>
        /// <param name="PayType">支付方式</param>
        /// <param name="Remark">备注</param>
        /// <param name="IsInitBalance">是否是余额导入初始化</param>
        /// <returns>处理结果</returns>
        ResultModel BalanceRecharge(BalanceInfo BalanceInfo, PayTypeEnum PayType, string Remark,int Operator, string OperatorName, bool IsInitBalance = false);

        /// <summary>
        /// 余额初始化导入
        /// 如果存在 跳过并返回
        /// </summary>
        /// <param name="BalanceInfoList">导入信息列表</param>
        /// <returns>处理结果 和 未初始化的导入信息</returns>
        ResultModel BalanceInitialization(IList<BalanceInfo> BalanceInfoList,int Operator, string OperatorName);

        /// <summary>
        /// 余额转移
        /// </summary>
        /// <param name="transfer"></param>
        /// <returns></returns>
        ResultModel BalanceTransfer(PreAccountCostTransferDTO transfer);
    }
}
