using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class PrepayAccountLog: IAggregateRoot
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
        /// 房间Id
        /// </summary>
		public int? HouseDeptId { get; set; }

		/// <summary>
        /// 资源Id
        /// </summary>
		public int? ResourcesId { get; set; }

		/// <summary>
        /// 预存账户Id
        /// </summary>
		public int? PrepayAccountId { get; set; }

		/// <summary>
        /// 内容
        /// </summary>
		public string Desc { get; set; }

		/// <summary>
        /// 操作人
        /// </summary>
		public string Operator { get; set; }

		/// <summary>
        /// 操作人Id
        /// </summary>
		public int? OperatorId { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 操作时间
        /// </summary>
		public DateTime? OperationTime { get; set; }
	 }
	public partial class PrepayAccountLogMapper : EntityMapper<PrepayAccountLog>
    {
        public PrepayAccountLogMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.ComDeptId).IsOptional();
			Property(s => s.HouseDeptId).IsOptional();
			Property(s => s.ResourcesId).IsOptional();
			Property(s => s.PrepayAccountId).IsOptional();
			Property(s => s.Desc).HasMaxLength(500).IsOptional();
			Property(s => s.Operator).HasMaxLength(50).IsOptional();
			Property(s => s.OperatorId).IsOptional();
			Property(s => s.Remark).HasMaxLength(500).IsOptional();
			Property(s => s.OperationTime).IsOptional();
        }
    }
}
