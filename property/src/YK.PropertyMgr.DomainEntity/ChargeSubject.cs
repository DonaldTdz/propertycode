using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class ChargeSubject: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 项目编号
        /// </summary>
		public string Code { get; set; }

		/// <summary>
        /// 项目名称
        /// </summary>
		public string Name { get; set; }

		/// <summary>
        /// 收费周期
        /// </summary>
		public int? BillPeriod { get; set; }

		/// <summary>
        /// 单价
        /// </summary>
		public decimal? Price { get; set; }

		/// <summary>
        /// 计费公式
        /// </summary>
		public string ChargeFormula { get; set; }

		/// <summary>
        /// 计费公式中文显示
        /// </summary>
		public string ChargeFormulaShow { get; set; }

		/// <summary>
        /// 滞纳金率
        /// </summary>
		public decimal? PenaltyRate { get; set; }

		/// <summary>
        /// 账单日
        /// </summary>
		public int? BillDay { get; set; }

		/// <summary>
        /// 线上支付
        /// </summary>
		public bool? IsOnline { get; set; }

		/// <summary>
        /// 是否启用
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 小区Id
        /// </summary>
		public int? ComDeptId { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 操作人
        /// </summary>
		public int? Operator { get; set; }

		/// <summary>
        /// 项目类型
        /// </summary>
		public int? SubjectType { get; set; }

		/// <summary>
        /// 计费开始日
        /// </summary>
		public DateTime? BeginDate { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }
		
      public virtual ICollection<ChargBill> ChargeSubjectItems { get; set; }
    
		
      public virtual ICollection<SubjectHouseRef> ChargeSubjectHouseRefItems { get; set; }
    

		/// <summary>
        /// 自动生成账单
        /// </summary>
		public int? AutomaticBill { get; set; }
	 }
	public partial class ChargeSubjectMapper : EntityMapper<ChargeSubject>
    {
        public ChargeSubjectMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.Code).HasMaxLength(50).IsRequired();
			Property(s => s.Name).HasMaxLength(100).IsRequired();
			Property(s => s.BillPeriod).IsRequired();
			Property(s => s.Price).IsRequired();
			Property(s => s.ChargeFormula).HasMaxLength(100).IsRequired();
			Property(s => s.ChargeFormulaShow).HasMaxLength(200).IsRequired();
			Property(s => s.PenaltyRate).IsRequired();
			Property(s => s.BillDay).IsOptional();
			Property(s => s.IsOnline).IsRequired();
			Property(s => s.IsDel).IsRequired();
			Property(s => s.ComDeptId).IsRequired();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.UpdateTime).IsOptional();
			Property(s => s.Operator).IsOptional();
			Property(s => s.SubjectType).IsRequired();
			Property(s => s.BeginDate).IsRequired();
			Property(s => s.Remark).HasMaxLength(300).IsOptional();
			
      HasMany(s => s.ChargeSubjectItems).WithRequired(s => s.ChargeSubject).HasForeignKey(s => s.ChargeSubjectId).WillCascadeOnDelete(true);
    
			
      HasMany(s => s.ChargeSubjectHouseRefItems).WithRequired(s => s.ChargeSubject).HasForeignKey(s => s.ChargeSubjecId).WillCascadeOnDelete(true);
    
			Property(s => s.AutomaticBill).IsOptional();
        }
    }
}
