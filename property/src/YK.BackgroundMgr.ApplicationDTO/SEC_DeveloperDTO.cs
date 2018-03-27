﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_DeveloperDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 开发商名称
        /// </summary>
		public string Name { get; set; }

		/// <summary>
        /// 法人代表
        /// </summary>
		public string Corporation { get; set; }

		/// <summary>
        /// 开发商类型
        /// </summary>
		public string Type { get; set; }

		/// <summary>
        /// 开发商描述
        /// </summary>
		public string Describe { get; set; }

		/// <summary>
        /// 成立时间
        /// </summary>
		public DateTime? Established { get; set; }

		/// <summary>
        /// 团队人数
        /// </summary>
		public int? TeamNum { get; set; }

		/// <summary>
        /// 通讯地址
        /// </summary>
		public string Address { get; set; }

		/// <summary>
        /// 邮箱
        /// </summary>
		public string Email { get; set; }

		/// <summary>
        /// 联系电话
        /// </summary>
		public string Telephone { get; set; }

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
