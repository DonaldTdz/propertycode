using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class AlipayApiRequestLogDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public string Id { get; set; }

		/// <summary>
        /// 请求API名称
        /// </summary>
		public string ApiName { get; set; }

		/// <summary>
        /// 请求时间
        /// </summary>
		public DateTime? RequestTime { get; set; }

		/// <summary>
        /// 支付宝反馈代码
        /// </summary>
		public string Response_code { get; set; }

		/// <summary>
        /// 支付宝反馈描述
        /// </summary>
		public string Response_msg { get; set; }

		/// <summary>
        /// 支付宝反馈业务代码
        /// </summary>
		public string Response_sub_code { get; set; }

		/// <summary>
        /// 支付宝反馈业务描述
        /// </summary>
		public string Response_sub_msg { get; set; }

		/// <summary>
        /// 操作人
        /// </summary>
		public string OperatorName { get; set; }

		/// <summary>
        /// 操作人Id
        /// </summary>
		public string OperatorId { get; set; }
	 }
}
