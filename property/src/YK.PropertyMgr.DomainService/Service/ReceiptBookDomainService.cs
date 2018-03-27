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
   public  partial class ReceiptBookDomainService
    {
        public IList<ReceiptBook> GetReceiptBookList(int startRowIndex, int PageSize, Expression<Func<ReceiptBook, bool>> predicate, string sortExpressions,out int totalCount)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = from r in propertyMgrUnitOfWork.ReceiptBookRepository.GetAll().Where(predicate)
                            select r;
                var dataList = query.Distinct().SortingAndPaging(sortExpressions, startRowIndex, PageSize, out totalCount).ToList();
                return dataList;
            }
        }

        /// <summary>
        /// 通过条件获取
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="Number"></param>
        /// <returns></returns>
        public ReceiptBook GetReceiptBookByNumber(Expression<Func<ReceiptBook, bool>> predicate, string Number)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = from r in propertyMgrUnitOfWork.ReceiptBookRepository.GetAll().Where(predicate)
                            join d in propertyMgrUnitOfWork.ReceiptBookDetailRepository.GetAll() on r.Id equals d.ReceiptBookId
                            where d.IsDel == false && d.Number == Number
                            select r;
                return query.FirstOrDefault();

            }
        }



        public IList<ReceiptBook> GetReceiptBookList(Expression<Func<ReceiptBook, bool>> predicate)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return propertyMgrUnitOfWork.ReceiptBookRepository.GetAll().Where(predicate).ToList();
            }
        }

        public bool InserReceiptBookAndHistory(ReceiptBook receiptbook, ReceiptBookHistory receiptbookhistory)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {

                propertyMgrUnitOfWork.ReceiptBookRepository.Add(receiptbook);
                propertyMgrUnitOfWork.ReceiptBookHistoryRepository.Add(receiptbookhistory);
                propertyMgrUnitOfWork.Commit();
                return true;

            }
        }

        public bool UpdateReceiptBookAndHistory(ReceiptBook receiptbook, ReceiptBookHistory receiptbookhistory)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {

                propertyMgrUnitOfWork.ReceiptBookRepository.Update(receiptbook);
                propertyMgrUnitOfWork.ReceiptBookHistoryRepository.Add(receiptbookhistory);
                propertyMgrUnitOfWork.Commit();
                return true;

            }
        }
   


 


    }
}
