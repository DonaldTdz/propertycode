using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.ApplicationMapper;
using YK.BackgroundMgr.DomainService;
using YK.BackgroundMgr.PresentationService;

namespace YK.BackgroundMgr.ApplicationService
{
    public partial class SEC_DeptAppService
    {
        public List<CustomTreeNodeModel> GetAsynDeptTree(string userName, int parentId, int?[] TypeArry)
        {
            return SEC_DeptService.GetAsynDeptTree(userName, parentId, TypeArry);
        }

        public List<AsynTreeNodeModel> GetAsynDeptTree(string userName, int parentId, int?[] TypeArry, ref Dictionary<string, List<string>> dic)
        {
            return SEC_DeptService.GetAsynDeptTree(userName, parentId, TypeArry, ref dic);
        }

        public List<CustomTreeNodeModel> GetDeptTree(string userName, string keyWord)
        {
            return SEC_DeptService.GetDeptTree(userName, keyWord);
        }

        public List<CustomTreeNodeModel> GetDeptTree(string userName, int?[] types)
        {
            return SEC_DeptService.GetDeptTree(userName, types);
        }
    }
}
