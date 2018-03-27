using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class ReceiptBookDetailShowDTO
    {
        public string Number { get; set; }

        public string ReceResourcesNum { get; set; }

        public decimal? Amount { get; set; }

        public decimal? DiscountAmount { get; set; }
        public DateTime? PayDate { get; set; }

        public string PayDateTimeStr { get {
                if (PayDate!=null&&PayDate>DateTime.MinValue)
                {
                    return this.PayDate.Value.ToString("yyyy-MM -dd");
                }
                return "";
            } }

        public string OperatorName { get; set; }

        public int? ChargeType { get; set; }

        public int? PayMthodId { get; set; }
        public string RefundRecordReason { get; set; }
        public string Remark { get;set; }
    }
}
