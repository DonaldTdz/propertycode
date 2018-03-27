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
    public partial class MeterDomainService
    {
        /// <summary>
        /// 获取单个Meter
        /// </summary>
        /// <returns></returns>
        public Meter GetMeterSingle(Expression<Func<Meter, bool>> where)
        {
            Meter model = null;
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                model = propertyMgrUnitOfWork.MeterRepository.GetAll().Where(where).FirstOrDefault();
                return model;
            }
        }
        /// <summary>
        /// 根据条件获取Meter集合
        /// </summary>
        /// <returns></returns>
        public List<Meter> GetMeterList(Expression<Func<Meter, bool>> where)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.MeterRepository.GetAll().Where(where).ToList();
            }
        }


    }
}
