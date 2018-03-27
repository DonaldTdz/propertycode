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
	public partial class AlipayApiRequestLogDomainService
	{
		public bool InsertAlipayApiRequestLog(AlipayApiRequestLog domainAlipayApiRequestLog)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayApiRequestLogRepository.Add(domainAlipayApiRequestLog);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateAlipayApiRequestLog(AlipayApiRequestLog domainAlipayApiRequestLog)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayApiRequestLogRepository.Update(domainAlipayApiRequestLog);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteAlipayApiRequestLog(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayApiRequestLogRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<AlipayApiRequestLog> GetAlipayApiRequestLogs()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayApiRequestLogRepository.GetAll().ToList();
            }
        }

		public AlipayApiRequestLog GetAlipayApiRequestLogByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayApiRequestLogRepository.GetByKey(id);
            }
        }

		public IList<AlipayApiRequestLog> Paging(int PageIndex, int PageSize, Expression<Func<AlipayApiRequestLog, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayApiRequestLogRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
