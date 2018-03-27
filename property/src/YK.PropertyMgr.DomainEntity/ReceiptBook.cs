using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class ReceiptBook: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 票据本名称
        /// </summary>
		public string Name { get; set; }

		/// <summary>
        /// 票据类型
        /// </summary>
		public int? ReceiptBookType { get; set; }

		/// <summary>
        /// 删除标志
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 票据状态
        /// </summary>
		public int? Status { get; set; }

		/// <summary>
        /// 前缀
        /// </summary>
		public string Prefix { get; set; }

		/// <summary>
        /// 后缀位数
        /// </summary>
		public int? Suffix { get; set; }

		/// <summary>
        /// 物业或者小区Id
        /// </summary>
		public int? DeptId { get; set; }

		/// <summary>
        /// 起号
        /// </summary>
		public int? BeginCode { get; set; }

		/// <summary>
        /// 止号
        /// </summary>
		public int? EndCode { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 票据总数量
        /// </summary>
		public int? TotalNumber { get; set; }

		/// <summary>
        /// 已用数量
        /// </summary>
		public int? UsedNumber { get; set; }

		/// <summary>
        /// 作废票据数量
        /// </summary>
		public int? InvalidNumber { get; set; }

		/// <summary>
        /// 当前票据号
        /// </summary>
		public string CurrentReceiptNum { get; set; }

		/// <summary>
        /// 票据金额
        /// </summary>
		public decimal? ReceiptAmount { get; set; }
	 }
	public partial class ReceiptBookMapper : EntityMapper<ReceiptBook>
    {
        public ReceiptBookMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.Name).HasMaxLength(50).IsOptional();
			Property(s => s.ReceiptBookType).IsOptional();
			Property(s => s.IsDel).IsOptional();
			Property(s => s.Status).IsOptional();
			Property(s => s.Prefix).HasMaxLength(50).IsOptional();
			Property(s => s.Suffix).IsOptional();
			Property(s => s.DeptId).IsOptional();
			Property(s => s.BeginCode).IsOptional();
			Property(s => s.EndCode).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.UpdateTime).IsOptional();
			Property(s => s.TotalNumber).IsOptional();
			Property(s => s.UsedNumber).IsOptional();
			Property(s => s.InvalidNumber).IsOptional();
			Property(s => s.CurrentReceiptNum).HasMaxLength(50).IsOptional();
			Property(s => s.ReceiptAmount).IsOptional();
        }
    }
}
