using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class AlipayInformDTO
    {
        /// <summary>
        /// 通知首次发送时间。格式为yyyy-MM-dd HH:mm:ss。
        /// </summary>
        public DateTime notify_time { get; set; }
        /// <summary>
        /// 签名类型，由开发者在应用网关->加签方式中指定
        /// </summary>
        public string sign_type { get; set; }
        /// <summary>
        /// 支付宝生成的签名字符串
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 支付宝交易号
        /// </summary>
        public string trade_no { get; set; }
        /// <summary>
        /// 买家支付宝账号对应的支付宝唯一用户号。以2088开头的纯16位数字
        /// </summary>
        public string buyer_user_id { get; set; }
        /// <summary>
        /// 脱敏后的买家支付宝账号。
        /// </summary>
        public string buyer_logon_id { get; set; }
        /// <summary>
        /// 卖家支付宝用户号
        /// </summary>
        public string seller_id { get; set; }
        /// <summary>
        /// 交易目前所处的状态，见交易状态说明
        /// </summary>
        public string trade_status { get; set; }
        /// <summary>
        /// 本次交易支付的订单金额，单位为人民币（元）
        /// </summary>
        public decimal total_amount { get; set; }
        /// <summary>
        /// 	商家在交易中实际收到的款项，单位为元
        /// </summary>
        public decimal receipt_amount { get; set; }
        /// <summary>
        /// 退款通知中，返回总退款金额，单位为元，支持两位小数
        /// </summary>
        public decimal refund_fee { get; set; }
        /// <summary>
        /// 交易订单描述，在本产品中用于表示该笔缴费归属的小区编号和户号
        /// </summary>
        public string body { get; set; }
        /// <summary>
        /// 用于标识该笔缴费对应的缴费明细项外部编号，格式：:明细Id1|明细Id2|明细Id3
        /// </summary>
        public string det_list { get; set; }
        /// <summary>
        /// 交易创建时间
        /// </summary>
        public DateTime gmt_create { get; set; }
        /// <summary>
        /// 交易付款时间
        /// </summary>
        public DateTime gmt_payment { get; set; }
        /// <summary>
        /// 交易退款时间
        /// </summary>
        public DateTime gmt_refund { get; set; }
        /// <summary>
        /// 交易结束时间
        /// </summary>
        public DateTime Dategmt_close { get; set; }
        /// <summary>
        /// 支付成功的各个渠道金额信息，详见资金明细信息说明
        /// </summary>
        public AlipayFindBill[] fund_bill_list { get; set; }


    }
}
