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
	public partial class SEC_GarageDomainService
	{
		public bool InsertSEC_Garage(SEC_Garage domainSEC_Garage)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.SEC_GarageRepository.Add(domainSEC_Garage);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateSEC_Garage(SEC_Garage domainSEC_Garage)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.SEC_GarageRepository.Update(domainSEC_Garage);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteSEC_Garage(object id)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.SEC_GarageRepository.Delete(id);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

		public SEC_Garage GetSEC_GarageByKey(object id)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                return _BackgroundMgrUnitOfWork.SEC_GarageRepository.GetByKey(id);
            }
        }

        public List<SEC_Garage> GetSEC_Garages()
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                return _BackgroundMgrUnitOfWork.SEC_GarageRepository.GetAll().ToList();
            }
        }
	}
}
