using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_Dept: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// 名称
        /// </summary>
		public string Name { get; set; }
		/// <summary>
        /// 父节点Id
        /// </summary>
		public int? PId { get; set; }
		/// <summary>
        /// 编码
        /// </summary>
		public string Code { get; set; }
		/// <summary>
        /// 排序
        /// </summary>
		public string Order { get; set; }
		/// <summary>
        /// 描述
        /// </summary>
		public string Remark { get; set; }
		/// <summary>
        /// 组织架构类型
        /// </summary>
		public int? DeptType { get; set; }
		/// <summary>
        /// 组织架构唯一Id
        /// </summary>
		public Guid? OrgId { get; set; }
		/// <summary>
        /// 其他系统主键Id
        /// </summary>
		public string OtherSysId { get; set; }
		
      public virtual ICollection<SEC_User> Users { get; set; }
      public virtual ICollection<SEC_AdminUser> SEC_AdminUsers { get; set; }
      public virtual ICollection<SEC_User_OwnerSEC_Dept> SEC_User_OwnerSEC_Dept { get; set; }
      public virtual ICollection<SEC_User_OwnerSEC_Carport> SEC_User_OwnerSEC_Carport { get; set; }
    
	 }
	public partial class SEC_DeptMapper : EntityMapper<SEC_Dept>
    {
        public SEC_DeptMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.Name).HasMaxLength(200).IsRequired();
			Property(s => s.PId).IsOptional();
			Property(s => s.Code).HasMaxLength(300).IsRequired();
			Property(s => s.Order).HasMaxLength(50).IsOptional();
			Property(s => s.Remark).HasMaxLength(4000).IsOptional();
			Property(s => s.DeptType).IsOptional();
			Property(s => s.OrgId).IsOptional();
			Property(s => s.OtherSysId).HasMaxLength(100).IsOptional();
			
      HasMany(s => s.SEC_User_OwnerSEC_Dept).WithRequired(s => s.SEC_Dept).HasForeignKey(s => s.SEC_Dept_Id).WillCascadeOnDelete(true);
      HasMany(s => s.SEC_User_OwnerSEC_Carport).WithRequired(s => s.SEC_Dept).HasForeignKey(s => s.SEC_Dept_Id).WillCascadeOnDelete(true);
    
        }
    }
}
