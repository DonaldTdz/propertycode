using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class PaymentTaskBySubjetDTO
    {
     
        public string Name { get; set; }        /// <summary>
        /// 现金
        /// </summary>
        public decimal Cash { get; set; }
        /// <summary>
        /// 银行卡
        /// </summary>
        public decimal BankCard { get; set; }
        /// <summary>
        /// 线下支付宝
        /// </summary>
        public decimal OffAlipay { get; set; }
        /// <summary>
        /// 线上支付宝
        /// </summary>
        public decimal OnAlipay { get; set; }
        /// <summary>
        /// 线下微信
        /// </summary>
        public decimal OffWeChat { get; set; }
        /// <summary>
        /// 线下微信
        /// </summary>
        public decimal OnWeChat { get; set; }
        /// <summary>
        /// 钱包抵扣
        /// </summary>
        public decimal Wallet { get; set; }
        /// <summary>
        /// 内部划账
        /// </summary>
        public decimal InternalDebit { get; set; }
        /// <summary>
        /// 一网通
        /// </summary>
        public decimal OneNetcom { get; set; }
        /// <summary>
        /// 预存抵扣
        /// </summary>
        public decimal InternalTransfer { get; set; }
        /// <summary>
        /// 优惠券
        /// </summary>
        public decimal Coupon { get; set; }
        /// <summary>
        /// 合计
        /// </summary>
        public decimal Total { get; set; }
        /// <summary>
        /// 金额（含优惠金额）
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 分组Id 3为合计组
        /// </summary>
        public int GroupId { get; set; }

    }
}
