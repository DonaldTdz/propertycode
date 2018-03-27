using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.DomainService;
using YK.BackgroundMgr.PresentationService;
using YK.ParkingSys.DomainEntity;

namespace YK.BackgroundMgr.DomainCompositeService
{
    public class PropertyDomainService : IPropertyDomainService
    {
        public List<SEC_House> GetHouseInfosByCommunityDeptId(int communityDeptId)
        {
            return new SEC_HouseDomainService().GetHouseInfosByCommunityDeptId(communityDeptId.ToString());
        }


        public List<HouseInfo> GetHouseListByCommunityDeptId(int communityDeptId)
        {
            return new SEC_HouseDomainService().GetHouseListByCommunityDeptId(communityDeptId.ToString());

        }

        public HouseInfo GetHouseInfo(int communityDeptId, int houseDeptID, bool isGetName = false)
        {
            return new SEC_HouseDomainService().GetHouseInfo(communityDeptId.ToString(), houseDeptID, isGetName);
        }

        public int GetCommunityDeptIdByHouseDeptId(int houseDeptID)
        {
            return new SEC_HouseDomainService().GetCommunityDeptIdByHouseDeptId(houseDeptID);
        }
        public int GetCommunityDeptIdByCarPortId(int CarPortId)
        {
            return new CarportDomainService().GetCommunityDeptIdByCarPortId(CarPortId);
        }

        public SEC_User_Owner GetUserOwnerMasterByCarPortId(int ResourcesId)
        {
            return new CarportDomainService().GetUserOwnerMasterByCarPortId(ResourcesId);
        }

      



        public List<DeptInfo> GetHouDeptListByCommunityDeptId(string communityDeptId)
        {
            return new SEC_DeptDomainService().GetHouDeptListByComDeptId(communityDeptId);
        }
        //对应综合报表查询 业主名
        public List<DeptInfo> GetHouDeptAndOwnerListByComDeptId(string ComDeptId)
        {
            return new SEC_DeptDomainService().GetHouDeptAndOwnerListByComDeptId(ComDeptId);
        }

        public List<DeptInfo> GetHouDeptAndOwnerListByComDeptId(string OwnerName,string ComDeptId)
        {
            return new SEC_DeptDomainService().GetHouDeptAndOwnerListByOwnerName( OwnerName,ComDeptId);
        }

        public List<DeptInfo> GetHouDeptAndOwnerListByIdArr(List<int?> IdList)
        {
            return new SEC_DeptDomainService().GetHouDeptAndOwnerListByIdArr(IdList);
        }

        

        public List<DeptInfo> GetHouDeptListByBuildDeptId(List<int?> BuildDeptIds)
        {
            return new SEC_DeptDomainService().GetHouDeptListByBuildDeptId(BuildDeptIds);
        }
        public BuildingSingle GetHouDeptListBySingleBuildDeptId(int? BuildDeptId)
        {
            return new SEC_DeptDomainService().GetHouDeptListBySingleBuildDeptId(BuildDeptId);
        }
        public List<DeptInfo> GetOwnerHouDeptListByComDeptId(string communityDeptId)
        {
            return new SEC_DeptDomainService().GetOwnerHouDeptListByComDeptId(communityDeptId);
        }

        public List<DeptInfo> GetComDeptByUserName(string UserName)
        {
            return new SEC_DeptDomainService().GetComDeptByUserName(UserName);
        }

        public SEC_Dept GetDeptInfoById(string ID)
        {
            return new SEC_DeptDomainService().GetDeptInfoById(ID);
        }


        public SEC_User_Owner GetUserOwnerMasterByHouseDeptId(int houseDeptId)
        {
            return new SEC_User_OwnerDomainService().GetUserOwnerMasterByHouseDeptId(houseDeptId);
        }

        public string GetDeptNosByHouseDeptIds(int?[] houseDeptIds)
        {
            return new SEC_DeptDomainService().GetDeptNosByHouseDeptIds(houseDeptIds);
        }

        public List<AppHouseView> GetHouseListByDoorNo(int communityDeptId, string doorNo)
        {
            return new SEC_HouseDomainService().GetHouseListByDoorNo(communityDeptId.ToString(), doorNo);
        }

        public List<ParkingSpaceInfo> GetParkingSpaceListByCommunityDeptId(int communityDeptId)
        {
            return new CarportDomainService().GetParkingSpaceListByCommunityDeptId(communityDeptId);
        }

        public List<ParkingSpaceInfo> GetParkingSpaceInfo(int communityDeptId, int houseDeptID)
        {
            return new CarportDomainService().GetParkingSpaceInfo(communityDeptId, houseDeptID);
        }


        public List<ParkingSpaceInfo> GetParkingSpaceListByResourcesId(int ResourcesId)
        {
            return new CarportDomainService().GetParkingSpaceListByResourcesId(ResourcesId);
        }

        public int?[] GetComDeptIdsByComDeptId(int ComDeptId)
        {
            return new SEC_DeptDomainService().GetComDeptIdsByComDeptId(ComDeptId);
        }


        public SEC_Community GetGetCommunityById(int communityDeptId)
        {
            return new SEC_CommunityDomainService().GetCommunityById(communityDeptId);
        }
        public IList<SEC_Community> GetCommunityList(string villageName)
        {
            return new SEC_CommunityDomainService().GetCommunityList(villageName);
        }

        public IList<SEC_Community> GetCommunityListByCity(string cityName)
        {
            return new SEC_CommunityDomainService().GetCommunityListByCity(cityName);
        }

        public IList<DeptInfo> GetBuildsByComDeptId(int comDeptId)
        {
            return new SEC_DeptDomainService().GetBuildsByComDeptId(comDeptId);
        }

        public List<BuildingInfo> GetBuildsInfoByBuildDeptId(List<int> BuildDeptIdList)
        {
            return new SEC_BuildingDomainService().GetBuildsInfoByBuildDeptId(BuildDeptIdList);
        }


        public HouseInfo GetHouseInfoByHouseNo(int communityDeptId, string houseNo)
        {
            return new SEC_HouseDomainService().GetHouseInfoByHouseNo(communityDeptId.ToString(), houseNo);
        }

        public ParkingSpaceInfo GetParkingSpaceInfoBySpaceNo(int communityDeptId, string parkingName, string spaceNo)
        {
            return new CarportDomainService().GetParkingSpaceInfoBySpaceNo(communityDeptId, parkingName, spaceNo);
        }
       

        public SEC_Property GetSECProperty(int deptId)
        {
            return new SEC_PropertyDomainService().GetSEC_PropertyByDeptId(deptId);
        }
        public List<HouseInfo> GetHouseListByHouseDeptIds(List<int?> deptIds)
        {
            return new SEC_HouseDomainService().GetHouseListByHouseDeptIds(deptIds);
        }

        public List<DeptInfo> GetDeptsByIds(List<int?> deptIds)
        {
            return new SEC_DeptDomainService().GetDeptsByIds(deptIds);
        }

        /// <summary>
        /// 通过小区Id获取 Build House
        /// </summary>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        public IList<DeptInfo> GetBuildAndHouseDeptInfoByComId(int ComDeptId)
        {
            return new SEC_DeptDomainService().GetBuildAndHouseDeptInfoByComId(ComDeptId);
        }

        public IList<DeptInfo> GetHouseDeptInfobyLouyuDeptId(int LouyuDeptId)
        {
            return new SEC_DeptDomainService().GetHouseDeptInfobyLouyuDeptId(LouyuDeptId);
        }




        public IList<CustomTreeNodeModel> GetCarParkTree(string UserName,string keyword="")
        {
            if(string.IsNullOrEmpty(keyword))
            return new CarportDomainService().GetCarParkTree(UserName);
            else
                return new CarportDomainService().GetCarParkTree(UserName,keyword);
        }


        public IList<CustomTreeNodeModel> GetCarParkByCommunityId(string CommunityId)
        {
            return new CarportDomainService().GetCarParkByCommunityId(CommunityId);
        }

        public IList<CustomTreeNodeModel> GetCarportByParkId(string ParkingId)
        {
            return new CarportDomainService().GetCarportByParkId(ParkingId);
            
        }
        public Carport GetCarPortById(int CarportId)
        {
            return new CarportDomainService().GetCarPortById(CarportId);
        }


        public List<Carport> GetCarPortListByComDeptId(int ComDeptId)
        {
            return new CarportDomainService().GetCarPortListByComDeptId(ComDeptId);
        }
        public IList<DeptInfo> GetDeptInfoByQuery(Expression<Func<SEC_Dept, bool>> predicate)
        {
            return new SEC_DeptDomainService().GetDeptInfoByQuery(predicate);
        }


        public DeptInfo GetComDeptInfoByName(string comName)
        {
            return new SEC_DeptDomainService().GetComDeptInfoByName(comName);
        }

        public int?[] GetHouseDeptIdsByHouseNum(int? comDeptId, string houseNum)
        {
            return new SEC_DeptDomainService().GetHouseDeptIdsByHouseNum(comDeptId.Value, houseNum);
        }

        public string[] GetUserPhonesByHouseDeptIds(int?[] houseDeptIdArr)
        {
            return new SEC_DeptDomainService().GetUserPhonesByHouseDeptIds(houseDeptIdArr);
        }

        public BuildingSingle GetHouDeptListBySingleBuildDeptId(int BuildDeptId)
        {
            return new SEC_DeptDomainService().GetHouDeptListBySingleBuildDeptId(BuildDeptId);
        }

        public IList<OwnerInformation> GetOwnerByBindingPhonerNumber(string BindingPhonerNumber)
        {
            return new SEC_DeptDomainService().GetOwnerByBindingPhonerNumber(BindingPhonerNumber);
        }

        public List<DeptInfo> GetBuildingByComDeptId(int? ComDeptId)
        {
            return new SEC_DeptDomainService().GetBuildingByComDeptId(ComDeptId);
        }

        public IList<OwnerInformation> GetOwnerByValidatePhonerNumber(int? RoomId, string BindingPhonerNumber)
        {
            return new SEC_DeptDomainService().GetOwnerByValidatePhonerNumber(RoomId, BindingPhonerNumber);
        }

        public OwnerInformation GetUserOwnerByPhoneNum(int communityDeptId, string phoneNum)
        {
            return new SEC_DeptDomainService().GetUserOwnerByPhoneNum(communityDeptId, phoneNum);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PropertyDeptId"></param>
        /// <returns></returns>
        public List<DeptInfo> GetDeptInfoListByPropertyId(int? PropertyDeptId)
        {
            return new SEC_DeptDomainService().GetDeptInfoListByPropertyId(PropertyDeptId); 
        }

        public IList<SEC_AreaDTO> GetSec_AreaList(Expression<Func<SEC_Area, bool>> predicate)
        {
            return new SEC_AreaDomainService().GetSEC_AreaList(predicate);
        }

        public List<HouseState> GetHouseByComDeptId(int? BuildId, int? DecorationState_PM, int? HouseState_PM)
        {
            return new SEC_DeptDomainService().GetHouseByComDeptId(BuildId, DecorationState_PM, HouseState_PM);
        }

        public CustomTreeNodeModel GetDeptTreeExcludeIdsbyComDeptId(List<int?> ExcludeIds, int ComDeptId)
        {
            return new SEC_DeptDomainService().GetDeptTreeExcludeIdsbyComDeptId(ExcludeIds, ComDeptId); 
        }

        public IList<SEC_Role> GetRoleListByAdminUsers(int? Id)
        {
            return new SEC_AdminUserDomainService().GetRoleListByAdminUsers(Id);
        }


    }
}
