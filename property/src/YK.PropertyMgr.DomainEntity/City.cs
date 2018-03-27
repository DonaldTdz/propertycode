using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.DomainEntity
{
	public partial class City: IAggregateRoot
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
        /// ProvinceID
        /// </summary>
		public int? ProvinceID { get; set; }
	 }
	public partial class CityMapper : EntityMapper<City>
    {
        public CityMapper()
        {
            HasKey(s => s.Id);
			Property(s => s.Name).HasMaxLength(255).IsOptional();
			Property(s => s.Code).HasMaxLength(16).IsOptional();
			Property(s => s.ProvinceID).IsOptional();
        }
    }
}
