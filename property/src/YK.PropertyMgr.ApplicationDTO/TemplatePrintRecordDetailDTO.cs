using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class TemplatePrintRecordDetailDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// X坐标位置
        /// </summary>
		public float? XAxis { get; set; }

		/// <summary>
        /// Y坐标位置
        /// </summary>
		public float? YAxis { get; set; }

		/// <summary>
        /// 显示内容
        /// </summary>
		public string ContentText { get; set; }

		/// <summary>
        /// 字体大小
        /// </summary>
		public float? FontSize { get; set; }

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
        /// 循环列
        /// </summary>
		public bool? Isloop { get; set; }

		/// <summary>
        /// 列增量
        /// </summary>
		public float? RowIncrement { get; set; }

		/// <summary>
        /// 属性名称
        /// </summary>
		public string AttributeName { get; set; }

		/// <summary>
        /// 属性名称中文
        /// </summary>
		public string AttributeCNName { get; set; }
	 }
}
