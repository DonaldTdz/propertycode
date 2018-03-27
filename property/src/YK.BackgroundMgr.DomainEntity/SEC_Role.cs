using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_Role: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// 角色名称
        /// </summary>
		public string Name { get; set; }
		/// <summary>
        /// 编码
        /// </summary>
		public string Code { get; set; }
		/// <summary>
        /// 描述
        /// </summary>
		public string Remark { get; set; }
		
      public virtual ICollection<SEC_Field> SEC_Fields { get; set; }
      public virtual ICollection<SEC_Operate> SEC_Operates { get; set; }
      public virtual ICollection<SEC_Module> SEC_Modules { get; set; }
      public virtual ICollection<SEC_AdminUser> SEC_AdminUsers { get; set; }
      public virtual ICollection<SEC_User> SEC_Users { get; set; }
    
	 }
	public partial class SEC_RoleMapper : EntityMapper<SEC_Role>
    {
        public SEC_RoleMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.Name).HasMaxLength(100).IsRequired();
			Property(s => s.Code).HasMaxLength(100).IsOptional();
			Property(s => s.Remark).HasMaxLength(4000).IsOptional();
        }
    }
}
