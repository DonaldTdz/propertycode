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
	public partial class ClientPaymentLogDomainService
	{
		public bool InsertClientPaymentLog(ClientPaymentLog domainClientPaymentLog)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ClientPaymentLogRepository.Add(domainClientPaymentLog);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateClientPaymentLog(ClientPaymentLog domainClientPaymentLog)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ClientPaymentLogRepository.Update(domainClientPaymentLog);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteClientPaymentLog(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ClientPaymentLogRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<ClientPaymentLog> GetClientPaymentLogs()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ClientPaymentLogRepository.GetAll().ToList();
            }
        }

		public ClientPaymentLog GetClientPaymentLogByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ClientPaymentLogRepository.GetByKey(id);
            }
        }

		public IList<ClientPaymentLog> Paging(int PageIndex, int PageSize, Expression<Func<ClientPaymentLog, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ClientPaymentLogRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
