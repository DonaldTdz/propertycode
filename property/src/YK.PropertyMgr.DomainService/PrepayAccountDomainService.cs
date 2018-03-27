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
	public partial class PrepayAccountDomainService
	{
		public bool InsertPrepayAccount(PrepayAccount domainPrepayAccount)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PrepayAccountRepository.Add(domainPrepayAccount);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdatePrepayAccount(PrepayAccount domainPrepayAccount)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PrepayAccountRepository.Update(domainPrepayAccount);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeletePrepayAccount(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PrepayAccountRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<PrepayAccount> GetPrepayAccounts()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PrepayAccountRepository.GetAll().ToList();
            }
        }

		public PrepayAccount GetPrepayAccountByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PrepayAccountRepository.GetByKey(id);
            }
        }

		public IList<PrepayAccount> Paging(int PageIndex, int PageSize, Expression<Func<PrepayAccount, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PrepayAccountRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
