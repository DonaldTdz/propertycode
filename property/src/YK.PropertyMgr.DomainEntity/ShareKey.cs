using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class ShareKey: IAggregateRoot
	{

		/// <summary>
        /// Id
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// UserId
        /// </summary>
		public string UserId { get; set; }

		/// <summary>
        /// Keys
        /// </summary>
		public string Keys { get; set; }

		/// <summary>
        /// SetNums
        /// </summary>
		public int? SetNums { get; set; }

		/// <summary>
        /// UseNums
        /// </summary>
		public int? UseNums { get; set; }

		/// <summary>
        /// KeyDate
        /// </summary>
		public DateTime KeyDate { get; set; }

		/// <summary>
        /// UpdateTime
        /// </summary>
		public DateTime UpdateTime { get; set; }

		/// <summary>
        /// CreateTime
        /// </summary>
		public DateTime CreateTime { get; set; }
	 }
	public partial class ShareKeyMapper : EntityMapper<ShareKey>
    {
        public ShareKeyMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.UserId).HasMaxLength(40).IsOptional();
			Property(s => s.Keys).HasMaxLength(1073741823).IsOptional();
			Property(s => s.SetNums).IsOptional();
			Property(s => s.UseNums).IsOptional();
			Property(s => s.KeyDate).IsOptional();
			Property(s => s.UpdateTime).IsOptional();
			Property(s => s.CreateTime).IsOptional();
        }
    }
}
