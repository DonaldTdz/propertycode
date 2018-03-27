using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.DomainEntity
{
    public class Sms_IdentifyingCode : IAggregateRoot
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string ValidationCode { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 验证过期时间
        /// </summary>
        public DateTime ValidTime { get; set; }
    }

    public partial class Sms_IdentifyingCodeMapper : EntityMapper<Sms_IdentifyingCode>
    {
        public Sms_IdentifyingCodeMapper()
        {
            HasKey(s => s.Id);
            Property(s => s.Telephone).HasMaxLength(20).IsOptional();
            Property(s => s.ValidationCode).HasMaxLength(10).IsOptional();
        }
    }
}
