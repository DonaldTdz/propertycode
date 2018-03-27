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
	public partial class PreferentialRecordDomainService
	{
		public bool InsertPreferentialRecord(PreferentialRecord domainPreferentialRecord)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PreferentialRecordRepository.Add(domainPreferentialRecord);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdatePreferentialRecord(PreferentialRecord domainPreferentialRecord)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PreferentialRecordRepository.Update(domainPreferentialRecord);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeletePreferentialRecord(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.PreferentialRecordRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<PreferentialRecord> GetPreferentialRecords()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PreferentialRecordRepository.GetAll().ToList();
            }
        }

		public PreferentialRecord GetPreferentialRecordByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PreferentialRecordRepository.GetByKey(id);
            }
        }

		public IList<PreferentialRecord> Paging(int PageIndex, int PageSize, Expression<Func<PreferentialRecord, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.PreferentialRecordRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
