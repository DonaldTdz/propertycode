using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class ReportDaySearchDTO : BaseSearchDTO
    {
        public DateTime? ChargeDate { get; set; }

        public string ComDeptIdStr { get; set; }

        public int? DefaultComDeptId { get; set; }

        public string LouyuIdStr { get; set; }

        public int? ComDeptId
        {
            get
            {
                if (!string.IsNullOrEmpty(ComDeptIdStr))
                {
                    return int.Parse(ComDeptIdStr.Replace("number:", ""));
                }
                return DefaultComDeptId;
            }
        }
    }
}
