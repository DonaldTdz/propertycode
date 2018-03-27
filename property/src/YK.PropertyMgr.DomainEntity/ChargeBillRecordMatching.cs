using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class ChargeBillRecordMatching: IAggregateRoot
	{

		/// <summary>
        /// 主键
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// 账单ID(外键)
        /// </summary>
		public string ChargeBillId { get; set; }

		/// <summary>
        /// 交易金额
        /// </summary>
		public decimal? Amount { get; set; }

		/// <summary>
        /// 房屋Id
        /// </summary>
		public int? HouseDeptId { get; set; }

		/// <summary>
        /// 资源Id
        /// </summary>
		public int? ResourcesId { get; set; }
		
      public string ChargeRecordId { get; set; }
      public virtual ChargeRecord ChargeRecord { get; set; }
    
	 }
	public partial class ChargeBillRecordMatchingMapper : EntityMapper<ChargeBillRecordMatching>
    {
        public ChargeBillRecordMatchingMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.ChargeBillId).HasMaxLength(36).IsOptional();
			Property(s => s.Amount).IsOptional();
			Property(s => s.HouseDeptId).IsOptional();
			Property(s => s.ResourcesId).IsOptional();
        }
    }
}
