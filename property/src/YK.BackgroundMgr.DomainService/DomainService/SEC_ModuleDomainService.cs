using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Practices.Unity;
using YK.BackgroundMgr.RepositoryContract;
using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.Crosscuting;
using System.Data.Entity.SqlServer;

namespace YK.BackgroundMgr.DomainService
{
    public partial class SEC_ModuleDomainService
    {
        public List<SEC_Module> GetUserModuls(string userName)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                var userModuls = (from module in _BackgroundMgrUnitOfWork.SEC_ModuleRepository.GetAll()
                                  from role in module.SEC_Roles
                                  from adminuser in role.SEC_AdminUsers
                                  where adminuser.UserName == userName && module.IsUsed && module.IsShow
                                  select module);

                return _BackgroundMgrUnitOfWork.SEC_ModuleRepository.GetAll()
                    .Where(r => userModuls
                    .Any(s => ("." + s.Code + SqlFunctions.StringConvert((double?)s.Id).Trim() + ".").Contains("." + SqlFunctions.StringConvert((double?)r.Id).Trim() + ".") //父+自己
                           || ("." + r.Code).Contains("." + SqlFunctions.StringConvert((double?)s.Id).Trim() + ".")//子
                           ))//父
                           .Where(r=>r.IsShow && r.IsUsed).ToList();
            }
        }

    }
}
