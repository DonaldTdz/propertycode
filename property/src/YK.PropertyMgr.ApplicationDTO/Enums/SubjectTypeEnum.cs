using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public enum SubjectTypeEnum
    {
        /// <summary>
        /// 房屋
        /// </summary>
        House = 1,
        /// <summary>
        /// 车位
        /// </summary>
        ParkingSpace = 2,
        /// <summary>
        /// 三表
        /// </summary>
        Meter = 3,
        /// <summary>
        /// 系统预置
        /// </summary>
        SystemPreset = 4,
        /// <summary>
        /// 其它
        /// </summary>
        Other = 5,
        /// <summary>
        /// 对外收费 小区
        /// </summary>
        Foreig = 6


    }
}
