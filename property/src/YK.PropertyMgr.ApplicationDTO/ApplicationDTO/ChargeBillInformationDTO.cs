using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class ChargeBillInformationDTO
    {
        public HouseInfo houseInfo { get; set; }

        public List<ParkingSpaceInfo> parkingSpaceInfo { get; set; }

        public List<MeterDTO> meter { get; set; }

        /// <summary>
        /// 是否显示房屋
        /// </summary>
        public bool IsShowHouse { get; set; }
        /// <summary>
        /// 是否显示车位
        /// </summary>
        public bool IsShowCarPark  { get;set; }
        /// <summary>
        /// 是否显示三表
        /// </summary>
        public bool IsShowMeter { get; set; }


    }
}
