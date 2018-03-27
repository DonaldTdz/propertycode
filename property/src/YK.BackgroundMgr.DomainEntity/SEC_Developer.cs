using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_Developer: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// 开发商名称
        /// </summary>
		public string Name { get; set; }
		/// <summary>
        /// 法人代表
        /// </summary>
		public string Corporation { get; set; }
		/// <summary>
        /// 开发商类型
        /// </summary>
		public string Type { get; set; }
		/// <summary>
        /// 开发商描述
        /// </summary>
		public string Describe { get; set; }
		/// <summary>
        /// 成立时间
        /// </summary>
		public DateTime? Established { get; set; }
		/// <summary>
        /// 团队人数
        /// </summary>
		public int? TeamNum { get; set; }
		/// <summary>
        /// 通讯地址
        /// </summary>
		public string Address { get; set; }
		/// <summary>
        /// 邮箱
        /// </summary>
		public string Email { get; set; }
		/// <summary>
        /// 联系电话
        /// </summary>
		public string Telephone { get; set; }
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
	public partial class SEC_DeveloperMapper : EntityMapper<SEC_Developer>
    {
        public SEC_DeveloperMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.Name).HasMaxLength(100).IsRequired();
			Property(s => s.Corporation).HasMaxLength(100).IsOptional();
			Property(s => s.Type).HasMaxLength(100).IsOptional();
			Property(s => s.Describe).HasMaxLength(4000).IsOptional();
			Property(s => s.Established).IsOptional();
			Property(s => s.TeamNum).IsOptional();
			Property(s => s.Address).HasMaxLength(200).IsOptional();
			Property(s => s.Email).HasMaxLength(100).IsOptional();
			Property(s => s.Telephone).HasMaxLength(50).IsOptional();
			Property(s => s.Remark).HasMaxLength(4000).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.DeptId).IsOptional();
        }
    }
}
