using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class CityDTO
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
}
