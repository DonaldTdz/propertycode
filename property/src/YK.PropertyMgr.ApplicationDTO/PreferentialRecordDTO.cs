using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class PreferentialRecordDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 收费账单Id   
        /// </summary>
		public string ChargBillId { get; set; }

		/// <summary>
        /// 优惠金额
        /// </summary>
		public decimal? Amount { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 操作人
        /// </summary>
		public int? Operator { get; set; }

		/// <summary>
        /// 优惠类型
        /// </summary>
		public int? PreferentialType { get; set; }

		/// <summary>
        /// 删除标识
        /// </summary>
		public bool? IsDel { get; set; }
	 }
}
