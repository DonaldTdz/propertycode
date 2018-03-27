using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class OtherSysErrorEntity: IAggregateRoot
	{
		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// 来源系统
        /// </summary>
		public string FromSys { get; set; }
		/// <summary>
        /// 来源地址
        /// </summary>
		public string FromUrl { get; set; }
		/// <summary>
        /// 其他系统主键Id
        /// </summary>
		public string OtherSysId { get; set; }
		/// <summary>
        /// 错误信息
        /// </summary>
		public string ErrorMsg { get; set; }
		/// <summary>
        /// 错误实体
        /// </summary>
		public string ErrorEntity { get; set; }
		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }
	 }
	public partial class OtherSysErrorEntityMapper : EntityMapper<OtherSysErrorEntity>
    {
        public OtherSysErrorEntityMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.FromSys).HasMaxLength(100).IsOptional();
			Property(s => s.FromUrl).HasMaxLength(500).IsOptional();
			Property(s => s.OtherSysId).HasMaxLength(100).IsOptional();
			Property(s => s.ErrorMsg).IsMaxLength().IsOptional();
			Property(s => s.ErrorEntity).IsMaxLength().IsOptional();
			Property(s => s.CreateTime).IsOptional();
        }
    }
}
