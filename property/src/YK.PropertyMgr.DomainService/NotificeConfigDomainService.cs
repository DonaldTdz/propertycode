using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using Microsoft.Practices.Unity;
using YK.PropertyMgr.RepositoryContract;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.Crosscuting;

namespace YK.PropertyMgr.DomainService
{
	public partial class NotificeConfigDomainService
	{
		public bool InsertNotificeConfig(NotificeConfig domainNotificeConfig)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.NotificeConfigRepository.Add(domainNotificeConfig);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateNotificeConfig(NotificeConfig domainNotificeConfig)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.NotificeConfigRepository.Update(domainNotificeConfig);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteNotificeConfig(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.NotificeConfigRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<NotificeConfig> GetNotificeConfigs()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.NotificeConfigRepository.GetAll().ToList();
            }
        }

		public NotificeConfig GetNotificeConfigByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.NotificeConfigRepository.GetByKey(id);
            }
        }

		public IList<NotificeConfig> Paging(int PageIndex, int PageSize, Expression<Func<NotificeConfig, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.NotificeConfigRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
