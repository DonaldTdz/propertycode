using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace YK.ParkingSys.DomainEntity
{
		[Table("Carport", Schema = "dbo")]
		public partial class Carport: IAggregateRoot
	{
		/// <summary>
        /// Id
        /// </summary>
		public int? Id { get; set; }
		/// <summary>
        /// OrgId
        /// </summary>
		public string OrgId { get; set; }
		/// <summary>
        /// Parking_id
        /// </summary>
		public string Parking_id { get; set; }
		/// <summary>
        /// CarportNum
        /// </summary>
		public string CarportNum { get; set; }
		/// <summary>
        /// CarportType
        /// </summary>
		public int? CarportType { get; set; }
		/// <summary>
        /// Area
        /// </summary>
		public double? Area { get; set; }
		/// <summary>
        /// CarportState
        /// </summary>
		public int? CarportState { get; set; }
		/// <summary>
        /// CarNum
        /// </summary>
		public string CarNum { get; set; }
		/// <summary>
        /// CardNum
        /// </summary>
		public string CardNum { get; set; }
		/// <summary>
        /// HouseDeptID
        /// </summary>
		public int? HouseDeptID { get; set; }
		/// <summary>
        /// Contact
        /// </summary>
		public string Contact { get; set; }
		/// <summary>
        /// Cellphone
        /// </summary>
		public string Cellphone { get; set; }
		/// <summary>
        /// Remark
        /// </summary>
		public string Remark { get; set; }
	 }
	public partial class CarportMapper : EntityMapper<Carport>
    {
        public CarportMapper()
        {
						HasKey(s => s.Id);
			            
			Property(s => s.OrgId).HasMaxLength(36).IsOptional();
			Property(s => s.Parking_id).HasMaxLength(36).IsOptional();
			Property(s => s.CarportNum).HasMaxLength(50).IsOptional();
			Property(s => s.CarportType).IsOptional();
			Property(s => s.Area).IsOptional();
			Property(s => s.CarportState).IsOptional();
			Property(s => s.CarNum).HasMaxLength(100).IsOptional();
			Property(s => s.CardNum).HasMaxLength(100).IsOptional();
			Property(s => s.HouseDeptID).IsOptional();
			Property(s => s.Contact).HasMaxLength(100).IsOptional();
			Property(s => s.Cellphone).HasMaxLength(20).IsOptional();
			Property(s => s.Remark).HasMaxLength(2000).IsOptional();
        }
    }
}
