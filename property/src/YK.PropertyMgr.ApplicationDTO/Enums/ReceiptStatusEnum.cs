using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.Enums
{
    /// <summary>
    /// 票据状态
    /// </summary>
    public enum ReceiptStatusEnum
    {
        /// <summary>
        /// 已付款
        /// </summary>
        Paid = 1,
        /// <summary>
        /// 已退款
        /// </summary>
        Refunded = 2
    }
}
