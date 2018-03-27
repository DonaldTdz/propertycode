using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YK.PropertyMgr.ApplicationDTO
{
	[Serializable]
	public partial class CountyDTO
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
}
