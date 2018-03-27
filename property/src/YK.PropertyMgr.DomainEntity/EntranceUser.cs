using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class EntranceUser: IAggregateRoot
	{

		/// <summary>
        /// Id
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// UserOwnerInfoId
        /// </summary>
		public string UserOwnerInfoId { get; set; }

		/// <summary>
        /// KeyID
        /// </summary>
		public int? KeyID { get; set; }

		/// <summary>
        /// KeyExpireTime
        /// </summary>
		public DateTime KeyExpireTime { get; set; }

		/// <summary>
        /// CreateTime
        /// </summary>
		public DateTime CreateTime { get; set; }

		/// <summary>
        /// EntranceID
        /// </summary>
		public int? EntranceID { get; set; }
	 }
	public partial class EntranceUserMapper : EntityMapper<EntranceUser>
    {
        public EntranceUserMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.UserOwnerInfoId).HasMaxLength(36).IsOptional();
			Property(s => s.KeyID).IsOptional();
			Property(s => s.KeyExpireTime).IsOptional();
			Property(s => s.CreateTime).IsOptional();
			Property(s => s.EntranceID).IsOptional();
        }
    }
}
