using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class EnumHelper
    {
        public static class BillPeriod
        {
            /// <summary>
            /// 每日计费按月收取
            /// </summary>
            public static int DailyCharge = BillPeriodEnum.DailyCharge.GetHashCode();
            /// <summary>
            /// 每月计费按月收取
            /// </summary>
            public static int MonthlyCharge = BillPeriodEnum.MonthlyCharge.GetHashCode();
            /// <summary>
            /// 一次性收费
            /// </summary>
            public static int Once = BillPeriodEnum.Once.GetHashCode();
            /// <summary>
            /// 对数收费
            /// </summary>
            public static int MeterCharge = BillPeriodEnum.MeterCharge.GetHashCode();
        }

        public static class SubjectType
        {
            /// <summary>
            /// 房屋
            /// </summary>
            public static int House = SubjectTypeEnum.House.GetHashCode();
            /// <summary>
            /// 车位
            /// </summary>
            public static int ParkingSpace = SubjectTypeEnum.ParkingSpace.GetHashCode();
            /// <summary>
            /// 三表
            /// </summary>
            public static int Meter = SubjectTypeEnum.Meter.GetHashCode();
            /// <summary>
            /// 系统预置
            /// </summary>
            public static int SystemPreset = SubjectTypeEnum.SystemPreset.GetHashCode();
        }
    }

    public enum PropertyEnumType
    {
        /// <summary>
        /// 收费周期
        /// </summary>
        BillPeriod = 18,
        /// <summary>
        /// 项目类型
        /// </summary>
        SubjectType = 20,
        /// <summary>
        /// 资源类型
        /// </summary>
        ReourceType = 21,
        /// <summary>
        /// 计算类型
        /// </summary>
        ChargeFormula = 22,
        /// <summary>
        /// 支付类型
        /// </summary>
        PayType = 24,
        /// <summary>
        /// 收费类型
        /// </summary>
        ChargeType = 23,
        /// <summary>
        /// 单据状态
        /// </summary>
        ReceiptStatus = 25,
        /// <summary>
        /// 交款状态
        /// </summary>
        PaymentTaskStatus = 26,
        /// <summary>
        /// 预结算状态
        /// </summary>
        AccountingStatus = 39,
        /// <summary>
        /// 物业优惠信息类型
        /// </summary>
        DiscountType = 40,
        /// <summary>
        /// 物业优惠记录状态
        /// </summary>
        DiscountStatus = 41,
        /// <summary>
        /// 票据类型
        /// </summary>
        ReceiptBookType=42,
        /// <summary>
        /// 票据状态
        /// </summary>
        ReceiptBookTypeStatus =43,

        /// <summary>
        /// 票据操作类型
        /// </summary>
        ReceiptBookHistoryType =44,
        /// <summary>
        /// 支付宝账单状态
        /// </summary>
        AlipayChargeBillStatus = 45,

    }
}
