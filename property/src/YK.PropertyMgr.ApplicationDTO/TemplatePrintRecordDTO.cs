using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class TemplatePrintRecordDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 套打模板名称
        /// </summary>
		public string Name { get; set; }

		/// <summary>
        /// 物业或者小区Id
        /// </summary>
		public int? DeptId { get; set; }

		/// <summary>
        /// 页面宽度
        /// </summary>
		public float? PageWidth { get; set; }

		/// <summary>
        /// 页面高度
        /// </summary>
		public float? PageHigh { get; set; }

		/// <summary>
        /// 表格条数
        /// </summary>
		public int? RowNumber { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 删除标志
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 基础组对应模板Id
        /// </summary>
		public int? templateId { get; set; }
	 }
}
