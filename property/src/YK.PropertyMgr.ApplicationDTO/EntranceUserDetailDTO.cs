using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class EntranceUserDetailDTO
	{

		/// <summary>
        /// Id
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// EntranceUser_Id
        /// </summary>
		public int? EntranceUser_Id { get; set; }

		/// <summary>
        /// KeyID
        /// </summary>
		public int? KeyID { get; set; }

		/// <summary>
        /// UserOwnerInfoId
        /// </summary>
		public string UserOwnerInfoId { get; set; }

		/// <summary>
        /// OperateName
        /// </summary>
		public string OperateName { get; set; }

		/// <summary>
        /// KeyExpireTime
        /// </summary>
		public DateTime KeyExpireTime { get; set; }

		/// <summary>
        /// EmpowerTime
        /// </summary>
		public DateTime EmpowerTime { get; set; }

		/// <summary>
        /// CreateTime
        /// </summary>
		public DateTime CreateTime { get; set; }
	 }
}
