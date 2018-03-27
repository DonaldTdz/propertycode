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
	public partial class PaymentDiscountInfoDomainService
	{
		public bool InsertPaymentDiscountInfo(PaymentDiscountInfo domainPaymentDiscountInfo)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PaymentDiscountInfoRepository.Add(domainPaymentDiscountInfo);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdatePaymentDiscountInfo(PaymentDiscountInfo domainPaymentDiscountInfo)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PaymentDiscountInfoRepository.Update(domainPaymentDiscountInfo);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeletePaymentDiscountInfo(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PaymentDiscountInfoRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<PaymentDiscountInfo> GetPaymentDiscountInfos()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PaymentDiscountInfoRepository.GetAll().ToList();
            }
        }

		public PaymentDiscountInfo GetPaymentDiscountInfoByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PaymentDiscountInfoRepository.GetByKey(id);
            }
        }

		public IList<PaymentDiscountInfo> Paging(int PageIndex, int PageSize, Expression<Func<PaymentDiscountInfo, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PaymentDiscountInfoRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
