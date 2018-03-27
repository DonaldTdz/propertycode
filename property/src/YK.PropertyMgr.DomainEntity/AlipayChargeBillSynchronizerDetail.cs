using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class AlipayChargeBillSynchronizerDetail: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 同步表Id
        /// </summary>
		public string AlipayChargeBillSynchronizerId { get; set; }

		/// <summary>
        /// 物业账单Id
        /// </summary>
		public string ChargeBillId { get; set; }

		/// <summary>
        /// 删除标志
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }
	 }
	public partial class AlipayChargeBillSynchronizerDetailMapper : EntityMapper<AlipayChargeBillSynchronizerDetail>
    {
        public AlipayChargeBillSynchronizerDetailMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.AlipayChargeBillSynchronizerId).HasMaxLength(36).IsOptional();
			Property(s => s.ChargeBillId).HasMaxLength(50).IsOptional();
			Property(s => s.IsDel).IsOptional();
			Property(s => s.CreateTime).IsOptional();
        }
    }
}
