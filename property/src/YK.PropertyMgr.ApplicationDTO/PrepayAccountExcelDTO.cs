using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class PrepayAccountExcelDTO
	{

		/// <summary>
        /// 房屋编号
        /// </summary>
		public string DoorNumber { get; set; }

		/// <summary>
        /// 客户姓名
        /// </summary>
		public string CustomerName { get; set; }

		/// <summary>
        /// 收费项目
        /// </summary>
		public string ChargeSubjectName { get; set; }

		/// <summary>
        /// 余额
        /// </summary>
		public decimal? Balance { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }
	 }
}
