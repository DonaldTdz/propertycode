using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_BuildingDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 编号
        /// </summary>
		public string Building_code { get; set; }

		/// <summary>
        /// 楼宇名称
        /// </summary>
		public string Building_name { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 组织架构Id
        /// </summary>
		public int? DeptId { get; set; }
	 }
}
