using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class TicketSerialNumberDTO
	{

		/// <summary>
        /// Id
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 小区Id
        /// </summary>
		public int? CommunityId { get; set; }

		/// <summary>
        /// 流水号当前值
        /// </summary>
		public int? SerialValue { get; set; }

		/// <summary>
        /// 流水号完整
        /// </summary>
		public string CompleteSerialValue { get; set; }
	 }
}
