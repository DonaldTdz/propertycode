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
	public partial class PrepayAccountLogDomainService
	{
		public bool InsertPrepayAccountLog(PrepayAccountLog domainPrepayAccountLog)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PrepayAccountLogRepository.Add(domainPrepayAccountLog);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdatePrepayAccountLog(PrepayAccountLog domainPrepayAccountLog)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PrepayAccountLogRepository.Update(domainPrepayAccountLog);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeletePrepayAccountLog(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PrepayAccountLogRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<PrepayAccountLog> GetPrepayAccountLogs()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PrepayAccountLogRepository.GetAll().ToList();
            }
        }

		public PrepayAccountLog GetPrepayAccountLogByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PrepayAccountLogRepository.GetByKey(id);
            }
        }

		public IList<PrepayAccountLog> Paging(int PageIndex, int PageSize, Expression<Func<PrepayAccountLog, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PrepayAccountLogRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
