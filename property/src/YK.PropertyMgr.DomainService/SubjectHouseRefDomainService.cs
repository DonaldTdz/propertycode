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
	public partial class SubjectHouseRefDomainService
	{
		public bool InsertSubjectHouseRef(SubjectHouseRef domainSubjectHouseRef)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.SubjectHouseRefRepository.Add(domainSubjectHouseRef);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateSubjectHouseRef(SubjectHouseRef domainSubjectHouseRef)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.SubjectHouseRefRepository.Update(domainSubjectHouseRef);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteSubjectHouseRef(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.SubjectHouseRefRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<SubjectHouseRef> GetSubjectHouseRefs()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.SubjectHouseRefRepository.GetAll().ToList();
            }
        }

		public SubjectHouseRef GetSubjectHouseRefByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.SubjectHouseRefRepository.GetByKey(id);
            }
        }

		public IList<SubjectHouseRef> Paging(int PageIndex, int PageSize, Expression<Func<SubjectHouseRef, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.SubjectHouseRefRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
