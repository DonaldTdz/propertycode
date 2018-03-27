using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.Enums
{
   public enum AlipayPropertyEnum
    {
    }
    public enum AlipayCommunityCreateStatusEnum
    {
        /// <summary>
        /// 未接入
        /// </summary>
        Disable=0,
        /// <summary>
        /// 启用
        /// </summary>
        Enable = 1
    }

    public enum AlipayChargeBillStatusEnum
    {
        /// <summary>
        /// 等待支付
        /// </summary>
        WAIT_PAYMENT = 1,
        /// <summary>
        /// 已下单
        /// </summary>
        UNDER_PAYMENT = 2,
        /// <summary>
        /// 完成支付
        /// </summary>
        FINISH_PAYMENT = 3,
        /// <summary>
        /// 已过截止日期
        /// </summary>
        OUT_OF_DATE = 4
    }
}
