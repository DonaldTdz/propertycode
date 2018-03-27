using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_User: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// 用户名
        /// </summary>
		public string UserName { get; set; }
		/// <summary>
        /// 密码
        /// </summary>
		public string Password { get; set; }
		/// <summary>
        /// 真实姓名
        /// </summary>
		public string RealName { get; set; }
		/// <summary>
        /// 是否启用
        /// </summary>
		public bool IsEnabled { get; set; }
		/// <summary>
        /// 手机号码
        /// </summary>
		public string MobilePhone { get; set; }
		/// <summary>
        /// 办公电话
        /// </summary>
		public string OfficePhone { get; set; }
		/// <summary>
        /// 邮箱
        /// </summary>
		public string Email { get; set; }
		/// <summary>
        /// 用户类型
        /// </summary>
		public int? UserType { get; set; }
		
      public virtual ICollection<SEC_Dept> Depts { get; set; }
      public virtual ICollection<SEC_Role> SEC_Roles { get; set; }
    
	 }
	public partial class SEC_UserMapper : EntityMapper<SEC_User>
    {
        public SEC_UserMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.UserName).HasMaxLength(50).IsRequired();
			Property(s => s.Password).HasMaxLength(300).IsRequired();
			Property(s => s.RealName).HasMaxLength(50).IsRequired();
			Property(s => s.IsEnabled).IsRequired();
			Property(s => s.MobilePhone).HasMaxLength(20).IsRequired();
			Property(s => s.OfficePhone).HasMaxLength(20).IsOptional();
			Property(s => s.Email).HasMaxLength(100).IsRequired();
			Property(s => s.UserType).IsRequired();
        }
    }
}
