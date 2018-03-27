using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using YK.Framework.ApplicationDTO;
using YK.Framework.PresentationService;
using PropertySysAPI.Entity;


namespace PropertySysAPI.Accessor
{

    public class PublicAPIHelper
    {

        //访问公共模块功能的制作
        private static string PublicApiWebUrl = ConfigurationManager.AppSettings["PublicApiWebReference"];

        //访问公共模块功能的制作
        public static string PublicSysdbo = ConfigurationManager.AppSettings["PublicApiWebDataName"];

        //访问ZNMS的地址
        private static string PublicApiWebZNMSUrl = ConfigurationManager.AppSettings["PublicApiWebZNMSReference"];

        #region 建立和公共模块的连接，返回查询方法
        /// <summary>
        /// 建立和公共模块的连接，返回查询方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="ParameterStr"></param>
        /// <param name="functionCode"></param>
        /// <returns></returns>
        private static HttpResponseMessage CreateApiConnection(string action, string ParameterStr, string functionCode)
        {
            try
            {


                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(PublicApiWebUrl);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("api/" + functionCode + "/" + action + ParameterStr).Result;




                if (response.IsSuccessStatusCode)
                {

                    return response;
                    // var users = response.Content.ReadAsAsync<IEnumerable<DeptInfo>>().Result;

                }
                else
                {
                    throw new Exception("Error Code" +
                     response.StatusCode + " : Message - " + response.ReasonPhrase);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        //---------------------系统基本

        #region 登陆系统验证

        /// <summary>
        ///登陆系统验证
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static bool ValidateUser(string UserName, string Password)
        {
            try
            {


                string SeachCode = "?UserName=" + UserName + "&Password=" + Password;

                HttpResponseMessage httpres = CreateApiConnection("ValidateUser", SeachCode, "FrameworkRight");
                var users = httpres.Content.ReadAsAsync<bool>().Result;
                return (bool)users;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        #region 返回用户能够访问的模块信息

        /// <summary>
        ///返回用户能够访问的模块信息
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static List<YK.Framework.ApplicationDTO.ModuleInfo> GetUserModules(string UserName, string ModuleName)
        {
            try
            {
                string SeachCode = "?UserName=" + UserName + "&ModuleName=" + ModuleName;
                HttpResponseMessage httpres = CreateApiConnection("GetUserModules", SeachCode, "FrameworkRight");
                var users = httpres.Content.ReadAsAsync<IEnumerable<YK.Framework.ApplicationDTO.ModuleInfo>>().Result;
                return (List<YK.Framework.ApplicationDTO.ModuleInfo>)users;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region 查询组织架构
        /// <summary>
        ///查询组织架构
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static List<DeptInfo> GetUserDepts(string UserName)
        {
            try
            {


                string SeachCode = "?UserName=" + UserName;

                HttpResponseMessage httpres = CreateApiConnection("GetUserDepts", SeachCode, "FrameworkRight");
                var users = httpres.Content.ReadAsAsync<IEnumerable<DeptInfo>>().Result;
                return (List<DeptInfo>)users;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region  查询房间集信息

        public static SearchResultData<YK.Framework.ApplicationDTO.HouseInfo> GetHouseInfo_List(string UserName, string DoorNo, string DeptId, int page, int PageSize)
        {
            try
            {
                string ParameterStr = string.Empty;
                ParameterStr += "?UserName=" + UserName;
                ParameterStr += "&DoorNo=" + DoorNo;
                ParameterStr += "&DeptId=" + DeptId;
                ParameterStr += "&Start=" + ((page - 1) * PageSize);
                ParameterStr += "&Length=" + PageSize;
                HttpResponseMessage httpres = PublicAPIHelper.CreateApiConnection("GetAdminUserHouses", ParameterStr, "PropertyManage");


                return httpres.Content.ReadAsAsync<SearchResultData<YK.Framework.ApplicationDTO.HouseInfo>>().Result;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 根据用户名获取管理员用户信息
        /// <summary>
        /// 根据用户名获取管理员用户信息
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static AdminUserInfo GetAdminUserInfoByName(string UserName)
        {
            try
            {
                string SeachCode = "?UserName=" + UserName;
                HttpResponseMessage httpres = CreateApiConnection("GetAdminUserInfoByName", SeachCode, "FrameworkRight");
                var users = httpres.Content.ReadAsAsync<AdminUserInfo>().Result;
                return (AdminUserInfo)users;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region 返回用户拥有的操作权限集合
        public static IEnumerable<YK.Framework.ApplicationDTO.OperateCodeAndRoleInfo> GetUserOperates(string UserName)
        {
            try
            {
                string SeachCode = "?UserName=" + UserName;
                HttpResponseMessage httpres = CreateApiConnection("GetUserOperates", SeachCode, "FrameworkRight");
                var usersRoleList = httpres.Content.ReadAsAsync<IEnumerable<YK.Framework.ApplicationDTO.OperateCodeAndRoleInfo>>().Result;
                return (IEnumerable<YK.Framework.ApplicationDTO.OperateCodeAndRoleInfo>)usersRoleList;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion


        ///--------------------物业



        #region 查询车辆列表信息
        /// <summary>
        /// 查询车辆列表信息
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="DoorNo">房间号</param>
        /// <param name="CarportNo">车位号</param>
        /// <param name="DeptId">组织架构Id</param>
        /// <param name="page">当前页码</param>
        /// <param name="PageSize">每页显示数据</param>
        /// <returns></returns>
        public static SearchResultData<YK.Framework.ApplicationDTO.HouseInfo> GetHouseInfo_List(string UserName, string DoorNo, string CarportNo, string DeptId, int page, int PageSize)
        {
            try
            {
                string ParameterStr = string.Empty;
                ParameterStr += "?UserName=" + UserName;
                ParameterStr += "&DoorNo=" + DoorNo;
                ParameterStr += "&DeptId=" + DeptId;
                ParameterStr += "&CarportNo=" + CarportNo;
                ParameterStr += "&Start=" + ((page - 1) * PageSize);
                ParameterStr += "&Length=" + PageSize;
                HttpResponseMessage httpres = PublicAPIHelper.CreateApiConnection("GetAdminUserHouses", ParameterStr, "PropertyManage");


                return httpres.Content.ReadAsAsync<SearchResultData<YK.Framework.ApplicationDTO.HouseInfo>>().Result;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region  获取单个房间信息
        /// <summary>
        /// 获取单个房间信息
        /// </summary>
        /// <param name="HouseId">房间Id</param>
        /// <returns></returns>
        public static YK.Framework.ApplicationDTO.HouseInfo GetGetHouseInfo(string HouseId)
        {
            try
            {
                HouseId = "?HouseId=" + HouseId;


                HttpResponseMessage httpres = CreateApiConnection("GetHouseInfo", HouseId, "PropertyManage");
                var users = httpres.Content.ReadAsAsync<YK.Framework.ApplicationDTO.HouseInfo>().Result;
                return (YK.Framework.ApplicationDTO.HouseInfo)users;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 获取单个业主信息
        /// <summary>
        /// 获取单个业主信息
        /// </summary>
        /// <param name="HouseId">业主信息</param>
        /// <returns></returns>
        public static YK.Framework.ApplicationDTO.UserOwnerInfo GetUserOwnerInfo(string UserOwnerId)
        {
            try
            {
                UserOwnerId = "?UserOwnerId=" + UserOwnerId;


                HttpResponseMessage httpres = CreateApiConnection("GetUserOwnerInfo", UserOwnerId, "PropertyManage");
                var users = httpres.Content.ReadAsAsync<YK.Framework.ApplicationDTO.UserOwnerInfo>().Result;
                return (YK.Framework.ApplicationDTO.UserOwnerInfo)users;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 获取单个车位信息
        /// <summary>
        /// 获取单个车位信息
        /// </summary>
        /// <param name="CarportId">车位Id</param>
        /// <returns></returns>
        public static YK.Framework.ApplicationDTO.CarportInfo GetCarportInfo(string CarportId)
        {
            try
            {
                CarportId = "?CarportId=" + CarportId;


                HttpResponseMessage httpres = CreateApiConnection("GetCarportInfo", CarportId, "PropertyManage");
                var users = httpres.Content.ReadAsAsync<YK.Framework.ApplicationDTO.CarportInfo>().Result;
                return (YK.Framework.ApplicationDTO.CarportInfo)users;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 传入组织架构Id返回组织架构信息
        /// <summary>
        /// 传入组织架构Id返回组织架构信息
        /// </summary>
        /// <param name="DeptId">组织架构Id</param>
        /// <returns></returns>
        public static DeptInfo GetDeptInfo(string DeptId)
        {
            try
            {
                DeptId = "?DeptId=" + DeptId;


                HttpResponseMessage httpres = CreateApiConnection("GetDeptInfo", DeptId, "PropertyManage");
                var users = httpres.Content.ReadAsAsync<DeptInfo>().Result;
                return (DeptInfo)users;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 返回该房间下的家庭成员
        /// <summary>
        /// 返回该房间下的家庭成员
        /// </summary>
        /// <param name="DeptId">组织架构Id</param>
        /// <param name="DoorNo">门牌号</param>
        /// <returns></returns>
        public static YK.Framework.ApplicationDTO.UserOwnerInfo GetHouseUsers(string DeptId, string DoorNo)
        {
            try
            {
                string SeachCode = "?DeptId=" + DeptId + "&DoorNo=" + DoorNo;


                HttpResponseMessage httpres = CreateApiConnection("GetHouseUsers", SeachCode, "PropertyManage");
                var users = httpres.Content.ReadAsAsync<YK.Framework.ApplicationDTO.UserOwnerInfo>().Result;
                return (YK.Framework.ApplicationDTO.UserOwnerInfo)users;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 通过房间Id返回家庭成员
        /// <summary>
        /// 通过房间Id返回家庭成员
        /// </summary>
        /// <param name="HouseInfoId"></param>
        /// <returns></returns>
        public static List<YK.Framework.ApplicationDTO.UserOwnerInfo> GetUserOwnerByHouse(string HouseInfoId)
        {
            try
            {
                HouseInfoId = "?HouseId=" + HouseInfoId;

                HttpResponseMessage httpres = CreateApiConnection("GetUserOwnerByHouse", HouseInfoId, "PropertyManage");
                var users = httpres.Content.ReadAsAsync<IEnumerable<YK.Framework.ApplicationDTO.UserOwnerInfo>>().Result;


                return (List<YK.Framework.ApplicationDTO.UserOwnerInfo>)users;

            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region 通过车位Id返回家庭成员
        /// <summary>
        /// 通过房间Id返回家庭成员
        /// </summary>
        /// <param name="HouseInfoId"></param>
        /// <returns></returns>
        public static List<YK.Framework.ApplicationDTO.UserOwnerInfo> GetUserOwnerInfoByCarportId(int CarportId)
        {
            try
            {
                string SeachCode = "?CarportId=" + CarportId.ToString();

                HttpResponseMessage httpres = CreateApiConnection("GetUserOwnerInfoByCarportId", SeachCode, "PropertyManage");
                var users = httpres.Content.ReadAsAsync<IEnumerable<YK.Framework.ApplicationDTO.UserOwnerInfo>>().Result;


                return (List<YK.Framework.ApplicationDTO.UserOwnerInfo>)users;

            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region 根据房屋OrgId获取小区组织架构信息

        /// <summary>
        /// 获取单个业主信息
        /// </summary>
        /// <param name="HouseId">业主信息</param>
        /// <returns></returns>
        public static DeptInfo GetCommunityDeptInfo(Guid HouseOrgId)
        {
            try
            {
                string HouseOrgId_Str = "?HouseOrgId=" + HouseOrgId.ToString();


                HttpResponseMessage httpres = CreateApiConnection("GetCommunityDeptInfo", HouseOrgId_Str, "PropertyManage");
                var users = httpres.Content.ReadAsAsync<DeptInfo>().Result;
                return (DeptInfo)users;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 传入逸社区小区编号和房间号，返回我方房间信息
        /// <summary>
        /// 获取单个业主信息
        /// </summary>
        /// <param name="HouseId">业主信息</param>
        /// <returns></returns>
        public static YK.Framework.ApplicationDTO.HouseInfo GetHouseInfoByDoorNo(string CommunityCode, string DoorNo)
        {
            try
            {
                string SeachCode = "?CommunityCode=" + CommunityCode + "&DoorNo=" + DoorNo;


                HttpResponseMessage httpres = CreateApiConnection("GetHouseInfoByDoorNo", SeachCode, "PropertyManage");
                var users = httpres.Content.ReadAsAsync<YK.Framework.ApplicationDTO.HouseInfo>().Result;
                return (YK.Framework.ApplicationDTO.HouseInfo)users;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 查询车辆列表信息
        /// <summary>
        /// 查询车辆列表信息
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="DoorNo">房间号</param>
        /// <param name="CarportNo">车位号</param>
        /// <param name="DeptId">组织架构Id</param>
        /// <param name="page">当前页码</param>
        /// <param name="PageSize">每页显示数据</param>
        /// <returns></returns>
        public static SearchResultData<YK.Framework.ApplicationDTO.CarportInfo> GetCarportInfo_List(string UserName, string DoorNo, string CarportNo, string DeptId, int page, int PageSize)
        {
            try
            {
                string ParameterStr = string.Empty;
                ParameterStr += "?UserName=" + UserName;
                ParameterStr += "&DoorNo=" + DoorNo;
                ParameterStr += "&DeptId=" + DeptId;
                ParameterStr += "&CarportNo=" + CarportNo;
                ParameterStr += "&Start=" + ((page - 1) * PageSize);
                ParameterStr += "&Length=" + PageSize;
                HttpResponseMessage httpres = PublicAPIHelper.CreateApiConnection("GetAdminUserCarports", ParameterStr, "PropertyManage");
                return httpres.Content.ReadAsAsync<SearchResultData<YK.Framework.ApplicationDTO.CarportInfo>>().Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 传入组织架构Id.返回该Id下房屋信息（门牌号  户主Id  套内面积 ）
        /// <summary>
        /// 传入组织架构Id.返回该Id下房屋信息（门牌号  户主Id  套内面积）
        /// </summary>
        /// <param name="DeptId"></param>
        /// <returns></returns>
        public static IEnumerable<YK.Framework.ApplicationDTO.CostInfo> GetCostInfoByDeptyId(int DeptId)
        {
            try
            {
                string SeachCode = "?DeptId=" + DeptId;
                HttpResponseMessage httpres = CreateApiConnection("GetCostInfoByDeptyId", SeachCode, "PropertyManage");
                var usersRoleList = httpres.Content.ReadAsAsync<IEnumerable<YK.Framework.ApplicationDTO.CostInfo>>().Result;
                return (IEnumerable<YK.Framework.ApplicationDTO.CostInfo>)usersRoleList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region 获取该用户能看的组织架构，下的组织
        /// <summary>
        /// 获取该用户能看的组织架构，下的组织
        /// </summary>
        /// <param name="DeptId">组织ID</param>
        /// <param name="UserName">登陆账户</param>
        /// <returns></returns>
        public static List<DeptInfo> GetDeptIdSublist(int DeptId, string UserName)
        {
            var list = GetUserDepts(UserName);
            list = list.Where(o => o.Id == DeptId).ToList();
            return list;
        }


        /// <summary>
        /// 获取该用户能看的组织架构，下的组织
        /// </summary>
        /// <param name="deptId">组织架构Id</param>
        /// <param name="listNew">传回新查询出的组织架构Id</param>
        /// <param name="UserName">用户姓名</param>
        public static void GetDeptIdSublist(int deptId, ref List<DeptInfo> listNew, string UserName)
        {

            List<DeptInfo> dempNew = new List<DeptInfo>();
            List<DeptInfo> listDeptInfos = PublicAPIHelper.GetUserDepts(UserName);
            List<DeptInfo> listDeptInfo = listDeptInfos.Where(o => o.Id == deptId).ToList();
            foreach (DeptInfo item in listDeptInfo)
            {
                //1.查询子集
                dempNew = listDeptInfos.Where(o => o.PId == item.Id).ToList();
                listNew.AddRange(dempNew);
                foreach (var itemChild in dempNew)
                {
                    GetDeptIdSublist(itemChild.Id, ref listNew, UserName);
                }
            }
        }

        #endregion

        #region 获取该小区下的车位信息
        /// <summary>
        /// 获取该小区下的车位信息
        /// </summary>
        /// <param name="deptId"></param>
        public static List<YK.Framework.ApplicationDTO.CostCarportInfo> GetCarPortInfo(string deptId)
        {

            try
            {
                deptId = "?DeptId=" + deptId;
                HttpResponseMessage httpres = CreateApiConnection("GetCostCarportInfoByDeptyId", deptId, "PropertyManage");
                var users = httpres.Content.ReadAsAsync<List<YK.Framework.ApplicationDTO.CostCarportInfo>>().Result;
                return (List<YK.Framework.ApplicationDTO.CostCarportInfo>)users;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region 通过小区编号获取小区信息
        public static CommunityInfo GetCommunityInfo(string Code)
        {
            try
            {
                Code = "?Code=" + Code;

                HttpResponseMessage httpres = CreateApiConnection("GetCommunityInfoByCode", Code, "PropertyManage");
                var users = httpres.Content.ReadAsAsync<CommunityInfo>().Result;
                return (CommunityInfo)users;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region 根据小区Id获取小区详细信息
        public static CommunityInfo GetCommunityInfoById(int id)
        {
            try
            {
                string villageId = "?CommunityId=" + id;

                HttpResponseMessage httpres = CreateApiConnection("GetCommunityInfo", villageId, "PropertyManage");
                var users = httpres.Content.ReadAsAsync<CommunityInfo>().Result;
                return (CommunityInfo)users;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region  根据组织架构Id获取组织架构详细信息
        public static YK.Framework.ApplicationDTO.DeptInfo GetDeptInfo(int id)
        {
            try
            {
                string DeptId = "?DeptId=" + id;

                HttpResponseMessage httpres = CreateApiConnection("GetDeptInfo", DeptId, "PropertyManage");
                var users = httpres.Content.ReadAsAsync<YK.Framework.ApplicationDTO.DeptInfo>().Result;
                return (YK.Framework.ApplicationDTO.DeptInfo)users;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region 返回业主信息

        /// <summary>
		///返回业主信息
		/// </summary>
		/// <param name="UserName"></param>
		/// <returns></returns>
		public static List<YK.Framework.ApplicationDTO.UserOwnerInfo> GetUserOwnerInfoByCommunityDeptId(int CommunityDeptId)
        {
            try
            {
                string SeachCode = "?CommunityDeptId=" + CommunityDeptId;
                HttpResponseMessage httpres = CreateApiConnection("GetUserOwnerInfoByCommunityDeptId", SeachCode, "CommunityManage");
                var users = httpres.Content.ReadAsAsync<List<YK.Framework.ApplicationDTO.UserOwnerInfo>>().Result;
                return users;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        /// <summary>
        ///返回业主信息
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static SearchResultData<YK.Framework.ApplicationDTO.UserOwnerInfo> GetAdminUserOwners(string UserName, int DeptId, string UserOwnerName, string BindingPhonerNumber, int page, int PageSize)
        {
            try
            {
                UserOwnerName = "";
                BindingPhonerNumber = "";
                string ParameterStr = string.Empty;
                ParameterStr += "?UserName=" + UserName;
                ParameterStr += "&DeptId=" + DeptId;
                ParameterStr += "&Start=" + ((page - 1) * PageSize);
                ParameterStr += "&Length=" + PageSize;
                ParameterStr += "&UserOwnerName=" + UserOwnerName;
                ParameterStr += "&BindingPhonerNumber=" + BindingPhonerNumber;
                HttpResponseMessage httpres = CreateApiConnection("GetAdminUserOwners", ParameterStr, "PropertyManage");
                var users = httpres.Content.ReadAsAsync<SearchResultData<YK.Framework.ApplicationDTO.UserOwnerInfo>>().Result;
                return users;

            }
            catch (Exception)
            {
                throw;
            }
        }


        #region 智能门锁返回
        /// <summary>
        ///智能门锁返回KeyId
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static string GetKeyId(Entrances obj)
        {
            try
            {
                try
                {
                    string SeachCode = "provinceId=" + obj.ProvinceID + "&cityId=" + obj.CityID + "&countyId=" + obj.CountyID + "&address=" + obj.Address;
                    HttpResponseMessage httpres = CreateApiZNMSConnection("AddLock", SeachCode, "PropertyWeb");
                    var users = httpres.Content.ReadAsAsync<string>().Result;
                    return users;
                }
                catch (Exception)
                {
                    throw;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 建立和智能模块的连接，返回查询方法


        /// <summary>
        /// 建立和公共模块的连接，返回查询方法
        /// </summary>
        /// <param name="action"></param>
        /// <param name="ParameterStr"></param>
        /// <param name="functionCode"></param>
        /// <returns></returns>
        private static HttpResponseMessage CreateApiZNMSConnection(string action, string ParameterStr, string functionCode)
        {
            try
            {


                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(PublicApiWebZNMSUrl);
                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("Api/" + functionCode + "/" + action + "?" + ParameterStr).Result;
                if (response.IsSuccessStatusCode)
                {

                    return response;
                    // var users = response.Content.ReadAsAsync<IEnumerable<DeptInfo>>().Result;

                }
                else
                {
                    throw new Exception("Error Code" +
                     response.StatusCode + " : Message - " + response.ReasonPhrase);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region 获取单个业主信息
        /// <summary>
        /// 获取单个业主信息
        /// </summary>
        /// <param name="HouseId">业主信息</param>
        /// <returns></returns>
        public static YK.Framework.ApplicationDTO.UserOwnerInfo GetUserOwnerInfoById(string UserOwnerId)
        {
            try
            {
                UserOwnerId = "?UserOwnerId=" + UserOwnerId;


                HttpResponseMessage httpres = CreateApiZNMSConnection("GetUserOwnerInfo", UserOwnerId, "PropertyManage");
                var users = httpres.Content.ReadAsAsync<YK.Framework.ApplicationDTO.UserOwnerInfo>().Result;
                return (YK.Framework.ApplicationDTO.UserOwnerInfo)users;

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        public static List<YK.Framework.ApplicationDTO.CommunityInfo> GetCommunityInfoByOwnerId(Guid UserOwnerGuid)
        {

            try
            {
                string uid = "?UserOwnerGuid=" + UserOwnerGuid;
                HttpResponseMessage httpres = CreateApiConnection("GetCommunityInfoByOwnerId", uid, "CommunityManage");
                var commu = httpres.Content.ReadAsAsync<List<YK.Framework.ApplicationDTO.CommunityInfo>>().Result;
                return (List<YK.Framework.ApplicationDTO.CommunityInfo>)commu;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /*门禁授权时获取的房屋、楼栋、单元ID*/


        public static List<YK.Framework.ApplicationDTO.InterfaceDTO.UserOwnerEntrance> GetUserOwnerEntrances(Guid UserOwnerId)
        {

            try
            {
                string uid = "?UserOwnerId=" + UserOwnerId;
                HttpResponseMessage httpres = CreateApiConnection("GetUserOwnerEntrances", uid, "PropertyManage");
                var userOwnerEntrance = httpres.Content.ReadAsAsync<List<YK.Framework.ApplicationDTO.InterfaceDTO.UserOwnerEntrance >> ().Result;
                return (List<YK.Framework.ApplicationDTO.InterfaceDTO.UserOwnerEntrance >)userOwnerEntrance;

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
