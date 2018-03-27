using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.ApplicationDTO;
using YK.BackgroundMgr.ApplicationMapper;
using YK.BackgroundMgr.DomainService;

namespace YK.BackgroundMgr.ApplicationService
{
	public partial class SEC_ModuleAppService
	{        
        /// <summary>
        /// 获取用户可以访问的模块节点
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>用户可以访问的模块节点</returns>
        public List<SEC_ModuleDTO> GetUserModuls(string userName)
        {
            var rootDomainSEC_Modules = SEC_ModuleService.GetUserModuls(userName);

            return SEC_ModuleMappers.ChangeSEC_ModuleToDTOs(rootDomainSEC_Modules);
        }
	}
}
