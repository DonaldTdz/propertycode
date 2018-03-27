using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class ChargeRecordSearchDTO : BaseSearchDTO
    {
        /// <summary>
        /// 票据号
        /// </summary>
        public string ReceiptNum { get; set; }
        /// <summary>
        /// 收费开始日期
        /// </summary>
        public DateTime? ChargeStartDate { get; set; }
        /// <summary>
        /// 收费结束日期
        /// </summary>
        public DateTime? ChargeEndDate { get; set; }
        /// <summary>
        /// 收费类型
        /// </summary>
        public ChargeTypeEnum? ChargeType { get; set; }

        public bool IsDeveloper { get; set; }

        public string SerialNumber { get; set; }

        public int? AccountingStatus { get; set; }

    }
}
