using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_PropertyDTO
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
}
