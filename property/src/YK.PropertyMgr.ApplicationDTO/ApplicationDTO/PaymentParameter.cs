using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    [Serializable]
    public class PaymentParameter
    {
        /// <summary>
        /// 支付账单
        /// </summary>
        public string[] BillIds { get; set; }
        /// <summary>
        /// 支付金额
        /// </summary>
        //public decimal Amount { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public PayTypeEnum PayType { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 优惠信息
        /// </summary>
        public PaymentDiscountInfoDTO DiscountInfo { get; set; }
    }

}
