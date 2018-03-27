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
	public partial class PaymentTasksDomainService
	{
		public bool InsertPaymentTasks(PaymentTasks domainPaymentTasks)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PaymentTasksRepository.Add(domainPaymentTasks);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdatePaymentTasks(PaymentTasks domainPaymentTasks)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PaymentTasksRepository.Update(domainPaymentTasks);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeletePaymentTasks(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PaymentTasksRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<PaymentTasks> GetPaymentTaskss()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PaymentTasksRepository.GetAll().ToList();
            }
        }

		public PaymentTasks GetPaymentTasksByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PaymentTasksRepository.GetByKey(id);
            }
        }

		public IList<PaymentTasks> Paging(int PageIndex, int PageSize, Expression<Func<PaymentTasks, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PaymentTasksRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
