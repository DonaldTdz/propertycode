using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_WeChatPublicNumber: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// 公众号名称
        /// </summary>
		public string Name { get; set; }
		/// <summary>
        /// AppId
        /// </summary>
		public string AppId { get; set; }
		/// <summary>
        /// AppSecret
        /// </summary>
		public string AppSecret { get; set; }
		/// <summary>
        /// AccessToken
        /// </summary>
		public string AccessToken { get; set; }
		/// <summary>
        /// TokenTime
        /// </summary>
		public string TokenTime { get; set; }
		/// <summary>
        /// 公共号Id
        /// </summary>
		public string PublicNumberId { get; set; }
		/// <summary>
        /// 是否属于多公众号
        /// </summary>
		public int? IsPublic { get; set; }
		/// <summary>
        /// PropertyIds
        /// </summary>
		public string PropertyIds { get; set; }
		/// <summary>
        /// PropertyNames
        /// </summary>
		public string PropertyNames { get; set; }
		/// <summary>
        /// MchId
        /// </summary>
		public string MchId { get; set; }
		/// <summary>
        /// ApiKey
        /// </summary>
		public string ApiKey { get; set; }
	 }
	public partial class SEC_WeChatPublicNumberMapper : EntityMapper<SEC_WeChatPublicNumber>
    {
        public SEC_WeChatPublicNumberMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.Name).HasMaxLength(50).IsRequired();
			Property(s => s.AppId).HasMaxLength(100).IsRequired();
			Property(s => s.AppSecret).HasMaxLength(100).IsRequired();
			Property(s => s.AccessToken).HasMaxLength(200).IsOptional();
			Property(s => s.TokenTime).HasMaxLength(200).IsOptional();
			Property(s => s.PublicNumberId).HasMaxLength(100).IsOptional();
			Property(s => s.IsPublic).IsRequired();
			Property(s => s.PropertyIds).HasMaxLength(500).IsOptional();
			Property(s => s.PropertyNames).HasMaxLength(1000).IsOptional();
			Property(s => s.MchId).HasMaxLength(100).IsOptional();
			Property(s => s.ApiKey).HasMaxLength(100).IsOptional();
        }
    }
}
