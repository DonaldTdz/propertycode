using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using YK.BackgroundMgr.RepositoryContract;
using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.Crosscuting;

namespace YK.BackgroundMgr.DomainService
{
    public partial class SEC_AdminUserDomainService
    {
        /// <summary>
        /// 根据用户名和密码获取用户信息，用于用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="passwork">迷茫期</param>
        /// <returns>用户信息</returns>
        public SEC_AdminUser ValidateSEC_AdminUser(string userName, string password)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                string encryptPassword = DataEncrypt.EncryptToDB(password);
                return _BackgroundMgrUnitOfWork.SEC_AdminUserRepository.GetAll().Where(r => r.UserName == userName && r.Password == encryptPassword).FirstOrDefault();
            }
        }

        public List<SEC_AdminUser> GetSEC_AdminUsers(int?[] ids)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                return _BackgroundMgrUnitOfWork.SEC_AdminUserRepository.GetAll().Where(o => ids.Contains(o.Id)).ToList();
            }
        }


        public IList<SEC_Role> GetRoleListByAdminUsers(int? Id)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {

                var AdminUsers =_BackgroundMgrUnitOfWork.SEC_AdminUserRepository.GetByKey(Id);
                 
                return AdminUsers.SEC_Roles.ToList();
                
            }
        }

    }
}
