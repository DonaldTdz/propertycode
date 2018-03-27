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
	public partial class TemplatePrintRecordDomainService
	{
		public bool InsertTemplatePrintRecord(TemplatePrintRecord domainTemplatePrintRecord)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.TemplatePrintRecordRepository.Add(domainTemplatePrintRecord);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateTemplatePrintRecord(TemplatePrintRecord domainTemplatePrintRecord)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.TemplatePrintRecordRepository.Update(domainTemplatePrintRecord);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteTemplatePrintRecord(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.TemplatePrintRecordRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<TemplatePrintRecord> GetTemplatePrintRecords()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.TemplatePrintRecordRepository.GetAll().ToList();
            }
        }

		public TemplatePrintRecord GetTemplatePrintRecordByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.TemplatePrintRecordRepository.GetByKey(id);
            }
        }

		public IList<TemplatePrintRecord> Paging(int PageIndex, int PageSize, Expression<Func<TemplatePrintRecord, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.TemplatePrintRecordRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
