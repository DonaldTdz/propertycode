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
	public partial class AlipayChargeBillSynchronizerDetailDomainService
	{
		public bool InsertAlipayChargeBillSynchronizerDetail(AlipayChargeBillSynchronizerDetail domainAlipayChargeBillSynchronizerDetail)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayChargeBillSynchronizerDetailRepository.Add(domainAlipayChargeBillSynchronizerDetail);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateAlipayChargeBillSynchronizerDetail(AlipayChargeBillSynchronizerDetail domainAlipayChargeBillSynchronizerDetail)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayChargeBillSynchronizerDetailRepository.Update(domainAlipayChargeBillSynchronizerDetail);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteAlipayChargeBillSynchronizerDetail(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayChargeBillSynchronizerDetailRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<AlipayChargeBillSynchronizerDetail> GetAlipayChargeBillSynchronizerDetails()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayChargeBillSynchronizerDetailRepository.GetAll().ToList();
            }
        }

		public AlipayChargeBillSynchronizerDetail GetAlipayChargeBillSynchronizerDetailByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayChargeBillSynchronizerDetailRepository.GetByKey(id);
            }
        }

		public IList<AlipayChargeBillSynchronizerDetail> Paging(int PageIndex, int PageSize, Expression<Func<AlipayChargeBillSynchronizerDetail, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayChargeBillSynchronizerDetailRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
