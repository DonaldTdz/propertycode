using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class Sys_DictionaryItemDTO
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
        /// 编码
        /// </summary>
		public string Code { get; set; }

		/// <summary>
        /// 排序
        /// </summary>
		public string Order { get; set; }

		/// <summary>
        /// 是否启用
        /// </summary>
		public bool IsUsed { get; set; }

		/// <summary>
        /// 最后修改人
        /// </summary>
		public string LastUpdateUser { get; set; }

		/// <summary>
        /// 最后修改时间
        /// </summary>
		public DateTime? LastUpdateTime { get; set; }

		/// <summary>
        /// 描述
        /// </summary>
		public string Remark { get; set; }
	 }
}
