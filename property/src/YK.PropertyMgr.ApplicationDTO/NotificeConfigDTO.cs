using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class NotificeConfigDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 小区Id
        /// </summary>
		public int? ComDeptId { get; set; }

		/// <summary>
        /// 小区名称
        /// </summary>
		public string ComDeptName { get; set; }

		/// <summary>
        /// 禁用、启用
        /// </summary>
		public int? IsEnable { get; set; }

		/// <summary>
        /// APP通知
        /// </summary>
		public bool? APPNotice { get; set; }

		/// <summary>
        /// 短信通知
        /// </summary>
		public bool? SMSNotice { get; set; }

		/// <summary>
        /// 欠费金额
        /// </summary>
		public decimal? ArrearsAmount { get; set; }

		/// <summary>
        /// 欠费月数
        /// </summary>
		public int? ArrearsMonth { get; set; }

		/// <summary>
        /// 通知频次类型
        /// </summary>
		public int? FrequencyType { get; set; }

		/// <summary>
        /// 通知周期（每周、每月）
        /// </summary>
		public int? NoticeDay { get; set; }

		/// <summary>
        /// 通知时间（格式为HH:mm）
        /// </summary>
		public int? NoticeTime { get; set; }

		/// <summary>
        /// 通知内容
        /// </summary>
		public string Content { get; set; }

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
        /// 操作者姓名
        /// </summary>
		public string OperatorName { get; set; }
	 }
}
