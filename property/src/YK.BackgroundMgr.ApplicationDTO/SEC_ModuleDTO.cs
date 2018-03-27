using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_ModuleDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 英文名称
        /// </summary>
		public string EnName { get; set; }

		/// <summary>
        /// 中文名称
        /// </summary>
		public string CnName { get; set; }

		/// <summary>
        /// 父节点Id
        /// </summary>
		public int? PId { get; set; }

		/// <summary>
        /// 编码
        /// </summary>
		public string Code { get; set; }

		/// <summary>
        /// Url
        /// </summary>
		public string Url { get; set; }

		/// <summary>
        /// 图标
        /// </summary>
		public string Iconic { get; set; }

		/// <summary>
        /// 排序
        /// </summary>
		public string Order { get; set; }

		/// <summary>
        /// 是否启用
        /// </summary>
		public bool IsUsed { get; set; }

		/// <summary>
        /// 是否显示
        /// </summary>
		public bool IsShow { get; set; }

		/// <summary>
        /// JSController
        /// </summary>
		public string JSController { get; set; }

		/// <summary>
        /// 导航图片
        /// </summary>
		public string NavImage { get; set; }

		/// <summary>
        /// 描述
        /// </summary>
		public string Remark { get; set; }
	 }
}
