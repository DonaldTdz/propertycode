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
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO;
using System.Data;
using YK.PropertyMgr.CompositeDomainService;

namespace YK.PropertyMgr.DomainService
{
    public partial class RefundRecordDomainService
    {

        public IList<RefundRecord> GetRefundRecordList(Expression<Func<RefundRecord, bool>> predicate)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.RefundRecordRepository.GetAll().Where(predicate).ToList();
            }
        }





    }
}
