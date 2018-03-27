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
	public partial class EntranceUserDetailDomainService
	{
		public bool InsertEntranceUserDetail(EntranceUserDetail domainEntranceUserDetail)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.EntranceUserDetailRepository.Add(domainEntranceUserDetail);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateEntranceUserDetail(EntranceUserDetail domainEntranceUserDetail)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.EntranceUserDetailRepository.Update(domainEntranceUserDetail);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteEntranceUserDetail(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.EntranceUserDetailRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<EntranceUserDetail> GetEntranceUserDetails()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.EntranceUserDetailRepository.GetAll().ToList();
            }
        }

		public EntranceUserDetail GetEntranceUserDetailByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.EntranceUserDetailRepository.GetByKey(id);
            }
        }

		public IList<EntranceUserDetail> Paging(int PageIndex, int PageSize, Expression<Func<EntranceUserDetail, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.EntranceUserDetailRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
