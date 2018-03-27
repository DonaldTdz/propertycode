using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.PresentationService;
using YK.ParkingSys.DomainEntity;

namespace YK.BackgroundMgr.DomainInterface
{
    /// <summary>
    /// 领域层服务接口约束
    /// </summary>
    public interface IPropertyDomainService : IDomainInterface
    {
        List<SEC_House> GetHouseInfosByCommunityDeptId(int communityDeptId);

        List<HouseInfo> GetHouseListByCommunityDeptId(int communityDeptId);

        HouseInfo GetHouseInfo(int communityDeptId, int houseDeptID, bool isGetName = false);

        List<DeptInfo> GetHouDeptListByCommunityDeptId(string communityDeptId);

        List<DeptInfo> GetHouDeptAndOwnerListByComDeptId(string ComDeptId);

        /// <summary>
        /// 通过姓名和小区找到房屋和业主
        /// </summary>
        /// <param name="OwnerName"></param>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        List<DeptInfo> GetHouDeptAndOwnerListByComDeptId(string OwnerName, string ComDeptId);
        /// <summary>
        ///通过id集合获取房屋和业主
        /// </summary>
        /// <param name="IdList"></param>
        /// <returns></returns>
        List<DeptInfo> GetHouDeptAndOwnerListByIdArr(List<int?> IdList);

        List<DeptInfo> GetOwnerHouDeptListByComDeptId(string communityDeptId);

        List<DeptInfo> GetComDeptByUserName(string UserName);

        SEC_Dept GetDeptInfoById(string ID);

        SEC_User_Owner GetUserOwnerMasterByHouseDeptId(int houseDeptId);

        OwnerInformation GetUserOwnerByPhoneNum(int communityDeptId, string phoneNum);

        string GetDeptNosByHouseDeptIds(int?[] houseDeptIds);

        List<AppHouseView> GetHouseListByDoorNo(int communityDeptId, string doorNo);

        List<ParkingSpaceInfo> GetParkingSpaceListByCommunityDeptId(int communityDeptId);

        List<ParkingSpaceInfo> GetParkingSpaceInfo(int communityDeptId, int houseDeptID);

        List<ParkingSpaceInfo> GetParkingSpaceListByResourcesId(int ResourcesIdId);

        List<BuildingInfo> GetBuildsInfoByBuildDeptId(List<int> BuildDeptIdList);

        int GetCommunityDeptIdByHouseDeptId(int houseDeptID);



        int GetCommunityDeptIdByCarPortId(int CarPortId);

        /// <summary>
        /// 通过小区获取同一个物业下面的其它小区
        /// </summary>
        int?[] GetComDeptIdsByComDeptId(int ComDeptId);

        List<DeptInfo> GetDeptInfoListByPropertyId(int? PropertyDeptId);
        /// <summary>
        /// 通过Id获取小区信息
        /// </summary>
        /// <param name="communityDeptId"></param>
        /// <returns></returns>
        SEC_Community GetGetCommunityById(int communityDeptId);

        /// <summary>
        /// 根据楼宇ID获取房屋信息
        /// </summary>
        /// <param name="BuildDeptIds"></param>
        /// <returns></returns>
        List<DeptInfo> GetHouDeptListByBuildDeptId(List<int?> BuildDeptIds);
        /// <summary>
        /// 根据楼宇Id获取独栋信息
        /// </summary>
        /// <param name="BuildDeptId"></param>
        /// <returns></returns>
        BuildingSingle GetHouDeptListBySingleBuildDeptId(int BuildDeptId);
        /// <summary>
        /// 根据小区名称获取小区
        /// </summary>
        /// <param name="villageName"></param>
        /// <returns></returns>
        IList<SEC_Community> GetCommunityList(string villageName);


        /// <summary>
        /// 根据城市获取小区
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        IList<SEC_Community> GetCommunityListByCity(string cityName);

        /// <summary>
        /// 根据小区ID获取楼宇ID
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns></returns>
        IList<DeptInfo> GetBuildsByComDeptId(int comDeptId);

        HouseInfo GetHouseInfoByHouseNo(int communityDeptId, string houseNo);
        /// <summary>
        ///根据ComDeptId获得楼栋，房屋信息
        /// </summary>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        List<DeptInfo> GetBuildingByComDeptId(int? ComDeptId);
        /// <summary>
        /// 根据BuildId获得房屋状态信息
        /// </summary>
        /// <param name="BuildId"></param>
        /// <returns></returns>
        List<HouseState> GetHouseByComDeptId(int? BuildId, int? DecorationState_PM, int? HouseState_PM);
        ParkingSpaceInfo GetParkingSpaceInfoBySpaceNo(int communityDeptId, string parkingName, string spaceNo);
        /// <summary>
        /// 获取物业信息
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        SEC_Property GetSECProperty(int deptId);
        /// <summary>
        /// 获取房屋信息
        /// </summary>
        /// <param name="deptIds"></param>
        /// <returns></returns>
        List<HouseInfo> GetHouseListByHouseDeptIds(List<int?> deptIds);
        /// <summary>
        /// 获取DEPT信息
        /// </summary>
        /// <param name="deptIds"></param>
        /// <returns></returns>
        List<DeptInfo> GetDeptsByIds(List<int?> deptIds);

        /// <summary>
        /// 通过小区Id获取 Build House
        /// </summary>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        IList<DeptInfo> GetBuildAndHouseDeptInfoByComId(int ComDeptId);

        /// <summary>
        /// 通过楼宇Id获取Deptinfo信息
        /// </summary>
        /// <param name="LouyuDeptId"></param>
        /// <returns></returns>
        IList<DeptInfo> GetHouseDeptInfobyLouyuDeptId(int LouyuDeptId);


        /// <summary>
        /// 获取车位树
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        IList<CustomTreeNodeModel> GetCarParkTree(string UserName,string keyword="");

        /// <summary>
        /// 通过小区获取停车场
        /// </summary>
        /// <param name="CommunityId"></param>
        /// <returns></returns>
        IList<CustomTreeNodeModel> GetCarParkByCommunityId(string CommunityId);


        IList<CustomTreeNodeModel> GetCarportByParkId(string ParkingId);

        /// <summary>
        /// 获取车位详细信息
        /// </summary>
        /// <param name="CarportId"></param>
        /// <returns></returns>
        Carport GetCarPortById(int CarportId);


        List<Carport> GetCarPortListByComDeptId(int ComDeptId);

        /// <summary>
        /// 通过车位获取业主
        /// </summary>
        /// <param name="ResourcesId"></param>
        /// <returns></returns>
        SEC_User_Owner GetUserOwnerMasterByCarPortId(int ResourcesId);
        /// <summary>
        /// 通过电话号码获取业主
        /// </summary>
        /// <param name="BindingPhonerNumber"></param>
        /// <returns></returns>
        IList<OwnerInformation> GetOwnerByBindingPhonerNumber(string BindingPhonerNumber);
        /// <summary>
        /// 通过RoomId和电话号码获取业主信息
        /// </summary>
        /// <param name="RoomId"></param>
        /// <returns></returns>
        IList<OwnerInformation> GetOwnerByValidatePhonerNumber(int? RoomId,string BindingPhonerNumber);
        /// <summary>
        /// 通过查询条件查询查询Dept的对象集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IList<DeptInfo> GetDeptInfoByQuery(Expression<Func<SEC_Dept, bool>> predicate);

        DeptInfo GetComDeptInfoByName(string comName);

        int?[] GetHouseDeptIdsByHouseNum(int? comDeptId, string houseNum);

        string[] GetUserPhonesByHouseDeptIds(int?[] houseDeptIdArr);

        IList<SEC_AreaDTO> GetSec_AreaList(Expression<Func<SEC_Area, bool>> predicate);
        /// <summary>
        /// 获取固定小区排除传入房屋ID后的树形集合，
        /// </summary>
        /// <param name="ExcludeIds"></param>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        CustomTreeNodeModel GetDeptTreeExcludeIdsbyComDeptId(List<int?> ExcludeIds, int ComDeptId);

        /// <summary>
        /// 传入用户Id 返回角色列表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        IList<SEC_Role> GetRoleListByAdminUsers(int? Id);

    }
}