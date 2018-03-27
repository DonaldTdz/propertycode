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
    public partial class SEC_PropertyDomainService
    {
        public SEC_Property GetSEC_PropertyByDeptId(int deptId)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                return _BackgroundMgrUnitOfWork.SEC_PropertyRepository.GetAll().Where(o=>o.DeptId== deptId).FirstOrDefault();
            }
        }
    }
}
