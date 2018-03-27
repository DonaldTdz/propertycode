using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.MVCWeb
{
    public class VacationInfo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public string Reason { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public int VacationType { get; set; }
        public string Description { get; set; }
        public Guid InstanceId { get; set; }
        public string CurrentActivityNames { get; set; }
        public bool IsCompleted { get; set; }
    }
}
