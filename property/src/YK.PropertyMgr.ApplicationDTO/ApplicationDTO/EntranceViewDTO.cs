using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class EntranceViewDTO : EntranceDTO
    {

        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string DeviceType { get; set; }
        public DateTime KeyExpireTime { get; set; }
        public string BindLock { get; set; }
    }
}
