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
	public partial class PrepayAccountDetailDomainService
	{
		public bool InsertPrepayAccountDetail(PrepayAccountDetail domainPrepayAccountDetail)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PrepayAccountDetailRepository.Add(domainPrepayAccountDetail);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdatePrepayAccountDetail(PrepayAccountDetail domainPrepayAccountDetail)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PrepayAccountDetailRepository.Update(domainPrepayAccountDetail);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeletePrepayAccountDetail(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PrepayAccountDetailRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<PrepayAccountDetail> GetPrepayAccountDetails()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PrepayAccountDetailRepository.GetAll().ToList();
            }
        }

		public PrepayAccountDetail GetPrepayAccountDetailByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PrepayAccountDetailRepository.GetByKey(id);
            }
        }

		public IList<PrepayAccountDetail> Paging(int PageIndex, int PageSize, Expression<Func<PrepayAccountDetail, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PrepayAccountDetailRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
