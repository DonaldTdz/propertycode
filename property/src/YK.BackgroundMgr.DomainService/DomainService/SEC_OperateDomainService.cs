using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using YK.BackgroundMgr.RepositoryContract;
using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.Crosscuting;
using YK.BackgroundMgr.ApplicationDTO;

namespace YK.BackgroundMgr.DomainService
{
	public partial class SEC_OperateDomainService
	{
        public List<OperateCodeAndRoleInfo> GetUserRoleAndOperates(string userName)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var PropertySysId = 1010;
                var operateQuery = from m in _BackgroundMgrUnitOfWork.SEC_ModuleRepository.GetAll()
                                   from o in m.SEC_Operates
                                   where m.IsUsed == true
                                   && (m.Id == PropertySysId || ("." +m.Code).Contains(".1010."))
                                   select o.Id;
                var userOperateAndRoles = from operate in _BackgroundMgrUnitOfWork.SEC_OperateRepository.GetAll()
                                          from role in operate.SEC_Roles
                                          from adminuser in role.SEC_AdminUsers
                                          where adminuser.UserName == userName && operate.IsUsed
                                          where operateQuery.Any(o => o == operate.Id)
                                          select new OperateCodeAndRoleInfo()
                                          {
                                              Code = operate.Code,
                                              SEC_Role_Id = role.Id.Value
                                          };
                //var userOperateAndRoles = from operate in _BackgroundMgrUnitOfWork.SEC_OperateRepository.GetAll()
                //                 from role in operate.SEC_Roles
                //                 from adminuser in role.SEC_AdminUsers
                //                 where adminuser.UserName == userName && operate.IsUsed
                //                          select new OperateCodeAndRoleInfo() { 
                //                      Code = operate.Code,
                //                      SEC_Role_Id = role.Id.Value
                //                 };

                return userOperateAndRoles.ToList();
            }
        }
	}
}
