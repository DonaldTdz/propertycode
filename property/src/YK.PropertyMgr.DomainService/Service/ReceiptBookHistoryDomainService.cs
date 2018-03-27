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
using YK.PropertyMgr.ApplicationDTO.Enums;

namespace YK.PropertyMgr.DomainService
{
   public partial class ReceiptBookHistoryDomainService
    {

        public IList<ReceiptBookHistory> GetReceiptBookHistoryList(int startRowIndex, int PageSize, Expression<Func<ReceiptBookHistory, bool>> predicate, string sortExpressions, out int totalCount)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = from r in propertyMgrUnitOfWork.ReceiptBookHistoryRepository.GetAll().Where(predicate)
                            select r;
                var dataList = query.Distinct().SortingAndPaging(sortExpressions, startRowIndex, PageSize, out totalCount).ToList();
                return dataList;
            }
        }
    }
}
