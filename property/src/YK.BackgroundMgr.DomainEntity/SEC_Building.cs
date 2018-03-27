using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_Building: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// 编号
        /// </summary>
		public string Building_code { get; set; }
		/// <summary>
        /// 楼宇名称
        /// </summary>
		public string Building_name { get; set; }
		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }
		/// <summary>
        /// 组织架构Id
        /// </summary>
		public int? DeptId { get; set; }
	 }
	public partial class SEC_BuildingMapper : EntityMapper<SEC_Building>
    {
        public SEC_BuildingMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.Building_code).HasMaxLength(100).IsOptional();
			Property(s => s.Building_name).HasMaxLength(100).IsRequired();
			Property(s => s.Remark).HasMaxLength(2000).IsOptional();
			Property(s => s.DeptId).IsOptional();
        }
    }
}
