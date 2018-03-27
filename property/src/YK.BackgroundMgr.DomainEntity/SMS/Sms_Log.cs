using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.DomainEntity
{
    public class Sms_Log : IAggregateRoot
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 发送消息日志
        /// </summary>
        public int SmsType { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 发送消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 短信服务器返回的Code
        /// </summary>
        public int ReceiptCode { get; set; }

        /// <summary>
        /// 短信服务器返回的消息
        /// </summary>
        public string ReceiptMsg { get; set; }

        /// <summary>
        /// 短信服务器返回的消息Id
        /// </summary>
        public string ReceiptSmsID { get; set; }
    }

    public partial class Sms_LogMapper : EntityMapper<Sms_Log>
    {
        public Sms_LogMapper()
        {
            HasKey(s => s.Id);
            Property(s => s.Phone).HasMaxLength(20).IsOptional();
            Property(s => s.Content).HasMaxLength(1024).IsOptional();
            Property(s => s.ReceiptMsg).HasMaxLength(1024).IsOptional();
            Property(s => s.ReceiptSmsID).IsOptional();
        }
    }
}
