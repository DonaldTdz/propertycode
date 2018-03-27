using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class EntranceUserDetail: IAggregateRoot
	{

		/// <summary>
        /// Id
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// EntranceUser_Id
        /// </summary>
		public int? EntranceUser_Id { get; set; }

		/// <summary>
        /// KeyID
        /// </summary>
		public int? KeyID { get; set; }

		/// <summary>
        /// UserOwnerInfoId
        /// </summary>
		public string UserOwnerInfoId { get; set; }

		/// <summary>
        /// OperateName
        /// </summary>
		public string OperateName { get; set; }

		/// <summary>
        /// KeyExpireTime
        /// </summary>
		public DateTime KeyExpireTime { get; set; }

		/// <summary>
        /// EmpowerTime
        /// </summary>
		public DateTime EmpowerTime { get; set; }

		/// <summary>
        /// CreateTime
        /// </summary>
		public DateTime CreateTime { get; set; }
	 }
	public partial class EntranceUserDetailMapper : EntityMapper<EntranceUserDetail>
    {
        public EntranceUserDetailMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.EntranceUser_Id).IsOptional();
			Property(s => s.KeyID).IsOptional();
			Property(s => s.UserOwnerInfoId).HasMaxLength(36).IsOptional();
			Property(s => s.OperateName).HasMaxLength(50).IsOptional();
			Property(s => s.KeyExpireTime).IsOptional();
			Property(s => s.EmpowerTime).IsOptional();
			Property(s => s.CreateTime).IsOptional();
        }
    }
}
