using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_Gateway: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// 网关Id
        /// </summary>
		public string GatewayId { get; set; }
		/// <summary>
        /// 退出App允许上网时间(分钟)
        /// </summary>
		public int? AppLogoutMainTime { get; set; }
		/// <summary>
        /// 部署位置
        /// </summary>
		public string Location { get; set; }
		/// <summary>
        /// 描述
        /// </summary>
		public string Notes { get; set; }
		/// <summary>
        /// 组织架构Id
        /// </summary>
		public int? DeptId { get; set; }
	 }
	public partial class SEC_GatewayMapper : EntityMapper<SEC_Gateway>
    {
        public SEC_GatewayMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.GatewayId).HasMaxLength(50).IsRequired();
			Property(s => s.AppLogoutMainTime).IsRequired();
			Property(s => s.Location).HasMaxLength(500).IsOptional();
			Property(s => s.Notes).HasMaxLength(4000).IsOptional();
			Property(s => s.DeptId).IsOptional();
        }
    }
}
