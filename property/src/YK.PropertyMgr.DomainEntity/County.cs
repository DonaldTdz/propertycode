using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class County: IAggregateRoot
	{

		/// <summary>
        /// Id
        /// </summary>
		public int? Id { get; set; }

		/// <summary>
        /// Name
        /// </summary>
		public string Name { get; set; }

		/// <summary>
        /// Code
        /// </summary>
		public string Code { get; set; }

		/// <summary>
        /// CityID
        /// </summary>
		public int? CityID { get; set; }
	 }
	public partial class CountyMapper : EntityMapper<County>
    {
        public CountyMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.Name).HasMaxLength(255).IsOptional();
			Property(s => s.Code).HasMaxLength(16).IsOptional();
			Property(s => s.CityID).IsOptional();
        }
    }
}
