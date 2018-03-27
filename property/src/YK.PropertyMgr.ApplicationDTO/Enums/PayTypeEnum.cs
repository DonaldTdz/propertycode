using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    /// <summary>
    /// 支付方式
    /// </summary>
    public enum PayTypeEnum
    {
        /// <summary>
        /// 预存抵扣
        /// </summary>
        InternalTransfer = 1,
        /// <summary>
        /// 现金
        /// </summary>
        Cash = 2,
        /// <summary>
        /// 银行卡
        /// </summary>
        BankCard = 3,
        /// <summary>
        /// 支付宝
        /// </summary>
        Alipay = 4,
        /// <summary>
        /// 微信
        /// </summary>
        WeChat = 5,
        /// <summary>
        /// 钱包抵扣
        /// </summary>
        Wallet = 6,

        /// <summary>
        /// 内部划账
        /// </summary>
        InternalDebit = 7,

        /// <summary>
        /// 一网通 2017-02-27
        /// </summary>
        OneNetcom = 8

    }
}
