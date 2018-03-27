using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.ApplicationDTO.ApplicationDTO
{
   public class HouseState
    {
        /// <summary>
        /// 房屋Id
        /// </summary>
        public int? HouseId { get; set; }
        /// <summary>
        /// 房屋装修状态
        /// </summary>
        public int? DecorationState_PM { get; set; }
        /// <summary>
        /// 房屋状态
        /// </summary>
        public int? HouseState_PM { get; set; }
    }
}
