﻿using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class PrepayAccount: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 房屋ID
        /// </summary>
		public int? HouseDeptId { get; set; }

		/// <summary>
        /// 余额
        /// </summary>
		public decimal? Balance { get; set; }

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
        /// 操作人
        /// </summary>
		public int? Operator { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 小区ID
        /// </summary>
		public int? CommDeptID { get; set; }

		/// <summary>
        /// 收费项目ID
        /// </summary>
		public int? ChargeSubjectID { get; set; }
		
      public virtual ICollection<PrepayAccountDetail> PrepayAccountItems { get; set; }
    
	 }
	public partial class PrepayAccountMapper : EntityMapper<PrepayAccount>
    {
        public PrepayAccountMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.HouseDeptId).IsOptional();
			Property(s => s.Balance).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.UpdateTime).IsOptional();
			Property(s => s.IsDel).IsOptional();
			Property(s => s.Operator).IsOptional();
			Property(s => s.Remark).HasMaxLength(300).IsOptional();
			Property(s => s.CommDeptID).IsOptional();
			Property(s => s.ChargeSubjectID).IsOptional();
			
      HasMany(s => s.PrepayAccountItems).WithRequired(s => s.PrepayAccount).HasForeignKey(s => s.PrepayAccountId).WillCascadeOnDelete(true);
    
        }
    }
}
