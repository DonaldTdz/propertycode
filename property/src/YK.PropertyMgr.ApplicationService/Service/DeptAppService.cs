using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.ApplicationMapper;
using YK.BackgroundMgr.DomainInterface;
using YK.BackgroundMgr.PresentationService;

namespace YK.PropertyMgr.ApplicationService.Service
{
    public partial class DeptAppService
    {

        public List<CustomTreeNodeModel> GetDeptTree(SEC_AdminUserDTO adminUserInfo, int Pid, string strFilter)
        {

            //string[] strFilterarry = strFilter.Split(';');

            //List<int?> intlist = new List<int?>();

            //foreach (string c in strFilterarry)
            //{
            //    if(!string.IsNullOrEmpty(c))
            //     intlist.Add(int.Parse(c));
            //}
            //List<AsynTreeNodeModel> treeNodes = new List<AsynTreeNodeModel>();
            //treeNodes = PresentationServiceHelper.LookUp<IPropertyService>().GetAsynDeptTree(adminUserInfo.UserName, Pid, intlist.ToArray<int?>());

            int?[] intArry = new int?[] { };
            if (!string.IsNullOrEmpty(strFilter))
            {
                intArry = Array.ConvertAll<string, int?>(strFilter.Split(';').Where(s => !string.IsNullOrEmpty(s)).ToArray(), s => int.Parse(s));
            }

            List<CustomTreeNodeModel> treeNodes = new List<CustomTreeNodeModel>();
            treeNodes = PresentationServiceHelper.LookUp<IPropertyService>().GetAsynDeptTree(adminUserInfo.UserName, Pid, intArry);

            //var treeNodesFilter = from Ftreenodes in treeNodes
            //                      where    

            return treeNodes;
        }
        public List<AsynTreeNodeModel> GetDeptTree(SEC_AdminUserDTO adminUserInfo, int Pid, string strFilter, ref Dictionary<string, List<string>> dic)
        {
            int?[] intArry = new int?[] { };
            if (!string.IsNullOrEmpty(strFilter))
            {
                intArry = Array.ConvertAll<string, int?>(strFilter.Split(';').Where(s => !string.IsNullOrEmpty(s)).ToArray(), s => int.Parse(s));
            }
            List<AsynTreeNodeModel> treeNodes = new List<AsynTreeNodeModel>();
            treeNodes = PresentationServiceHelper.LookUp<IPropertyService>().GetAsynDeptTree(adminUserInfo.UserName, Pid, intArry, ref dic);
            return treeNodes;
        }

        public List<CustomTreeNodeModel> GetDeptTree(string userName, string keyWord)
        {
            return PresentationServiceHelper.LookUp<IPropertyService>().GetDeptTree(userName, keyWord);
        }

        public List<CustomTreeNodeModel> GetDeptCustomTree(string userName, string strFilter)
        {
            int?[] intArry = new int?[] { };
            if (!string.IsNullOrEmpty(strFilter))
            {
                intArry = Array.ConvertAll<string, int?>(strFilter.Split(';').Where(s => !string.IsNullOrEmpty(s)).ToArray(), s => int.Parse(s));
            }
            return PresentationServiceHelper.LookUp<IPropertyService>().GetDeptTree(userName, intArry);
        }


        public SEC_DeptDTO GetDeptInfoById(string ID)
        {
            return SEC_DeptMappers.ChangeSEC_DeptToDTO(DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoById(ID));
        }

        public List<DeptInfo> GetComDeptList(string UserName)
        {
            return DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetComDeptByUserName(UserName);
        }
        public List<DeptInfo> GetDeptHouseList(string comDeptId)
        {
            return DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptListByCommunityDeptId(comDeptId);
        }
        public List<DeptInfo> GetDeptHouseList(List<int?> builds)
        {
            return DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptListByBuildDeptId(builds);
        }
        public List<DeptInfo> GetBuildsByComDeptId(int comDeptId)
        {
            return DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildsByComDeptId(comDeptId).ToList();
        }

        public HouseInfo GetHouseInfo(int houdeDeptId, int communityId)
        {
            return DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseInfo(communityId, houdeDeptId, true);
        }

        public List<DeptInfo> GetCommunityDeptInfoByPropertyId(int? ProDeptId)
        {
            return DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoListByPropertyId(ProDeptId).ToList();
        }
        


    }
}
