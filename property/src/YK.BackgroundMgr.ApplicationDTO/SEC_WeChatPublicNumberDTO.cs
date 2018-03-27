using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_WeChatPublicNumberDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 公众号名称
        /// </summary>
		public string Name { get; set; }

		/// <summary>
        /// AppId
        /// </summary>
		public string AppId { get; set; }

		/// <summary>
        /// AppSecret
        /// </summary>
		public string AppSecret { get; set; }

		/// <summary>
        /// AccessToken
        /// </summary>
		public string AccessToken { get; set; }

		/// <summary>
        /// TokenTime
        /// </summary>
		public string TokenTime { get; set; }

		/// <summary>
        /// 公共号Id
        /// </summary>
		public string PublicNumberId { get; set; }

		/// <summary>
        /// 是否属于多公众号
        /// </summary>
		public int? IsPublic { get; set; }

		/// <summary>
        /// PropertyIds
        /// </summary>
		public string PropertyIds { get; set; }

		/// <summary>
        /// PropertyNames
        /// </summary>
		public string PropertyNames { get; set; }

		/// <summary>
        /// MchId
        /// </summary>
		public string MchId { get; set; }

		/// <summary>
        /// ApiKey
        /// </summary>
		public string ApiKey { get; set; }
	 }
}
