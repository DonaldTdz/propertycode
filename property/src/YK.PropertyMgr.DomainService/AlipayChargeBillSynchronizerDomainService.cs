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
	public partial class AlipayChargeBillSynchronizerDomainService
	{
		public bool InsertAlipayChargeBillSynchronizer(AlipayChargeBillSynchronizer domainAlipayChargeBillSynchronizer)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayChargeBillSynchronizerRepository.Add(domainAlipayChargeBillSynchronizer);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateAlipayChargeBillSynchronizer(AlipayChargeBillSynchronizer domainAlipayChargeBillSynchronizer)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayChargeBillSynchronizerRepository.Update(domainAlipayChargeBillSynchronizer);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteAlipayChargeBillSynchronizer(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayChargeBillSynchronizerRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<AlipayChargeBillSynchronizer> GetAlipayChargeBillSynchronizers()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayChargeBillSynchronizerRepository.GetAll().ToList();
            }
        }

		public AlipayChargeBillSynchronizer GetAlipayChargeBillSynchronizerByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayChargeBillSynchronizerRepository.GetByKey(id);
            }
        }

		public IList<AlipayChargeBillSynchronizer> Paging(int PageIndex, int PageSize, Expression<Func<AlipayChargeBillSynchronizer, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayChargeBillSynchronizerRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
