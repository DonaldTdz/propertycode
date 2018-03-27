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
	public partial class ChargeSubjectDomainService
	{
		public bool InsertChargeSubject(ChargeSubject domainChargeSubject)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ChargeSubjectRepository.Add(domainChargeSubject);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateChargeSubject(ChargeSubject domainChargeSubject)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ChargeSubjectRepository.Update(domainChargeSubject);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteChargeSubject(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ChargeSubjectRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<ChargeSubject> GetChargeSubjects()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll().ToList();
            }
        }

		public ChargeSubject GetChargeSubjectByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ChargeSubjectRepository.GetByKey(id);
            }
        }

		public IList<ChargeSubject> Paging(int PageIndex, int PageSize, Expression<Func<ChargeSubject, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ChargeSubjectRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
