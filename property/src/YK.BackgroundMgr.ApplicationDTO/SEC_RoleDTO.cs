using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_RoleDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 角色名称
        /// </summary>
		public string Name { get; set; }

		/// <summary>
        /// 编码
        /// </summary>
		public string Code { get; set; }

		/// <summary>
        /// 描述
        /// </summary>
		public string Remark { get; set; }
	 }
}
