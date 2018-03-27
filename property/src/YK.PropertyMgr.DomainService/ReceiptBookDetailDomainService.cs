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
	public partial class ReceiptBookDetailDomainService
	{
		public bool InsertReceiptBookDetail(ReceiptBookDetail domainReceiptBookDetail)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ReceiptBookDetailRepository.Add(domainReceiptBookDetail);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateReceiptBookDetail(ReceiptBookDetail domainReceiptBookDetail)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ReceiptBookDetailRepository.Update(domainReceiptBookDetail);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteReceiptBookDetail(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ReceiptBookDetailRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<ReceiptBookDetail> GetReceiptBookDetails()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ReceiptBookDetailRepository.GetAll().ToList();
            }
        }

		public ReceiptBookDetail GetReceiptBookDetailByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ReceiptBookDetailRepository.GetByKey(id);
            }
        }

		public IList<ReceiptBookDetail> Paging(int PageIndex, int PageSize, Expression<Func<ReceiptBookDetail, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ReceiptBookDetailRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
