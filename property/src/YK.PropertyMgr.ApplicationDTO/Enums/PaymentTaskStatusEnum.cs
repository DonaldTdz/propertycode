using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public enum PaymentTaskStatusEnum
    {
        /// <summary>
        /// 已提交
        /// </summary>
        Submitted = 1,
        /// <summary>
        /// 已审核
        /// </summary>
        Audited = 2,
        /// <summary>
        /// 已退回
        /// </summary>
        Back = 3
    }
}
