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
    public partial class CountyDomainService
    {

        /// <summary>
        /// 根据条件获取集合
        /// </summary>
        /// <returns></returns>
        public List<County> GetCountryList(Expression<Func<County, bool>> where)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.CountyRepository.GetAll().Where(where).ToList();
            }
        }
    }
}
