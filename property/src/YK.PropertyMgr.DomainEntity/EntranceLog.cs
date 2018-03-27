using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class EntranceLog: IAggregateRoot
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
        /// UserOwnerInfoId
        /// </summary>
		public string UserOwnerInfoId { get; set; }

		/// <summary>
        /// KeyID
        /// </summary>
		public int? KeyID { get; set; }

		/// <summary>
        /// OPenTime
        /// </summary>
		public DateTime OPenTime { get; set; }

		/// <summary>
        /// OpenState
        /// </summary>
		public int? OpenState { get; set; }

		/// <summary>
        /// CreateTime
        /// </summary>
		public DateTime CreateTime { get; set; }
	 }
	public partial class EntranceLogMapper : EntityMapper<EntranceLog>
    {
        public EntranceLogMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.VillageID).IsOptional();
			Property(s => s.UserOwnerInfoId).HasMaxLength(36).IsOptional();
			Property(s => s.KeyID).IsOptional();
			Property(s => s.OPenTime).IsOptional();
			Property(s => s.OpenState).IsOptional();
			Property(s => s.CreateTime).IsOptional();
        }
    }
}
