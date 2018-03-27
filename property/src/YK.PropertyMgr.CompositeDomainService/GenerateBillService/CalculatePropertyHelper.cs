using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.CompositeDomainService.GenerateBillService.Interface;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.DomainCompositeService;
using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;

namespace YK.PropertyMgr.CompositeDomainService.GenerateBillService
{
    public class CalculatePropertyHelper
    {
        /// <summary>
        /// 获取计算房屋属性
        /// </summary>
        /// <param name="CommunityID">小区ID</param>
        /// <returns>房屋属性列表</returns>
        public static IList<CalculateProperty> GetHouseCalculateProperty(int CommunityID, int houseDeptId = 0)
        {
            IList<CalculateProperty> result = new List<CalculateProperty>();
            if (houseDeptId != 0)
            {
                HouseInfo houseInfo = DomainInterfaceHelper
                .LookUp<IPropertyDomainService>()
                .GetHouseInfo(CommunityID, houseDeptId);
                if (houseInfo != null)
                {
                    result.Add(new CalculateProperty()
                    {
                        ComDeptId = CommunityID,
                        HouseDeptID = houseInfo.HouseDeptID,
                        ResourcesId = houseInfo.Id.Value,
                        Properties = GetHoustProperties(houseInfo),
                        HouseStatus = GetHouseStatus(houseInfo),
                        UnsoldIsBindDeveloper = houseInfo.UnsoldIsBindDeveloper,
                        ResourcesName = houseInfo.DoorNo,
                        HouseDoorNo = houseInfo.DoorNo
                    });
                }
            }
            else
            {
                result = DomainInterfaceHelper
                .LookUp<IPropertyDomainService>()
                .GetHouseListByCommunityDeptId(CommunityID)
                .Select(c => new CalculateProperty()
                {
                    ComDeptId = CommunityID,
                    HouseDeptID = c.HouseDeptID,
                    ResourcesId = c.Id.HasValue? c.Id.Value : 0,
                    Properties = GetHoustProperties(c),
                    HouseStatus = GetHouseStatus(c),
                    UnsoldIsBindDeveloper = c.UnsoldIsBindDeveloper,
                    ResourcesName = c.DoorNo,
                    HouseDoorNo = c.DoorNo
                })
                .ToList();
            }
            return result;
        }

        public static IList<CalculateProperty> GetParkingSpaceCalculateProperty(int CommunityID, IList<CalculateProperty> houseList, int houseDeptId = 0,int ResourcesId=0,int RefType=0)
        {
            if (houseDeptId != 0)
            {
                var result = DomainInterfaceHelper
                .LookUp<IPropertyDomainService>()
                .GetParkingSpaceInfo(CommunityID, houseDeptId)
                .Select(c => new CalculateProperty()
                {
                    ComDeptId = CommunityID,
                    HouseDeptID = c.HouseDeptID.HasValue ? c.HouseDeptID : 0,
                    ResourcesId = c.ParkingSpaceId.Value,
                    Properties = GetParkingProperties(c),
                    ResourcesName = c.CarportNum,
                    HouseDoorNo = GetHouseDoorNo(houseList, c.HouseDeptID)
                })
                .ToList();
                return result;
            }
            else if (ResourcesId == 0)
            {//寻找房间关联的车位
                var result = DomainInterfaceHelper
                  .LookUp<IPropertyDomainService>()
                  .GetParkingSpaceListByCommunityDeptId(CommunityID)
                  .Select(c => new CalculateProperty()
                  {
                      ComDeptId = CommunityID,
                      HouseDeptID = c.HouseDeptID.HasValue ? c.HouseDeptID : 0,
                      ResourcesId = c.ParkingSpaceId.Value,
                      Properties = GetParkingProperties(c),
                      ResourcesName = c.CarportNum,
                      HouseDoorNo = GetHouseDoorNo(houseList, c.HouseDeptID)
                  })
                  .ToList();
                return result;
            }
            else if (ResourcesId > 0 && RefType == (int)ReourceTypeEnum.CarPark)
            {
                var result = DomainInterfaceHelper
                .LookUp<IPropertyDomainService>()
                .GetParkingSpaceListByResourcesId(ResourcesId)
                .Select(c => new CalculateProperty()
                {
                    ComDeptId = CommunityID,
                    HouseDeptID = c.HouseDeptID.HasValue ? c.HouseDeptID : 0,
                    ResourcesId = c.ParkingSpaceId.Value,
                    Properties = GetParkingProperties(c),
                    ResourcesName = c.CarportNum,
                    HouseDoorNo = GetHouseDoorNo(houseList, c.HouseDeptID)
                })
                .ToList();
                return result;
            }
            return new List<CalculateProperty>();
             
        }

        /// <summary>
        /// 获取公式属性字符串格式
        /// </summary>
        /// <param name="chargeFormula">公式属性</param>
        /// <returns>公式属性字符串格式</returns>
        public static string GetChargeFormulaProperty(ChargeFormulaEnum chargeFormula)
        {
            return "," + chargeFormula.GetHashCode().ToString() + ",";
        }

        /// <summary>
        /// 获取房屋属性值
        /// </summary>
        /// <param name="house">房屋</param>
        /// <returns>属性值集合</returns>
        private static Dictionary<ChargeFormulaEnum, string> GetHoustProperties(HouseInfo house)
        {
            Dictionary<ChargeFormulaEnum, string> houstProperties = new Dictionary<ChargeFormulaEnum, string>();
            houstProperties.Add(ChargeFormulaEnum.BuildArea, house.BuildArea.HasValue?house.BuildArea.ToString() : "0");//房屋建筑面积
            houstProperties.Add(ChargeFormulaEnum.HouseInArea, house.HouseInArea.HasValue?house.HouseInArea.ToString() : "0");//房屋套内面积
            return houstProperties;
        }

        private static HouseStatusEnum? GetHouseStatus(HouseInfo house)
        {
            if (house.HouseStatus.HasValue)
            {
                return (HouseStatusEnum)house.HouseStatus.Value;
            }
            return HouseStatusEnum.Received;
        }

        /// <summary>
        /// 获取车位属性值
        /// </summary>
        /// <param name="space">车位信息</param>
        /// <returns>属性值集合</returns>
        private static Dictionary<ChargeFormulaEnum, string> GetParkingProperties(ParkingSpaceInfo space)
        {
            Dictionary<ChargeFormulaEnum, string> houstProperties = new Dictionary<ChargeFormulaEnum, string>();
            houstProperties.Add(ChargeFormulaEnum.ParkingSpaceArea, space.Area.HasValue? space.Area.ToString() : "0");//车位面积
            return houstProperties;
        }

        /// <summary>
        /// 获取三表属性
        /// </summary>
        /// <param name="meterEnum">仪表类型</param>
        /// <param name="value">仪表值</param>
        /// <returns>属性值集合</returns>
        public static Dictionary<ChargeFormulaEnum, string> GetMeterProperties(MeterTypeEnum meterEnum, decimal value)
        {
            Dictionary<ChargeFormulaEnum, string> houstProperties = new Dictionary<ChargeFormulaEnum, string>();
            switch (meterEnum)
            {
                case MeterTypeEnum.WaterMeter:
                    {
                        houstProperties.Add(ChargeFormulaEnum.WaterUnit, value.ToString());
                    }
                    break;
                case MeterTypeEnum.GasMeter:
                    {
                        houstProperties.Add(ChargeFormulaEnum.GasUnit, value.ToString());
                    }
                    break;
                case MeterTypeEnum.WattHourMeter:
                    {
                        houstProperties.Add(ChargeFormulaEnum.ElectricUnit, value.ToString());
                    }
                    break;
            }
            return houstProperties;
        }

        public static string GetHouseDoorNo(IList<CalculateProperty> houseList, int? houseDeptId)
        {
            if (houseList == null || houseList.Count() == 0)
            {
                return null;
            }
            var house = houseList.Where(h => h.HouseDeptID == houseDeptId).FirstOrDefault();
            if (house != null)
            {
                return house.HouseDoorNo;
            }
            return null;
        }

        public static decimal GetTotalHouseBuildArea(IList<CalculateProperty> houseList)
        {
            decimal totalArea = 0;
            foreach (var item in houseList)
            {
                var buildAreaStr = item.Properties[ChargeFormulaEnum.BuildArea];
                if (!string.IsNullOrEmpty(buildAreaStr))
                {
                    totalArea += decimal.Parse(buildAreaStr);
                }
            }
            return totalArea;
        }
    }
}
