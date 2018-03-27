using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class PrepayAccountDetail: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 期初金额
        /// </summary>
		public decimal? BeginningBalance { get; set; }

		/// <summary>
        /// 发生金额
        /// </summary>
		public decimal? ProductionAmount { get; set; }

		/// <summary>
        /// 期末金额
        /// </summary>
		public decimal? EndingBalance { get; set; }

		/// <summary>
        /// 操作人
        /// </summary>
		public int? Operator { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 删除标识
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 费用流水ID
        /// </summary>
		public string ChargeRecordId { get; set; }

		/// <summary>
        /// 描述
        /// </summary>
		public string Description { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 付款方式
        /// </summary>
		public int? PayTypeId { get; set; }

		/// <summary>
        /// 客户名称
        /// </summary>
		public string CustomerName { get; set; }
		
      public int PrepayAccountId { get; set; }
      public virtual PrepayAccount PrepayAccount { get; set; }
    
	 }
	public partial class PrepayAccountDetailMapper : EntityMapper<PrepayAccountDetail>
    {
        public PrepayAccountDetailMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.BeginningBalance).IsOptional();
			Property(s => s.ProductionAmount).IsOptional();
			Property(s => s.EndingBalance).IsOptional();
			Property(s => s.Operator).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.UpdateTime).IsOptional();
			Property(s => s.IsDel).IsOptional();
			Property(s => s.ChargeRecordId).HasMaxLength(36).IsOptional();
			Property(s => s.Description).HasMaxLength(300).IsOptional();
			Property(s => s.Remark).HasMaxLength(300).IsOptional();
			Property(s => s.PayTypeId).IsOptional();
			Property(s => s.CustomerName).HasMaxLength(50).IsOptional();
        }
    }
}
