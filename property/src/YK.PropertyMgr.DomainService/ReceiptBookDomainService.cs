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
	public partial class ReceiptBookDomainService
	{
		public bool InsertReceiptBook(ReceiptBook domainReceiptBook)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ReceiptBookRepository.Add(domainReceiptBook);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateReceiptBook(ReceiptBook domainReceiptBook)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ReceiptBookRepository.Update(domainReceiptBook);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteReceiptBook(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ReceiptBookRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<ReceiptBook> GetReceiptBooks()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ReceiptBookRepository.GetAll().ToList();
            }
        }

		public ReceiptBook GetReceiptBookByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ReceiptBookRepository.GetByKey(id);
            }
        }

		public IList<ReceiptBook> Paging(int PageIndex, int PageSize, Expression<Func<ReceiptBook, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ReceiptBookRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
