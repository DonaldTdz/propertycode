using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class PrepayAccountLogDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 小区Id
        /// </summary>
		public int? ComDeptId { get; set; }

		/// <summary>
        /// 房间Id
        /// </summary>
		public int? HouseDeptId { get; set; }

		/// <summary>
        /// 资源Id
        /// </summary>
		public int? ResourcesId { get; set; }

		/// <summary>
        /// 预存账户Id
        /// </summary>
		public int? PrepayAccountId { get; set; }

		/// <summary>
        /// 内容
        /// </summary>
		public string Desc { get; set; }

		/// <summary>
        /// 操作人
        /// </summary>
		public string Operator { get; set; }

		/// <summary>
        /// 操作人Id
        /// </summary>
		public int? OperatorId { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 操作时间
        /// </summary>
		public DateTime? OperationTime { get; set; }
	 }
}
