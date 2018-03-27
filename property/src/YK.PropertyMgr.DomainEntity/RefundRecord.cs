using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class RefundRecord: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 房屋ID（外键）
        /// </summary>
		public int? HouseDeptId { get; set; }

		/// <summary>
        /// 费用记录ID(外键)
        /// </summary>
		public string ChargeRecordId { get; set; }

		/// <summary>
        /// 退款记录引用的费用ID
        /// </summary>
		public string RefChargeRecordId { get; set; }

		/// <summary>
        /// 客户姓名
        /// </summary>
		public string Customer { get; set; }

		/// <summary>
        /// 票据ID
        /// </summary>
		public string ReceiptID { get; set; }

		/// <summary>
        /// 付款方式
        /// </summary>
		public int? PayType { get; set; }

		/// <summary>
        /// 退款原因
        /// </summary>
		public string Reason { get; set; }

		/// <summary>
        /// 退款时间
        /// </summary>
		public DateTime RefundTime { get; set; }

		/// <summary>
        /// 是否删除
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 操作人
        /// </summary>
		public int? Operator { get; set; }

		/// <summary>
        /// 操作人名
        /// </summary>
		public string OperatorName { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime UpdateTime { get; set; }
	 }
	public partial class RefundRecordMapper : EntityMapper<RefundRecord>
    {
        public RefundRecordMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.HouseDeptId).IsOptional();
			Property(s => s.ChargeRecordId).HasMaxLength(36).IsOptional();
			Property(s => s.RefChargeRecordId).HasMaxLength(36).IsOptional();
			Property(s => s.Customer).HasMaxLength(50).IsOptional();
			Property(s => s.ReceiptID).HasMaxLength(36).IsOptional();
			Property(s => s.PayType).IsOptional();
			Property(s => s.Reason).HasMaxLength(200).IsOptional();
			Property(s => s.RefundTime).IsOptional();
			Property(s => s.IsDel).IsOptional();
			Property(s => s.Operator).IsOptional();
			Property(s => s.OperatorName).HasMaxLength(50).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.UpdateTime).IsOptional();
        }
    }
}
