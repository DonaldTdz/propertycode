using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_Area: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// 父节点Id
        /// </summary>
		public int? PId { get; set; }
		/// <summary>
        /// 名称
        /// </summary>
		public string Name { get; set; }
		/// <summary>
        /// 简称
        /// </summary>
		public string ShortName { get; set; }
		/// <summary>
        /// 经度
        /// </summary>
		public double? Longitude { get; set; }
		/// <summary>
        /// 纬度
        /// </summary>
		public double? Latitude { get; set; }
		/// <summary>
        /// 地区类型
        /// </summary>
		public int? AreaType { get; set; }
		/// <summary>
        /// 排序
        /// </summary>
		public int? Sort { get; set; }
		/// <summary>
        /// 是否启用
        /// </summary>
		public int? Enabled { get; set; }
	 }
	public partial class SEC_AreaMapper : EntityMapper<SEC_Area>
    {
        public SEC_AreaMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.PId).IsRequired();
			Property(s => s.Name).HasMaxLength(200).IsRequired();
			Property(s => s.ShortName).HasMaxLength(200).IsRequired();
			Property(s => s.Longitude).IsRequired();
			Property(s => s.Latitude).IsRequired();
			Property(s => s.AreaType).IsRequired();
			Property(s => s.Sort).IsRequired();
			Property(s => s.Enabled).IsRequired();
        }
    }
}
