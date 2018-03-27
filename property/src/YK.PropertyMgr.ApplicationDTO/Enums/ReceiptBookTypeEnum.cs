using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.Enums
{
     
    //票据类型
    public enum ReceiptBookTypeEnum
    {
        /// <summary>
        /// 收费票据
        /// </summary>
        ChargeBill=1
    }

    //票据状态
    public enum ReceiptBookStatusEnum
    {
        /// <summary>
        /// 启用
        /// </summary>
        Enabled=1,
        /// <summary>
        /// 停用
        /// </summary>
        Disabled=-1,
        

    }
}
