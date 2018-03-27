using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class Sys_Dictionary: IAggregateRoot
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
        /// 是否只读
        /// </summary>
		public bool IsReadonly { get; set; }
		/// <summary>
        /// 描述
        /// </summary>
		public string Remark { get; set; }
		
      public virtual ICollection<Sys_DictionaryItem> Sys_DictionaryItems { get; set; }
    
	 }
	public partial class Sys_DictionaryMapper : EntityMapper<Sys_Dictionary>
    {
        public Sys_DictionaryMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.EnName).HasMaxLength(100).IsRequired();
			Property(s => s.CnName).HasMaxLength(100).IsRequired();
			Property(s => s.IsReadonly).IsOptional();
			Property(s => s.Remark).HasMaxLength(4000).IsOptional();
			
      HasMany(s => s.Sys_DictionaryItems).WithRequired(s => s.Sys_Dictionary).HasForeignKey(s => s.Sys_Dictionary_Id).WillCascadeOnDelete(true);
    
        }
    }
}
