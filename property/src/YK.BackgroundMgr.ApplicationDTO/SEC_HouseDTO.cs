using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_HouseDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 栋
        /// </summary>
		public string Dong { get; set; }

		/// <summary>
        /// 单元
        /// </summary>
		public string Danyuan { get; set; }

		/// <summary>
        /// 楼高
        /// </summary>
		public int? FloorHeight { get; set; }

		/// <summary>
        /// 门牌号
        /// </summary>
		public string DoorNo { get; set; }

		/// <summary>
        /// 房屋编号
        /// </summary>
		public string HouseNum { get; set; }

		/// <summary>
        /// 房屋名称
        /// </summary>
		public string HouseName { get; set; }

		/// <summary>
        /// 楼层编号
        /// </summary>
		public string FloorNum { get; set; }

		/// <summary>
        /// 房屋朝向
        /// </summary>
		public string HouseDirecation { get; set; }

		/// <summary>
        /// 房屋户型
        /// </summary>
		public string HouseType { get; set; }

		/// <summary>
        /// 房屋状态
        /// </summary>
		public int? HouseState_PM { get; set; }

		/// <summary>
        /// 房屋装修状态
        /// </summary>
		public int? DecorationState_PM { get; set; }

		/// <summary>
        /// 建筑结构
        /// </summary>
		public string BuildFormation { get; set; }

		/// <summary>
        /// 建筑面积
        /// </summary>
		public double? BuildArea { get; set; }

		/// <summary>
        /// 套内面积
        /// </summary>
		public double? HouseInArea { get; set; }

		/// <summary>
        /// 公摊面积
        /// </summary>
		public double? PublicArea { get; set; }

		/// <summary>
        /// 花园面积
        /// </summary>
		public double? GardenArea { get; set; }

		/// <summary>
        /// 产权性质
        /// </summary>
		public string ChanQuanQuale { get; set; }

		/// <summary>
        /// 使用状态
        /// </summary>
		public string UserState { get; set; }

		/// <summary>
        /// 房屋配置情况
        /// </summary>
		public string Deploy { get; set; }

		/// <summary>
        /// 装修情况
        /// </summary>
		public string Fitment { get; set; }

		/// <summary>
        /// 备注
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 组织架构Id
        /// </summary>
		public Guid OrgId { get; set; }
	 }
}
