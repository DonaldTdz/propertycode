using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class Sys_DictionaryDTO
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
        /// 是否只读
        /// </summary>
		public bool IsReadonly { get; set; }

		/// <summary>
        /// 描述
        /// </summary>
		public string Remark { get; set; }
	 }
}
