using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class BillChargeRecordSearchDTO : BaseSearchDTO
    {
        /// <summary>
        /// 票据号
        /// </summary>
        public string ReceiptNum { get; set; }
        /// <summary>
        /// 收费开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 收费结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 交易项描述
        /// </summary>
        public string TransactionDesc { get; set; }

        public int? PaymentTaskId { get; set; }

        public DateTime? PaymentDateMax { get; set; }

        public int? HouseDeptId { get; set; }

        public string RecordId { get; set; }
       
        public int? SECRole_AdminId { get; set; }

        /// <summary>
        /// 外部费用
        /// </summary>
        public bool IsForegi { get; set; }

        //public bool IsDeveloper { get; set; }

    }
}
