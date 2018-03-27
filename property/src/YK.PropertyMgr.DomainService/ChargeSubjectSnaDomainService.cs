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
	public partial class ChargeSubjectSnaDomainService
	{
		public bool InsertChargeSubjectSna(ChargeSubjectSna domainChargeSubjectSna)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ChargeSubjectSnaRepository.Add(domainChargeSubjectSna);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateChargeSubjectSna(ChargeSubjectSna domainChargeSubjectSna)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ChargeSubjectSnaRepository.Update(domainChargeSubjectSna);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteChargeSubjectSna(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.ChargeSubjectSnaRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<ChargeSubjectSna> GetChargeSubjectSnas()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ChargeSubjectSnaRepository.GetAll().ToList();
            }
        }

		public ChargeSubjectSna GetChargeSubjectSnaByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ChargeSubjectSnaRepository.GetByKey(id);
            }
        }

		public IList<ChargeSubjectSna> Paging(int PageIndex, int PageSize, Expression<Func<ChargeSubjectSna, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ChargeSubjectSnaRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
