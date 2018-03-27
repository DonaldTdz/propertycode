using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class SubjectBindLogSearchDTO : BaseSearchDTO
    {
        public string SubjectId { get; set; }
        public string ResourceName { get; set; }
        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }
    }
}
