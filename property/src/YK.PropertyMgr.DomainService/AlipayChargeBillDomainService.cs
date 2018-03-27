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
	public partial class AlipayChargeBillDomainService
	{
		public bool InsertAlipayChargeBill(AlipayChargeBill domainAlipayChargeBill)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayChargeBillRepository.Add(domainAlipayChargeBill);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateAlipayChargeBill(AlipayChargeBill domainAlipayChargeBill)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayChargeBillRepository.Update(domainAlipayChargeBill);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteAlipayChargeBill(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayChargeBillRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<AlipayChargeBill> GetAlipayChargeBills()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayChargeBillRepository.GetAll().ToList();
            }
        }

		public AlipayChargeBill GetAlipayChargeBillByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayChargeBillRepository.GetByKey(id);
            }
        }

		public IList<AlipayChargeBill> Paging(int PageIndex, int PageSize, Expression<Func<AlipayChargeBill, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayChargeBillRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
