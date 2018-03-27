using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.Enums
{
    public enum DiscountStatusEnum
    {
        /// <summary>
        /// 已使用
        /// </summary>
        Used = 1,
        /// <summary>
        /// 已退回
        /// </summary>
        Returned = 2,
        /// <summary>
        /// 退回异常
        /// </summary>
        ReturnException = 3
    }
}
