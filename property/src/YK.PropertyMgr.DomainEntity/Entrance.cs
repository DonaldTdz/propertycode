using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class Entrance: IAggregateRoot
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
		
      public virtual City City { get; set; }
      public virtual County County { get; set; }
      public virtual Province Province { get; set; }
    
	 }
	public partial class EntranceMapper : EntityMapper<Entrance>
    {
        public EntranceMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.VillageID).IsOptional();
			Property(s => s.ProvinceID).IsOptional();
			Property(s => s.CityID).IsOptional();
			Property(s => s.CountyID).IsOptional();
			Property(s => s.Name).HasMaxLength(250).IsOptional();
			Property(s => s.Address).HasMaxLength(250).IsOptional();
			Property(s => s.KeyID).IsOptional();
			Property(s => s.State).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.UnitName).HasMaxLength(250).IsOptional();
			Property(s => s.BuildId).IsOptional();
			Property(s => s.DoorId).IsOptional();
			Property(s => s.DoorName).HasMaxLength(300).IsOptional();
			Property(s => s.BindSockState).IsOptional();
			Property(s => s.CommunityDeptId).IsOptional();
			Property(s => s.IncrementId).IsOptional();
			Property(s => s.PwdGroup).IsOptional();
			Property(s => s.DeviceId).HasMaxLength(30).IsOptional();
        }
    }
}
