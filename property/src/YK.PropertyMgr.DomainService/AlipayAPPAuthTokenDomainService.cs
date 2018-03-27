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
	public partial class AlipayAPPAuthTokenDomainService
	{
		public bool InsertAlipayAPPAuthToken(AlipayAPPAuthToken domainAlipayAPPAuthToken)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayAPPAuthTokenRepository.Add(domainAlipayAPPAuthToken);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool UpdateAlipayAPPAuthToken(AlipayAPPAuthToken domainAlipayAPPAuthToken)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayAPPAuthTokenRepository.Update(domainAlipayAPPAuthToken);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public bool DeleteAlipayAPPAuthToken(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                propertyMgrUnitOfWork.AlipayAPPAuthTokenRepository.Delete(id);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public List<AlipayAPPAuthToken> GetAlipayAPPAuthTokens()
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayAPPAuthTokenRepository.GetAll().ToList();
            }
        }

		public AlipayAPPAuthToken GetAlipayAPPAuthTokenByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayAPPAuthTokenRepository.GetByKey(id);
            }
        }

		public IList<AlipayAPPAuthToken> Paging(int PageIndex, int PageSize, Expression<Func<AlipayAPPAuthToken, bool>> predicate, string expressions,out int totalCount)
        {
			 using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.AlipayAPPAuthTokenRepository.Paging(PageIndex,PageSize,predicate,expressions,out totalCount).ToList();
            }
        }
	}
}
