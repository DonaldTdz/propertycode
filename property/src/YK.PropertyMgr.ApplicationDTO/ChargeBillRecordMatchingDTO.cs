using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class ChargeBillRecordMatchingDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 账单ID(外键)
        /// </summary>
		public string ChargeBillId { get; set; }

		/// <summary>
        /// 交易金额
        /// </summary>
		public decimal? Amount { get; set; }

		/// <summary>
        /// 房屋Id
        /// </summary>
		public int? HouseDeptId { get; set; }

		/// <summary>
        /// 资源Id
        /// </summary>
		public int? ResourcesId { get; set; }
	 }
}
