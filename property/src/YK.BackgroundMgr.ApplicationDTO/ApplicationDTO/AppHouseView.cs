using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.ApplicationDTO.ApplicationDTO
{
    public class AppHouseView
    {
        /// <summary>
        /// 房屋的DeptID
        /// </summary>
        public int? HouseDeptID { get; set; }
        /// <summary>
        /// 房屋编号
        /// </summary>
        public string HouseDoorNo { get; set; }
    }
}
