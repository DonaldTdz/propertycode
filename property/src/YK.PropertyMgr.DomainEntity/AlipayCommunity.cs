using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class AlipayCommunity: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 小区Id
        /// </summary>
		public int? ComDeptId { get; set; }

		/// <summary>
        /// 支付宝小区Id
        /// </summary>
		public string AlipayCommunityId { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 创建人
        /// </summary>
		public string CreateOperator { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 更新人
        /// </summary>
		public string UpdateOperator { get; set; }

		/// <summary>
        /// 所属物业ID
        /// </summary>
		public int? ProDeptId { get; set; }

		/// <summary>
        /// 小区名称
        /// </summary>
		public string CommunityName { get; set; }

		/// <summary>
        /// 小区接入状态
        /// </summary>
		public int? Status { get; set; }

		/// <summary>
        /// 删除标志
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 初始化小区基础服务
        /// </summary>
		public bool? IsInitialize { get; set; }
	 }
	public partial class AlipayCommunityMapper : EntityMapper<AlipayCommunity>
    {
        public AlipayCommunityMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.ComDeptId).IsOptional();
			Property(s => s.AlipayCommunityId).HasMaxLength(32).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.CreateOperator).HasMaxLength(50).IsOptional();
			Property(s => s.UpdateTime).IsOptional();
			Property(s => s.UpdateOperator).HasMaxLength(50).IsOptional();
			Property(s => s.ProDeptId).IsOptional();
			Property(s => s.CommunityName).HasMaxLength(50).IsOptional();
			Property(s => s.Status).IsOptional();
			Property(s => s.IsDel).IsOptional();
			Property(s => s.IsInitialize).IsOptional();
        }
    }
}
