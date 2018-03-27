using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class EntranceDTO
	{

		/// <summary>
        /// Id
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// VillageID
        /// </summary>
		public int? VillageID { get; set; }

		/// <summary>
        /// ProvinceID
        /// </summary>
		public int? ProvinceID { get; set; }

		/// <summary>
        /// CityID
        /// </summary>
		public int? CityID { get; set; }

		/// <summary>
        /// CountyID
        /// </summary>
		public int? CountyID { get; set; }

		/// <summary>
        /// Name
        /// </summary>
		public string Name { get; set; }

		/// <summary>
        /// Address
        /// </summary>
		public string Address { get; set; }

		/// <summary>
        /// KeyID
        /// </summary>
		public int? KeyID { get; set; }

		/// <summary>
        /// State
        /// </summary>
		public int? State { get; set; }

		/// <summary>
        /// CreateTime
        /// </summary>
		public DateTime CreateTime { get; set; }

		/// <summary>
        /// UnitName
        /// </summary>
		public string UnitName { get; set; }

		/// <summary>
        /// BuildId
        /// </summary>
		public int? BuildId { get; set; }

		/// <summary>
        /// DoorId
        /// </summary>
		public int? DoorId { get; set; }

		/// <summary>
        /// DoorName
        /// </summary>
		public string DoorName { get; set; }

		/// <summary>
        /// State
        /// </summary>
		public int? BindSockState { get; set; }

		/// <summary>
        /// 小区ID(通用)
        /// </summary>
		public int? CommunityDeptId { get; set; }

		/// <summary>
        /// 门禁卡自增Id
        /// </summary>
		public int? IncrementId { get; set; }

		/// <summary>
        /// 门禁卡分组Id
        /// </summary>
		public int? PwdGroup { get; set; }

		/// <summary>
        /// 设备ID
        /// </summary>
		public string DeviceId { get; set; }
	 }
}
