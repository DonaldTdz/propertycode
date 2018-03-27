using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.Enums
{
    public enum AllocationTypeEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 按房屋数量均摊
        /// </summary>
        HouseNumber = 1,
        /// <summary>
        /// 按建筑面积均摊
        /// </summary>
        BuiltArea = 2
    }
}
