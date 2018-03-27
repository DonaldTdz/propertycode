using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class MeterReadRecordDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 设备ID
        /// </summary>
		public int? MeterId { get; set; }

		/// <summary>
        /// 下一次抄表读数的ID
        /// </summary>
		public int? NextRefID { get; set; }

		/// <summary>
        /// 是否生成账单
        /// </summary>
		public bool? IsBill { get; set; }

		/// <summary>
        /// 账单ID
        /// </summary>
		public string BillID { get; set; }

		/// <summary>
        /// 仪表读数
        /// </summary>
		public decimal? MeterValue { get; set; }

		/// <summary>
        /// 抄表日期
        /// </summary>
		public DateTime? ReadDate { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }
	 }
}
