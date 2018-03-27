using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class ChargBillDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public string Id { get; set; }

		/// <summary>
        /// 描述
        /// </summary>
		public string Description { get; set; }

		/// <summary>
        /// 计算表达式
        /// </summary>
		public string Quantity { get; set; }

		/// <summary>
        /// 账单开始日期
        /// </summary>
		public DateTime? BeginDate { get; set; }

		/// <summary>
        /// 账单结束日期
        /// </summary>
		public DateTime? EndDate { get; set; }

		/// <summary>
        /// 房间Id
        /// </summary>
		public int? HouseDeptId { get; set; }

		/// <summary>
        /// 资源Id
        /// </summary>
		public int? ResourcesId { get; set; }

		/// <summary>
        /// 资源名称
        /// </summary>
		public string ResourcesName { get; set; }

		/// <summary>
        /// 房屋编号
        /// </summary>
		public string HouseDoorNo { get; set; }

		/// <summary>
        /// 关联类型
        /// </summary>
		public int? RefType { get; set; }

		/// <summary>
        /// 账单金额
        /// </summary>
		public decimal? BillAmount { get; set; }

		/// <summary>
        /// 已收金额
        /// </summary>
		public decimal? ReceivedAmount { get; set; }

		/// <summary>
        /// 减免金额
        /// </summary>
		public decimal? ReliefAmount { get; set; }

		/// <summary>
        /// 滞纳金额
        /// </summary>
		public decimal? PenaltyAmount { get; set; }

		/// <summary>
        /// 小区Id
        /// </summary>
		public int? ComDeptId { get; set; }

		/// <summary>
        /// 开发商代缴
        /// </summary>
		public bool? IsDevPay { get; set; }

		/// <summary>
        ///  状态
        /// </summary>
		public int? Status { get; set; }

		/// <summary>
        /// 删除标志
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 生成时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

	 }
}
