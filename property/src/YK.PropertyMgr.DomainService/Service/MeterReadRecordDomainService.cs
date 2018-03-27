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
        public IList<MeterReadRecord> GetMeterRecords(Expression<Func<MeterReadRecord, bool>> predicate)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<RepositoryContract.IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.MeterReadRecordRepository.GetAll().Where(predicate).ToList();
            }
        }
    }
}
