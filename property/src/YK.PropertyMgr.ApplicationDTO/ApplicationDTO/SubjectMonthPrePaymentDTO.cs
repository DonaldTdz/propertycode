using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    [Serializable]
    public class SubjectMonthPrePaymentDTO
    {
        /// <summary>
        /// 收费项目Id
        /// </summary>
        public int? SubjectId { get; set; }

        /// <summary>
        /// 收费项目名称
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 收费周期
        /// </summary>
        public int? BillPeriod { get; set; }

        /// <summary>
        /// 每月费用
        /// </summary>
        public decimal? PreAmount { get; set; }
    }
}
