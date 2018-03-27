using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    /// <summary>
    /// 单据状态
    /// </summary>
    public enum BillStatusEnum
    {
        /// <summary>
        /// 未付款 或 部分付款
        /// </summary>
        NoPayment = 1,
        /// <summary>
        /// 已付款 全部付款
        /// </summary>
        Paid = 2,
        /// <summary>
        /// 已退款(只适用于临时收费和对外收费)
        /// </summary>
        Refunded = 3
        ///// <summary>
        ///// 已扎帐（收费）
        ///// </summary>
        //TieOffCharge = 4,
        ///// <summary>
        ///// 已扎帐（退费）
        ///// </summary>
        //TieOffRefund = 5
    }
}
