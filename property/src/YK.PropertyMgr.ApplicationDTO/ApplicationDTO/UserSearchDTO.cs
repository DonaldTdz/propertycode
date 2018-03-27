using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class UserSearchDTO : BaseSearchDTO
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public string DoorNum { get; set; }
        public string Phone { get; set; }
        //public int DeptType { get; set; }
    }
}
