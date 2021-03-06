﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_UserDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 用户名
        /// </summary>
		public string UserName { get; set; }

		/// <summary>
        /// 密码
        /// </summary>
		public string Password { get; set; }

		/// <summary>
        /// 真实姓名
        /// </summary>
		public string RealName { get; set; }

		/// <summary>
        /// 是否启用
        /// </summary>
		public bool IsEnabled { get; set; }

		/// <summary>
        /// 手机号码
        /// </summary>
		public string MobilePhone { get; set; }

		/// <summary>
        /// 办公电话
        /// </summary>
		public string OfficePhone { get; set; }

		/// <summary>
        /// 邮箱
        /// </summary>
		public string Email { get; set; }

		/// <summary>
        /// 用户类型
        /// </summary>
		public int? UserType { get; set; }
	 }
}
