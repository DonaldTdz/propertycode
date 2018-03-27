using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.CompositeDomainService.GenerateBillService
{
    /// <summary>
    /// 账单日期范围
    /// </summary>
    public class BillDateRange
    {
        /// <summary>
        /// 账单开始日
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 账单结束日
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 是否开发商代缴
        /// </summary>
        public bool IsDevPay { get; set; }

        public BillDateRange()
        {
            IsDevPay = false;
        }

    }
}
