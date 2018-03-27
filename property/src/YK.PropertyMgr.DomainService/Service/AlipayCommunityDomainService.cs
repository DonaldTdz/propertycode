using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using Microsoft.Practices.Unity;

namespace YK.PropertyMgr.DomainService
{
   public partial class AlipayCommunityDomainService
    {
        public IList<AlipayCommunity> GetAlipayCommunityList(Expression<Func<AlipayCommunity, bool>> predicate)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {

                return propertyMgrUnitOfWork.AlipayCommunityRepository.GetAll().Where(predicate).ToList();


            }
        }
    }
}
