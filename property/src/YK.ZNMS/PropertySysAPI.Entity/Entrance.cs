using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace PropertySysAPI.Entity
{
	/// <summary>
	/// 实体类 Entrance
	/// </summary>
	[Serializable]
	public class Entrances
	{
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		public int VillageID
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		public int ProvinceID
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		public int CityID
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		public int CountyID
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		public string Address
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		public int KeyID
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		public int State
		{
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
		public DateTime CreateTime
		{
			get;
			set;
		}


	}
}