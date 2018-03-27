using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    /// <summary>
    /// 收费类型
    /// </summary>
    public enum ChargeTypeEnum
    {
        /// <summary>
        /// 日常收费
        /// </summary>
        DailyCharge = 1,
        /// <summary>
        /// 临时收费
        /// </summary>
        TemporaryCharge = 2,
        /// <summary>
        /// 余额初始化
        /// </summary>
        InitBalance = 3,
        /// <summary>
        /// 退款
        /// </summary>
        Refund = 4,
        /// <summary>
        /// 对外收费
        /// </summary>
        ForeignCharge = 5,
        /// <summary>
        /// 预存转移
        /// </summary>
        BalanceTransfer = 6
    }
}
