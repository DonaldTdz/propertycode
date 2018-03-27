using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class CommunityConfig: IAggregateRoot
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
        /// 配置类型
        /// </summary>
		public int? ConfigType { get; set; }

		/// <summary>
        /// 是否显示楼栋
        /// </summary>
		public bool? IsBuilding { get; set; }

		/// <summary>
        /// 是否显示单元
        /// </summary>
		public bool? IsUnit { get; set; }

		/// <summary>
        /// 是否显示楼层
        /// </summary>
		public bool? IsFloor { get; set; }

		/// <summary>
        /// 是否显示楼号
        /// </summary>
		public bool? IsNumber { get; set; }

		/// <summary>
        /// 收费后默认打印票据
        /// </summary>
		public bool? IsDefaultPrintReceipt { get; set; }

		/// <summary>
        /// 确认收费弹出框
        /// </summary>
		public bool? IsChargeConfirm { get; set; }

		/// <summary>
        /// 生成账单自动预存抵扣
        /// </summary>
		public bool? IsPreAutomaticDeduction { get; set; }

		/// <summary>
        /// 预存抵扣收费记录合并
        /// </summary>
		public bool? IsPreMergeChargeRecord  { get; set; }
	 }
	public partial class CommunityConfigMapper : EntityMapper<CommunityConfig>
    {
        public CommunityConfigMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.ComDeptId).IsOptional();
			Property(s => s.ConfigType).IsOptional();
			Property(s => s.IsBuilding).IsRequired();
			Property(s => s.IsUnit).IsRequired();
			Property(s => s.IsFloor).IsRequired();
			Property(s => s.IsNumber).IsRequired();
			Property(s => s.IsDefaultPrintReceipt).IsRequired();
			Property(s => s.IsChargeConfirm).IsRequired();
			Property(s => s.IsPreAutomaticDeduction).IsRequired();
			Property(s => s.IsPreMergeChargeRecord ).IsRequired();
        }
    }
}
