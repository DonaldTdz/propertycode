using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class PrepayAccountDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 房屋ID
        /// </summary>
		public int? HouseDeptId { get; set; }

		/// <summary>
        /// 余额
        /// </summary>
		public decimal? Balance { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 删除标识
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 操作人
        /// </summary>
		public int? Operator { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 小区ID
        /// </summary>
		public int? CommDeptID { get; set; }

		/// <summary>
        /// 收费项目ID
        /// </summary>
		public int? ChargeSubjectID { get; set; }
	 }
}
