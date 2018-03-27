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
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;

namespace YK.PropertyMgr.DomainService
{
    public partial class PaymentDiscountInfoDomainService
    {
        public List<ReportPayDisInf> GetPaymentDiscountInfoReport(Expression<Func<PaymentDiscountInfo, bool>> predicate, string expressions, out int totalCount, int PageStart, int PageSize, out decimal outSum)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = propertyMgrUnitOfWork.PaymentDiscountInfoRepository.GetAll();
                var dataList = query.Where(predicate);
                var sumDisAmo = dataList.Sum(s => s.DiscountAmount);
                outSum = sumDisAmo ?? 0; 
                totalCount = query.Where(predicate).Count();
                var list = dataList.Sorting(expressions).Skip(PageStart).Take(PageSize)
                    .Select(s => new ReportPayDisInf
                    {
                        DiscountType = s.DiscountType,
                        DiscountDesc = s.DiscountDesc,
                        DiscountAmount = s.DiscountAmount,
                        CustomerName = s.CustomerName,
                        CreateTime = s.CreateTime,
                        ResourcesNames = s.ChargeRecord.HouseDeptNos != null ? (s.ChargeRecord.HouseDeptNos != s.ChargeRecord.ResourcesNames ? s.ChargeRecord.HouseDeptNos + "(" + s.ChargeRecord.ResourcesNames + ")" : s.ChargeRecord.HouseDeptNos) : s.ChargeRecord.ResourcesNames,
                        Number = s.ChargeRecord.Receipt.Number,
                        Status = s.Status
                    });

                return list.ToList();
            }
        }
        public List<ReportPayDisInf> GetPaymentDiscountInfoReport(Expression<Func<PaymentDiscountInfo, bool>> predicate, string expressions)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = propertyMgrUnitOfWork.PaymentDiscountInfoRepository.GetAll();
                var dataList = query.Where(predicate);
                var list = dataList.Sorting(expressions)
                    .Select(s => new ReportPayDisInf
                    {
                        DiscountType = s.DiscountType,
                        DiscountDesc = s.DiscountDesc,
                        DiscountAmount = s.DiscountAmount,
                        CustomerName = s.CustomerName,
                        CreateTime = s.CreateTime,
                        ResourcesNames = s.ChargeRecord.HouseDeptNos != null ?(s.ChargeRecord.HouseDeptNos!=s.ChargeRecord.ResourcesNames?s.ChargeRecord.HouseDeptNos + "(" + s.ChargeRecord.ResourcesNames + ")": s.ChargeRecord.HouseDeptNos) : s.ChargeRecord.ResourcesNames,
                        Number = s.ChargeRecord.Receipt.Number,
                        Status = s.Status
                    });

                return list.ToList();
            }
        }
    }
}
