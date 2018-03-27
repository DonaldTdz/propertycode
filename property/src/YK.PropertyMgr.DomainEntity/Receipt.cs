using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class Receipt: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public string Id { get; set; }

		/// <summary>
        /// 票据编号
        /// </summary>
		public string Number { get; set; }

		/// <summary>
        /// 票据状态
        /// </summary>
		public int? Status { get; set; }

		/// <summary>
        /// 是否删除
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 操作人
        /// </summary>
		public int? Operator { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime UpdateTime { get; set; }

		/// <summary>
        /// 操作人名
        /// </summary>
		public string OperatorName { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }
		
      public virtual ICollection<ChargeRecord> ChargeRecordList { get; set; }
    
	 }
	public partial class ReceiptMapper : EntityMapper<Receipt>
    {
        public ReceiptMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.Number).HasMaxLength(50).IsOptional();
			Property(s => s.Status).IsOptional();
			Property(s => s.IsDel).IsOptional();
			Property(s => s.Operator).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.UpdateTime).IsOptional();
			Property(s => s.OperatorName).HasMaxLength(50).IsOptional();
			Property(s => s.Remark).HasMaxLength(300).IsOptional();
			
      HasMany(s => s.ChargeRecordList).WithRequired(s => s.Receipt).HasForeignKey(s => s.ReceiptId).WillCascadeOnDelete(true);
    
        }
    }
}
