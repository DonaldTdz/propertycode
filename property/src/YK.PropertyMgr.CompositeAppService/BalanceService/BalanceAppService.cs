using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.CompositeDomainService;
using YK.PropertyMgr.DomainEntity;

namespace YK.PropertyMgr.CompositeAppService
{
    public class BalanceAppService
    {
        #region 充值

         /// <summary>
        /// 余额充值
        /// 指用户自助充值 和 后台充值（APP 或 自助缴费 非小区手动交预缴费）
        /// </summary>
        /// <param name="BalanceInfo">充值信息</param>
        /// <param name="PayType">支付方式</param>
        /// <param name="Remark">备注</param>
        /// <returns>处理结果</returns>
        public static ResultModel BalanceRecharge(BalanceInfo BalanceInfo, PayTypeEnum PayType, string Remark, int Operator, string OperatorName) 
        {
            if (Operator == 0 && string.IsNullOrEmpty(OperatorName))
            {
                OperatorName = BillCommonService.SystemOperatorName;
            }
            return BalanceService.Instance.BalanceRecharge(BalanceInfo, PayType, Remark, Operator, OperatorName);
        }

        #endregion

        #region 余额导入

        /// <summary>
        /// 余额初始化导入
        /// 如果存在 跳过并返回
        /// </summary>
        /// <param name="BalanceInfoList">导入信息列表</param>
        /// <returns>处理结果 和 未初始化的导入信息</returns>
        public static ResultModel BalanceInitialization(IList<BalanceInfo> BalanceInfoList,int Operator,string OperatorName) 
        {
            if (Operator == 0 && string.IsNullOrEmpty(OperatorName))
            {
                OperatorName = BillCommonService.SystemOperatorName;
            }
            return BalanceService.Instance.BalanceInitialization(BalanceInfoList, Operator, OperatorName);
        }

        #endregion

        #region 余额转移

        public static ResultModel BalanceTransfer(PreAccountCostTransferDTO transfer)
        {
            return BalanceService.Instance.BalanceTransfer(transfer);
        }

        #endregion
    }
}
