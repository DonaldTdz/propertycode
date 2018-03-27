using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_Module: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// 英文名称
        /// </summary>
		public string EnName { get; set; }
		/// <summary>
        /// 中文名称
        /// </summary>
		public string CnName { get; set; }
		/// <summary>
        /// 父节点Id
        /// </summary>
		public int? PId { get; set; }
		/// <summary>
        /// 编码
        /// </summary>
		public string Code { get; set; }
		/// <summary>
        /// Url
        /// </summary>
		public string Url { get; set; }
		/// <summary>
        /// 图标
        /// </summary>
		public string Iconic { get; set; }
		/// <summary>
        /// 排序
        /// </summary>
		public string Order { get; set; }
		/// <summary>
        /// 是否启用
        /// </summary>
		public bool IsUsed { get; set; }
		/// <summary>
        /// 是否显示
        /// </summary>
		public bool IsShow { get; set; }
		/// <summary>
        /// JSController
        /// </summary>
		public string JSController { get; set; }
		/// <summary>
        /// 导航图片
        /// </summary>
		public string NavImage { get; set; }
		/// <summary>
        /// 描述
        /// </summary>
		public string Remark { get; set; }
		
      public virtual ICollection<SEC_Field> SEC_Fields { get; set; }
      public virtual ICollection<SEC_Operate> SEC_Operates { get; set; }
      public virtual ICollection<SEC_Role> SEC_Roles { get; set; }
    
	 }
	public partial class SEC_ModuleMapper : EntityMapper<SEC_Module>
    {
        public SEC_ModuleMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.EnName).HasMaxLength(100).IsRequired();
			Property(s => s.CnName).HasMaxLength(100).IsRequired();
			Property(s => s.PId).IsOptional();
			Property(s => s.Code).HasMaxLength(100).IsOptional();
			Property(s => s.Url).HasMaxLength(300).IsOptional();
			Property(s => s.Iconic).HasMaxLength(300).IsRequired();
			Property(s => s.Order).HasMaxLength(200).IsRequired();
			Property(s => s.IsUsed).IsRequired();
			Property(s => s.IsShow).IsRequired();
			Property(s => s.JSController).HasMaxLength(500).IsOptional();
			Property(s => s.NavImage).HasMaxLength(500).IsOptional();
			Property(s => s.Remark).HasMaxLength(4000).IsOptional();
			
      HasMany(s => s.SEC_Fields).WithRequired(s => s.SEC_Module).HasForeignKey(s => s.SEC_Module_Id).WillCascadeOnDelete(true);
      HasMany(s => s.SEC_Operates).WithRequired(s => s.SEC_Module).HasForeignKey(s => s.SEC_Module_Id).WillCascadeOnDelete(true);
    
        }
    }
}
