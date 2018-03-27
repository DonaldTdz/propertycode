using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class TicketSerialNumber: IAggregateRoot
	{

		/// <summary>
        /// Id
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 小区Id
        /// </summary>
		public int? CommunityId { get; set; }

		/// <summary>
        /// 流水号当前值
        /// </summary>
		public int? SerialValue { get; set; }

		/// <summary>
        /// 流水号完整
        /// </summary>
		public string CompleteSerialValue { get; set; }
	 }
	public partial class TicketSerialNumberMapper : EntityMapper<TicketSerialNumber>
    {
        public TicketSerialNumberMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.CommunityId).IsOptional();
			Property(s => s.SerialValue).IsOptional();
			Property(s => s.CompleteSerialValue).HasMaxLength(50).IsOptional();
        }
    }
}
