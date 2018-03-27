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
	public partial class CountyDomainService
	{
		public bool InsertCounty(County domainCounty)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.CountyRepository.Add(domainCounty);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateCounty(County domainCounty)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.CountyRepository.Update(domainCounty);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteCounty(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.CountyRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<County> GetCountys()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.CountyRepository.GetAll().ToList();
            }
        }

		public County GetCountyByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.CountyRepository.GetByKey(id);
            }
        }

		public IList<County> Paging(int PageIndex, int PageSize, Expression<Func<County, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.CountyRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
