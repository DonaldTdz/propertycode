using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class PaymentTasks: IAggregateRoot
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
        /// 操作人
        /// </summary>
		public int? Operator { get; set; }

		/// <summary>
        /// 操作人名
        /// </summary>
		public string OperatorName { get; set; }

		/// <summary>
        /// 交款时间
        /// </summary>
		public DateTime? PaymentDate { get; set; }

		/// <summary>
        /// 交款金额
        /// </summary>
		public decimal? Money { get; set; }

		/// <summary>
        /// 删除标志
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 是否余额初始化
        /// </summary>
		public bool? IsInitBalance { get; set; }

		/// <summary>
        /// 审核时间
        /// </summary>
		public DateTime? ReviewDate { get; set; }

		/// <summary>
        /// 审核人Id
        /// </summary>
		public int? ReviewerId { get; set; }

		/// <summary>
        /// 审核人名
        /// </summary>
		public string ReviewerName { get; set; }

		/// <summary>
        /// 申请人Id
        /// </summary>
		public int? ApplicantId { get; set; }

		/// <summary>
        /// 申请人名
        /// </summary>
		public string ApplicantName { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 审核状态
        /// </summary>
		public int? Status { get; set; }

		/// <summary>
        /// 编号
        /// </summary>
		public string Code { get; set; }

		/// <summary>
        /// 审核备注
        /// </summary>
		public string CheckRemark { get; set; }
		
      public virtual ICollection<PaymentTaskDetail> PaymentTaskItems { get; set; }
    
	 }
	public partial class PaymentTasksMapper : EntityMapper<PaymentTasks>
    {
        public PaymentTasksMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.ComDeptId).IsOptional();
			Property(s => s.Operator).IsOptional();
			Property(s => s.OperatorName).HasMaxLength(50).IsOptional();
			Property(s => s.PaymentDate).IsOptional();
			Property(s => s.Money).IsOptional();
			Property(s => s.IsDel).IsOptional();
			Property(s => s.IsInitBalance).IsOptional();
			Property(s => s.ReviewDate).IsOptional();
			Property(s => s.ReviewerId).IsOptional();
			Property(s => s.ReviewerName).HasMaxLength(50).IsOptional();
			Property(s => s.ApplicantId).IsOptional();
			Property(s => s.ApplicantName).HasMaxLength(50).IsOptional();
			Property(s => s.Remark).HasMaxLength(500).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.UpdateTime).IsOptional();
			Property(s => s.Status).IsOptional();
			Property(s => s.Code).HasMaxLength(100).IsOptional();
			Property(s => s.CheckRemark).HasMaxLength(500).IsOptional();
			
      HasMany(s => s.PaymentTaskItems).WithRequired(s => s.PaymentTasks).HasForeignKey(s => s.PaymentTaskID).WillCascadeOnDelete(true);
    
        }
    }
}
