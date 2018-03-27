using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.Enums
{
    /// <summary>
    /// 生成结算状态
    /// </summary>
    public enum AccountingStatusEnum
    {
        /// <summary>
        /// 不适用
        /// </summary>
        NotApplicable = 1,
        /// <summary>
        /// 未生成结算
        /// </summary>
        NoSettlement = 2,
        /// <summary>
        /// 已生成结算
        /// </summary>
        BeenSettled = 3,
    }
}
