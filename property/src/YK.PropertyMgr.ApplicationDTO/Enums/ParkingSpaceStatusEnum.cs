using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.Enums
{
    /// <summary>
    /// 车位状态
    /// </summary>
    public enum ParkingSpaceStatusEnum
    {
        /// <summary>
        /// 购买
        /// </summary>
        Bought = 1,
        /// <summary>
        /// 包租
        /// </summary>
        Rent = 2,
        /// <summary>
        /// 空置
        /// </summary>
        Idle = 3,
        /// <summary>
        /// 废弃
        /// </summary>
        Disuse = 4
    }
}
