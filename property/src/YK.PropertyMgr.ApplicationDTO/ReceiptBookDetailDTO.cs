﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class ReceiptBookDetailDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 票据号
        /// </summary>
		public string Number { get; set; }

		/// <summary>
        /// 票据Id
        /// </summary>
		public string ReceiptId { get; set; }

		/// <summary>
        /// 删除标志
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 操作人名
        /// </summary>
		public string OperatorName { get; set; }

		/// <summary>
        /// 票据本Id
        /// </summary>
		public int? ReceiptBookId { get; set; }

		/// <summary>
        /// 票据金额
        /// </summary>
		public decimal? ReceiptAmount { get; set; }
	 }
}
