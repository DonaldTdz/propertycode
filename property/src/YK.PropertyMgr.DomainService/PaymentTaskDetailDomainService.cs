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
	public partial class PaymentTaskDetailDomainService
	{
		public bool InsertPaymentTaskDetail(PaymentTaskDetail domainPaymentTaskDetail)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PaymentTaskDetailRepository.Add(domainPaymentTaskDetail);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdatePaymentTaskDetail(PaymentTaskDetail domainPaymentTaskDetail)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PaymentTaskDetailRepository.Update(domainPaymentTaskDetail);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeletePaymentTaskDetail(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PaymentTaskDetailRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<PaymentTaskDetail> GetPaymentTaskDetails()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll().ToList();
            }
        }

		public PaymentTaskDetail GetPaymentTaskDetailByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetByKey(id);
            }
        }

		public IList<PaymentTaskDetail> Paging(int PageIndex, int PageSize, Expression<Func<PaymentTaskDetail, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PaymentTaskDetailRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
