using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class ReceiptBookDetail: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 票据号
        /// </summary>
		public string Number { get; set; }

		/// <summary>
        /// 票据Id
        /// </summary>
		public string ReceiptId { get; set; }

		/// <summary>
        /// 删除标志
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 操作人名
        /// </summary>
		public string OperatorName { get; set; }

		/// <summary>
        /// 票据本Id
        /// </summary>
		public int? ReceiptBookId { get; set; }

		/// <summary>
        /// 票据金额
        /// </summary>
		public decimal? ReceiptAmount { get; set; }
	 }
	public partial class ReceiptBookDetailMapper : EntityMapper<ReceiptBookDetail>
    {
        public ReceiptBookDetailMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.Number).HasMaxLength(50).IsOptional();
			Property(s => s.ReceiptId).HasMaxLength(50).IsOptional();
			Property(s => s.IsDel).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.UpdateTime).IsOptional();
			Property(s => s.OperatorName).HasMaxLength(50).IsOptional();
			Property(s => s.ReceiptBookId).IsOptional();
			Property(s => s.ReceiptAmount).IsOptional();
        }
    }
}
