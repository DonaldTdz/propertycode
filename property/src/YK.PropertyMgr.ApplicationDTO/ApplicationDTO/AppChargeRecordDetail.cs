using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    [Serializable]
    public class AppChargeRecordDetail
    {
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal? PaymentAmount
        {
            get
            {
                //总金额 - 优惠金额
                return Amount - DiscountAmount;
            }
        }

        /// <summary>
        /// 优惠金额
        /// </summary>
        public decimal? DiscountAmount { get; set; }

        /// <summary>
        /// 优惠类型
        /// </summary>
        public string DiscountTypeName { get; set; }

        /// <summary>
        /// 缴费时间
        /// </summary>
        public string PaymentDate { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PaymentTypeName { get; set; }

        /// <summary>
        /// 账单明细列表
        /// </summary>
        public List<AppBillDetail> BillDetailList { get; set; }
    }

    [Serializable]
    public class AppBillDetail
    {
        /// <summary>
        /// 收费项目
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 账单描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 账单金额
        /// </summary>
        public decimal? Amount { get; set; }
    }

    [Serializable]
    public class AppChargeRecord
    {
        /// <summary>
        /// 费用记录 Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 费用记录描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 缴费时间
        /// </summary>
        public string PaymentDate { get; set; }

        /// <summary>
        /// 缴费金额
        /// </summary>
        public decimal? Amount { get; set; }
    }
}
