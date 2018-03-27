using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_Operate: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// 操作名称
        /// </summary>
		public string Name { get; set; }
		/// <summary>
        /// 编码
        /// </summary>
		public string Code { get; set; }
		/// <summary>
        /// 是否启用
        /// </summary>
		public bool IsUsed { get; set; }
		/// <summary>
        /// 描述
        /// </summary>
		public string Remark { get; set; }
		/// <summary>
        /// Id
        /// </summary>
		public string ControlId { get; set; }
		/// <summary>
        /// Class
        /// </summary>
		public string ControlClass { get; set; }
		
      public int SEC_Module_Id { get; set; }
      public virtual SEC_Module SEC_Module { get; set; }
      public virtual ICollection<SEC_Role> SEC_Roles { get; set; }
    
	 }
	public partial class SEC_OperateMapper : EntityMapper<SEC_Operate>
    {
        public SEC_OperateMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.Name).HasMaxLength(100).IsRequired();
			Property(s => s.Code).HasMaxLength(100).IsRequired();
			Property(s => s.IsUsed).IsRequired();
			Property(s => s.Remark).HasMaxLength(4000).IsOptional();
			Property(s => s.ControlId).HasMaxLength(100).IsOptional();
			Property(s => s.ControlClass).HasMaxLength(200).IsOptional();
        }
    }
}
