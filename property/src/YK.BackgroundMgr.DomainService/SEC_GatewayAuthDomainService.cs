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
	public partial class SEC_GatewayAuthDomainService
	{
		public bool InsertSEC_GatewayAuth(SEC_GatewayAuth domainSEC_GatewayAuth)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.SEC_GatewayAuthRepository.Add(domainSEC_GatewayAuth);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateSEC_GatewayAuth(SEC_GatewayAuth domainSEC_GatewayAuth)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.SEC_GatewayAuthRepository.Update(domainSEC_GatewayAuth);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteSEC_GatewayAuth(object id)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.SEC_GatewayAuthRepository.Delete(id);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

		public SEC_GatewayAuth GetSEC_GatewayAuthByKey(object id)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                return _BackgroundMgrUnitOfWork.SEC_GatewayAuthRepository.GetByKey(id);
            }
        }

        public List<SEC_GatewayAuth> GetSEC_GatewayAuths()
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                return _BackgroundMgrUnitOfWork.SEC_GatewayAuthRepository.GetAll().ToList();
            }
        }
	}
}
