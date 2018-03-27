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
	public partial class MeterReadRecordDomainService
	{
		public bool InsertMeterReadRecord(MeterReadRecord domainMeterReadRecord)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.MeterReadRecordRepository.Add(domainMeterReadRecord);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateMeterReadRecord(MeterReadRecord domainMeterReadRecord)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.MeterReadRecordRepository.Update(domainMeterReadRecord);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteMeterReadRecord(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.MeterReadRecordRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<MeterReadRecord> GetMeterReadRecords()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.MeterReadRecordRepository.GetAll().ToList();
            }
        }

		public MeterReadRecord GetMeterReadRecordByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.MeterReadRecordRepository.GetByKey(id);
            }
        }

		public IList<MeterReadRecord> Paging(int PageIndex, int PageSize, Expression<Func<MeterReadRecord, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.MeterReadRecordRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
