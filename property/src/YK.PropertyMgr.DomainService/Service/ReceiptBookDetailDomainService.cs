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
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;

namespace YK.PropertyMgr.DomainService
{
    public partial class ReceiptBookDetailDomainService
    {


        public IList<ReceiptBookDetail> GetReceiptBookDetailList(Expression<Func<ReceiptBookDetail, bool>> predicate,int ComDeptId,int ReceiptBookType)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                return (from d in propertyMgrUnitOfWork.ReceiptBookDetailRepository.GetAll().Where(predicate)
                        join r in propertyMgrUnitOfWork.ReceiptBookRepository.GetAll() on d.ReceiptBookId equals r.Id
                        where r.DeptId == ComDeptId&&r.ReceiptBookType== ReceiptBookType

                        select d).ToList();

              //  return propertyMgrUnitOfWork.ReceiptBookDetailRepository.GetAll().Where(predicate).ToList();
            }
        }

        public IList<ReceiptBookDetailShowDTO> GetReceiptBookDetailShowList(Expression<Func<ReceiptBookDetail, bool>> predicate, Expression<Func<ChargeRecord, bool>> predicateChargerecord, string expressions, out int totalCount, int PageIndex, int PageSize, bool IsExport)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
            

                var list = (from rd in propertyMgrUnitOfWork.ReceiptBookDetailRepository.GetAll().Where(predicate).Sorting(expressions)
                            join re in propertyMgrUnitOfWork.ReceiptRepository.GetAll() on rd.ReceiptId equals re.Id
                            join rc in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(predicateChargerecord) on re.Id equals rc.ReceiptId
                            join refund in propertyMgrUnitOfWork.RefundRecordRepository.GetAll() on rc.Id equals refund.ChargeRecordId into refundtemp
                            from refundcord in refundtemp.DefaultIfEmpty()
                            select new ReceiptBookDetailShowDTO
                            {
                                Number = rd.Number,
                                ReceResourcesNum = rc.ResourcesNames,
                                Amount =rc.Amount==null?0:rc.Amount,
                                DiscountAmount =rc.DiscountAmount==null?0:rc.DiscountAmount,
                                PayDate =rc.PayDate,
                                OperatorName =rc.OperatorName,
                                ChargeType=rc.ChargeType,
                                PayMthodId =rc.PayMthodId,
                                RefundRecordReason = refundcord==null?"": refundcord.Reason,
                                Remark =rc.Remark
                            }

                           ).OrderByDescending(o=>o.Number);
                totalCount = list.Count();
                var dataList = new List<ReceiptBookDetailShowDTO>();
                if (IsExport)
                    dataList = list.ToList();
                else
                    dataList = list.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                return dataList;
            }
        }


    }
}
    