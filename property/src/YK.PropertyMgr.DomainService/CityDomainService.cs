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
	public partial class CityDomainService
	{
		public bool InsertCity(City domainCity)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.CityRepository.Add(domainCity);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateCity(City domainCity)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.CityRepository.Update(domainCity);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteCity(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.CityRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<City> GetCitys()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.CityRepository.GetAll().ToList();
            }
        }

		public City GetCityByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.CityRepository.GetByKey(id);
            }
        }

		public IList<City> Paging(int PageIndex, int PageSize, Expression<Func<City, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.CityRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
