using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.BackgroundMgr.DomainEntity
{
		public partial class SEC_Community: IAggregateRoot
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
	public partial class SEC_CommunityMapper : EntityMapper<SEC_Community>
    {
        public SEC_CommunityMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.Name).HasMaxLength(200).IsRequired();
			Property(s => s.Code).HasMaxLength(100).IsOptional();
			Property(s => s.Developers).HasMaxLength(200).IsOptional();
			Property(s => s.BindingPhone).HasMaxLength(20).IsOptional();
			Property(s => s.Address).HasMaxLength(300).IsOptional();
			Property(s => s.EngineerUnit).HasMaxLength(300).IsOptional();
			Property(s => s.Province).HasMaxLength(20).IsOptional();
			Property(s => s.City).HasMaxLength(20).IsOptional();
			Property(s => s.County).HasMaxLength(10).IsOptional();
			Property(s => s.DongCount).IsOptional();
			Property(s => s.Mj_all).IsOptional();
			Property(s => s.Mj_jz).IsOptional();
			Property(s => s.Mj_use).IsOptional();
			Property(s => s.GreenRange).IsOptional();
			Property(s => s.VolumeRange).IsOptional();
			Property(s => s.WuguanUse).IsOptional();
			Property(s => s.ParkCount).IsOptional();
			Property(s => s.RoomCount).IsOptional();
			Property(s => s.PersonCount).IsOptional();
			Property(s => s.StartDate).IsOptional();
			Property(s => s.EndDate).IsOptional();
			Property(s => s.WayArea).IsOptional();
			Property(s => s.HightBuildCount).IsOptional();
			Property(s => s.CarportArea).IsOptional();
			Property(s => s.PublicArea).IsOptional();
			Property(s => s.MultilayerCount).IsOptional();
			Property(s => s.GreenArea).IsOptional();
			Property(s => s.ContactName).HasMaxLength(50).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.Remark).HasMaxLength(4000).IsOptional();
			Property(s => s.DeptId).IsOptional();
			Property(s => s.Long).IsRequired();
			Property(s => s.Lat).IsRequired();
			Property(s => s.WeChatCodeUrl).HasMaxLength(2000).IsOptional();
			Property(s => s.UnsoldCharge).IsOptional();
        }
    }
}
