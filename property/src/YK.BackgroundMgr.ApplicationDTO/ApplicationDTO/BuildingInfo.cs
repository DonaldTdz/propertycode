using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.ApplicationDTO.ApplicationDTO
{
    [Serializable]
   public  class BuildingInfo
    {
        public int? id { get; set; }
        public string Building_code { get; set; }
        public string Building_name { get; set; }
        public int? DeptId { get; set; }


    }
}
