using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class EntranceUserDTO
	{

		/// <summary>
        /// Id
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// UserOwnerInfoId
        /// </summary>
		public string UserOwnerInfoId { get; set; }

		/// <summary>
        /// KeyID
        /// </summary>
		public int? KeyID { get; set; }

		/// <summary>
        /// KeyExpireTime
        /// </summary>
		public DateTime KeyExpireTime { get; set; }

		/// <summary>
        /// CreateTime
        /// </summary>
		public DateTime CreateTime { get; set; }

		/// <summary>
        /// EntranceID
        /// </summary>
		public int? EntranceID { get; set; }
	 }
}
