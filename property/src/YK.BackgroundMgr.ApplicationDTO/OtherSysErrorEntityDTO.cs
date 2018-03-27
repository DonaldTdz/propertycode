using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class OtherSysErrorEntityDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 来源系统
        /// </summary>
		public string FromSys { get; set; }

		/// <summary>
        /// 来源地址
        /// </summary>
		public string FromUrl { get; set; }

		/// <summary>
        /// 其他系统主键Id
        /// </summary>
		public string OtherSysId { get; set; }

		/// <summary>
        /// 错误信息
        /// </summary>
		public string ErrorMsg { get; set; }

		/// <summary>
        /// 错误实体
        /// </summary>
		public string ErrorEntity { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }
	 }
}
