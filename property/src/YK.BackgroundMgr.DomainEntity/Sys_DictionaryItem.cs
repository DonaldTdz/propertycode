using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class Sys_DictionaryItem: IAggregateRoot
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
        /// 编码
        /// </summary>
		public string Code { get; set; }
		/// <summary>
        /// 排序
        /// </summary>
		public string Order { get; set; }
		/// <summary>
        /// 是否启用
        /// </summary>
		public bool IsUsed { get; set; }
		/// <summary>
        /// 最后修改人
        /// </summary>
		public string LastUpdateUser { get; set; }
		/// <summary>
        /// 最后修改时间
        /// </summary>
		public DateTime? LastUpdateTime { get; set; }
		/// <summary>
        /// 描述
        /// </summary>
		public string Remark { get; set; }
		
      public int Sys_Dictionary_Id { get; set; }
      public virtual Sys_Dictionary Sys_Dictionary { get; set; }
    
	 }
	public partial class Sys_DictionaryItemMapper : EntityMapper<Sys_DictionaryItem>
    {
        public Sys_DictionaryItemMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.EnName).HasMaxLength(100).IsRequired();
			Property(s => s.CnName).HasMaxLength(100).IsRequired();
			Property(s => s.Code).HasMaxLength(30).IsRequired();
			Property(s => s.Order).HasMaxLength(100).IsRequired();
			Property(s => s.IsUsed).IsOptional();
			Property(s => s.LastUpdateUser).HasMaxLength(50).IsOptional();
			Property(s => s.LastUpdateTime).IsOptional();
			Property(s => s.Remark).HasMaxLength(4000).IsOptional();
        }
    }
}
