using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    /// <summary>
    /// 短信发送实体
    /// </summary>
    public class SmsEntityModel
    {
        /// <summary>
        /// 接收短信电话集合 分隔符','
        /// </summary>
        public string Phones { get; set; }

        /// <summary>
        /// 短信内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 抬头
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 短信账户标识
        /// </summary>
        public string SmsAccountId { get; set; }

        /// <summary>
        /// 请求来源
        /// </summary>
        public ERequestFrom RequestFrom { get; set; }

        /// <summary>
        /// 请求作用
        /// </summary>
        public string RequestScope { get; set; }

        /// <summary>
        /// 是否收费
        /// </summary>
        public bool IsPay { get; set; }

        /// <summary>
        /// 签名
        /// </summary>
        public string Sign { get; set; }
    }

    /// <summary>
    /// 短信请求来源
    /// </summary>
    public enum ERequestFrom
    {
        基础后台 = 1,
        物业收费 = 2,
        社区服务 = 3,
        易购商圈 = 4,
    }
}
