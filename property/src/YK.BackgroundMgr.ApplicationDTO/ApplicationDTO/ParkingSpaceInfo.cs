using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.ApplicationDTO.ApplicationDTO
{
    public class ParkingSpaceInfo
    {
        /// <summary>
        /// 车库Id
        /// </summary>
        public string ParkingId { get; set; }
        /// <summary>
        /// 车位Id
        /// </summary>
        public int? ParkingSpaceId { get; set; }
        /// <summary>
        /// 房屋Id
        /// </summary>
        public int? HouseDeptID { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public double? Area { get; set; }
        /// <summary>
        /// 车位编号
        /// </summary>
        public string CarportNum { get; set; }
    }
}
