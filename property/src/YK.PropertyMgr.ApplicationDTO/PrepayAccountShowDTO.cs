using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class PrepayAccountShowDTO
	{

		/// <summary>
        /// 房屋编号
        /// </summary>
		public string HouseNumber { get; set; }

		/// <summary>
        /// 收费项目
        /// </summary>
		public string ChargeSubjectName { get; set; }

		/// <summary>
        /// 房屋Id
        /// </summary>
		public int? HouseDeptId { get; set; }

		/// <summary>
        /// 姓名
        /// </summary>
		public string HouseHolders { get; set; }

		/// <summary>
        /// 余额
        /// </summary>
		public decimal? Balance { get; set; }

		/// <summary>
        /// 角色
        /// </summary>
		public string RoleName { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 小区Id
        /// </summary>
		public int? ComDeptId { get; set; }

		/// <summary>
        /// 收费项目Id
        /// </summary>
		public int? ChargeSubjectId { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 组织Id
        /// </summary>
		public int? DeptId { get; set; }
	 }
}
