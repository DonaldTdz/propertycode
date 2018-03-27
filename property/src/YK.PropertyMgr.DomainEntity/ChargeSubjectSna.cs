using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class ChargeSubjectSna: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 账单Id
        /// </summary>
		public string ChargeBillId { get; set; }

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
        /// 滞纳金率 /日
        /// </summary>
		public decimal? PenaltyRate { get; set; }

		/// <summary>
        /// 账单日
        /// </summary>
		public int? BillDay { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 在线支付
        /// </summary>
		public bool? IsOnline { get; set; }

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
        /// 操作人Id
        /// </summary>
		public int? Operator { get; set; }
	 }
	public partial class ChargeSubjectSnaMapper : EntityMapper<ChargeSubjectSna>
    {
        public ChargeSubjectSnaMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.ChargeBillId).HasMaxLength(36).IsOptional();
			Property(s => s.Price).IsOptional();
			Property(s => s.ChargeFormula).HasMaxLength(100).IsOptional();
			Property(s => s.ChargeFormulaShow).HasMaxLength(100).IsOptional();
			Property(s => s.PenaltyRate).IsOptional();
			Property(s => s.BillDay).IsOptional();
			Property(s => s.Remark).HasMaxLength(300).IsOptional();
			Property(s => s.IsOnline).IsOptional();
			Property(s => s.ComDeptId).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.UpdateTime).IsOptional();
			Property(s => s.Operator).IsOptional();
        }
    }
}
