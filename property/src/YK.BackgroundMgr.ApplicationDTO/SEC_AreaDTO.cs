using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_AreaDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 父节点Id
        /// </summary>
		public int? PId { get; set; }

		/// <summary>
        /// 名称
        /// </summary>
		public string Name { get; set; }

		/// <summary>
        /// 简称
        /// </summary>
		public string ShortName { get; set; }

		/// <summary>
        /// 经度
        /// </summary>
		public double? Longitude { get; set; }

		/// <summary>
        /// 纬度
        /// </summary>
		public double? Latitude { get; set; }

		/// <summary>
        /// 地区类型
        /// </summary>
		public int? AreaType { get; set; }

		/// <summary>
        /// 排序
        /// </summary>
		public int? Sort { get; set; }

		/// <summary>
        /// 是否启用
        /// </summary>
		public int? Enabled { get; set; }
	 }
}
