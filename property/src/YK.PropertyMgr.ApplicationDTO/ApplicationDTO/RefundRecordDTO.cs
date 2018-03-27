using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public partial class RefundRecordDTO
    {
        public string HouseDoorNo { get; set; }

        public string CustomerName { get; set; }

        public decimal Amount { get; set; }

        public string ReceiptNum { get; set; }
    }
}
