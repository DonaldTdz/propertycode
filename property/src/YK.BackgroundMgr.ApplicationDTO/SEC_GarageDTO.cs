using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_GarageDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 车库名称
        /// </summary>
		public string GarageName { get; set; }

		/// <summary>
        /// 车库编号
        /// </summary>
		public string GarageCode { get; set; }

		/// <summary>
        /// 车位数量
        /// </summary>
		public int? GarageNum { get; set; }

		/// <summary>
        /// 开工日期
        /// </summary>
		public DateTime? StartDate { get; set; }

		/// <summary>
        /// 完工日期
        /// </summary>
		public DateTime? EndDate { get; set; }

		/// <summary>
        /// 描述信息
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 组织架构Id
        /// </summary>
		public int? DeptId { get; set; }
	 }
}
