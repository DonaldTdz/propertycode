using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class ChargeRecordDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public string Id { get; set; }

		/// <summary>
        /// 流水号
        /// </summary>
		public string SerialNumber { get; set; }

		/// <summary>
        /// 描述
        /// </summary>
		public string Description { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 发生金额
        /// </summary>
		public decimal? Amount { get; set; }

		/// <summary>
        /// 优惠金额
        /// </summary>
		public decimal? DiscountAmount { get; set; }

		/// <summary>
        /// 收费类型
        /// </summary>
		public int? ChargeType { get; set; }

		/// <summary>
        /// 付款日期
        /// </summary>
		public DateTime? PayDate { get; set; }

		/// <summary>
        /// 操作人
        /// </summary>
		public int? Operator { get; set; }

		/// <summary>
        /// 小区Id
        /// </summary>
		public int? ComDeptId { get; set; }

		/// <summary>
        /// 支付方式
        /// </summary>
		public int? PayMthodId { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 删除标志   
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 状态
        /// </summary>
		public int? Status { get; set; }

		/// <summary>
        /// 操作人名
        /// </summary>
		public string OperatorName { get; set; }

		/// <summary>
        /// 房屋Id
        /// </summary>
		public int? HouseDeptId { get; set; }

		/// <summary>
        /// 房屋编号
        /// </summary>
		public string HouseDeptNos { get; set; }

		/// <summary>
        /// 资源名称
        /// </summary>
		public string ResourcesNames { get; set; }

		/// <summary>
        /// 客户Id
        /// </summary>
		public string CustomerId { get; set; }

		/// <summary>
        /// 客户名称
        /// </summary>
		public string CustomerName { get; set; }

		/// <summary>
        /// 结算状态
        /// </summary>
		public int? AccountingStatus { get; set; }

		/// <summary>
        /// 是否是线上支付
        /// </summary>
		public bool? IsOnline { get; set; }
	 }
}
