﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_CarportDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 车库名称
        /// </summary>
		public string CarbarnName { get; set; }

		/// <summary>
        /// 车位名称
        /// </summary>
		public string CarportName { get; set; }

		/// <summary>
        /// 车位编号
        /// </summary>
		public string CarportNo { get; set; }

		/// <summary>
        /// 是否业主所有
        /// </summary>
		public int? CarportIsOwner { get; set; }

		/// <summary>
        /// 产权面积
        /// </summary>
		public double? PropertyArea { get; set; }

		/// <summary>
        /// 车位类型
        /// </summary>
		public string CarportType { get; set; }

		/// <summary>
        /// 使用面积
        /// </summary>
		public double? UseArea { get; set; }

		/// <summary>
        /// 是否计费
        /// </summary>
		public int? IsCharge { get; set; }

		/// <summary>
        /// 销售日期
        /// </summary>
		public DateTime? SalesDate { get; set; }

		/// <summary>
        /// 收房日期
        /// </summary>
		public DateTime? TakeDate { get; set; }

		/// <summary>
        /// 入住日期
        /// </summary>
		public DateTime? CheckDate { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 组织架构Id
        /// </summary>
		public Guid OrgId { get; set; }
	 }
}
