using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using YK.PropertyMgr.RepositoryContract;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.Crosscuting;
using System.Linq.Expressions;

namespace YK.PropertyMgr.DomainService
{
   public partial class TemplatePrintRecordDetailDomainService
    {
        public List<TemplatePrintRecordDetail> GetTemplatePrintRecordDetails(Expression<Func<TemplatePrintRecordDetail, bool>> predicate)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.TemplatePrintRecordDetailRepository.GetAll().Where(predicate).ToList();
            }
        }

    }
}
