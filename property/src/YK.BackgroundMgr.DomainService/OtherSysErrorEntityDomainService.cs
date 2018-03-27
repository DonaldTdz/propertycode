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
	public partial class OtherSysErrorEntityDomainService
	{
		public bool InsertOtherSysErrorEntity(OtherSysErrorEntity domainOtherSysErrorEntity)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.OtherSysErrorEntityRepository.Add(domainOtherSysErrorEntity);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateOtherSysErrorEntity(OtherSysErrorEntity domainOtherSysErrorEntity)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.OtherSysErrorEntityRepository.Update(domainOtherSysErrorEntity);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteOtherSysErrorEntity(object id)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                _BackgroundMgrUnitOfWork.OtherSysErrorEntityRepository.Delete(id);
                _BackgroundMgrUnitOfWork.Commit();
                return true;
            }
        }

		public OtherSysErrorEntity GetOtherSysErrorEntityByKey(object id)
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                return _BackgroundMgrUnitOfWork.OtherSysErrorEntityRepository.GetByKey(id);
            }
        }

        public List<OtherSysErrorEntity> GetOtherSysErrorEntitys()
        {
            using (var _BackgroundMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IBackgroundMgrUnitOfWork>())
            {
                return _BackgroundMgrUnitOfWork.OtherSysErrorEntityRepository.GetAll().ToList();
            }
        }
	}
}
