using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_Field: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// 字段名称
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
        /// 显示字段
        /// </summary>
		public string AgreeFileds { get; set; }
		/// <summary>
        /// 描述
        /// </summary>
		public string Remark { get; set; }
		
      public int SEC_Module_Id { get; set; }
      public virtual SEC_Module SEC_Module { get; set; }
      public virtual ICollection<SEC_Role> SEC_Roles { get; set; }
    
	 }
	public partial class SEC_FieldMapper : EntityMapper<SEC_Field>
    {
        public SEC_FieldMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.Name).HasMaxLength(100).IsRequired();
			Property(s => s.Code).HasMaxLength(100).IsRequired();
			Property(s => s.IsUsed).IsRequired();
			Property(s => s.AgreeFileds).HasMaxLength(500).IsOptional();
			Property(s => s.Remark).HasMaxLength(4000).IsOptional();
        }
    }
}
