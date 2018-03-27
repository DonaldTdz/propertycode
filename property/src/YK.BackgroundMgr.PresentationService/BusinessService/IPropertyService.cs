using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;

namespace YK.BackgroundMgr.PresentationService
{
    /// <summary>
    /// 物业收费服务接口
    /// </summary>
    public interface IPropertyService : IPresentationService
    {
        /// <summary>
        /// 异步获取资源树信息
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <param name="parentId">父节点Id</param>
        /// <returns>资源树信息</returns>
        List<CustomTreeNodeModel> GetAsynDeptTree(string userName, int parentId, int?[] TypeArry);


        /// <summary>
        /// 异步获取资源树信息
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <param name="parentId">父节点Id</param>
        /// <param name="TypeArry"></param>
        /// <param name="dic">Dictionary【子节点ID，子节点的子节点Id】 子节点的子节点个数</param>
        /// <returns></returns>
        List<AsynTreeNodeModel> GetAsynDeptTree(string userName, int parentId, int?[] TypeArry, ref Dictionary<string, List<string>> dic);

        /// <summary>
        /// 获取房屋信息同步树
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <param name="keyWord">房屋、客户名、电话</param>
        /// <returns></returns>
        List<CustomTreeNodeModel> GetDeptTree(string userName, string keyWord);

        /// <summary>
        /// 同步异步混合树
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        List<CustomTreeNodeModel> GetDeptTree(string userName, int?[] types);

        /// <summary>
        /// 获取停车场树
        /// </summary>
        /// <param name="communityDeptId"></param>
        /// <returns></returns>
        List<AsynTreeNodeModel> GetAsynParkingTree(int communityDeptId);

        /// <summary>
        /// 获取停车场树
        /// </summary>
        /// <param name="communityDeptId"></param>
        /// <returns></returns>
        List<AsynTreeNodeModel> GetAsynParkingTree(int communityDeptId, ref Dictionary<string, List<string>> dics);

        /// <summary>
        /// 获取停车场树和车库
        /// </summary>
        /// <param name="communityDeptId"></param>
        /// <returns></returns>
        List<CustomTreeNodeModel> GetParkingSpaceAndCarPortTree(int communityDeptId, ref Dictionary<string, List<string>> dics, string carPortState);

        /// <summary>
        /// 获取停车场树和车库
        /// </summary>
        /// <param name="communityDeptId"></param>
        /// <returns></returns>
        List<TreeNodeModel> GetParkingSpaceAndCarPortTree(int communityDeptId);

        /// <summary>
        /// 获取停车位树
        /// </summary>
        /// <param name="parkingId"></param>
        /// <returns></returns>
        List<CustomTreeNodeModel> GetAsynParkingSpaceTree(string parkingId);

        /// <summary>
        /// 获取停车位树
        /// </summary>
        /// <param name="parkingId"></param>
        /// <param name="carportState">车位状态</param>
        /// <returns></returns>
        List<AsynTreeNodeModel> GetAsynParkingSpaceTree(string parkingId, int carportState);

        /// <summary>
        /// 获取停车位树
        /// </summary>
        /// <param name="parkingId"></param>
        /// <param name="carportStateAndType">车位状态/车位类型</param>
        /// <returns></returns>
        List<CustomTreeNodeModel> GetParkingSpaceCarportStateAndType(string parkingId, string carportStateAndType);



        /// <summary>
        /// 获取字典信息
        /// </summary>
        /// <param name="dictionaryId">字典的ID</param>
        /// <returns></returns>
        List<DictionaryModel> GetDictionaryModels(int dictionaryId);
        /// <summary>
        /// 获取字典
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        List<DictionaryModel> GetDictionaryModels(string code);

        /// <summary>
        /// 获取Sec_DetInfo
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        SEC_DeptDTO GetSecDeptInfo(int deptId);

        /// <summary>
        /// 获取登陆用户的信息
        /// </summary>
        /// <param name="userId">用户的ID</param>
        /// <returns></returns>
        SEC_AdminUserDTO GetAdminUser(string userId);

        /// <summary>
        /// 获取操作人员信息
        /// </summary>
        /// <param name="userId">用户的IDS</param>
        /// <returns></returns>
        List<SEC_AdminUserDTO> GetAdminUsers(int?[] userId);

        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <param name="userId">用户的ID</param>
        /// <returns></returns>
        SEC_AdminUserDTO Login(string user, string pwd);

        /// <summary>
        /// 获取DeptInfos
        /// </summary>
        /// <param name="deptIds">deptIds</param>
        /// <returns></returns>
        List<DeptInfo> GetDeptsByIds(List<int?> deptIds);

        /// <summary>
        /// 根据车位ID返回车位名称
        /// </summary>
        /// <param name="parkingIds"></param>
        /// <returns></returns>
        Dictionary<int, string> GetParkingSpace(List<int?> parkingIds);

        /// <summary>
        /// 获取停车位关联房屋ID
        /// </summary>
        /// <param name="parkingSpaceId"></param>
        /// <returns></returns>
        int GetHoseDeptIdByParkingSpaceId(int parkingSpaceId);

    }
}