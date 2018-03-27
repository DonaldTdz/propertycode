using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_GatewayAuthDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// EntityKey
        /// </summary>
		public string EntityKey { get; set; }

		/// <summary>
        /// 用户Token
        /// </summary>
		public string UserToken { get; set; }

		/// <summary>
        /// 网关Id
        /// </summary>
		public string WifiGatewayId { get; set; }

		/// <summary>
        /// 用户电话号码
        /// </summary>
		public string Phone { get; set; }

		/// <summary>
        /// IP地址
        /// </summary>
		public string IP { get; set; }

		/// <summary>
        /// Mac地址
        /// </summary>
		public string Mac { get; set; }

		/// <summary>
        /// 客户端类型
        /// </summary>
		public int? ClientType { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 最后一些轮询访问
        /// </summary>
		public DateTime? LastUpdateTime { get; set; }

		/// <summary>
        /// 过期时间
        /// </summary>
		public DateTime? ExpirTime { get; set; }
	 }
}
