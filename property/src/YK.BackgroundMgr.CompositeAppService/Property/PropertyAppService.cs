using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.ApplicationService;
using YK.BackgroundMgr.DomainService;
using YK.BackgroundMgr.PresentationService;

namespace YK.BackgroundMgr.CompositeAppService
{
    public class PropertyAppService : IPropertyService
    {
        public List<CustomTreeNodeModel> GetAsynDeptTree(string userName, int parentId, int?[] TypeArry)
        {
            return new SEC_DeptAppService().GetAsynDeptTree(userName, parentId, TypeArry);
        }
        public List<AsynTreeNodeModel> GetAsynDeptTree(string userName, int parentId, int?[] TypeArry, ref Dictionary<string, List<string>> dic)
        {
            return new SEC_DeptAppService().GetAsynDeptTree(userName, parentId, TypeArry, ref dic);
        }

        public List<CustomTreeNodeModel> GetDeptTree(string userName, string keyWord)
        {
            return new SEC_DeptAppService().GetDeptTree(userName, keyWord);
        }

        public List<CustomTreeNodeModel> GetDeptTree(string userName, int?[] types)
        {
            return new SEC_DeptAppService().GetDeptTree(userName, types);
        }

        public List<CustomTreeNodeModel> GetAsynParkingSpaceTree(string parkingId)
        {
            return new CarportDomainService().GetAsynParkingSpaceTree(parkingId);
        }
        public List<AsynTreeNodeModel> GetAsynParkingSpaceTree(string parkingId, int carportState)
        {
            return new CarportDomainService().GetAsynParkingSpaceTree(parkingId, carportState);
        }

        public List<AsynTreeNodeModel> GetAsynParkingTree(int communityDeptId)
        {
            return new CarportDomainService().GetAsynParkingTree(communityDeptId);
        }
        public List<AsynTreeNodeModel> GetAsynParkingTree(int communityDeptId, ref Dictionary<string, List<string>> dics)
        {
            return new CarportDomainService().GetAsynParkingTree(communityDeptId, ref dics);
        }

        public List<DictionaryModel> GetDictionaryModels(int dictionaryId)
        {
            var sys_DictionaryItems = new Sys_DictionaryAppService().GetDictionaryModels(dictionaryId);
            return sys_DictionaryItems;
        }

        public List<DictionaryModel> GetDictionaryModels(string code)
        {
            var sys_DictionaryItems = new Sys_DictionaryAppService().GetDictionaryModels(code);
            return sys_DictionaryItems;
        }
        public SEC_DeptDTO GetSecDeptInfo(int deptId)
        {
            return new SEC_DeptAppService().GetSEC_DeptByKey(deptId);
        }

        public List<CustomTreeNodeModel> GetParkingSpaceAndCarPortTree(int communityDeptId, ref Dictionary<string, List<string>> dics, string carPortState)
        {
            return new CarportDomainService().GetParkingSpaceAndCarPortTree(communityDeptId, ref dics, carPortState);
        }
        public List<TreeNodeModel> GetParkingSpaceAndCarPortTree(int communityDeptId)
        {
            return new CarportDomainService().GetParkingSpaceAndCarPortTree(communityDeptId);
        }
        
        public List<CustomTreeNodeModel> GetParkingSpaceCarportStateAndType(string parkingId, string carportStateAndType)
        {
            return new CarportDomainService().GetParkingSpaceCarportStateAndType(parkingId, carportStateAndType);
        }

        public SEC_AdminUserDTO GetAdminUser(string userId)
        {
            return new SEC_AdminUserDTO();
        }

        public List<SEC_AdminUserDTO> GetAdminUsers(int?[] ids)
        {
            return new SEC_AdminUserAppService().GetSEC_AdminUsers(ids);
        }

        public SEC_AdminUserDTO Login(string user, string pwd)
        {
            return new SEC_AdminUserAppService().ValidateSEC_AdminUser(user, pwd);
        }
        public List<DeptInfo> GetDeptsByIds(List<int?> deptIds)
        {
            return new SEC_DeptDomainService().GetDeptsByIds(deptIds);
        }

        public Dictionary<int,string> GetParkingSpace(List<int?> ids)
        {
            return new CarportDomainService().GetParkingSpace(ids);
        }

        public int GetHoseDeptIdByParkingSpaceId(int parkingSpaceId)
        {
            return new CarportDomainService().GetHoseDeptIdByParkingSpaceId(parkingSpaceId);
        }
    }
}
