using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class AlipayAPPAuthToken: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public string Id { get; set; }

		/// <summary>
        /// 所属物业ID
        /// </summary>
		public int? ProDeptId { get; set; }

		/// <summary>
        /// 商户授权令牌
        /// </summary>
		public string app_auth_token { get; set; }

		/// <summary>
        /// 授权商户的ID
        /// </summary>
		public string user_id { get; set; }

		/// <summary>
        /// 授权商户的AppIdID
        /// </summary>
		public string auth_app_id { get; set; }

		/// <summary>
        /// 令牌有效期
        /// </summary>
		public int? expires_in { get; set; }

		/// <summary>
        /// 刷新令牌有效期
        /// </summary>
		public int? re_expires_in { get; set; }

		/// <summary>
        /// 刷新令牌时使用
        /// </summary>
		public string app_refresh_token { get; set; }

		/// <summary>
        /// 删除标志
        /// </summary>
		public bool? IsDel { get; set; }
	 }
	public partial class AlipayAPPAuthTokenMapper : EntityMapper<AlipayAPPAuthToken>
    {
        public AlipayAPPAuthTokenMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.ProDeptId).IsOptional();
			Property(s => s.app_auth_token).HasMaxLength(50).IsOptional();
			Property(s => s.user_id).HasMaxLength(50).IsOptional();
			Property(s => s.auth_app_id).HasMaxLength(50).IsOptional();
			Property(s => s.expires_in).IsOptional();
			Property(s => s.re_expires_in).IsOptional();
			Property(s => s.app_refresh_token).HasMaxLength(50).IsOptional();
			Property(s => s.IsDel).IsOptional();
        }
    }
}
