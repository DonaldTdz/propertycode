﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_DeptDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 名称
        /// </summary>
		public string Name { get; set; }

		/// <summary>
        /// 父节点Id
        /// </summary>
		public int? PId { get; set; }

		/// <summary>
        /// 编码
        /// </summary>
		public string Code { get; set; }

		/// <summary>
        /// 排序
        /// </summary>
		public string Order { get; set; }

		/// <summary>
        /// 描述
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 组织架构类型
        /// </summary>
		public int? DeptType { get; set; }

		/// <summary>
        /// 组织架构唯一Id
        /// </summary>
		public Guid OrgId { get; set; }

		/// <summary>
        /// 其他系统主键Id
        /// </summary>
		public string OtherSysId { get; set; }
	 }
}