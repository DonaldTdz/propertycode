using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class ShareKeyDTO
	{

		/// <summary>
        /// Id
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// UserId
        /// </summary>
		public string UserId { get; set; }

		/// <summary>
        /// Keys
        /// </summary>
		public string Keys { get; set; }

		/// <summary>
        /// SetNums
        /// </summary>
		public int? SetNums { get; set; }

		/// <summary>
        /// UseNums
        /// </summary>
		public int? UseNums { get; set; }

		/// <summary>
        /// KeyDate
        /// </summary>
		public DateTime KeyDate { get; set; }

		/// <summary>
        /// UpdateTime
        /// </summary>
		public DateTime UpdateTime { get; set; }

		/// <summary>
        /// CreateTime
        /// </summary>
		public DateTime CreateTime { get; set; }
	 }
}
