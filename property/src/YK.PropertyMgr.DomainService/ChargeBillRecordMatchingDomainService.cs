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
	public partial class ChargeBillRecordMatchingDomainService
	{
		public bool InsertChargeBillRecordMatching(ChargeBillRecordMatching domainChargeBillRecordMatching)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.Add(domainChargeBillRecordMatching);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateChargeBillRecordMatching(ChargeBillRecordMatching domainChargeBillRecordMatching)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.Update(domainChargeBillRecordMatching);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteChargeBillRecordMatching(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<ChargeBillRecordMatching> GetChargeBillRecordMatchings()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll().ToList();
            }
        }

		public ChargeBillRecordMatching GetChargeBillRecordMatchingByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetByKey(id);
            }
        }

		public IList<ChargeBillRecordMatching> Paging(int PageIndex, int PageSize, Expression<Func<ChargeBillRecordMatching, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
