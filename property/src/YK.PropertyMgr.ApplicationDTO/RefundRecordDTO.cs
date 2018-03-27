using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class RefundRecordDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 房屋ID（外键）
        /// </summary>
		public int? HouseDeptId { get; set; }

		/// <summary>
        /// 费用记录ID(外键)
        /// </summary>
		public string ChargeRecordId { get; set; }

		/// <summary>
        /// 退款记录引用的费用ID
        /// </summary>
		public string RefChargeRecordId { get; set; }

		/// <summary>
        /// 客户姓名
        /// </summary>
		public string Customer { get; set; }

		/// <summary>
        /// 票据ID
        /// </summary>
		public string ReceiptID { get; set; }

		/// <summary>
        /// 付款方式
        /// </summary>
		public int? PayType { get; set; }

		/// <summary>
        /// 退款原因
        /// </summary>
		public string Reason { get; set; }

		/// <summary>
        /// 退款时间
        /// </summary>
		public DateTime RefundTime { get; set; }

		/// <summary>
        /// 是否删除
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 操作人
        /// </summary>
		public int? Operator { get; set; }

		/// <summary>
        /// 操作人名
        /// </summary>
		public string OperatorName { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime UpdateTime { get; set; }
	 }
}
