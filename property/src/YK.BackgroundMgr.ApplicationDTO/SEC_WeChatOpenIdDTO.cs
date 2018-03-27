using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_WeChatOpenIdDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// OpenId
        /// </summary>
		public string OpenId { get; set; }

		/// <summary>
        /// WeChatName
        /// </summary>
		public string WeChatName { get; set; }

		/// <summary>
        /// WeChatImg
        /// </summary>
		public string WeChatImg { get; set; }

		/// <summary>
        /// WeChatPublicNumberId
        /// </summary>
		public int? WeChatPublicNumberId { get; set; }

		/// <summary>
        /// 渠道码
        /// </summary>
		public string ChannelCode { get; set; }

		/// <summary>
        /// 关注时间
        /// </summary>
		public DateTime? SubscribeTime { get; set; }
	 }
}
