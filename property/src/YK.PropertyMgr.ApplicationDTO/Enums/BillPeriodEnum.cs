using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public enum BillPeriodEnum
    {
        /// <summary>
        /// 每日计费按月收取
        /// </summary>
        DailyCharge = 1,
        /// <summary>
        /// 每月计费按月收取
        /// </summary>
        MonthlyCharge = 2,
        /// <summary>
        /// 一次性收费
        /// </summary>
        Once = 3,
        /// <summary>
        /// 按读数收费 对于三表 新增：2017-01-11
        /// </summary>
        MeterCharge = 4
    }
}
