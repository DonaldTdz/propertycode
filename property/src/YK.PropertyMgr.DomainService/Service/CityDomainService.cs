using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Practices.Unity;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.RepositoryContract;
using System.Linq.Expressions;

namespace YK.PropertyMgr.DomainService
{
    public partial class CityDomainService
    {
        /// <summary>
        /// 根据条件获取City集合
        /// </summary>
        /// <returns></returns>
        public List<City> GetCityList(Expression<Func<City, bool>> where)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.CityRepository.GetAll().Where(where).ToList();
            }
        }
    }
}
