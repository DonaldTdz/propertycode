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
    public partial class SEC_AdminUserAppService
    {
        /// <summary>
        /// 根据用户名和密码获取用户信息，用于用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passwork">迷茫期</param>
        /// <returns>用户信息</returns>
        public SEC_AdminUserDTO ValidateSEC_AdminUser(string userName, string password)
        {
            var domainSEC_AdminUser = SEC_AdminUserService.ValidateSEC_AdminUser(userName, password);
            if (domainSEC_AdminUser == null)
            {
                return null;
            }
            else
            {
                return AdminUsersMapper.ChangeSEC_AdminUserToDTO(domainSEC_AdminUser);
            }
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public List<SEC_AdminUserDTO> GetSEC_AdminUsers(int?[] ids)
        {
            return AdminUsersMapper.ChangeSEC_AdminUserToDTOs(SEC_AdminUserService.GetSEC_AdminUsers(ids));
        }
    }
}
