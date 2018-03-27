using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_OperateDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 操作名称
        /// </summary>
		public string Name { get; set; }

		/// <summary>
        /// 编码
        /// </summary>
		public string Code { get; set; }

		/// <summary>
        /// 是否启用
        /// </summary>
		public bool IsUsed { get; set; }

		/// <summary>
        /// 描述
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// Id
        /// </summary>
		public string ControlId { get; set; }

		/// <summary>
        /// Class
        /// </summary>
		public string ControlClass { get; set; }
	 }
}
