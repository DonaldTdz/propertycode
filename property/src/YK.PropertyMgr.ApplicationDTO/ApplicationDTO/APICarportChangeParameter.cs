using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    [Serializable]
    public class APICarportChangeParameter
    {
        public int? HouseDeptId { get; set; }

        public int? CarportId { get; set; }
        public int? RelieveOperator { get; set; }
        public CarActionStatusEnum ActionStatus { get; set; }
    }

    public enum CarActionStatusEnum
    {
        Update = 1,
        Delete = 2
    }
}
