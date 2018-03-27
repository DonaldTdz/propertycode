using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class AlipayChargeBillSynchronizer: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public string Id { get; set; }

		/// <summary>
        /// 小区Id
        /// </summary>
		public int? ComDeptId { get; set; }

		/// <summary>
        /// 计数
        /// </summary>
		public int? CountNumber { get; set; }

		/// <summary>
        /// 批次号
        /// </summary>
		public string BatchCode { get; set; }

		/// <summary>
        /// 完成状态
        /// </summary>
		public bool? IsFinish { get; set; }

		/// <summary>
        /// 删除标志
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }
	 }
	public partial class AlipayChargeBillSynchronizerMapper : EntityMapper<AlipayChargeBillSynchronizer>
    {
        public AlipayChargeBillSynchronizerMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.ComDeptId).IsOptional();
			Property(s => s.CountNumber).IsOptional();
			Property(s => s.BatchCode).HasMaxLength(50).IsOptional();
			Property(s => s.IsFinish).IsOptional();
			Property(s => s.IsDel).IsOptional();
			Property(s => s.CreateTime).IsOptional();
        }
    }
}
