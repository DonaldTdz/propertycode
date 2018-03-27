using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class ChargeSubjectDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 项目编号
        /// </summary>
		public string Code { get; set; }

		/// <summary>
        /// 项目名称
        /// </summary>
		public string Name { get; set; }

		/// <summary>
        /// 收费周期
        /// </summary>
		public int? BillPeriod { get; set; }

		/// <summary>
        /// 单价
        /// </summary>
		public decimal? Price { get; set; }

		/// <summary>
        /// 计费公式
        /// </summary>
		public string ChargeFormula { get; set; }

		/// <summary>
        /// 计费公式中文显示
        /// </summary>
		public string ChargeFormulaShow { get; set; }

		/// <summary>
        /// 滞纳金率
        /// </summary>
		public decimal? PenaltyRate { get; set; }

		/// <summary>
        /// 账单日
        /// </summary>
		public int? BillDay { get; set; }

		/// <summary>
        /// 线上支付
        /// </summary>
		public bool? IsOnline { get; set; }

		/// <summary>
        /// 是否启用
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 小区Id
        /// </summary>
		public int? ComDeptId { get; set; }

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
        /// 项目类型
        /// </summary>
		public int? SubjectType { get; set; }

		/// <summary>
        /// 计费开始日
        /// </summary>
		public DateTime? BeginDate { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 自动生成账单
        /// </summary>
		public int? AutomaticBill { get; set; }
	 }
}
