using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class ReceiptBookHistory: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 票据本操作类型
        /// </summary>
		public int? ReceiptBookHistoryType { get; set; }

		/// <summary>
        /// 操作内容
        /// </summary>
		public string OperatorContent { get; set; }

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
        /// 删除标志
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 物业或者小区Id
        /// </summary>
		public int? DeptId { get; set; }
	 }
	public partial class ReceiptBookHistoryMapper : EntityMapper<ReceiptBookHistory>
    {
        public ReceiptBookHistoryMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.ReceiptBookHistoryType).IsOptional();
			Property(s => s.OperatorContent).HasMaxLength(500).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.UpdateTime).IsOptional();
			Property(s => s.OperatorName).HasMaxLength(50).IsOptional();
			Property(s => s.IsDel).IsOptional();
			Property(s => s.DeptId).IsOptional();
        }
    }
}
