using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.ParkingSys.DomainEntity
{
		[Table("Parking", Schema = "dbo")]
		public partial class Parking: IAggregateRoot
	{
		/// <summary>
        /// Parking_id
        /// </summary>
		public string Parking_id { get; set; }
		/// <summary>
        /// Parking_name
        /// </summary>
		public string Parking_name { get; set; }
		/// <summary>
        /// Parking_lotnum
        /// </summary>
		public int? Parking_lotnum { get; set; }
		/// <summary>
        /// Parking_guid
        /// </summary>
		public string Parking_guid { get; set; }
		/// <summary>
        /// Updatetime
        /// </summary>
		public DateTime Updatetime { get; set; }
		/// <summary>
        /// Updatestatu
        /// </summary>
		public int? Updatestatu { get; set; }
		/// <summary>
        /// Parking_tempnum
        /// </summary>
		public int? Parking_tempnum { get; set; }
		/// <summary>
        /// Parking_tempsurplus
        /// </summary>
		public int? Parking_tempsurplus { get; set; }
		/// <summary>
        /// Community_code
        /// </summary>
		public string Community_code { get; set; }
		/// <summary>
        /// Dept_id
        /// </summary>
		public int? Dept_id { get; set; }
		/// <summary>
        /// UpdateUser
        /// </summary>
		public string UpdateUser { get; set; }
		/// <summary>
        /// Parking_parent
        /// </summary>
		public string Parking_parent { get; set; }
		/// <summary>
        /// Parking_ischarge
        /// </summary>
		public bool Parking_ischarge { get; set; }
		/// <summary>
        /// Parking_address
        /// </summary>
		public string Parking_address { get; set; }
		/// <summary>
        /// Remark
        /// </summary>
		public string Remark { get; set; }
	 }
	public partial class ParkingMapper : EntityMapper<Parking>
    {
        public ParkingMapper()
        {
						HasKey(s => s.Parking_id);
			            
			Property(s => s.Parking_name).HasMaxLength(255).IsOptional();
			Property(s => s.Parking_lotnum).IsOptional();
			Property(s => s.Parking_guid).HasMaxLength(255).IsOptional();
			Property(s => s.Updatetime).IsOptional();
			Property(s => s.Updatestatu).IsOptional();
			Property(s => s.Parking_tempnum).IsOptional();
			Property(s => s.Parking_tempsurplus).IsOptional();
			Property(s => s.Community_code).HasMaxLength(100).IsOptional();
			Property(s => s.Dept_id).IsOptional();
			Property(s => s.UpdateUser).HasMaxLength(100).IsOptional();
			Property(s => s.Parking_parent).HasMaxLength(255).IsOptional();
			Property(s => s.Parking_ischarge).IsOptional();
			Property(s => s.Parking_address).HasMaxLength(200).IsOptional();
			Property(s => s.Remark).HasMaxLength(2000).IsOptional();
        }
    }
}
