using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_Garage: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// 车库名称
        /// </summary>
		public string GarageName { get; set; }
		/// <summary>
        /// 车库编号
        /// </summary>
		public string GarageCode { get; set; }
		/// <summary>
        /// 车位数量
        /// </summary>
		public int? GarageNum { get; set; }
		/// <summary>
        /// 开工日期
        /// </summary>
		public DateTime? StartDate { get; set; }
		/// <summary>
        /// 完工日期
        /// </summary>
		public DateTime? EndDate { get; set; }
		/// <summary>
        /// 描述信息
        /// </summary>
		public string Remark { get; set; }
		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
        /// 组织架构Id
        /// </summary>
		public int? DeptId { get; set; }
	 }
	public partial class SEC_GarageMapper : EntityMapper<SEC_Garage>
    {
        public SEC_GarageMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.GarageName).HasMaxLength(50).IsRequired();
			Property(s => s.GarageCode).HasMaxLength(100).IsOptional();
			Property(s => s.GarageNum).IsOptional();
			Property(s => s.StartDate).IsOptional();
			Property(s => s.EndDate).IsOptional();
			Property(s => s.Remark).HasMaxLength(4000).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.DeptId).IsOptional();
        }
    }
}
