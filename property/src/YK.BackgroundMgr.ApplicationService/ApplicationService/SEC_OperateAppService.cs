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
	public partial class SEC_OperateAppService
	{
        /// <summary>
        /// 根据用户名获取用户操作和角色对应的信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>操作和角色对应的信息</returns>
        public List<OperateCodeAndRoleInfo> GetUserRoleAndOperates(string userName)
        {
            return SEC_OperateService.GetUserRoleAndOperates(userName);
        }
	}
}
