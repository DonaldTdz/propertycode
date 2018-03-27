using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_WeChatOpenId: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// OpenId
        /// </summary>
		public string OpenId { get; set; }
		/// <summary>
        /// WeChatName
        /// </summary>
		public string WeChatName { get; set; }
		/// <summary>
        /// WeChatImg
        /// </summary>
		public string WeChatImg { get; set; }
		/// <summary>
        /// WeChatPublicNumberId
        /// </summary>
		public int? WeChatPublicNumberId { get; set; }
		/// <summary>
        /// 渠道码
        /// </summary>
		public string ChannelCode { get; set; }
		/// <summary>
        /// 关注时间
        /// </summary>
		public DateTime? SubscribeTime { get; set; }
	 }
	public partial class SEC_WeChatOpenIdMapper : EntityMapper<SEC_WeChatOpenId>
    {
        public SEC_WeChatOpenIdMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.OpenId).HasMaxLength(200).IsRequired();
			Property(s => s.WeChatName).HasMaxLength(100).IsRequired();
			Property(s => s.WeChatImg).IsMaxLength().IsOptional();
			Property(s => s.WeChatPublicNumberId).IsRequired();
			Property(s => s.ChannelCode).HasMaxLength(100).IsOptional();
			Property(s => s.SubscribeTime).IsRequired();
        }
    }
}
