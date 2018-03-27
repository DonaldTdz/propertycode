using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class AlipayChargeBillSynchronizerDetailDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 同步表Id
        /// </summary>
		public string AlipayChargeBillSynchronizerId { get; set; }

		/// <summary>
        /// 物业账单Id
        /// </summary>
		public string ChargeBillId { get; set; }

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
