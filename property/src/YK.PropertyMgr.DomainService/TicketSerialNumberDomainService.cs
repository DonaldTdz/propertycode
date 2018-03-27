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
	public partial class TicketSerialNumberDomainService
	{
		public bool InsertTicketSerialNumber(TicketSerialNumber domainTicketSerialNumber)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.TicketSerialNumberRepository.Add(domainTicketSerialNumber);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateTicketSerialNumber(TicketSerialNumber domainTicketSerialNumber)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.TicketSerialNumberRepository.Update(domainTicketSerialNumber);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteTicketSerialNumber(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.TicketSerialNumberRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<TicketSerialNumber> GetTicketSerialNumbers()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.TicketSerialNumberRepository.GetAll().ToList();
            }
        }

		public TicketSerialNumber GetTicketSerialNumberByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.TicketSerialNumberRepository.GetByKey(id);
            }
        }

		public IList<TicketSerialNumber> Paging(int PageIndex, int PageSize, Expression<Func<TicketSerialNumber, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.TicketSerialNumberRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
