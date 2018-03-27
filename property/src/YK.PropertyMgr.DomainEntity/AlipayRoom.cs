using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class AlipayRoom: IAggregateRoot
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
        /// 所属物业ID
        /// </summary>
		public int? HouseDeptId { get; set; }

		/// <summary>
        /// 支付宝房间Id
        /// </summary>
		public string AlipayRoomId { get; set; }

		/// <summary>
        /// 操作人
        /// </summary>
		public string OperatorName { get; set; }

		/// <summary>
        /// 操作人Id
        /// </summary>
		public string OperatorId { get; set; }

		/// <summary>
        /// 更新操作人
        /// </summary>
		public string UpdateOperatorName { get; set; }

		/// <summary>
        /// 更新操作人Id
        /// </summary>
		public string UpdateOperatorId { get; set; }

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
        /// 批次号
        /// </summary>
		public string BatchCode { get; set; }

		/// <summary>
        /// 房间号
        /// </summary>
		public string HouseNumber { get; set; }
	 }
	public partial class AlipayRoomMapper : EntityMapper<AlipayRoom>
    {
        public AlipayRoomMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.ComDeptId).IsOptional();
			Property(s => s.HouseDeptId).IsOptional();
			Property(s => s.AlipayRoomId).HasMaxLength(32).IsOptional();
			Property(s => s.OperatorName).HasMaxLength(50).IsOptional();
			Property(s => s.OperatorId).HasMaxLength(50).IsOptional();
			Property(s => s.UpdateOperatorName).HasMaxLength(50).IsOptional();
			Property(s => s.UpdateOperatorId).HasMaxLength(50).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.UpdateTime).IsOptional();
			Property(s => s.IsDel).IsOptional();
			Property(s => s.BatchCode).HasMaxLength(50).IsOptional();
			Property(s => s.HouseNumber).HasMaxLength(50).IsOptional();
        }
    }
}
