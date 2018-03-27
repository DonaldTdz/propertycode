using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class SubjectHouseRefDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 房屋HouseDeptId
        /// </summary>
		public int? HouseDeptId { get; set; }

		/// <summary>
        /// 资源Id
        /// </summary>
		public int? ResourcesId { get; set; }

		/// <summary>
        /// 开发商代缴
        /// </summary>
		public bool? IsDevPay { get; set; }

		/// <summary>
        /// 开发商代缴开始时间
        /// </summary>
		public DateTime? DevBeginDate { get; set; }

		/// <summary>
        /// 开发商代缴结束时间
        /// </summary>
		public DateTime? DevEndDate { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 更新时间
        /// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
        /// 删除标志
        /// </summary>
		public bool? IsDel { get; set; }

		/// <summary>
        /// 操作人
        /// </summary>
		public int? Operator { get; set; }

		/// <summary>
        /// 操作人解绑
        /// </summary>
		public int? RelieveOperator { get; set; }

		/// <summary>
        /// 项目类型
        /// </summary>
		public int? SubjectType { get; set; }

		/// <summary>
        /// 资源名称
        /// </summary>
		public string ResourceName { get; set; }

		/// <summary>
        /// 计费开始日
        /// </summary>
		public DateTime? BeginDateBill { get; set; }
	 }
}
