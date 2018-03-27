using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
  public  class AlipayFindBill
    {
        /// <summary>
        /// 支付渠道，参见下面的“支付渠道说明”。
        /// </summary>
        public string fund_channel { get; set; }
        /// <summary>
        /// 对应支付渠道支付的金额，单位为元。
        /// </summary>
        public decimal amount { get; set; }
    }
}
