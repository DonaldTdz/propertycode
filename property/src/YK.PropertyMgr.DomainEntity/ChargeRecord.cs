using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class ChargeRecord: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public string Id { get; set; }

		/// <summary>
        /// 流水号
        /// </summary>
		public string SerialNumber { get; set; }

		/// <summary>
        /// 描述
        /// </summary>
		public string Description { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 发生金额
        /// </summary>
		public decimal? Amount { get; set; }

		/// <summary>
        /// 优惠金额
        /// </summary>
		public decimal? DiscountAmount { get; set; }

		/// <summary>
        /// 收费类型
        /// </summary>
		public int? ChargeType { get; set; }

		/// <summary>
        /// 付款日期
        /// </summary>
		public DateTime? PayDate { get; set; }

		/// <summary>
        /// 操作人
        /// </summary>
		public int? Operator { get; set; }

		/// <summary>
        /// 小区Id
        /// </summary>
		public int? ComDeptId { get; set; }

		/// <summary>
        /// 支付方式
        /// </summary>
		public int? PayMthodId { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 删除标志   
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 状态
        /// </summary>
		public int? Status { get; set; }

		/// <summary>
        /// 操作人名
        /// </summary>
		public string OperatorName { get; set; }

		/// <summary>
        /// 房屋Id
        /// </summary>
		public int? HouseDeptId { get; set; }

		/// <summary>
        /// 房屋编号
        /// </summary>
		public string HouseDeptNos { get; set; }

		/// <summary>
        /// 资源名称
        /// </summary>
		public string ResourcesNames { get; set; }

		/// <summary>
        /// 客户Id
        /// </summary>
		public string CustomerId { get; set; }

		/// <summary>
        /// 客户名称
        /// </summary>
		public string CustomerName { get; set; }

		/// <summary>
        /// 结算状态
        /// </summary>
		public int? AccountingStatus { get; set; }

		/// <summary>
        /// 是否是线上支付
        /// </summary>
		public bool? IsOnline { get; set; }
		
      public string ReceiptId { get; set; }
      public virtual Receipt Receipt { get; set; }
      public virtual ICollection<ChargeBillRecordMatching> ChargeBillRecordMatchingList { get; set; }
      public virtual ICollection<PaymentDiscountInfo> PaymentDiscountList { get; set; }
    
	 }
	public partial class ChargeRecordMapper : EntityMapper<ChargeRecord>
    {
        public ChargeRecordMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.SerialNumber).HasMaxLength(80).IsOptional();
			Property(s => s.Description).HasMaxLength(300).IsOptional();
			Property(s => s.Remark).HasMaxLength(300).IsOptional();
			Property(s => s.Amount).IsOptional();
			Property(s => s.DiscountAmount).IsOptional();
			Property(s => s.ChargeType).IsOptional();
			Property(s => s.PayDate).IsOptional();
			Property(s => s.Operator).IsOptional();
			Property(s => s.ComDeptId).IsOptional();
			Property(s => s.PayMthodId).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.UpdateTime).IsOptional();
			Property(s => s.IsDel).IsOptional();
			Property(s => s.Status).IsOptional();
			Property(s => s.OperatorName).HasMaxLength(50).IsOptional();
			Property(s => s.HouseDeptId).IsOptional();
			Property(s => s.HouseDeptNos).HasMaxLength(500).IsOptional();
			Property(s => s.ResourcesNames).HasMaxLength(1000).IsOptional();
			Property(s => s.CustomerId).HasMaxLength(36).IsOptional();
			Property(s => s.CustomerName).HasMaxLength(50).IsOptional();
			Property(s => s.AccountingStatus).IsOptional();
			Property(s => s.IsOnline).IsOptional();
			
      HasMany(s => s.ChargeBillRecordMatchingList).WithRequired(s => s.ChargeRecord).HasForeignKey(s => s.ChargeRecordId).WillCascadeOnDelete(true);
      HasMany(s => s.PaymentDiscountList).WithRequired(s => s.ChargeRecord).HasForeignKey(s => s.ChargeRecordId).WillCascadeOnDelete(true);
    
        }
    }
}
