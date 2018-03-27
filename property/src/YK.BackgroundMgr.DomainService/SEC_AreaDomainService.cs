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
	public partial class SEC_AreaDomainService
	{
		public bool InsertSEC_Area(SEC_Area domainSEC_Area)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.SEC_AreaRepository.Add(domainSEC_Area);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateSEC_Area(SEC_Area domainSEC_Area)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.SEC_AreaRepository.Update(domainSEC_Area);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteSEC_Area(object id)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.SEC_AreaRepository.Delete(id);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

		public SEC_Area GetSEC_AreaByKey(object id)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                return _BackgroundMgrUnitOfWork.SEC_AreaRepository.GetByKey(id);
            }
        }

        public List<SEC_Area> GetSEC_Areas()
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                return _BackgroundMgrUnitOfWork.SEC_AreaRepository.GetAll().ToList();
            }
        }
	}
}
