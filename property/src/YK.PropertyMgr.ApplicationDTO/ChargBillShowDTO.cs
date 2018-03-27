using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class ChargBillShowDTO
	{

		/// <summary>
        /// 账单Id
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 收费项目
        /// </summary>
		public string SubjectName { get; set; }

		/// <summary>
        /// 计费金额
        /// </summary>
		public decimal? Amount { get; set; }

		/// <summary>
        /// 已交金额
        /// </summary>
		public decimal? AmountPaid { get; set; }

		/// <summary>
        /// 应收金额
        /// </summary>
		public decimal? AmountReceivable { get; set; }

		/// <summary>
        /// 开始日期
        /// </summary>
		public DateTime? BeginDate { get; set; }

		/// <summary>
        /// 结束日期
        /// </summary>
		public DateTime? EndDate { get; set; }

		/// <summary>
        /// 减免金额
        /// </summary>
		public decimal? AmountReduce { get; set; }
	 }
}
