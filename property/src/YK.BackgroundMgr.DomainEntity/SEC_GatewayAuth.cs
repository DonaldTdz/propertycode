using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_GatewayAuth: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// EntityKey
        /// </summary>
		public string EntityKey { get; set; }
		/// <summary>
        /// 用户Token
        /// </summary>
		public string UserToken { get; set; }
		/// <summary>
        /// 网关Id
        /// </summary>
		public string WifiGatewayId { get; set; }
		/// <summary>
        /// 用户电话号码
        /// </summary>
		public string Phone { get; set; }
		/// <summary>
        /// IP地址
        /// </summary>
		public string IP { get; set; }
		/// <summary>
        /// Mac地址
        /// </summary>
		public string Mac { get; set; }
		/// <summary>
        /// 客户端类型
        /// </summary>
		public int? ClientType { get; set; }
		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
        /// 最后一些轮询访问
        /// </summary>
		public DateTime? LastUpdateTime { get; set; }
		/// <summary>
        /// 过期时间
        /// </summary>
		public DateTime? ExpirTime { get; set; }
	 }
	public partial class SEC_GatewayAuthMapper : EntityMapper<SEC_GatewayAuth>
    {
        public SEC_GatewayAuthMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.EntityKey).HasMaxLength(100).IsOptional();
			Property(s => s.UserToken).HasMaxLength(100).IsRequired();
			Property(s => s.WifiGatewayId).HasMaxLength(100).IsOptional();
			Property(s => s.Phone).HasMaxLength(20).IsOptional();
			Property(s => s.IP).HasMaxLength(30).IsOptional();
			Property(s => s.Mac).HasMaxLength(100).IsOptional();
			Property(s => s.ClientType).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.LastUpdateTime).IsOptional();
			Property(s => s.ExpirTime).IsOptional();
        }
    }
}
