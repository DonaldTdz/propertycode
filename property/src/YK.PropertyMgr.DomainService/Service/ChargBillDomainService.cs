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
using YK.PropertyMgr.ApplicationDTO;

namespace YK.PropertyMgr.DomainService
{
    public partial class ChargBillDomainService
    {
        public IList<ChargBill> GetChargBillList( Expression<Func<ChargBill, bool>> predicate, string expressions, out int totalCount)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {

                var dataList = propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(predicate).Sorting(expressions);  //Paging(PageIndex, PageSize, predicate, expressions, out totalCount).ToList();
                totalCount = dataList.Count();
                foreach (var item in dataList)
                {
                    item.ChargeSubjectId = item.ChargeSubject.Id;
                }
                return dataList.ToList();
            }
        }


        public IList<ChargBill> GetChargBillListPage(Expression<Func<ChargBill, bool>> predicate, string expressions, out int totalCount,int PageIndex,int PageSize)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var dataList = propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(predicate).Sorting(expressions);

                //var dataList = propertyMgrUnitOfWork.ChargBillRepository.GetAll().Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();                    // propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(predicate).Sorting(expressions);  //Paging(PageIndex, PageSize, predicate, expressions, out totalCount).ToList();
                totalCount = dataList.Count();
                 var  list= dataList.Skip((PageIndex - 1) * PageSize).Take(PageSize);
                foreach (var item in list)
                {
                    item.ChargeSubjectId = item.ChargeSubject.Id;
                }
                return list.ToList();
            }
        }

        public IList<BillDetailInfo> GetBillDetailListPage(Expression<Func<BillDetailInfo, bool>> predicate, string expressions, out int totalCount, int PageStart, int PageSize)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //费用记录信息
                var rquery = from c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll()
                             join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll()
                             on c.Id equals m.ChargeRecordId
                             join r in propertyMgrUnitOfWork.ReceiptRepository.GetAll()
                             on c.ReceiptId equals r.Id
                             join rr in propertyMgrUnitOfWork.RefundRecordRepository.GetAll()
                             on c.Id equals rr.ChargeRecordId into rc from lrc in rc.DefaultIfEmpty()
                             select new
                             {
                                 ChargeRecordId = c.Id,
                                 m.ChargeBillId,
                                 c.CustomerName,
                                 c.OperatorName,
                                 c.ChargeType,
                                 c.PayMthodId,
                                 c.PayDate,
                                 m.Amount,
                                 c.SerialNumber,
                                 c.AccountingStatus,
                                 RefundReason = lrc.Reason,
                                 r.Number,
                                 r.Status
                             };
                //最终查询
                var query = from b in propertyMgrUnitOfWork.ChargBillRepository.GetAll()
                            join r in rquery
                            on b.Id equals r.ChargeBillId into br
                            from lbr in br.DefaultIfEmpty()
                            where b.IsDel == false
                            select new BillDetailInfo()
                            {
                                Id = b.Id,
                                ChargeRecordId = lbr.ChargeRecordId,
                                ResourcesName = b.ResourcesName,
                                ReceiptNum = lbr.Number,
                                CustomerName = lbr.CustomerName,
                                OperatorName = lbr.OperatorName,
                                ChargeSubjectId = b.ChargeSubjectId,
                                ChargeType = lbr.ChargeType,
                                BillStatus = b.Status,
                                PayType = lbr.PayMthodId,
                                StartDate = b.BeginDate,
                                EndDate = b.EndDate,
                                ChargeDate = lbr.PayDate,
                                GenerationDate = b.CreateTime,
                                BillAmount = b.BillAmount,
                                Amount = lbr.Amount,
                                SerialNumber = lbr.SerialNumber,
                                AccountingStatus = lbr.AccountingStatus,
                                RefundReason = lbr.RefundReason,
                                Remark = b.Remark,
                                BillDesc = b.Description,
                                DeptId = b.ComDeptId,
                                ReceiptStatus = lbr.Status
                            };
                var dataList = query.Where(predicate).Sorting(expressions);
                totalCount = query.Where(predicate).Count();
                var list = dataList.Skip(PageStart).Take(PageSize);
                return list.ToList();
            }
        }



        public IList<ChargBill> GetChargBillAll(Expression<Func<ChargBill, bool>> predicate)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var dataList = propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(predicate);
                foreach (var item in dataList)
                {
                    item.ChargeSubjectId = item.ChargeSubject.Id;
                }
                return dataList.ToList();
            }
        }

        public IList<PaymentNoticePrintModel> GetPaymentNotice(Expression<Func<ChargBill, bool>> predicate)
        {
            try
            {
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    propertyMgrUnitOfWork.ChargBillRepository.DatabaseContext.Database.CommandTimeout = 3600;
                 
                    var dataList = from b in propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(predicate)
                                   join s in propertyMgrUnitOfWork.ChargeSubjectSnaRepository.GetAll() on b.Id equals s.ChargeBillId

                                   select (new PaymentNoticePrintModel
                                   {
                                       BillId = b.Id,
                                       Description = b.ChargeSubject.Name,
                                       ChargeSubjectId = b.ChargeSubjectId,
                                       BeginDate = b.BeginDate,
                                       EndDate = b.EndDate,
                                       Price = s.Price,
                                       Quantity = b.Quantity,
                                       BillAmount = b.BillAmount,
                                       ReceivedAmount = b.ReceivedAmount,
                                       ReliefAmount = b.ReliefAmount,
                                       PenaltyAmount = b.PenaltyAmount,
                                       RefType = b.RefType,
                                       HouseDeptId = b.HouseDeptId,
                                       ResourcesName = b.ResourcesName
                                   });

                    return dataList.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
        public IList<ChargeSubject> GetHouseSubjectList(int houseDeptId)
        {
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //var query = from sb in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll()
                //            join sr in propertyMgrUnitOfWork.SubjectHouseRefRepository.GetAll()
                //            on sb.Id equals sr.ChargeSubjecId
                //            where sb.IsDel == false
                //            //周期性收费项目 + 三表收费项目
                //            && (sb.BillPeriod == (int)BillPeriodEnum.DailyCharge
                //                || sb.BillPeriod == (int)BillPeriodEnum.MonthlyCharge
                //                || sb.BillPeriod == (int)BillPeriodEnum.MeterCharge)
                //            && sr.HouseDeptId == houseDeptId
                //            select sb;
                var query = from sb in pmUnitWork.ChargeSubjectRepository.GetAll()
                            where sb.IsDel == false
                            //周期性收费项目 + 三表收费项目
                            && (sb.BillPeriod == (int)BillPeriodEnum.DailyCharge
                                || sb.BillPeriod == (int)BillPeriodEnum.MonthlyCharge
                                || sb.BillPeriod == (int)BillPeriodEnum.MeterCharge)
                            && sb.ChargeSubjectHouseRefItems.Any(sr => sr.HouseDeptId == houseDeptId && sr.IsDel == false)
                            select sb;
                //公区表部分 2017-9-6
                var hdeptIdStr = "," + houseDeptId.ToString() + ",";
                var publicMeter = from sb in pmUnitWork.ChargeSubjectRepository.GetAll()
                                  from hr in sb.ChargeSubjectHouseRefItems
                                  join m in pmUnitWork.MeterRepository.GetAll()
                                  on hr.ResourcesId equals m.Id
                                  where sb.IsDel == false
                                  && sb.BillPeriod == (int)BillPeriodEnum.MeterCharge
                                  && hr.IsDel == false
                                  && m.IsEnabled == true
                                  && m.IsPublicArea == true
                                  && ("," + m.PublicAreaHouseDeptIDs + ",").Contains(hdeptIdStr)
                                  select sb;
                var dataList = query.ToList();
                dataList.AddRange(publicMeter.ToList());
                return dataList.Distinct().ToList();
            }
        }
    }
}
