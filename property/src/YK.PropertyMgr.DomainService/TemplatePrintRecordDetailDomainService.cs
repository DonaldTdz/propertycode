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
	public partial class TemplatePrintRecordDetailDomainService
	{
		public bool InsertTemplatePrintRecordDetail(TemplatePrintRecordDetail domainTemplatePrintRecordDetail)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.TemplatePrintRecordDetailRepository.Add(domainTemplatePrintRecordDetail);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateTemplatePrintRecordDetail(TemplatePrintRecordDetail domainTemplatePrintRecordDetail)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.TemplatePrintRecordDetailRepository.Update(domainTemplatePrintRecordDetail);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteTemplatePrintRecordDetail(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.TemplatePrintRecordDetailRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<TemplatePrintRecordDetail> GetTemplatePrintRecordDetails()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.TemplatePrintRecordDetailRepository.GetAll().ToList();
            }
        }

		public TemplatePrintRecordDetail GetTemplatePrintRecordDetailByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.TemplatePrintRecordDetailRepository.GetByKey(id);
            }
        }

		public IList<TemplatePrintRecordDetail> Paging(int PageIndex, int PageSize, Expression<Func<TemplatePrintRecordDetail, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.TemplatePrintRecordDetailRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
