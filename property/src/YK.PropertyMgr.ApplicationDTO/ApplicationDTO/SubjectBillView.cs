using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class SubjectBillView
    {
        public int SubjectId { get; set; }
        public string SubjctName { get; set; }
        public DateTime? BillEndTime { get; set; }
        public string ResourceName { get; set; }
        public string ResourceId { get; set; }
        public string HouseDeptId { get; set; }
        public int ResType { get; set; }
        public bool IsChecked { get; set; }

        public string BillEndTimeFormat
        {
            get
            {
                return BillEndTime.HasValue ? BillEndTime.Value.ToString("yyyy-MM-dd") : "";
            }
        }
    }
}
