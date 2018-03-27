using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class PrepayAccountDetailDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 期初金额
        /// </summary>
		public decimal? BeginningBalance { get; set; }

		/// <summary>
        /// 发生金额
        /// </summary>
		public decimal? ProductionAmount { get; set; }

		/// <summary>
        /// 期末金额
        /// </summary>
		public decimal? EndingBalance { get; set; }

		/// <summary>
        /// 操作人
        /// </summary>
		public int? Operator { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 删除标识
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 费用流水ID
        /// </summary>
		public string ChargeRecordId { get; set; }

		/// <summary>
        /// 描述
        /// </summary>
		public string Description { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 付款方式
        /// </summary>
		public int? PayTypeId { get; set; }

		/// <summary>
        /// 客户名称
        /// </summary>
		public string CustomerName { get; set; }
	 }
}
