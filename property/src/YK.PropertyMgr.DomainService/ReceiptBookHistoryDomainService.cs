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
	public partial class ReceiptBookHistoryDomainService
	{
		public bool InsertReceiptBookHistory(ReceiptBookHistory domainReceiptBookHistory)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ReceiptBookHistoryRepository.Add(domainReceiptBookHistory);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateReceiptBookHistory(ReceiptBookHistory domainReceiptBookHistory)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ReceiptBookHistoryRepository.Update(domainReceiptBookHistory);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteReceiptBookHistory(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ReceiptBookHistoryRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<ReceiptBookHistory> GetReceiptBookHistorys()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ReceiptBookHistoryRepository.GetAll().ToList();
            }
        }

		public ReceiptBookHistory GetReceiptBookHistoryByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ReceiptBookHistoryRepository.GetByKey(id);
            }
        }

		public IList<ReceiptBookHistory> Paging(int PageIndex, int PageSize, Expression<Func<ReceiptBookHistory, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ReceiptBookHistoryRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
