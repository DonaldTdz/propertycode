using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class MeterReadRecord: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 设备ID
        /// </summary>
		public int? MeterId { get; set; }

		/// <summary>
        /// 下一次抄表读数的ID
        /// </summary>
		public int? NextRefID { get; set; }

		/// <summary>
        /// 是否生成账单
        /// </summary>
		public bool? IsBill { get; set; }

		/// <summary>
        /// 账单ID
        /// </summary>
		public string BillID { get; set; }

		/// <summary>
        /// 仪表读数
        /// </summary>
		public decimal? MeterValue { get; set; }

		/// <summary>
        /// 抄表日期
        /// </summary>
		public DateTime? ReadDate { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }
	 }
	public partial class MeterReadRecordMapper : EntityMapper<MeterReadRecord>
    {
        public MeterReadRecordMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.MeterId).IsOptional();
			Property(s => s.NextRefID).IsOptional();
			Property(s => s.IsBill).IsOptional();
			Property(s => s.BillID).HasMaxLength(200).IsOptional();
			Property(s => s.MeterValue).IsOptional();
			Property(s => s.ReadDate).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.Remark).HasMaxLength(300).IsOptional();
        }
    }
}
