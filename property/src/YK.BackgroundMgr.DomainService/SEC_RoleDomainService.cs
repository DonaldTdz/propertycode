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
	public partial class SEC_RoleDomainService
	{
		public bool InsertSEC_Role(SEC_Role domainSEC_Role)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.SEC_RoleRepository.Add(domainSEC_Role);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateSEC_Role(SEC_Role domainSEC_Role)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.SEC_RoleRepository.Update(domainSEC_Role);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteSEC_Role(object id)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.SEC_RoleRepository.Delete(id);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

		public SEC_Role GetSEC_RoleByKey(object id)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                return _BackgroundMgrUnitOfWork.SEC_RoleRepository.GetByKey(id);
            }
        }

        public List<SEC_Role> GetSEC_Roles()
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                return _BackgroundMgrUnitOfWork.SEC_RoleRepository.GetAll().ToList();
            }
        }
	}
}
