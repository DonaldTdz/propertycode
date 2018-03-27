using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.BackgroundMgr.ApplicationDTO
{
	[Serializable]
	public partial class SEC_CommunityDTO
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 小区名称
        /// </summary>
		public string Name { get; set; }

		/// <summary>
        /// 小区编号
        /// </summary>
		public string Code { get; set; }

		/// <summary>
        /// 开发商
        /// </summary>
		public string Developers { get; set; }

		/// <summary>
        /// 绑定电话号码
        /// </summary>
		public string BindingPhone { get; set; }

		/// <summary>
        /// 地理位置
        /// </summary>
		public string Address { get; set; }

		/// <summary>
        /// 施工单位
        /// </summary>
		public string EngineerUnit { get; set; }

		/// <summary>
        /// 省
        /// </summary>
		public string Province { get; set; }

		/// <summary>
        /// 市
        /// </summary>
		public string City { get; set; }

		/// <summary>
        /// 县
        /// </summary>
		public string County { get; set; }

		/// <summary>
        /// 楼栋个数
        /// </summary>
		public int? DongCount { get; set; }

		/// <summary>
        /// 占地面积
        /// </summary>
		public double? Mj_all { get; set; }

		/// <summary>
        /// 建筑面积
        /// </summary>
		public double? Mj_jz { get; set; }

		/// <summary>
        /// 使用面积
        /// </summary>
		public double? Mj_use { get; set; }

		/// <summary>
        /// 绿化率
        /// </summary>
		public double? GreenRange { get; set; }

		/// <summary>
        /// 容积率
        /// </summary>
		public double? VolumeRange { get; set; }

		/// <summary>
        /// 物管用房面积
        /// </summary>
		public double? WuguanUse { get; set; }

		/// <summary>
        /// 停车位个数
        /// </summary>
		public int? ParkCount { get; set; }

		/// <summary>
        /// 住房总套数
        /// </summary>
		public int? RoomCount { get; set; }

		/// <summary>
        /// 总人口数
        /// </summary>
		public int? PersonCount { get; set; }

		/// <summary>
        /// 开工日期
        /// </summary>
		public DateTime? StartDate { get; set; }

		/// <summary>
        /// 完工日期
        /// </summary>
		public DateTime? EndDate { get; set; }

		/// <summary>
        /// 道路面积
        /// </summary>
		public double? WayArea { get; set; }

		/// <summary>
        /// 高层楼宇数量
        /// </summary>
		public int? HightBuildCount { get; set; }

		/// <summary>
        /// 车库面积
        /// </summary>
		public double? CarportArea { get; set; }

		/// <summary>
        /// 公共场所面积
        /// </summary>
		public int? PublicArea { get; set; }

		/// <summary>
        /// 多层楼宇数量
        /// </summary>
		public int? MultilayerCount { get; set; }

		/// <summary>
        /// 绿化面积
        /// </summary>
		public double? GreenArea { get; set; }

		/// <summary>
        /// 联系人
        /// </summary>
		public string ContactName { get; set; }

		/// <summary>
        /// 创建时间
        /// </summary>
		public DateTime? CreateTime { get; set; }

		/// <summary>
        /// 描述信息
        /// </summary>
		public string Remark { get; set; }

		/// <summary>
        /// 组织架构Id
        /// </summary>
		public int? DeptId { get; set; }

		/// <summary>
        /// 经度
        /// </summary>
		public double? Long { get; set; }

		/// <summary>
        /// 维度
        /// </summary>
		public double? Lat { get; set; }

		/// <summary>
        /// 渠道码
        /// </summary>
		public string WeChatCodeUrl { get; set; }

		/// <summary>
        /// 未售房开发商收费
        /// </summary>
		public int? UnsoldCharge { get; set; }
	 }
}
