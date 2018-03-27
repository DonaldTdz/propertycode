using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{

    public class DeveloperSetTimeListDTO
    {
        public int ResId { get; set; }
        public int HouseDeptId { get; set; }
        public int SubjectType { get; set; }
        public List<DeveloperSetTime> DeveloperSetTimelist { get; set; }
    }
    public class DeveloperSetTime
    {
        public int SubjectId { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? BeginDateBill { get; set; }
        public bool BindSubject { get; set; }
    }
}
