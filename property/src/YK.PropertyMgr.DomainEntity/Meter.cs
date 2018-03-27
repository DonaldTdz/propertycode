using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class Meter: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 仪表编号
        /// </summary>
		public string MeterNum { get; set; }

		/// <summary>
        /// 仪表类型
        /// </summary>
		public int? MeterType { get; set; }

		/// <summary>
        /// 分摊方式
        /// </summary>
		public int? AllocationType { get; set; }

		/// <summary>
        /// 小区DetpId
        /// </summary>
		public int? ComDeptId { get; set; }

		/// <summary>
        /// 地址房屋
        /// </summary>
		public int? HouseDeptID { get; set; }

		/// <summary>
        /// 仪表读数
        /// </summary>
		public decimal? MeterValue { get; set; }

		/// <summary>
        /// 抄表日期
        /// </summary>
		public DateTime? ReadDate { get; set; }

		/// <summary>
        /// 是否启用
        /// </summary>
		public bool? IsEnabled { get; set; }

		/// <summary>
        /// 仪表最大值
        /// </summary>
		public decimal? MaxValue { get; set; }

		/// <summary>
        /// 是否是公区表
        /// </summary>
		public bool? IsPublicArea { get; set; }

		/// <summary>
        /// 公区绑定房屋DeptIds
        /// </summary>
		public string PublicAreaHouseDeptIDs { get; set; }

		/// <summary>
        /// 房屋编号
        /// </summary>
		public string HouseDoorNos { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 导入批次号
        /// </summary>
		public Guid? BulkVersion { get; set; }
	 }
	public partial class MeterMapper : EntityMapper<Meter>
    {
        public MeterMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.MeterNum).HasMaxLength(200).IsOptional();
			Property(s => s.MeterType).IsOptional();
			Property(s => s.AllocationType).IsOptional();
			Property(s => s.ComDeptId).IsOptional();
			Property(s => s.HouseDeptID).IsOptional();
			Property(s => s.MeterValue).IsOptional();
			Property(s => s.ReadDate).IsOptional();
			Property(s => s.IsEnabled).IsOptional();
			Property(s => s.MaxValue).IsOptional();
			Property(s => s.IsPublicArea).IsOptional();
			Property(s => s.PublicAreaHouseDeptIDs).HasMaxLength(8000).IsOptional();
			Property(s => s.HouseDoorNos).HasMaxLength(8000).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.Remark).HasMaxLength(500).IsOptional();
			Property(s => s.BulkVersion).IsOptional();
        }
    }
}
