using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    [Serializable]
    public class AppOldBillView
    {
        public string ChargePlanId { get; set; }

        public decimal? Money { get; set; }

        public string SubjectName { get; set; }

        public int Subject_Id { get; set; }

        public string Remark { get; set; }

        public string BeginTime { get; set; }

        public string EndTime { get; set; }

        public string ReceivableDate { get; set; }
    }
}
