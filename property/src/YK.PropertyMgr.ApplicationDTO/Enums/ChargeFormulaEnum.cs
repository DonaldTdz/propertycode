using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public enum ChargeFormulaEnum
    {
        /// <summary>
        /// 单价
        /// </summary>
        Price = 1,
        /// <summary>
        /// 房屋套内面积
        /// </summary>
        HouseInArea = 2,
        /// <summary>
        /// 房屋建筑面积
        /// </summary>
        BuildArea = 3,
        /// <summary>
        /// 水表单位 吨
        /// </summary>
        WaterUnit = 4,
        /// <summary>
        /// 气表单位 方
        /// </summary>
        GasUnit = 5,
        /// <summary>
        /// 电表单位 度
        /// </summary>
        ElectricUnit = 6,
        /// <summary>
        /// 车位面积
        /// </summary>
        ParkingSpaceArea = 7
    }
}
