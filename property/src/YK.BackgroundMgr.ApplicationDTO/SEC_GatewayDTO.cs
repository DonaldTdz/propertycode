using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_GatewayDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 网关Id
        /// </summary>
		public string GatewayId { get; set; }

		/// <summary>
        /// 退出App允许上网时间(分钟)
        /// </summary>
		public int? AppLogoutMainTime { get; set; }

		/// <summary>
        /// 部署位置
        /// </summary>
		public string Location { get; set; }

		/// <summary>
        /// 描述
        /// </summary>
		public string Notes { get; set; }

		/// <summary>
        /// 组织架构Id
        /// </summary>
		public int? DeptId { get; set; }
	 }
}
