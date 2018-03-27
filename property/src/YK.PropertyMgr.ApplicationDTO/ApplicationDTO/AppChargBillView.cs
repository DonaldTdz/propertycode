using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class AppChargBillView
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 账单开始日期
        /// </summary>
        public string BeginDate { get; set; }

        /// <summary>
        /// 账单结束日期
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal Amount { get; set; }

    }
}
