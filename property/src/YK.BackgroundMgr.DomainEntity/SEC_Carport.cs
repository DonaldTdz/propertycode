using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_Carport: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// 车库名称
        /// </summary>
		public string CarbarnName { get; set; }
		/// <summary>
        /// 车位名称
        /// </summary>
		public string CarportName { get; set; }
		/// <summary>
        /// 车位编号
        /// </summary>
		public string CarportNo { get; set; }
		/// <summary>
        /// 是否业主所有
        /// </summary>
		public int? CarportIsOwner { get; set; }
		/// <summary>
        /// 产权面积
        /// </summary>
		public double? PropertyArea { get; set; }
		/// <summary>
        /// 车位类型
        /// </summary>
		public string CarportType { get; set; }
		/// <summary>
        /// 使用面积
        /// </summary>
		public double? UseArea { get; set; }
		/// <summary>
        /// 是否计费
        /// </summary>
		public int? IsCharge { get; set; }
		/// <summary>
        /// 销售日期
        /// </summary>
		public DateTime? SalesDate { get; set; }
		/// <summary>
        /// 收房日期
        /// </summary>
		public DateTime? TakeDate { get; set; }
		/// <summary>
        /// 入住日期
        /// </summary>
		public DateTime? CheckDate { get; set; }
		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }
		/// <summary>
        /// 组织架构Id
        /// </summary>
		public Guid? OrgId { get; set; }
	 }
	public partial class SEC_CarportMapper : EntityMapper<SEC_Carport>
    {
        public SEC_CarportMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.CarbarnName).HasMaxLength(100).IsRequired();
			Property(s => s.CarportName).HasMaxLength(100).IsOptional();
			Property(s => s.CarportNo).HasMaxLength(50).IsRequired();
			Property(s => s.CarportIsOwner).IsOptional();
			Property(s => s.PropertyArea).IsRequired();
			Property(s => s.CarportType).HasMaxLength(200).IsOptional();
			Property(s => s.UseArea).IsOptional();
			Property(s => s.IsCharge).IsOptional();
			Property(s => s.SalesDate).IsOptional();
			Property(s => s.TakeDate).IsOptional();
			Property(s => s.CheckDate).IsOptional();
			Property(s => s.Remark).HasMaxLength(2000).IsOptional();
			Property(s => s.OrgId).IsOptional();
        }
    }
}
