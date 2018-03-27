using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_Property: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// 物业名称
        /// </summary>
		public string PropertyName { get; set; }
		/// <summary>
        /// 物业编号
        /// </summary>
		public string PropertyCode { get; set; }
		/// <summary>
        /// 物业简介
        /// </summary>
		public string Profile { get; set; }
		/// <summary>
        /// 组织架构Id
        /// </summary>
		public int? DeptId { get; set; }
		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }
		/// <summary>
        /// 物业收费票据模板
        /// </summary>
		public int? BillTemplate { get; set; }
		/// <summary>
        /// 物业收费打印模板
        /// </summary>
		public string PrintTemplate { get; set; }
	 }
	public partial class SEC_PropertyMapper : EntityMapper<SEC_Property>
    {
        public SEC_PropertyMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.PropertyName).HasMaxLength(200).IsRequired();
			Property(s => s.PropertyCode).HasMaxLength(100).IsOptional();
			Property(s => s.Profile).IsMaxLength().IsOptional();
			Property(s => s.DeptId).IsRequired();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.Remark).HasMaxLength(2000).IsOptional();
			Property(s => s.BillTemplate).IsRequired();
			Property(s => s.PrintTemplate).HasMaxLength(100).IsRequired();
        }
    }
}
