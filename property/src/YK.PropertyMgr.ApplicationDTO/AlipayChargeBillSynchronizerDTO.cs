using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class AlipayChargeBillSynchronizerDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public string Id { get; set; }

		/// <summary>
        /// 小区Id
        /// </summary>
		public int? ComDeptId { get; set; }

		/// <summary>
        /// 计数
        /// </summary>
		public int? CountNumber { get; set; }

		/// <summary>
        /// 批次号
        /// </summary>
		public string BatchCode { get; set; }

		/// <summary>
        /// 完成状态
        /// </summary>
		public bool? IsFinish { get; set; }

		/// <summary>
        /// 删除标志
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }
	 }
}
