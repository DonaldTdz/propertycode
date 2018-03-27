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
	public partial class CommunityConfigDomainService
	{
		public bool InsertCommunityConfig(CommunityConfig domainCommunityConfig)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.CommunityConfigRepository.Add(domainCommunityConfig);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateCommunityConfig(CommunityConfig domainCommunityConfig)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.CommunityConfigRepository.Update(domainCommunityConfig);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteCommunityConfig(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.CommunityConfigRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<CommunityConfig> GetCommunityConfigs()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.CommunityConfigRepository.GetAll().ToList();
            }
        }

		public CommunityConfig GetCommunityConfigByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.CommunityConfigRepository.GetByKey(id);
            }
        }

		public IList<CommunityConfig> Paging(int PageIndex, int PageSize, Expression<Func<CommunityConfig, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.CommunityConfigRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
