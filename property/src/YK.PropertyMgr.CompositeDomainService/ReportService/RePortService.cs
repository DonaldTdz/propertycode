using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.Crosscuting;
using YK.PropertyMgr.DomainEntity;
using YK.PropertyMgr.RepositoryContract;
using YK.BackgroundMgr.DomainInterface;
using Microsoft.Practices.Unity;
using YK.BackgroundMgr.PresentationService;
using YK.PropertyMgr.ApplicationDTO;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using System.Data;
using YK.PropertyMgr.ApplicationDTO.Enums;
using System.Data.Entity.SqlServer;
using System.Data.Entity.Core.Objects;

namespace YK.PropertyMgr.CompositeDomainService
{
    public class RePortService : IReportService
    {
        #region 单例

        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static RePortService Instance { get { return SingletonInstance; } }
        private static readonly RePortService SingletonInstance = new RePortService();

        #endregion

        #region private Method

        #endregion

        #region 接口实现方法

        #region 欠费
        public ResultModel ArrearsChargeSubjectReport(int PageIndex, int PageSize, out int totalCount, string queryStr)
        {

            try
            {
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {

                    string sql = "[dbo].[p_ArrearsChargeSubjectReport] " + queryStr;
                    DbRawSqlQuery<ReportTableDTO> query = propertyMgrUnitOfWork.PrepayAccountRepository.DatabaseContext.Database.SqlQuery<ReportTableDTO>(sql);
                    totalCount = query.Count();
                    var datalist = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                    return new ResultModel() { IsSuccess = true, ErrorCode = "", Msg = "查询成功", Data = datalist };
                }



            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[应收报表-科目]Code:900  ErrorMsg:{0}", ex.Message), "RePortService", FileLogType.Exception);
                totalCount = 0;
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "报表查询失败" };
            }




        }


        public ResultModel ArrearsCommunityReport(int PageIndex, int PageSize, out int totalCount, string queryStr)
        {
            try
            {
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {

                    string sql = "[dbo].[p_ArrearsChargeCommunityReport] " + queryStr;
                    DbRawSqlQuery<ReportTableDTO> query = propertyMgrUnitOfWork.PrepayAccountRepository.DatabaseContext.Database.SqlQuery<ReportTableDTO>(sql);
                    totalCount = query.Count();
                    var datalist = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                    return new ResultModel() { IsSuccess = true, ErrorCode = "", Msg = "查询成功", Data = datalist };
                }



            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[应收报表-小区]Code:900  ErrorMsg:{0}", ex.Message), "RePortService", FileLogType.Exception);
                totalCount = 0;
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "报表查询失败" };
            }

        }

        public ResultModel ArrearsDetailReport(int PageIndex, int PageSize, out int totalCount, string queryStr)
        {
            try
            {
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {

                    string sql = "[dbo].[p_ArrearsDetilReport] " + queryStr;
                    DbRawSqlQuery<ReportTableDTO> query = propertyMgrUnitOfWork.PrepayAccountRepository.DatabaseContext.Database.SqlQuery<ReportTableDTO>(sql);
                    totalCount = query.Count();
                    var datalist = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                    return new ResultModel() { IsSuccess = true, ErrorCode = "", Msg = "查询成功", Data = datalist };
                }



            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[应收报表-明细]Code:900  ErrorMsg:{0}", ex.Message), "RePortService", FileLogType.Exception);
                totalCount = 0;
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "报表查询失败" };
            }

        }


        #endregion

        #region 实收

        public ResultModel CollectionsChargeSubjectReport(int PageIndex, int PageSize, out int totalCount, string queryStr)
        {

            try
            {
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {

                    string sql = "[dbo].[p_CollectionsChargeSubjectReport] " + queryStr;
                    DbRawSqlQuery<ReportTableDTO> query = propertyMgrUnitOfWork.PrepayAccountRepository.DatabaseContext.Database.SqlQuery<ReportTableDTO>(sql);
                    totalCount = query.Count();
                    var datalist = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                    return new ResultModel() { IsSuccess = true, ErrorCode = "", Msg = "查询成功", Data = datalist };
                }



            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[实收收报表-科目]Code:900  ErrorMsg:{0}", ex.Message), "RePortService", FileLogType.Exception);
                totalCount = 0;
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "报表查询失败" };
            }




        }

        public ResultModel CollectionsCommunityReport(int PageIndex, int PageSize, out int totalCount, string queryStr)
        {
            try
            {
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {

                    string sql = "[dbo].[p_CollectionsCommunityReport] " + queryStr;
                    DbRawSqlQuery<ReportTableDTO> query = propertyMgrUnitOfWork.PrepayAccountRepository.DatabaseContext.Database.SqlQuery<ReportTableDTO>(sql);
                    totalCount = query.Count();
                    var datalist = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                    return new ResultModel() { IsSuccess = true, ErrorCode = "", Msg = "查询成功", Data = datalist };
                }



            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[实收报表-小区]Code:900  ErrorMsg:{0}", ex.Message), "RePortService", FileLogType.Exception);
                totalCount = 0;
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "报表查询失败" };
            }

        }


        public ResultModel CollectionsDetailReport(int PageIndex, int PageSize, out int totalCount, string queryStr)
        {
            try
            {
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {

                    string sql = "[dbo].[p_CollectionsDetilReport] " + queryStr;
                    DbRawSqlQuery<ReportTableDTO> query = propertyMgrUnitOfWork.PrepayAccountRepository.DatabaseContext.Database.SqlQuery<ReportTableDTO>(sql);
                    totalCount = query.Count();
                    var datalist = query.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                    return new ResultModel() { IsSuccess = true, ErrorCode = "", Msg = "查询成功", Data = datalist };
                }



            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[实收报表-明细]Code:900  ErrorMsg:{0}", ex.Message), "RePortService", FileLogType.Exception);
                totalCount = 0;
                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "报表查询失败" };
            }

        }

        public List<ChargeRecordDTO> CollectionsDetailReport(int startRowIndex, int PageSize, Expression<Func<ChargeRecord, bool>> predicate, string sortExpressions, out int totalCount)
        {
            try
            {
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    var query = (
                           from r in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(predicate)
                           join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on r.Id equals m.ChargeRecordId
                           join b in propertyMgrUnitOfWork.ChargBillRepository.GetAll() on m.ChargeBillId equals b.Id
                           join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on b.ChargeSubjectId equals s.Id
                           join rp in propertyMgrUnitOfWork.ReceiptRepository.GetAll() on r.ReceiptId equals rp.Id

                           select new ChargeRecordDTO()
                           {
                               ReceiptNum = rp.Number,
                               ReceiptId = r.ReceiptId,
                               ComDeptId = r.ComDeptId,
                               HouseDeptId = r.HouseDeptId,
                               HouseDeptNos = r.HouseDeptNos,
                               ChargeSubjectName = s.Name,
                               Amount = m.Amount,
                               PayMthodId = r.PayMthodId,
                               ChargeType = r.ChargeType,
                               PayDate = r.PayDate,
                               Remark = r.Remark,
                               Id = r.Id

                           });
                    return query.SortingAndPaging(sortExpressions, startRowIndex, PageSize, out totalCount).ToList();
                }



            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[实收报表-明细]Code:900  ErrorMsg:{0}", ex.Message), "RePortService", FileLogType.Exception);
                totalCount = 0;
                return new List<ChargeRecordDTO>();
            }

        }


        public decimal GetCollectionsMoney(Expression<Func<ChargeRecord, bool>> predicate)
        {
            try
            {
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    var query = (
                           from r in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(predicate)
                           join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on r.Id equals m.ChargeRecordId
                           join b in propertyMgrUnitOfWork.ChargBillRepository.GetAll() on m.ChargeBillId equals b.Id
                           join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on b.ChargeSubjectId equals s.Id
                           join rp in propertyMgrUnitOfWork.ReceiptRepository.GetAll() on r.ReceiptId equals rp.Id

                           select new ChargeRecordDTO()
                           {
                               Id = r.Id,
                               Amount = r.ChargeType == (int)ChargeTypeEnum.Refund ? (-m.Amount) : m.Amount,
                               ChargeType = r.ChargeType
                           });


                    var ac = query.ToList();

                    return query.Sum(o => o.Amount.Value);

                }



            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[实收报表-金额合计]Code:900  ErrorMsg:{0}", ex.Message), "RePortService", FileLogType.Exception);

                return 0;
            }
        }


        #endregion

        #region  对比报表
        public ResultModel GetArrearsCollComparisoCharts(int ComDeptId)
        {
            try
            {
                ResultModel result = new ResultModel();
                using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    //获取全部未缴金额
                    var BillMoneyTotal = propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(o => o.ComDeptId == ComDeptId && o.IsDel == false).Sum(o => o.BillAmount) ?? 0;//全部金额
                    var RececiveTotal = propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(o => o.ComDeptId == ComDeptId && o.IsDel == false).Sum(o => o.ReceivedAmount) ?? 0;//实收金额
                    var ReliefAmountTotal = propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(o => o.ComDeptId == ComDeptId && o.IsDel == false).Sum(o => o.ReliefAmount) ?? 0;//减免金额
                    ReportTableDTO model = new ReportTableDTO()
                    {
                        RececiveTotal = Exchange(RececiveTotal),
                        BillMoneyTotal = Exchange(BillMoneyTotal),
                        ReliefAmountTotal = Exchange(ReliefAmountTotal),
                        UnPaidAmountTotal = Exchange(BillMoneyTotal) - Exchange(RececiveTotal) - Exchange(ReliefAmountTotal)

                    };

                    result.IsSuccess = true;
                    result.Data = model;
                    return result;

                }
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[收费对比报表-科目]Code:900  ErrorMsg:{0}", ex.Message), "RePortService", FileLogType.Exception);

                return new ResultModel() { IsSuccess = false, ErrorCode = "900", Msg = "报表查询失败" };
            }
        }


        private decimal Exchange(decimal? a)
        {
            if (a == null)
                return 0;
            else
                return Convert.ToDecimal(a);

        }

        #endregion

        #endregion

        #region 每日报表

        public IList<ReportDayDTO> GetDayReportDataList(ReportDaySearchDTO search)
        {
            if (!search.ComDeptId.HasValue)
            {
                return new List<ReportDayDTO>();
            }
            search.ChargeDate = search.ChargeDate ?? DateTime.Now;
            Condition<ChargBill> conditions = new Condition<ChargBill>(o => true);
            if (search.LouyuIdStr != null && search.LouyuIdStr.Length > 0)
            {

                var LouyuList = search.LouyuIdStr.Split(',').ToList().ConvertAll(i => int.Parse(i));
                var bulidList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildsInfoByBuildDeptId(LouyuList);
                string BuildCode = bulidList[0].Building_code;

                Condition<ChargBill> condition_bill_OR = new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(BuildCode));
                foreach (var c in bulidList)
                {
                    condition_bill_OR = condition_bill_OR | new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(c.Building_code));

                }
                conditions = conditions & condition_bill_OR;
            }



            using (var puw = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //1.查询小区下面所有未删除的收费项目
                var resultList = (from s in puw.ChargeSubjectRepository.GetAll()
                                  where s.ComDeptId == search.ComDeptId
                                  //&& s.IsDel == false
                                  select new ReportDayDTO()
                                  {
                                      Id = s.Id,
                                      SubjectName = s.Name,
                                      SubjectType = s.SubjectType
                                  }).ToList();
                //2.费用统计
                DateTime? payDate = search.ChargeDate.Value.AddDays(1);
                var query = from b in puw.ChargBillRepository.GetAll().Where(conditions.ExpressionBody)
                            join m in puw.ChargeBillRecordMatchingRepository.GetAll()
                            on b.Id equals m.ChargeBillId
                            where b.ComDeptId == search.ComDeptId
                            //where (b.Status == (int)BillStatusEnum.Paid || b.Status == (int)BillStatusEnum.Refunded)
                            //where b.ReceivedAmount > 0 //已收金额大于0的
                            where //SqlFunctions.DateDiff("day", m.ChargeRecord.PayDate,search.ChargeDate) == 0
                            m.ChargeRecord.PayDate < payDate
                            && m.ChargeRecord.PayDate >= search.ChargeDate.Value //取搜索条件当天
                            && m.ChargeRecord.ChargeType != (int)ChargeTypeEnum.BalanceTransfer//排除预存转移
                            && b.IsDel == false //排除作废账单 2017-9-6
                            select new
                            {
                                b.ChargeSubjectId,
                                //b.Status,
                                b.BeginDate,
                                b.EndDate,
                                b.ChargeSubject.SubjectType,
                                m.Amount,
                                m.ChargeRecord.ChargeType,
                                m.ChargeRecordId
                            };


                //当月第一天
                DateTime? cbDate = new DateTime(search.ChargeDate.Value.Year, search.ChargeDate.Value.Month, 1);
                //当月最后一天 对于 预收款和退款
                DateTime? cbEnd = cbDate.Value.AddMonths(1).AddDays(-1);
                //N月前期
                var beforeMonthList = (from q in query
                                       where q.SubjectType != (int)SubjectTypeEnum.SystemPreset//排除预存费
                                       where q.ChargeType != (int)ChargeTypeEnum.Refund //排除退款
                                       where q.EndDate < cbDate//“N月前期”表示收取费用的账单结束日期在查询日期所在月份之前
                                       group q by q.ChargeSubjectId into gq
                                       select new
                                       {
                                           ChargeSubjectId = gq.Key,
                                           Amount = gq.Sum(g => g.Amount)
                                       }).ToDictionary(key => key.ChargeSubjectId, value => value.Amount);
                //当月 “当月”表示收取费用的账单结束日期在查询的日期所在月份 或 缴费月在 账单账单开始结束日期月份之中
                var currentMonthList = (from q in query
                                        where q.SubjectType != (int)SubjectTypeEnum.SystemPreset//排除预存费
                                        where q.ChargeType != (int)ChargeTypeEnum.Refund //排除退款
                                        where ((q.EndDate >= cbDate && q.EndDate <= cbEnd) //收取费用的账单结束日期在查询的日期所在月份
                                        || (q.BeginDate <= cbEnd && cbDate <= q.EndDate))// 或 缴费月在 账单账单开始结束日期月份之中
                                        || (!q.BeginDate.HasValue && !q.EndDate.HasValue) //如果没有账单开始和结束时间 算到支付月 2017-08-29
                                        group q by q.ChargeSubjectId into gq
                                        select new
                                        {
                                            ChargeSubjectId = gq.Key,
                                            Amount = gq.Sum(g => g.Amount)
                                        }).ToDictionary(key => key.ChargeSubjectId, value => value.Amount);
                //预收金额
                var preList = (from q in query
                               where q.SubjectType != (int)SubjectTypeEnum.SystemPreset//排除预存费
                               where q.ChargeType != (int)ChargeTypeEnum.Refund //排除退款
                               where q.BeginDate > cbEnd//“预收金额”表示收取费用的账单开始时间在查询日期所在月份之后
                               group q by q.ChargeSubjectId into gq
                               select new
                               {
                                   ChargeSubjectId = gq.Key,
                                   Amount = gq.Sum(g => g.Amount)
                               }).ToDictionary(key => key.ChargeSubjectId, value => value.Amount);
                //预存金额
                //var preStoreList = (from q in query
                //                    where q.SubjectType == (int)SubjectTypeEnum.SystemPreset//预存费
                //                    where q.ChargeType != (int)ChargeTypeEnum.Refund //排除退款
                //                    group q by q.ChargeSubjectId into gq
                //                    select new
                //                    {
                //                        ChargeSubjectId = gq.Key,
                //                        Amount = gq.Sum(g => g.Amount)
                //                    }).ToDictionary(key => key.ChargeSubjectId, value => value.Amount);
                //预存金额（按收费项目算）
                var preStoreList = (from t in (from p in puw.PrepayAccountDetailRepository.GetAll()
                                               join q in query
                                               on p.ChargeRecordId equals q.ChargeRecordId
                                               where q.SubjectType == (int)SubjectTypeEnum.SystemPreset//预存费
                                               where q.ChargeType != (int)ChargeTypeEnum.Refund //排除退款
                                               select new
                                               {
                                                   p.Id,
                                                   p.PrepayAccount.ChargeSubjectID,
                                                   p.PrepayAccount.HouseDeptId,//相同收费项目 和 相同房屋去重复问题
                                                   p.ProductionAmount
                                               }).Distinct()//去重复
                                    group new { t.ChargeSubjectID, t.ProductionAmount }
                                    by t.ChargeSubjectID into gq
                                    select new
                                    {
                                        ChargeSubjectId = gq.Key,
                                        Amount = gq.Sum(g => g.ProductionAmount)
                                    }).ToDictionary(key => key.ChargeSubjectId, value => value.Amount);
                //实收退款
                var refundList = (from q in query
                                  where q.SubjectType != (int)SubjectTypeEnum.SystemPreset//排除预存费
                                  where q.ChargeType == (int)ChargeTypeEnum.Refund //退款
                                  where (q.EndDate <= cbEnd) || (q.BeginDate <= cbEnd && cbDate <= q.EndDate)//N月前+当月
                                  group q by q.ChargeSubjectId into gq
                                  select new
                                  {
                                      ChargeSubjectId = gq.Key,
                                      Amount = gq.Sum(g => g.Amount)
                                  }).ToDictionary(key => key.ChargeSubjectId, value => value.Amount);
                //预收退款
                var preRefundList = (from q in query
                                     where q.SubjectType != (int)SubjectTypeEnum.SystemPreset//排除预存费
                                     where q.ChargeType == (int)ChargeTypeEnum.Refund //退款
                                     where q.BeginDate > cbEnd//在之后
                                     group q by q.ChargeSubjectId into gq
                                     select new
                                     {
                                         ChargeSubjectId = gq.Key,
                                         Amount = gq.Sum(g => g.Amount)
                                     }).ToDictionary(key => key.ChargeSubjectId, value => value.Amount);

                //预存退款
                //var preStoreRefundList = (from q in query
                //                          where q.SubjectType == (int)SubjectTypeEnum.SystemPreset//预存费
                //                          where q.ChargeType == (int)ChargeTypeEnum.Refund //退款
                //                          group q by q.ChargeSubjectId into gq
                //                          select new
                //                          {
                //                              ChargeSubjectId = gq.Key,
                //                              Amount = gq.Sum(g => g.Amount)
                //                          }).ToDictionary(key => key.ChargeSubjectId, value => value.Amount);
                var preStoreRefundList = (from t in (from p in puw.PrepayAccountDetailRepository.GetAll()
                                                     join q in query
                                                     on p.ChargeRecordId equals q.ChargeRecordId
                                                     where q.SubjectType == (int)SubjectTypeEnum.SystemPreset//预存费
                                                     where q.ChargeType == (int)ChargeTypeEnum.Refund //排除退款
                                                     select new
                                                     {
                                                         p.Id,
                                                         p.PrepayAccount.ChargeSubjectID,
                                                         p.PrepayAccount.HouseDeptId,//添加房屋 2017-09-06
                                                         p.ProductionAmount
                                                     }).Distinct()//去重复
                                          group new { t.ChargeSubjectID, t.ProductionAmount }
                                          by t.ChargeSubjectID into gq
                                          select new
                                          {
                                              ChargeSubjectId = gq.Key,
                                              Amount = gq.Sum(g => g.ProductionAmount)
                                          }).ToDictionary(key => key.ChargeSubjectId, value => value.Amount);

                //预存抵扣
                var deductionQuery = from b in puw.ChargBillRepository.GetAll().Where(conditions.ExpressionBody)
                                     join m in puw.ChargeBillRecordMatchingRepository.GetAll()
                                     on b.Id equals m.ChargeBillId
                                     join p in puw.PrepayAccountDetailRepository.GetAll()
                                     on m.ChargeRecordId equals p.ChargeRecordId
                                     where //SqlFunctions.DateDiff("day", m.ChargeRecord.PayDate, search.ChargeDate) == 0
                                     m.ChargeRecord.PayDate < payDate
                                     && m.ChargeRecord.PayDate >= search.ChargeDate.Value //取搜索条件当天
                                     where m.ChargeRecord.ComDeptId == search.ComDeptId
                                     && p.PayTypeId == (int)PayTypeEnum.InternalTransfer//预存抵扣
                                     && p.ProductionAmount < 0
                                     select new
                                     {
                                         b.ChargeSubjectId,
                                         p.ProductionAmount
                                     };
                var preStoreDeductionList = (from q in deductionQuery
                                             group q by q.ChargeSubjectId into gq
                                             select new
                                             {
                                                 ChargeSubjectId = gq.Key,
                                                 Amount = gq.Sum(g => g.ProductionAmount)
                                             }).ToDictionary(key => key.ChargeSubjectId, value => value.Amount);

                //3.统计组合
                foreach (var item in resultList)
                {
                    if (beforeMonthList.Keys.Contains(item.Id))
                    {
                        item.BeforeMonthAmount = beforeMonthList[item.Id];
                        item.HasData = true;
                    }
                    if (currentMonthList.Keys.Contains(item.Id))
                    {
                        item.CurrentMonthAmount = currentMonthList[item.Id];
                        item.HasData = true;
                    }
                    if (preList.Keys.Contains(item.Id))
                    {
                        item.PreAmount = preList[item.Id];
                        item.HasData = true;
                    }
                    //收费项目预存费
                    if (preStoreList.Keys.Contains(item.Id))
                    {
                        item.PreStoreAmount = preStoreList[item.Id];
                        //item.GroupId = 2;
                        item.HasData = true;
                    }

                    //全部收费项目预存金额 
                    if (item.SubjectType == (int)SubjectTypeEnum.SystemPreset)
                    {
                        if (preStoreList.Keys.Contains(0))
                        {
                            item.PreStoreAmount = preStoreList[0];
                            item.GroupId = 2;
                            item.HasData = true;
                        }
                    }
                    if (refundList.Keys.Contains(item.Id))
                    {
                        item.Refund = refundList[item.Id];
                        item.HasData = true;
                    }
                    if (preRefundList.Keys.Contains(item.Id))
                    {
                        item.PreRefund = preRefundList[item.Id];
                        item.HasData = true;
                    }
                    //收费项目预存退款
                    if (preStoreRefundList.Keys.Contains(item.Id))
                    {
                        item.PreStoreRefund = preStoreRefundList[item.Id] * -1;
                        //item.GroupId = 2;
                        item.HasData = true;
                    }
                    //全部收费项目预存退款
                    if (item.SubjectType == (int)SubjectTypeEnum.SystemPreset)
                    {
                        if (preStoreRefundList.Keys.Contains(0))
                        {
                            item.PreStoreRefund = preStoreRefundList[0] * -1;
                            item.GroupId = 2;
                            item.HasData = true;
                        }
                    }
                    if (preStoreDeductionList.Keys.Contains(item.Id))
                    {
                        item.PreStoreDeduction = preStoreDeductionList[item.Id] * -1;
                        item.HasData = true;
                    }

                    item.ShowActualAmount = item.ActualAmount;
                    item.ShowTotalAmount = item.TotalAmount;
                }

                //4.添加合计
                if (resultList.Where(r => r.HasData).Count() > 0)
                {
                    ReportDayDTO entity = new ReportDayDTO();
                    entity.SubjectName = "合计：";
                    entity.BeforeMonthAmount = resultList.Sum(r => r.BeforeMonthAmount);
                    entity.CurrentMonthAmount = resultList.Sum(r => r.CurrentMonthAmount);
                    entity.PreAmount = resultList.Sum(r => r.PreAmount);
                    entity.PreStoreAmount = resultList.Sum(r => r.PreStoreAmount);
                    entity.ShowActualAmount = resultList.Sum(r => r.ActualAmount);
                    entity.Refund = resultList.Sum(r => r.Refund);
                    entity.PreRefund = resultList.Sum(r => r.PreRefund);
                    entity.PreStoreRefund = resultList.Sum(r => r.PreStoreRefund);
                    entity.ShowTotalAmount = resultList.Sum(r => r.TotalAmount);
                    entity.PreStoreDeduction = resultList.Sum(r => r.PreStoreDeduction);
                    entity.HasData = true;
                    entity.GroupId = 3;
                    resultList.Add(entity);
                }

                //5.过滤掉空数据并返回
                return resultList.Where(r => r.HasData).OrderBy(r => r.GroupId).ThenBy(r => r.SubjectName).ToList();
            }
        }

        //每日报表明细
        public IList<BillDetailInfo> GetBillDetailReportDataList(Expression<Func<BillDetailInfo, bool>> predicate, Expression<Func<ChargBill, bool>> predicate_chargebill, string expressions, out int totalCount, int PageStart, int PageSize)
        {
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
               

                var query = from b in pmUnitWork.ChargBillRepository.GetAll().Where(predicate_chargebill)
                            join m in pmUnitWork.ChargeBillRecordMatchingRepository.GetAll() on b.Id equals m.ChargeBillId
                            join r in pmUnitWork.ChargeRecordRepository.GetAll() on m.ChargeRecordId equals r.Id
                            where b.IsDel == false
                            where r.ChargeType != (int)ChargeTypeEnum.BalanceTransfer//排除预存转移
                            select new BillDetailInfo()
                            {
                                ResourcesName = b.ResourcesName,
                                CustomerName = r.CustomerName,
                                BillDesc = b.Description,
                                ChargeType = r.ChargeType,
                                StartDate = b.BeginDate,
                                EndDate = b.EndDate,
                                BillAmount = b.BillAmount,
                                //Amount = b.ReceivedAmount,//已收金额
                                Amount = m.Amount,//已收金额
                                ChargeDate = r.PayDate,
                                ReceiptNum = r.Receipt.Number,
                                OperatorName = r.OperatorName,
                                PayType = r.PayMthodId,
                                Remark = b.Remark,
                                DeptId = b.ComDeptId //小区Id
                            };
                totalCount = query.Where(predicate).Count();
                var list = query.Where(predicate).Sorting(expressions).Skip(PageStart).Take(PageSize);
                return list.ToList();
            }
        }

        //每日报表明细 导出
        public IList<BillDetailInfo> GetDayDetailReportExportData(Expression<Func<BillDetailInfo, bool>> predicate, Expression<Func<ChargBill, bool>> predicate_chargebill, string expressions)
        {
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = from b in pmUnitWork.ChargBillRepository.GetAll().Where(predicate_chargebill)
                            join m in pmUnitWork.ChargeBillRecordMatchingRepository.GetAll() on b.Id equals m.ChargeBillId
                            join r in pmUnitWork.ChargeRecordRepository.GetAll() on m.ChargeRecordId equals r.Id
                            where b.IsDel == false
                            where r.ChargeType != (int)ChargeTypeEnum.BalanceTransfer//排除预存转移
                            select new BillDetailInfo()
                            {
                                ResourcesName = b.ResourcesName,
                                CustomerName = r.CustomerName,
                                BillDesc = b.Description,
                                ChargeType = r.ChargeType,
                                StartDate = b.BeginDate,
                                EndDate = b.EndDate,
                                BillAmount = b.BillAmount,
                                //Amount = b.ReceivedAmount,//已收金额
                                Amount = m.Amount,//已收金额
                                ChargeDate = r.PayDate,
                                ReceiptNum = r.Receipt.Number,
                                OperatorName = r.OperatorName,
                                PayType = r.PayMthodId,
                                Remark = b.Remark,
                                DeptId = b.ComDeptId //小区Id
                            };
                var list = query.Where(predicate).Sorting(expressions);
                return list.ToList();
            }
        }

        #endregion

        #region  收款月报表

        public IList<ReportMonthDTO> GetMonthReportDataList(ReportDaySearchDTO search)
        {
            if (!search.ComDeptId.HasValue)
            {
                return new List<ReportMonthDTO>();
            }

            Condition<ChargBill> condition_bill = new Condition<ChargBill>(o => o.IsDel == false);

            if (search.LouyuIdStr != null && search.LouyuIdStr.Length > 0)
            {

                var LouyuList = search.LouyuIdStr.Split(',').ToList().ConvertAll(i => int.Parse(i));
                var bulidList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildsInfoByBuildDeptId(LouyuList);
                string BuildCode = bulidList[0].Building_code;

                Condition<ChargBill> condition_bill_OR = new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(BuildCode));
                foreach (var c in bulidList)
                {
                    condition_bill_OR = condition_bill_OR | new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(c.Building_code));

                }
                condition_bill = condition_bill & condition_bill_OR;
            }


            search.ChargeDate = search.ChargeDate ?? DateTime.Now;
            //int iMonth = search.ChargeDate.Value.Month;
            DateTime mbeginDate = search.ChargeDate.Value;
            DateTime mendDate = search.ChargeDate.Value.AddMonths(1).AddSeconds(-1);
            using (var puw = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {

                puw.ChargBillRepository.DatabaseContext.Database.CommandTimeout = 3600;

                //1.查询小区下面所有未删除的收费项目
                var resultList = (from s in puw.ChargeSubjectRepository.GetAll()
                                  where s.ComDeptId == search.ComDeptId
                                  //&& s.IsDel == false
                                  select new ReportMonthDTO()
                                  {
                                      Id = s.Id,
                                      SubjectName = s.Name,
                                      SubjectType = s.SubjectType
                                  }).ToList();
                //2.费用统计
                //未付款部分
                var nopquery = from b in puw.ChargBillRepository.GetAll().Where(condition_bill.ExpressionBody)
                               where b.ComDeptId == search.ComDeptId
                               && b.Status == (int)BillStatusEnum.NoPayment //未付款
                               && b.ChargeSubject.SubjectType != (int)SubjectTypeEnum.SystemPreset//不等于预付款
                               && b.IsDel == false
                               select new
                               {
                                   b.Id,
                                   ShouldAmount = b.BillAmount,//应付款
                                   ArrearsAmount = b.ReceivedAmount,//已付款
                                   b.ResourcesId,
                                   b.ChargeSubjectId,
                                   b.BeginDate,
                                   b.EndDate,
                                   b.ChargeSubject.SubjectType
                               };

                //已付款部分
                var pquery = from b in puw.ChargBillRepository.GetAll().Where(condition_bill.ExpressionBody)
                             join m in puw.ChargeBillRecordMatchingRepository.GetAll()
                             on b.Id equals m.ChargeBillId
                             where b.ComDeptId == search.ComDeptId
                             && b.Status == (int)BillStatusEnum.Paid //已付款
                             && b.ChargeSubject.SubjectType != (int)SubjectTypeEnum.SystemPreset//不等于预付款
                             && b.IsDel == false
                             && m.ChargeRecord.Receipt.Status == (int)ReceiptStatusEnum.Paid
                             && mbeginDate <= m.ChargeRecord.PayDate && mendDate >= m.ChargeRecord.PayDate
                             select new
                             {
                                 b.Id,
                                 ShouldAmount = m.Amount,//应付款
                                 ArrearsAmount = m.Amount,//已付款
                                 b.ResourcesId,
                                 b.ChargeSubjectId,
                                 b.BeginDate,
                                 b.EndDate,
                                 b.ChargeSubject.SubjectType,
                                 //m.ChargeRecord.PayDate,
                                 m.ChargeRecordId,
                                 m.ChargeRecord.PayMthodId
                             };

                //部分付款
                var BFquery = from b in puw.ChargBillRepository.GetAll().Where(condition_bill.ExpressionBody)
                              join m in puw.ChargeBillRecordMatchingRepository.GetAll()
                              on b.Id equals m.ChargeBillId
                              where b.ComDeptId == search.ComDeptId
                              && b.Status == (int)BillStatusEnum.NoPayment //未付款
                              && b.ReceivedAmount > 0
                              && b.ChargeSubject.SubjectType != (int)SubjectTypeEnum.SystemPreset//不等于预付款
                              && b.IsDel == false
                              && m.ChargeRecord.Receipt.Status == (int)ReceiptStatusEnum.Paid
                              && mbeginDate <= m.ChargeRecord.PayDate && mendDate >= m.ChargeRecord.PayDate
                              select new
                              {
                                  b.Id,
                                  ShouldAmount = m.Amount,//应付款
                                  ArrearsAmount = m.Amount,//已付款
                                  b.ResourcesId,
                                  b.ChargeSubjectId,
                                  b.BeginDate,
                                  b.EndDate,
                                  b.ChargeSubject.SubjectType,
                                  //m.ChargeRecord.PayDate,
                                  m.ChargeRecordId,
                                  m.ChargeRecord.PayMthodId
                              };

                //预付款部分
                var prquery = from b in puw.ChargBillRepository.GetAll().Where(condition_bill.ExpressionBody)
                              join m in puw.ChargeBillRecordMatchingRepository.GetAll()
                              on b.Id equals m.ChargeBillId
                              where b.ComDeptId == search.ComDeptId
                              && b.Status == (int)BillStatusEnum.Paid //已付款
                              && b.ChargeSubject.SubjectType == (int)SubjectTypeEnum.SystemPreset//等于预付款
                              && b.IsDel == false
                              && m.ChargeRecord.Receipt.Status == (int)ReceiptStatusEnum.Paid
                              && m.ChargeRecord.ChargeType != (int)ChargeTypeEnum.BalanceTransfer//排除预存转移
                              && mbeginDate <= m.ChargeRecord.PayDate && mendDate >= m.ChargeRecord.PayDate
                              select new
                              {
                                  b.Id,
                                  ShouldAmount = m.Amount,//应付款
                                  ArrearsAmount = m.Amount,//已付款
                                  b.ResourcesId,
                                  b.ChargeSubjectId,
                                  b.BeginDate,
                                  b.EndDate,
                                  b.ChargeSubject.SubjectType,
                                  //m.ChargeRecord.PayDate,
                                  m.ChargeRecordId,
                                  m.ChargeRecord.PayMthodId
                              };

                //账单月第一天
                DateTime? cbFirst = new DateTime(search.ChargeDate.Value.Year, search.ChargeDate.Value.Month, 1);
                //账单月最后一天 对于 预收款和退款
                DateTime? cbLast = cbFirst.Value.AddMonths(1).AddDays(-1);
                //本月应收金额和户数
                var shouldMonthList = (from b in (from q in (from c in nopquery//未付款
                                                             where (c.BeginDate <= mbeginDate && mbeginDate <= c.EndDate)
                                                                 || (c.BeginDate <= mendDate && mendDate <= c.EndDate)
                                                                 || (mbeginDate <= c.BeginDate && c.BeginDate <= mendDate)
                                                                 || (mbeginDate <= c.EndDate && c.EndDate <= mendDate)
                                                             select new { c.ChargeSubjectId, c.ResourcesId, c.ShouldAmount })
                                                            .Concat(
                                                            from c in pquery//已付款
                                                                            //where mbeginDate <= c.PayDate && mendDate >= c.PayDate
                                                            where ((c.BeginDate <= mbeginDate && mbeginDate <= c.EndDate)
                                                                   || (c.BeginDate <= mendDate && mendDate <= c.EndDate)
                                                                   || (mbeginDate <= c.BeginDate && c.BeginDate <= mendDate)
                                                                   || (mbeginDate <= c.EndDate && c.EndDate <= mendDate)
                                                                   || (!c.BeginDate.HasValue && !c.EndDate.HasValue) //如果没有账单开始和结束时间默认为付款月 2017-8-28 fixed bug 5394
                                                                   )
                                                            select new { c.ChargeSubjectId, c.ResourcesId, c.ShouldAmount }
                                                            )
                                                  group q by new { q.ChargeSubjectId, q.ResourcesId } into gc
                                                  select new
                                                  {
                                                      ChargeSubjectId = gc.Key.ChargeSubjectId,
                                                      ResourcesId = gc.Key.ResourcesId,
                                                      Amount = gc.Sum(s => s.ShouldAmount)
                                                  }
                                        )
                                       group b by b.ChargeSubjectId into gc
                                       select new
                                       {
                                           ChargeSubjectId = gc.Key,
                                           Amount = gc.Sum(s => s.Amount),
                                           Houses = gc.Count()
                                       }).ToList();

                //当月 “当月”表示收取费用的账单结束日期在查询的日期所在月份 或 缴费月在 账单账单开始结束日期月份之中
                //实收金额 （对于应收）
                var currentMonthList = (from b in (from q in ((from c in BFquery//部分付款
                                                               where ((c.BeginDate <= mbeginDate && mbeginDate <= c.EndDate)
                                                                   || (c.BeginDate <= mendDate && mendDate <= c.EndDate)
                                                                   || (mbeginDate <= c.BeginDate && c.BeginDate <= mendDate)
                                                                   || (mbeginDate <= c.EndDate && c.EndDate <= mendDate))
                                                               where c.PayMthodId != (int)PayTypeEnum.InternalTransfer //add 2017-7-19 v2.8 6
                                                               //&& mbeginDate <= c.PayDate && mendDate >= c.PayDate
                                                               select new { c.ChargeSubjectId, c.ResourcesId, c.ArrearsAmount })
                                                            .Concat(from c in pquery//已付款
                                                                    where ((c.BeginDate <= mbeginDate && mbeginDate <= c.EndDate)
                                                                            || (c.BeginDate <= mendDate && mendDate <= c.EndDate)
                                                                            || (mbeginDate <= c.BeginDate && c.BeginDate <= mendDate)
                                                                            || (mbeginDate <= c.EndDate && c.EndDate <= mendDate)
                                                                            || (!c.BeginDate.HasValue && !c.EndDate.HasValue) //如果没有账单开始和结束时间默认为付款月 2017-8-28 fixed bug 5394
                                                                            )
                                                                    where c.PayMthodId != (int)PayTypeEnum.InternalTransfer //add 2017-7-19 v2.8 6
                                                                    //where mbeginDate <= c.PayDate && mendDate >= c.PayDate
                                                                    select new { c.ChargeSubjectId, c.ResourcesId, c.ArrearsAmount })
                                                            )
                                                   group q by new { q.ChargeSubjectId, q.ResourcesId } into gc
                                                   select new
                                                   {
                                                       ChargeSubjectId = gc.Key.ChargeSubjectId,
                                                       ResourcesId = gc.Key.ResourcesId,
                                                       Amount = gc.Sum(s => s.ArrearsAmount)
                                                   }
                                        )
                                        group b by b.ChargeSubjectId into gc
                                        select new
                                        {
                                            ChargeSubjectId = gc.Key,
                                            Amount = gc.Sum(s => s.Amount),
                                            Houses = gc.Count()
                                        }).ToList();

                //预存抵扣 预存抵扣只是本期 2017-8-28
                var preStoreDeductionList = (from b in (from a in ((from q in pquery//全部付
                                                                    where //mbeginDate <= q.PayDate && mendDate >= q.PayDate
                                                                     ((q.BeginDate <= mbeginDate && mbeginDate <= q.EndDate)
                                                                        || (q.BeginDate <= mendDate && mendDate <= q.EndDate)
                                                                        || (mbeginDate <= q.BeginDate && q.BeginDate <= mendDate)
                                                                        || (mbeginDate <= q.EndDate && q.EndDate <= mendDate)
                                                                    ) //add 2017-08-28 fixed bug 5397
                                                                    && q.PayMthodId == (int)PayTypeEnum.InternalTransfer
                                                                    select new { q.ChargeRecordId, q.ChargeSubjectId })
                                                        .Union(from c in BFquery//付部分
                                                               where //mbeginDate <= c.PayDate && mendDate >= c.PayDate
                                                               ((c.BeginDate <= mbeginDate && mbeginDate <= c.EndDate)
                                                                    || (c.BeginDate <= mendDate && mendDate <= c.EndDate)
                                                                    || (mbeginDate <= c.BeginDate && c.BeginDate <= mendDate)
                                                                    || (mbeginDate <= c.EndDate && c.EndDate <= mendDate)
                                                                ) //add 2017-08-28 fixed bug 5397
                                                               && c.PayMthodId == (int)PayTypeEnum.InternalTransfer
                                                               select new { c.ChargeRecordId, c.ChargeSubjectId }))
                                                        join p in puw.PrepayAccountDetailRepository.GetAll()
                                                        on a.ChargeRecordId equals p.ChargeRecordId
                                                        where p.ProductionAmount < 0
                                                        select new
                                                        {
                                                            a.ChargeSubjectId,
                                                            p.ProductionAmount
                                                        })
                                             group b by b.ChargeSubjectId into gq
                                             select new
                                             {
                                                 ChargeSubjectId = gq.Key,
                                                 Amount = gq.Sum(g => g.ProductionAmount)
                                             }).ToList();

                //预收金额
                var preList = (from a in (
                                (from c in BFquery//部分付款
                                 where //(mbeginDate <= c.PayDate && mendDate >= c.PayDate)
                                  c.BeginDate > mendDate
                                 select new { c.ChargeSubjectId, c.ArrearsAmount })
                                .Concat(from q in pquery //全部收
                                        where //(mbeginDate <= q.PayDate && mendDate >= q.PayDate)
                                        q.BeginDate > mendDate
                                        select new { q.ChargeSubjectId, q.ArrearsAmount })
                                    )
                               group a by a.ChargeSubjectId into gq
                               select new
                               {
                                   ChargeSubjectId = gq.Key,
                                   Amount = gq.Sum(g => g.ArrearsAmount)
                               }).ToList();

                //预存金额（按收费项目算） 
                var preStoreList = (from t in (from p in puw.PrepayAccountDetailRepository.GetAll()
                                               join q in prquery
                                               on p.ChargeRecordId equals q.ChargeRecordId
                                               //where (mbeginDate <= q.PayDate && mendDate >= q.PayDate)
                                               select new
                                               {
                                                   p.Id,
                                                   p.PrepayAccount.ChargeSubjectID,
                                                   p.PrepayAccount.HouseDeptId,
                                                   p.ProductionAmount
                                               }).Distinct()//去重复
                                    group new { t.ChargeSubjectID, t.ProductionAmount }
                                    by t.ChargeSubjectID into gq
                                    select new
                                    {
                                        ChargeSubjectId = gq.Key,
                                        Amount = gq.Sum(g => g.ProductionAmount)
                                    }).ToList();
                //当月欠收
                var monthArrearsList = (from q in (from q in nopquery
                                                   where
                                                    ((q.BeginDate <= mbeginDate && mbeginDate <= q.EndDate)
                                                    || (q.BeginDate <= mendDate && mendDate <= q.EndDate)
                                                    ||
                                                    (mbeginDate <= q.BeginDate && q.BeginDate <= mendDate)
                                                    || (mbeginDate <= q.EndDate && q.EndDate <= mendDate)
                                                    )
                                                   group q by new { q.ChargeSubjectId, q.ResourcesId } into gq
                                                   select new
                                                   {
                                                       ChargeSubjectId = gq.Key.ChargeSubjectId,
                                                       ResourcesId = gq.Key.ResourcesId,
                                                       Amount = gq.Sum(g => g.ShouldAmount - g.ArrearsAmount)
                                                   })
                                        group q by q.ChargeSubjectId into gc
                                        select new
                                        {
                                            ChargeSubjectId = gc.Key,
                                            Amount = gc.Sum(s => s.Amount),
                                            Houses = gc.Count()
                                        }).ToList();

                //往期欠款
                var beforeMonthArrearsList = (from q in nopquery
                                              where q.EndDate < mbeginDate
                                              group q by q.ChargeSubjectId into gq
                                              select new
                                              {
                                                  ChargeSubjectId = gq.Key,
                                                  Amount = gq.Sum(g => g.ShouldAmount - g.ArrearsAmount)
                                              }).ToList();

                //收往期
                var beforeMonthList = (from a in ((from c in BFquery//部分收
                                                   where //(mbeginDate <= c.PayDate && mendDate >= c.PayDate)
                                                    c.EndDate < mbeginDate
                                                   select new { c.ChargeSubjectId, c.ArrearsAmount })
                                                 .Concat(from q in pquery//全部收
                                                         where //(mbeginDate <= q.PayDate && mendDate >= q.PayDate)
                                                         q.EndDate < mbeginDate
                                                         select new { q.ChargeSubjectId, q.ArrearsAmount })
                                              )
                                       group a by a.ChargeSubjectId into gq
                                       select new
                                       {
                                           ChargeSubjectId = gq.Key,
                                           Amount = gq.Sum(g => g.ArrearsAmount)
                                       }).ToList();

                //3.统计组合
                foreach (var item in resultList)
                {
                    //应收情况
                    var shouldMonth = shouldMonthList.FirstOrDefault(f => f.ChargeSubjectId == item.Id);
                    if (shouldMonth != null)
                    {
                        item.ShouldMonthAmount = shouldMonth.Amount; //应收金额
                        item.ShouldMonthHouses = shouldMonth.Houses; //应收户数
                        item.HasData = true;
                    }
                    //当月收取
                    var currentMonth = currentMonthList.FirstOrDefault(f => f.ChargeSubjectId == item.Id);
                    if (currentMonth != null)
                    {
                        item.CurrentMonthAmount = currentMonth.Amount;
                        //实收户数
                        item.ActualMonthHouses = currentMonth.Houses;
                        item.HasData = true;
                    }
                    //预存抵扣
                    var preStoreDeduction = preStoreDeductionList.FirstOrDefault(f => f.ChargeSubjectId == item.Id);
                    if (preStoreDeduction != null)
                    {
                        item.PreStoreDeduction = preStoreDeduction.Amount * -1;
                        item.HasData = true;
                    }
                    //预收金额
                    var pre = preList.FirstOrDefault(f => f.ChargeSubjectId == item.Id);
                    if (pre != null)
                    {
                        item.PreAmount = pre.Amount;
                        item.HasData = true;
                    }
                    //预存金额
                    var preStore = preStoreList.FirstOrDefault(f => f.ChargeSubjectId == item.Id);
                    if (preStore != null)
                    {
                        item.PreStoreAmount = preStore.Amount;
                        //item.GroupId = 2;
                        item.HasData = true;
                    }
                    //全部收费项目预存金额 
                    if (item.SubjectType == (int)SubjectTypeEnum.SystemPreset)
                    {
                        var allpreStore = preStoreList.FirstOrDefault(f => f.ChargeSubjectId == 0);
                        if (allpreStore != null)
                        {
                            item.PreStoreAmount = allpreStore.Amount;
                            item.GroupId = 2;
                            item.HasData = true;
                        }
                    }
                    //当月欠款
                    var monthArrears = monthArrearsList.FirstOrDefault(f => f.ChargeSubjectId == item.Id);
                    if (monthArrears != null)
                    {
                        item.MonthArrears = monthArrears.Amount;
                        item.MonthArrearsHouses = monthArrears.Houses;
                        item.HasData = true;
                    }
                    //往期欠款
                    var beforeMonthArrears = beforeMonthArrearsList.FirstOrDefault(f => f.ChargeSubjectId == item.Id);
                    if (beforeMonthArrears != null)
                    {
                        item.BeforeMonthArrears = beforeMonthArrears.Amount;
                        item.HasData = true;
                    }
                    //往期收款
                    var beforeMonth = beforeMonthList.FirstOrDefault(f => f.ChargeSubjectId == item.Id);
                    if (beforeMonth != null)
                    {
                        item.BeforeMonthAmount = beforeMonth.Amount;
                        item.HasData = true;
                    }
                }

                //4.添加合计
                if (resultList.Where(r => r.HasData).Count() > 0)
                {
                    ReportMonthDTO entity = new ReportMonthDTO();
                    entity.SubjectName = "合计：";
                    entity.ShouldMonthAmount = resultList.Sum(r => r.ShouldMonthAmount);
                    entity.ShouldMonthHouses = resultList.Sum(r => r.ShouldMonthHouses);
                    entity.ActualMonthHouses = resultList.Sum(r => r.ActualMonthHouses);
                    entity.CurrentMonthAmount = resultList.Sum(r => r.CurrentMonthAmount);
                    entity.PreStoreDeduction = resultList.Sum(r => r.PreStoreDeduction);
                    entity.PreAmount = resultList.Sum(r => r.PreAmount);
                    entity.PreStoreAmount = resultList.Sum(r => r.PreStoreAmount);
                    entity.MonthArrearsHouses = resultList.Sum(r => r.MonthArrearsHouses);
                    entity.MonthArrears = resultList.Sum(r => r.MonthArrears);
                    entity.BeforeMonthArrears = resultList.Sum(r => r.BeforeMonthArrears);
                    entity.BeforeMonthAmount = resultList.Sum(r => r.BeforeMonthAmount);
                    entity.HasData = true;
                    entity.GroupId = 3;
                    resultList.Add(entity);
                }

                //5.过滤掉空数据并返回
                return resultList.Where(r => r.HasData).OrderBy(r => r.GroupId).ThenBy(r => r.SubjectName).ToList();
            }
        }

        #endregion

        #region 2.7版本三表费用报表

        public ReportMeterModels GetMeterReportDataList(int PageIndex, int PageSize, ReportMeterSearchDTO search, out int totalCount, bool IsExport = false)
        {

            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                totalCount = 0;
                return GetAllMeterReportDataList(propertyMgrUnitOfWork, PageIndex, PageSize, out totalCount, search, IsExport);
            }

        }
        private ReportMeterModels GetAllMeterReportDataList(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, int PageIndex, int PageSize, out int totalCount, ReportMeterSearchDTO search, bool IsExport = false)
        {
            ReportMeterModels ReportModel = new ReportMeterModels();
            totalCount = 0;
            //获取房间号

            //房间号

            List<DeptInfo> HouseDeptList = new List<DeptInfo>();
            List<DeptInfo> OwnerDeptList = new List<DeptInfo>();

            var UnionList = GetMeterUnionList(propertyMgrUnitOfWork, search, ref HouseDeptList, ref OwnerDeptList).ToList();


            ReportModel.ReportArrearsSum = GetMeterReportDataSumList(UnionList);


            var BasePageList = UnionList.GroupBy(o => new { o.ResourcesId, o.RefType, o.ResourcesName }).OrderBy(o => o.Key.ResourcesName);
            totalCount = BasePageList.Count();
            var pageList = BasePageList.Skip((PageIndex - 1) * PageSize).Take(PageSize);

            if (IsExport)
            {
                pageList = BasePageList;
            }

            var allList = (from a in UnionList
                           join p in pageList on new { ResourcesId = a.ResourcesId, RefType = a.RefType } equals new { ResourcesId = p.Key.ResourcesId, RefType = p.Key.RefType }
                           select a).OrderBy(o => o.ResourcesName).ToList();

            if (string.IsNullOrEmpty(search.ResourceName) || string.IsNullOrEmpty(search.OwnerName))
            {
                var ArrHouseId = allList.Where(o => o.RefType == (int)ReourceTypeEnum.ThreeMeter).Select(p => p.ResourcesId).ToList();
                if (string.IsNullOrEmpty(search.OwnerName))
                {
                    OwnerDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptAndOwnerListByIdArr(ArrHouseId).ToList();
                }

                Condition<SEC_Dept> condition = new Condition<SEC_Dept>(o => ArrHouseId.Contains(o.Id));
                HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoByQuery(condition.ExpressionBody).ToList();
            }



            //收费对象列表
            var ResourcesList = allList.GroupBy(o => new { o.ResourcesId, o.RefType, o.ResourcesName }).OrderBy(o => o.Key.ResourcesName).ToList();
            var ChargeSubjectList = UnionList.GroupBy(o => new { o.ChargeSubjectId, o.ChargeSubjectName }).ToList();
            //构建DataTable

            DataTable dt = new DataTable();
            List<ReportHead> ReportHeadList = new List<ReportHead>();
            List<ReportMeterDTO> ReportMeterList = new List<ReportMeterDTO>();


            ReportHeadList.Add(new ReportHead() { Id = "ResourcesName", Name = "资源名称" });
            ReportHeadList.Add(new ReportHead() { Id = "OwnerName", Name = "业主姓名" });

            //加入固定的前两列
            dt.Columns.Add("ResourcesName", typeof(string));

            dt.Columns.Add("OwnerName", typeof(string));

            foreach (var ChargeSubjectItem in ChargeSubjectList)
            {
                ReportHeadList.Add(new ReportHead() { Id = ChargeSubjectItem.Key.ChargeSubjectId.ToString(), Name = ChargeSubjectItem.Key.ChargeSubjectName.ToString() });


                dt.Columns.Add(ChargeSubjectItem.Key.ChargeSubjectId.ToString(), typeof(decimal));
            }

            dt.Columns.Add("ArrearsAmount", typeof(string));
            ReportHeadList.Add(new ReportHead() { Id = "ArrearsAmount", Name = "合计金额" });


            foreach (var a in ResourcesList)
            {
                ReportMeterDTO reportArrearsDTO = new ReportMeterDTO();

                List<ReportRowData> ReportRowDataList = new List<ReportRowData>();

                //赋值
                DataRow dr = dt.NewRow();
                var ResourcesSubject = allList.Where(o => o.ResourcesId == a.Key.ResourcesId && o.RefType == a.Key.RefType).ToList();
                var ArrearsReportDataObj = ResourcesSubject[0];
                //取出姓名
                ReportRowData RowDataName = new ReportRowData();
                ReportRowData RowDataOwnerName = new ReportRowData();
                if (ArrearsReportDataObj.RefType == (int)ReourceTypeEnum.ThreeMeter)
                {

                    dr["ResourcesName"] = HouseDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault() == null ? "" : HouseDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault().Name;
                    RowDataName.Text = HouseDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault() == null ? "" : HouseDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault().Name;


                    dr["OwnerName"] = OwnerDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault() == null ? "" : OwnerDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault().OwnerUserName;
                    RowDataOwnerName.Text = OwnerDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault() == null ? "" : OwnerDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault().OwnerUserName;

                }
                ReportRowDataList.Add(RowDataName);
                ReportRowDataList.Add(RowDataOwnerName);
                decimal SumAmount = 0;


                for (int i = 2; i < ReportHeadList.Count() - 1; i++)
                {
                    var HeadModel = ReportHeadList[i];
                    int ChargeSubjectId = Convert.ToInt32(HeadModel.Id);

                    if (ResourcesSubject.Any(o => o.ChargeSubjectId == ChargeSubjectId))
                    {
                        var ResourcesSubjectItem = ResourcesSubject.Where(o => o.ChargeSubjectId == ChargeSubjectId).FirstOrDefault();
                        ReportRowData RowDataSubject = new ReportRowData();
                        dr[ResourcesSubjectItem.ChargeSubjectId.ToString()] = ResourcesSubjectItem.Amount;

                        RowDataSubject.Text = ResourcesSubjectItem.Amount.ToString();
                        SumAmount += ResourcesSubjectItem.Amount.Value;
                        ReportRowDataList.Add(RowDataSubject);
                    }
                    else
                    {
                        ReportRowData RowDataSubject = new ReportRowData();
                        RowDataSubject.Text = "0";
                        ReportRowDataList.Add(RowDataSubject);
                    }
                }

                dr["ArrearsAmount"] = SumAmount;

                ReportRowData RowDataArrearsAmount = new ReportRowData();
                RowDataArrearsAmount.Text = SumAmount.ToString();
                ReportRowDataList.Add(RowDataArrearsAmount);
                dt.Rows.Add(dr);


                reportArrearsDTO.RowDataList = ReportRowDataList;

                ReportMeterList.Add(reportArrearsDTO);
            }


            ReportModel.ReportHeadList = ReportHeadList;
            ReportModel.ReportMeterDTOList = ReportMeterList;
            return ReportModel;


        }
        public void GetAllMeterReportQuery(ref Condition<ChargBill> PredicateHouse, ref Condition<ChargeSubject> ConditionSubject, ref Condition<ChargeRecord> ConditionRecord, ReportMeterSearchDTO search, ref List<DeptInfo> HouseDeptList, ref List<DeptInfo> OwnerDeptList)
        {
            string ComDeptId = search.ComDeptId.ToString();

            if (!string.IsNullOrEmpty(search.ResourceName))
            {//资源类型是空或者NULL
                Condition<SEC_Dept> condition = new Condition<SEC_Dept>(c => c.Name.Contains(search.ResourceName) && c.DeptType == (int)EDeptType.FangWu && c.Code.Contains(ComDeptId));
                HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoByQuery(condition.ExpressionBody).ToList();
                var DeptIdstr = HouseDeptList.Select(o => o.Id).ToArray();
                PredicateHouse = PredicateHouse & new Condition<ChargBill>(o => DeptIdstr.Contains(o.HouseDeptId));

            }

            if (!string.IsNullOrEmpty(search.OwnerName))
            {
                OwnerDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptAndOwnerListByComDeptId(search.OwnerName, search.ComDeptId.ToString()).ToList();
                var OwenerDeptIdstr = OwnerDeptList.Select(o => o.Id).ToArray();
                PredicateHouse = PredicateHouse & new Condition<ChargBill>(o => OwenerDeptIdstr.Contains(o.HouseDeptId));

            }
            //收费开始日期
            if (search.BeginDate != null && search.BeginDate.Value > DateTime.MinValue)
            {
                ConditionRecord = ConditionRecord & new Condition<ChargeRecord>(o => o.PayDate >= search.BeginDate.Value);

            }
            //收费结束日期
            if (search.EndDate != null && search.EndDate.Value > DateTime.MinValue)
            {
                var EndDate = search.EndDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
                ConditionRecord = ConditionRecord & new Condition<ChargeRecord>(o => o.PayDate <= EndDate);

            }
            if (search.LouyuIdStr != null && search.LouyuIdStr.Length > 0)
            {

                var LouyuList = search.LouyuIdStr.Split(',').ToList().ConvertAll(i => int.Parse(i));
                var bulidList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildsInfoByBuildDeptId(LouyuList);
                string BuildCode = bulidList[0].Building_code;

                Condition<ChargBill> condition_bill_OR = new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(BuildCode));
                foreach (var c in bulidList)
                {
                    condition_bill_OR = condition_bill_OR | new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(c.Building_code));

                }
                PredicateHouse = PredicateHouse & condition_bill_OR;
              

            }





        }
        private IQueryable<MeterReportData> GetMeterUnionList(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, ReportMeterSearchDTO search, ref List<DeptInfo> HouseDeptList, ref List<DeptInfo> OwnerDeptList)
        {

            Condition<ChargBill> ConditionHouse = new Condition<ChargBill>(c => true);
            Condition<ChargBill> ConditionCarPort = new Condition<ChargBill>(c => true);
            Condition<ChargeSubject> ConditionSubject = new Condition<ChargeSubject>(c => true);
            Condition<ChargeRecord> ConditionRecord = new Condition<ChargeRecord>(c => c.ComDeptId == search.ComDeptId);//收费记录
            GetAllMeterReportQuery(ref ConditionHouse, ref ConditionSubject, ref ConditionRecord, search, ref HouseDeptList, ref OwnerDeptList);

            //房间号
            var query_HouseList = (from b in propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(o => o.ComDeptId == search.ComDeptId
                                 //&& o.Status == (int)BillStatusEnum.Paid 
                                 && o.IsDel == false
                                  && (o.HouseDeptId != null && o.HouseDeptId > 0) && o.RefType == (int)ReourceTypeEnum.ThreeMeter).Where(ConditionHouse.ExpressionBody)

                                   join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll().Where(o => o.SubjectType == (int)SubjectTypeEnum.Meter && o.ComDeptId == search.ComDeptId) on b.ChargeSubjectId equals s.Id
                                   join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on b.Id equals m.ChargeBillId
                                   join c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(ConditionRecord.ExpressionBody) on m.ChargeRecordId equals c.Id

                                   group new { b.HouseDeptId, s.Id, s.Name, m.Amount, c.ChargeType }//修改4573 2017/7/5 zzh
                                   by new { b.HouseDeptId, s.Id, s.Name, b.HouseDoorNo } into r
                                   select new MeterReportData
                                   {
                                       //Amount =r.Key.Amount,
                                       Amount = r.Sum(o => o.ChargeType == (int)ChargeTypeEnum.Refund ? o.Amount * -1 : o.Amount),
                                       ResourcesId = r.Key.HouseDeptId,
                                       //ChargeType=r.Key.ChargeType,
                                       RefType = (int)ReourceTypeEnum.ThreeMeter,
                                       //SubjectType == (int)SubjectTypeEnum.Meter
                                       ChargeSubjectName = r.Key.Name,
                                       ChargeSubjectId = r.Key.Id,
                                       ResourcesName = r.Key.HouseDoorNo
                                   });

            //foreach ( var Amount in query_HouseList.Where(o => o.ChargeType == (int)ChargeTypeEnum.Refund))
            //{
            //    Amoun
            //    query_HouseList.Select(o => o.Amount = Amount);
            //}
            //var totalAll = query_HouseList.Sum(h => h.Amount);
            //var rtotal = query_HouseList.Where(h => h.ChargeType == (int)ChargeTypeEnum.Refund).Sum(h => h.Amount);
            //var total = totalAll - rtotal;
            return query_HouseList;


        }

        private class MeterReportData
        {
            public decimal? Amount { get; set; }
            public int? ChargeType { get; set; }
            public int? ResourcesId { get; set; }
            public int? RefType { get; set; }
            public string ChargeSubjectName { get; set; }
            public string ResourcesName { get; set; }
            public int? ChargeSubjectId { get; set; }


        }

        private ReportMeterDTO GetMeterReportDataSumList(List<MeterReportData> UnionList)
        {


            var ChargeSubjectList = UnionList.GroupBy(o => new { o.ChargeSubjectId, o.ChargeSubjectName }).ToList();
            ReportMeterDTO obj = new ReportMeterDTO();
            List<ReportRowData> RowList = new List<ReportRowData>();
            //合计
            ReportRowData TotalName = new ReportRowData()
            {
                Text = "合计"
            };
            RowList.Add(TotalName);
            ReportRowData ArrearsCount = new ReportRowData()
            {
                Text = ""
            };
            RowList.Add(ArrearsCount);

            decimal? SumAmount = 0;

            var unlist = UnionList.ToList();

            foreach (var subjectItem in ChargeSubjectList)
            {
                ReportRowData MeterSubjectSum = new ReportRowData();
                var sumMoney = (from r in unlist
                                where r.ChargeSubjectId == subjectItem.Key.ChargeSubjectId
                                group new { r.ChargeSubjectId, r.Amount } by r.ChargeSubjectId into b
                                select new
                                {
                                    Money = b.Sum(c => c.Amount)
                                }
                                );
                MeterSubjectSum.Text = sumMoney.FirstOrDefault() == null ? "0.00" : sumMoney.FirstOrDefault().Money.ToString();
                SumAmount += sumMoney.FirstOrDefault() == null ? 0 : sumMoney.FirstOrDefault().Money;
                RowList.Add(MeterSubjectSum);
            }
            ReportRowData ArrearsSumAmount = new ReportRowData()
            {
                Text = SumAmount.ToString()
            };
            RowList.Add(ArrearsSumAmount);
            obj.RowDataList = RowList;


            return obj;

        }

        public ReportMeterExportModel GetMeterReportExport(ReportMeterSearchDTO search)
        {

            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                ReportMeterExportModel Model = new ReportMeterExportModel();
                List<DeptInfo> HouseDeptList = new List<DeptInfo>();
                List<DeptInfo> OwnerDeptList = new List<DeptInfo>();
                var UnionList = GetMeterUnionList(propertyMgrUnitOfWork, search, ref HouseDeptList, ref OwnerDeptList).ToList();
                var BasePageList = UnionList.GroupBy(o => new { o.ResourcesId, o.RefType, o.ResourcesName }).OrderBy(o => o.Key.ResourcesName);
                var allList =
                (from a in UnionList
                 join p in BasePageList
                 on new { ResourcesId = a.ResourcesId, RefType = a.RefType } equals new { ResourcesId = p.Key.ResourcesId, RefType = p.Key.RefType }
                 select a).OrderBy(o => o.ResourcesId).ThenBy(o => o.RefType).ToList();

                if (string.IsNullOrEmpty(search.ResourceName) || string.IsNullOrEmpty(search.OwnerName))
                {
                    var ArrHouseId = allList.Where(o => o.RefType == (int)SubjectTypeEnum.Meter).Select(p => p.ResourcesId).ToList();
                    OwnerDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptAndOwnerListByIdArr(ArrHouseId).ToList();

                    Condition<SEC_Dept> condition = new Condition<SEC_Dept>(o => ArrHouseId.Contains(o.Id));
                    HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoByQuery(condition.ExpressionBody).ToList();
                }
                //返回数据
                DataTable dt = new DataTable();

                //TEMP
                List<TemplateModel> TemplateModelList = new List<TemplateModel>();

                //收费对象列表
                var ResourcesList = allList.GroupBy(o => new { o.ResourcesId, o.RefType }).ToList();
                var ChargeSubjectList = UnionList.GroupBy(o => new { o.ChargeSubjectId, o.ChargeSubjectName }).ToList();

                dt.Columns.Add("ResourcesName", typeof(string));
                int seq = 1;
                TemplateModelList.Add(new TemplateModel()
                {
                    CnName = "资源名称",
                    EnName = "ResourcesName",
                    Seq = seq++,
                    Type = "string",
                    IsExport = true
                });
                dt.Columns.Add("OwnerName", typeof(string));
                TemplateModelList.Add(new TemplateModel()
                {
                    CnName = "业主姓名",
                    EnName = "OwnerName",
                    Seq = seq++,
                    Type = "string",
                    IsExport = true
                });



                foreach (var ChargeSubjectItem in ChargeSubjectList)
                {

                    dt.Columns.Add(ChargeSubjectItem.Key.ChargeSubjectId.ToString(), typeof(decimal));

                    TemplateModelList.Add(new TemplateModel()
                    {
                        CnName = ChargeSubjectItem.Key.ChargeSubjectName,
                        EnName = ChargeSubjectItem.Key.ChargeSubjectId.ToString(),
                        Seq = seq++,
                        Type = "Decimal",
                        IsExport = true
                    });

                }

                dt.Columns.Add("ArrearsAmount", typeof(decimal));

                TemplateModelList.Add(new TemplateModel()
                {
                    CnName = "总金额",
                    EnName = "ArrearsAmount",
                    Seq = seq++,
                    Type = "Decimal",
                    IsExport = true
                });



                foreach (var a in ResourcesList)
                {


                    //赋值
                    DataRow dr = dt.NewRow();
                    var ResourcesSubject = allList.Where(o => o.ResourcesId == a.Key.ResourcesId && o.RefType == a.Key.RefType).ToList();
                    var ArrearsReportDataObj = ResourcesSubject[0];
                    //取出姓名

                    if (ArrearsReportDataObj.RefType == (int)SubjectTypeEnum.Meter)
                    {
                        dr["ResourcesName"] = HouseDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault() == null ? "" : HouseDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault().Name;
                        dr["OwnerName"] = OwnerDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault() == null ? "" : OwnerDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault().OwnerUserName;
                    }
                    //else if (ArrearsReportDataObj.RefType == (int)ReourceTypeEnum.CarPark)
                    //{
                    //    dr["ResourcesName"] = ArrearsReportDataObj.ResourcesName;
                    //    dr["OwnerName"] = "";
                    //}
                    decimal SumAmount = 0;
                    foreach (var HeadList in ChargeSubjectList)
                    {
                        int ChargeSubjectId = Convert.ToInt32(HeadList.Key.ChargeSubjectId);
                        var ResourcesSubjectItem = ResourcesSubject.Where(o => o.ChargeSubjectId == ChargeSubjectId).FirstOrDefault();
                        if (ResourcesSubject.Any(o => o.ChargeSubjectId == ChargeSubjectId))
                        {
                            ReportRowData RowDataSubject = new ReportRowData();
                            dr[ResourcesSubjectItem.ChargeSubjectId.ToString()] = ResourcesSubjectItem.Amount;
                            SumAmount += ResourcesSubjectItem.Amount.Value;
                        }
                        else
                        {
                            dr[HeadList.Key.ChargeSubjectId.ToString()] = 0;
                        }
                    }

                    dr["ArrearsAmount"] = SumAmount;
                    dt.Rows.Add(dr);
                }

                var ReportMeterSum = GetMeterReportDataSumList(UnionList);
                DataRow drsum = dt.NewRow();
                int i = 0;
                foreach (var sumobj in ReportMeterSum.RowDataList)
                {

                    drsum[i] = sumobj.Text;
                    i++;
                }
                dt.Rows.Add(drsum);

                Model.ExportData = dt;
                Model.TemPlateList = TemplateModelList;
                return Model;
            }





        }
        #endregion

        #region 2.7版本三表费用明细报表
        public List<ReportMeterDetailDTO> GetMeterReportDetailList(ReportMeterSearchDTO search, out int totalCount, bool IsExport)
        {
            //1.条件
            Condition<ReportMeterDetailDTO> conditions = new Condition<ReportMeterDetailDTO>(c => c.SubjectType == (int)SubjectTypeEnum.Meter);//只针对三表
            //小区
            conditions = conditions & new Condition<ReportMeterDetailDTO>(c => c.ComDeptId == search.ComDeptId);
            //资源名称
            if (!string.IsNullOrEmpty(search.ResourceName))
            {
                conditions = conditions & new Condition<ReportMeterDetailDTO>(c => c.ResourcesName.Contains(search.ResourceName));
            }

            //业主姓名
            if (!string.IsNullOrEmpty(search.OwnerName))
            {
                conditions = conditions & new Condition<ReportMeterDetailDTO>(c => c.OwnerName.Contains(search.OwnerName));
            }

            //收费开始时间
            if (search.BeginDate.HasValue)
            {
                conditions = conditions & new Condition<ReportMeterDetailDTO>(c => c.PayDate >= search.BeginDate);
            }
            //收费结束时间
            if (search.EndDate.HasValue)
            {
                search.EndDate = search.EndDate.Value.AddDays(1);
                conditions = conditions & new Condition<ReportMeterDetailDTO>(c => c.PayDate < search.EndDate);
            }

            Condition<ChargBill> condition = new Condition<ChargBill>(o => true);

            if (search.LouyuIdStr != null && search.LouyuIdStr.Length > 0)
            {

                var LouyuList = search.LouyuIdStr.Split(',').ToList().ConvertAll(i => int.Parse(i));
                var bulidList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildsInfoByBuildDeptId(LouyuList);
                string BuildCode = bulidList[0].Building_code;

                Condition<ChargBill> condition_bill_OR = new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(BuildCode));
                foreach (var c in bulidList)
                {
                    condition_bill_OR = condition_bill_OR | new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(c.Building_code));

                }
                condition = condition & condition_bill_OR;


            }


            //2.查询
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = from b in pmUnitWork.ChargBillRepository.GetAll().Where(condition.ExpressionBody)
                            join m in pmUnitWork.ChargeBillRecordMatchingRepository.GetAll()
                            on b.Id equals m.ChargeBillId
                            select new ReportMeterDetailDTO()
                            {
                                ComDeptId = b.ComDeptId,
                                ResourcesName = b.ResourcesName,
                                ChargeSubjectName = b.ChargeSubject.Name,
                                ChargeType = m.ChargeRecord.ChargeType,
                                SubjectType = b.ChargeSubject.SubjectType,
                                BeginDate = b.BeginDate,
                                EndDate = b.EndDate,
                                PayDate = m.ChargeRecord.PayDate,
                                BillAmount = b.BillAmount,
                                ArrearsAmount = m.Amount,
                                Number = m.ChargeRecord.Receipt.Number,
                                OperatorName = m.ChargeRecord.OperatorName,
                                OwnerName = m.ChargeRecord.CustomerName,
                                PayMthodId = m.ChargeRecord.PayMthodId,
                                Remark = b.Remark,
                                HouseDoorNo = b.HouseDoorNo//修改4575 2017/7/5 zzh
                            };

                totalCount = query.Where(conditions.ExpressionBody).Count();
                //如果导出
                if (IsExport)
                {
                    return query.Where(conditions.ExpressionBody).OrderByDescending(o => o.PayDate).ToList();
                }
                return query.Where(conditions.ExpressionBody).OrderByDescending(o => o.PayDate).Skip(search.PageStart).Take(search.PageSize).ToList();//修改4576 2017/7/5 zzh
            }
        }

        private class MeterReportDataDetail
        {
            public decimal? BillAmount { get; set; }
            public decimal? Amount { get; set; }
            public int? ResourcesId { get; set; }
            public int? RefType { get; set; }
            public int? SubjectType { get; set; }
            public string ChargeSubjectName { get; set; }
            public string ResourcesName { get; set; }
            public string OwnerName { get; set; }
            public DateTime? PayDate { get; set; }
            public string Number { get; set; }
            public DateTime? BeginDate { get; set; }
            public string OperatorName { get; set; }
            public int? PayMthodId { get; set; }
            public DateTime? EndDate { get; set; }
            public DateTime? CreateTime { get; set; }
            public string Remark { get; set; }




        }
        #endregion

        #region 对外收费报表

        public ReportExternalchargeModels GetExternalchargeReportDataList(int PageIndex, int PageSize, ReportExternalchargeSearchDTO search, out int totalCount, bool IsExport = false)
        {

            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                totalCount = 0;
                return GetAllExternalchargeReportDataList(propertyMgrUnitOfWork, PageIndex, PageSize, out totalCount, search, IsExport);
            }

        }
        private ReportExternalchargeModels GetAllExternalchargeReportDataList(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, int PageIndex, int PageSize, out int totalCount, ReportExternalchargeSearchDTO search, bool IsExport = false)
        {
            ReportExternalchargeModels ReportModel = new ReportExternalchargeModels();
            totalCount = 0;
            //获取房间号

            //房间号

            List<DeptInfo> HouseDeptList = new List<DeptInfo>();
            List<DeptInfo> OwnerDeptList = new List<DeptInfo>();

            var UnionList = GetExternalchargeUnionList(propertyMgrUnitOfWork, search, ref HouseDeptList).Where(o => o.Amount != 0).ToList();//修改5431 2017/8/31 zzh


            ReportModel.ReportArrearsSum = GetExternalchargeReportDataSumList(UnionList);


            var BasePageList = UnionList.GroupBy(o => new { o.ResourcesId, o.RefType, o.ResourcesName }).OrderBy(o => o.Key.ResourcesName);
            totalCount = BasePageList.Count();
            var pageList = BasePageList.Skip((PageIndex - 1) * PageSize).Take(PageSize);

            if (IsExport)
            {
                pageList = BasePageList;
            }

            var allList = (from a in UnionList
                           join p in pageList on new { ResourcesId = a.ResourcesId, RefType = a.RefType, ResourcesName=a.ResourcesName } equals new { ResourcesId = p.Key.ResourcesId, RefType = p.Key.RefType , ResourcesName =p.Key.ResourcesName}
                           select a).OrderBy(o => o.ResourcesName).ToList();

            if (string.IsNullOrEmpty(search.ResourceName))
            {
                var ArrHouseId = allList.Where(o => o.RefType == (int)SubjectTypeEnum.Foreig).Select(p => p.ResourcesId).ToList();
                Condition<SEC_Dept> condition = new Condition<SEC_Dept>(o => ArrHouseId.Contains(o.Id));
                HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoByQuery(condition.ExpressionBody).ToList();
            }



            //收费对象列表
            var ResourcesList = allList.GroupBy(o => new { o.ResourcesId, o.RefType, o.ResourcesName }).OrderBy(o => o.Key.ResourcesName).ToList();
            var ChargeSubjectList = UnionList.GroupBy(o => new { o.ChargeSubjectId, o.ChargeSubjectName }).ToList();
            //构建DataTable

            DataTable dt = new DataTable();
            List<ReportHead> ReportHeadList = new List<ReportHead>();
            List<ReportExternalchargeDTO> ReportExternalchargeList = new List<ReportExternalchargeDTO>();


            ReportHeadList.Add(new ReportHead() { Id = "ResourcesName", Name = "收费对象" });

            //加入固定的前列
            dt.Columns.Add("ResourcesName", typeof(string));

            foreach (var ChargeSubjectItem in ChargeSubjectList)
            {
                ReportHeadList.Add(new ReportHead() { Id = ChargeSubjectItem.Key.ChargeSubjectId.ToString(), Name = ChargeSubjectItem.Key.ChargeSubjectName.ToString() });

                dt.Columns.Add(ChargeSubjectItem.Key.ChargeSubjectId.ToString(), typeof(decimal));
            }

            dt.Columns.Add("ArrearsAmount", typeof(string));
            ReportHeadList.Add(new ReportHead() { Id = "ArrearsAmount", Name = "合计金额" });


            foreach (var a in ResourcesList)
            {
                ReportExternalchargeDTO reportArrearsDTO = new ReportExternalchargeDTO();

                List<ReportRowData> ReportRowDataList = new List<ReportRowData>();

                //赋值
                DataRow dr = dt.NewRow();
                var ResourcesSubject = allList.Where(o => o.ResourcesId == a.Key.ResourcesId && o.RefType == a.Key.RefType && o.ResourcesName.Contains(a.Key.ResourcesName)).ToList();
                var ArrearsReportDataObj = ResourcesSubject[0];
                //取出姓名
                ReportRowData RowDataName = new ReportRowData();
                ReportRowData RowDataOwnerName = new ReportRowData();
                if (ArrearsReportDataObj.RefType == (int)SubjectTypeEnum.Foreig)
                {
                    dr["ResourcesName"] = ArrearsReportDataObj.ResourcesName ?? "";
                    RowDataName.Text = ArrearsReportDataObj.ResourcesName ?? "";
                }
                ReportRowDataList.Add(RowDataName);
                decimal SumAmount = 0;


                for (int i = 1; i < ReportHeadList.Count() - 1; i++)
                {
                    var HeadModel = ReportHeadList[i];
                    int ChargeSubjectId = Convert.ToInt32(HeadModel.Id);

                    if (ResourcesSubject.Any(o => o.ChargeSubjectId == ChargeSubjectId))
                    {
                        var ResourcesSubjectItem = ResourcesSubject.Where(o => o.ChargeSubjectId == ChargeSubjectId).FirstOrDefault();
                        ReportRowData RowDataSubject = new ReportRowData();
                        dr[ResourcesSubjectItem.ChargeSubjectId.ToString()] = ResourcesSubjectItem.Amount;

                        RowDataSubject.Text = ResourcesSubjectItem.Amount.ToString();
                        SumAmount += ResourcesSubjectItem.Amount.Value;
                        ReportRowDataList.Add(RowDataSubject);
                    }
                    else
                    {
                        ReportRowData RowDataSubject = new ReportRowData();
                        RowDataSubject.Text = "0";
                        ReportRowDataList.Add(RowDataSubject);
                    }
                }

                dr["ArrearsAmount"] = SumAmount;
                if (SumAmount!=0)
                {
                    ReportRowData RowDataArrearsAmount = new ReportRowData();
                    RowDataArrearsAmount.Text = SumAmount.ToString();
                    ReportRowDataList.Add(RowDataArrearsAmount);
                    dt.Rows.Add(dr);


                    reportArrearsDTO.RowDataList = ReportRowDataList;

                    ReportExternalchargeList.Add(reportArrearsDTO);
                }
                
            }


            ReportModel.ReportHeadList = ReportHeadList;
            ReportModel.ReportExternalchargeDTOList = ReportExternalchargeList;
            return ReportModel;
        }
        public void GetAllExternalchargeReportQuery(ref Condition<ChargBill> PredicateHouse, ref Condition<ChargeSubject> ConditionSubject, ref Condition<ChargeRecord> ConditionRecord, ReportExternalchargeSearchDTO search, ref List<DeptInfo> HouseDeptList)
        {
            string ComDeptId = search.ComDeptId.ToString();

            if (!string.IsNullOrEmpty(search.ResourceName))
            {//资源类型是空或者NULL
                //Condition<SEC_Dept> condition = new Condition<SEC_Dept>(c => c.Name.Contains(search.ResourceName) && c.DeptType == (int)EDeptType.XiaoQu && c.Id.Value == int.Parse(ComDeptId));
                //HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoByQuery(condition.ExpressionBody).ToList();
                //var DeptIdstr = HouseDeptList.Select(o => o.Id).ToArray();
                //PredicateHouse = PredicateHouse & new Condition<ChargBill>(o => DeptIdstr.Contains(o.HouseDeptId));
                ConditionRecord = ConditionRecord & new Condition<ChargeRecord>(o => o.CustomerName.Contains(search.ResourceName));
            }

            //收费开始日期
            if (search.BeginDate != null && search.BeginDate.Value > DateTime.MinValue)
            {
                ConditionRecord = ConditionRecord & new Condition<ChargeRecord>(o => o.PayDate >= search.BeginDate.Value);

            }
            //收费结束日期
            if (search.EndDate != null && search.EndDate.Value > DateTime.MinValue)
            {
                var EndDate = search.EndDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
                ConditionRecord = ConditionRecord & new Condition<ChargeRecord>(o => o.PayDate <= EndDate);

            }
        }
        private IQueryable<ExternalchargeReportData> GetExternalchargeUnionList(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, ReportExternalchargeSearchDTO search, ref List<DeptInfo> HouseDeptList)
        {

            Condition<ChargBill> ConditionHouse = new Condition<ChargBill>(c => true);
            Condition<ChargBill> ConditionCarPort = new Condition<ChargBill>(c => true);
            Condition<ChargeSubject> ConditionSubject = new Condition<ChargeSubject>(c => true);
            Condition<ChargeRecord> ConditionRecord = new Condition<ChargeRecord>(c => c.ComDeptId == search.ComDeptId);//收费记录
            GetAllExternalchargeReportQuery(ref ConditionHouse, ref ConditionSubject, ref ConditionRecord, search, ref HouseDeptList);

            //房间号
            var query_HouseList = (from b in propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(o => o.ComDeptId == search.ComDeptId
                                 && o.RefType == (int)SubjectTypeEnum.Foreig
                                 && o.IsDel == false).Where(ConditionHouse.ExpressionBody)

                                   join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll().Where(o => o.ComDeptId == search.ComDeptId) on b.ChargeSubjectId equals s.Id
                                   join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on b.Id equals m.ChargeBillId
                                   join c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(ConditionRecord.ExpressionBody) on m.ChargeRecordId equals c.Id
                                   group new { b.ResourcesId, s.Id, s.Name, m.Amount, c.ChargeType }
                                   by new { b.ResourcesId, s.Id, s.Name, b.HouseDoorNo,c.CustomerName} into r
                                   select new ExternalchargeReportData
                                   {
                                       //Amount =r.Key.Amount,
                                       Amount = r.Sum(o => o.ChargeType == (int)ChargeTypeEnum.Refund ? o.Amount * -1 : o.Amount),
                                       ResourcesId = r.Key.ResourcesId,
                                       //ChargeType=r.Key.ChargeType,
                                       RefType = (int)SubjectTypeEnum.Foreig,
                                       //SubjectType == (int)SubjectTypeEnum.Meter
                                       ChargeSubjectName = r.Key.Name,
                                       ChargeSubjectId = r.Key.Id,
                                       ResourcesName = r.Key.CustomerName
                                   });
            return query_HouseList;


        }
        private class ExternalchargeReportData
        {
            public decimal? Amount { get; set; }
            public int? ChargeType { get; set; }
            public int? ResourcesId { get; set; }
            public int? RefType { get; set; }
            public string ChargeSubjectName { get; set; }
            public string ResourcesName { get; set; }
            public int? ChargeSubjectId { get; set; }

        }
        private ReportExternalchargeDTO GetExternalchargeReportDataSumList(List<ExternalchargeReportData> UnionList)
        {


            var ChargeSubjectList = UnionList.GroupBy(o => new { o.ChargeSubjectId, o.ChargeSubjectName }).ToList();
            ReportExternalchargeDTO obj = new ReportExternalchargeDTO();
            List<ReportRowData> RowList = new List<ReportRowData>();
            //合计
            ReportRowData TotalName = new ReportRowData()
            {
                Text = "合计"
            };
            RowList.Add(TotalName);
            //ReportRowData ArrearsCount = new ReportRowData()
            //{
            //    Text = ""
            //};
            //RowList.Add(ArrearsCount);

            decimal? SumAmount = 0;

            var unlist = UnionList.ToList();

            foreach (var subjectItem in ChargeSubjectList)
            {
                ReportRowData MeterSubjectSum = new ReportRowData();
                var sumMoney = (from r in unlist
                                where r.ChargeSubjectId == subjectItem.Key.ChargeSubjectId
                                group new { r.ChargeSubjectId, r.Amount } by r.ChargeSubjectId into b
                                select new
                                {
                                    Money = b.Sum(c => c.Amount)
                                }
                                );
                MeterSubjectSum.Text = sumMoney.FirstOrDefault() == null ? "0.00" : sumMoney.FirstOrDefault().Money.ToString();
                SumAmount += sumMoney.FirstOrDefault() == null ? 0 : sumMoney.FirstOrDefault().Money;
                RowList.Add(MeterSubjectSum);
            }
            ReportRowData ArrearsSumAmount = new ReportRowData()
            {
                Text = SumAmount.ToString()
            };
            RowList.Add(ArrearsSumAmount);
            obj.RowDataList = RowList;


            return obj;

        }
        public ReportExternalchargeExportModel GetExternalchargeReportExport(ReportExternalchargeSearchDTO search)
        {

            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                ReportExternalchargeExportModel Model = new ReportExternalchargeExportModel();
                List<DeptInfo> HouseDeptList = new List<DeptInfo>();
                var UnionList = GetExternalchargeUnionList(propertyMgrUnitOfWork, search, ref HouseDeptList).ToList();
                var BasePageList = UnionList.GroupBy(o => new { o.ResourcesId, o.RefType, o.ResourcesName }).OrderBy(o => o.Key.ResourcesName);
                var allList = (from a in UnionList
                               join p in BasePageList on new { ResourcesId = a.ResourcesId, RefType = a.RefType, ResourcesName = a.ResourcesName } equals new { ResourcesId = p.Key.ResourcesId, RefType = p.Key.RefType, ResourcesName = p.Key.ResourcesName }
                               select a).OrderBy(o => o.ResourcesName).ThenBy(o => o.RefType).ToList();

                if (string.IsNullOrEmpty(search.ResourceName))
                {
                    var ArrHouseId = allList.Where(o => o.RefType == (int)SubjectTypeEnum.Foreig).Select(p => p.ResourcesId).ToList();
                    //OwnerDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptAndOwnerListByIdArr(ArrHouseId).ToList();

                    Condition<SEC_Dept> condition = new Condition<SEC_Dept>(o => ArrHouseId.Contains(o.Id));
                    HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoByQuery(condition.ExpressionBody).ToList();
                }
                //返回数据
                DataTable dt = new DataTable();

                //TEMP
                List<TemplateModel> TemplateModelList = new List<TemplateModel>();

                //收费对象列表
                var ResourcesList = allList.GroupBy(o => new { o.ResourcesId, o.RefType , o.ResourcesName }).ToList();
                var ChargeSubjectList = UnionList.GroupBy(o => new { o.ChargeSubjectId, o.ChargeSubjectName }).ToList();

                dt.Columns.Add("ResourcesName", typeof(string));
                int seq = 1;
                TemplateModelList.Add(new TemplateModel()
                {
                    CnName = "收费对象",
                    EnName = "ResourcesName",
                    Seq = seq++,
                    Type = "string",
                    IsExport = true
                });

                foreach (var ChargeSubjectItem in ChargeSubjectList)
                {

                    dt.Columns.Add(ChargeSubjectItem.Key.ChargeSubjectId.ToString(), typeof(decimal));

                    TemplateModelList.Add(new TemplateModel()
                    {
                        CnName = ChargeSubjectItem.Key.ChargeSubjectName,
                        EnName = ChargeSubjectItem.Key.ChargeSubjectId.ToString(),
                        Seq = seq++,
                        Type = "Decimal",
                        IsExport = true
                    });

                }

                dt.Columns.Add("ArrearsAmount", typeof(decimal));

                TemplateModelList.Add(new TemplateModel()
                {
                    CnName = "总金额",
                    EnName = "ArrearsAmount",
                    Seq = seq++,
                    Type = "Decimal",
                    IsExport = true
                });



                foreach (var a in ResourcesList)
                {

                    //赋值
                    DataRow dr = dt.NewRow();
                    var ResourcesSubject = allList.Where(o => o.ResourcesId == a.Key.ResourcesId && o.RefType == a.Key.RefType && o.ResourcesName.Contains(a.Key.ResourcesName)).ToList();
                    var ArrearsReportDataObj = ResourcesSubject[0];
                    //取出姓名

                    if (ArrearsReportDataObj.RefType == (int)SubjectTypeEnum.Foreig)
                    {
                        dr["ResourcesName"] = ArrearsReportDataObj.ResourcesName ?? "";
                    }
                    //else if (ArrearsReportDataObj.RefType == (int)ReourceTypeEnum.CarPark)
                    //{
                    //    dr["ResourcesName"] = ArrearsReportDataObj.ResourcesName;
                    //    dr["OwnerName"] = "";
                    //}
                    decimal SumAmount = 0;
                    foreach (var HeadList in ChargeSubjectList)
                    {
                        int ChargeSubjectId = Convert.ToInt32(HeadList.Key.ChargeSubjectId);
                        var ResourcesSubjectItem = ResourcesSubject.Where(o => o.ChargeSubjectId == ChargeSubjectId).FirstOrDefault();
                        if (ResourcesSubject.Any(o => o.ChargeSubjectId == ChargeSubjectId))
                        {
                            ReportRowData RowDataSubject = new ReportRowData();
                            dr[ResourcesSubjectItem.ChargeSubjectId.ToString()] = ResourcesSubjectItem.Amount;
                            SumAmount += ResourcesSubjectItem.Amount.Value;
                        }
                        else
                        {
                            dr[HeadList.Key.ChargeSubjectId.ToString()] = 0;
                        }
                    }

                    dr["ArrearsAmount"] = SumAmount;
                    if (SumAmount!=0)
                    {
                        dt.Rows.Add(dr);
                    }
                    
                }

                var ReportExternalchargeSum = GetExternalchargeReportDataSumList(UnionList);
                DataRow drsum = dt.NewRow();
                int i = 0;
                foreach (var sumobj in ReportExternalchargeSum.RowDataList)
                {

                    drsum[i] = sumobj.Text;
                    i++;
                }
                dt.Rows.Add(drsum);

                Model.ExportData = dt;
                Model.TemPlateList = TemplateModelList;
                return Model;
            }

        }
        #endregion

        #region 对外收费报表明细表
        public List<ReportExternalchargeDetailDTO> GetExternalchargeReportDetailList(ReportExternalchargeSearchDTO search, out int totalCount, bool IsExport)
        {
            //1.条件
            Condition<ReportExternalchargeDetailDTO> conditions = new Condition<ReportExternalchargeDetailDTO>(c => c.SubjectType == (int)SubjectTypeEnum.Foreig);//对外收费
            //小区
            conditions = conditions & new Condition<ReportExternalchargeDetailDTO>(c => c.ComDeptId == search.ComDeptId);
            //资源名称
            if (!string.IsNullOrEmpty(search.ResourceName))
            {
                conditions = conditions & new Condition<ReportExternalchargeDetailDTO>(c => c.ResourcesName.Contains(search.ResourceName));
            }

            //票据号
            if (!string.IsNullOrEmpty(search.Number))
            {
                conditions = conditions & new Condition<ReportExternalchargeDetailDTO>(c => c.Number.Contains(search.Number));
            }

            //收费开始时间
            if (search.BeginDate.HasValue)
            {
                conditions = conditions & new Condition<ReportExternalchargeDetailDTO>(c => c.PayDate >= search.BeginDate);
            }
            //收费结束时间
            if (search.EndDate.HasValue)
            {
                search.EndDate = search.EndDate.Value.AddDays(1);
                conditions = conditions & new Condition<ReportExternalchargeDetailDTO>(c => c.PayDate < search.EndDate);
            }
            //2.查询
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = from b in pmUnitWork.ChargBillRepository.GetAll()
                            join m in pmUnitWork.ChargeBillRecordMatchingRepository.GetAll()
                            on b.Id equals m.ChargeBillId
                            join c in pmUnitWork.ChargeRecordRepository.GetAll()
                            on m.ChargeRecordId equals c.Id
                            select new ReportExternalchargeDetailDTO()
                            {
                                ComDeptId = b.ComDeptId,
                                ResourcesName = c.CustomerName,
                                ChargeSubjectName = b.ChargeSubject.Name,
                                ChargeType = m.ChargeRecord.ChargeType,
                                SubjectType = b.ChargeSubject.SubjectType,
                                BeginDate = b.BeginDate,
                                EndDate = b.EndDate,
                                PayDate = m.ChargeRecord.PayDate,
                                BillAmount = b.BillAmount,
                                ArrearsAmount = m.Amount,
                                Number = m.ChargeRecord.Receipt.Number,
                                OperatorName = m.ChargeRecord.OperatorName,
                                PayMthodId = m.ChargeRecord.PayMthodId,
                                Remark = b.Remark,
                            };

                totalCount = query.Where(conditions.ExpressionBody).Count();
               
                var data= query.Where(conditions.ExpressionBody).OrderByDescending(o => o.PayDate).ToList();
                //修改退款数据显示
                foreach (var item in data)
                {
                    if (item.ChargeType == 4)
                    {
                        item.BillAmount = item.BillAmount * (-1);
                        item.ArrearsAmount = item.ArrearsAmount * (-1);
                    }
                }
                //如果导出
                if (IsExport)
                {
                    return data;
                }
                return data.Skip(search.PageStart).Take(search.PageSize).ToList();
            }
        }
        #endregion

        #region 2.4版本欠费报表

        public ReportArrearsModels GetArrearsReportDataList(int PageIndex, int PageSize, ReportArrearsSearchDTO search, out int totalCount, bool IsExport = false)
        {

            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                totalCount = 0;


                return GetAllArrearsReportDataList(propertyMgrUnitOfWork, PageIndex, PageSize, out totalCount, search, IsExport);



            }

        }

        private ReportArrearsModels GetAllArrearsReportDataList(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, int PageIndex, int PageSize, out int totalCount, ReportArrearsSearchDTO search, bool IsExport = false)
        {
            ReportArrearsModels ReportModel = new ReportArrearsModels();
            totalCount = 0;
            //获取房间号

            //房间号

            List<DeptInfo> HouseDeptList = new List<DeptInfo>();
            List<DeptInfo> OwnerDeptList = new List<DeptInfo>();

            var UnionList = GetArrearsUnionList(propertyMgrUnitOfWork, search, ref HouseDeptList, ref OwnerDeptList);


            ReportModel.ReportArrearsSum = GetArrearsReportDataSumList(UnionList);


            var BasePageList = UnionList.GroupBy(o => new { o.ResourcesId, o.RefType, o.ResourcesName }).OrderBy(o => o.Key.ResourcesName);
            totalCount = BasePageList.Count();
            var pageList = BasePageList.Skip((PageIndex - 1) * PageSize).Take(PageSize);

            if (IsExport)
            {
                pageList = BasePageList;
            }

            var allList = (from a in UnionList
                           join p in pageList on new { ResourcesId = a.ResourcesId, RefType = a.RefType } equals new { ResourcesId = p.Key.ResourcesId, RefType = p.Key.RefType }
                           select a).OrderBy(o => o.ResourcesId).ThenBy(o => o.RefType).ToList();

            if (string.IsNullOrEmpty(search.ResourceName) || string.IsNullOrEmpty(search.OwnerName))
            {
                var ArrHouseId = allList.Where(o => o.RefType == (int)ReourceTypeEnum.House).Select(p => p.ResourcesId).ToList();
                if (string.IsNullOrEmpty(search.OwnerName))
                {
                    OwnerDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptAndOwnerListByIdArr(ArrHouseId).ToList();
                }

                Condition<SEC_Dept> condition = new Condition<SEC_Dept>(o => ArrHouseId.Contains(o.Id));
                HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoByQuery(condition.ExpressionBody).ToList();
            }



            //收费对象列表
            var ResourcesList = allList.GroupBy(o => new { o.ResourcesId, o.RefType }).ToList();
            var ChargeSubjectList = UnionList.GroupBy(o => new { o.ChargeSubjectId, o.ChargeSubjectName }).ToList();

            //构建DataTable

            DataTable dt = new DataTable();
            List<ReportHead> ReportHeadList = new List<ReportHead>();
            List<ReportArrearsDTO> ReportArrearsList = new List<ReportArrearsDTO>();


            ReportHeadList.Add(new ReportHead() { Id = "ResourcesName", Name = "资源名称" });
            ReportHeadList.Add(new ReportHead() { Id = "OwnerName", Name = "业主姓名" });

            //加入固定的前两列
            dt.Columns.Add("ResourcesName", typeof(string));

            dt.Columns.Add("OwnerName", typeof(string));

            foreach (var ChargeSubjectItem in ChargeSubjectList)
            {
                ReportHeadList.Add(new ReportHead() { Id = ChargeSubjectItem.Key.ChargeSubjectId.ToString(), Name = ChargeSubjectItem.Key.ChargeSubjectName.ToString() });


                dt.Columns.Add(ChargeSubjectItem.Key.ChargeSubjectId.ToString(), typeof(decimal));
            }

            dt.Columns.Add("ArrearsAmount", typeof(string));
            ReportHeadList.Add(new ReportHead() { Id = "ArrearsAmount", Name = "欠费金额" });


            foreach (var a in ResourcesList)
            {
                ReportArrearsDTO reportArrearsDTO = new ReportArrearsDTO();

                List<ReportRowData> ReportRowDataList = new List<ReportRowData>();

                //赋值
                DataRow dr = dt.NewRow();
                var ResourcesSubject = allList.Where(o => o.ResourcesId == a.Key.ResourcesId && o.RefType == a.Key.RefType).ToList();
                var ArrearsReportDataObj = ResourcesSubject[0];
                //取出姓名
                ReportRowData RowDataName = new ReportRowData();
                ReportRowData RowDataOwnerName = new ReportRowData();
                if (ArrearsReportDataObj.RefType == (int)ReourceTypeEnum.House)
                {

                    dr["ResourcesName"] = HouseDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault() == null ? "" : HouseDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault().Name;
                    RowDataName.Text = HouseDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault() == null ? "" : HouseDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault().Name;


                    dr["OwnerName"] = OwnerDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault() == null ? "" : OwnerDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault().OwnerUserName;
                    RowDataOwnerName.Text = OwnerDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault() == null ? "" : OwnerDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault().OwnerUserName;

                }
                else if (ArrearsReportDataObj.RefType == (int)ReourceTypeEnum.CarPark)
                {
                    RowDataName.Text = ArrearsReportDataObj.ResourcesName; ;
                    dr["ResourcesName"] = ArrearsReportDataObj.ResourcesName;
                    RowDataOwnerName.Text = string.Empty;
                    dr["OwnerName"] = "";
                }
                ReportRowDataList.Add(RowDataName);
                ReportRowDataList.Add(RowDataOwnerName);



                decimal SumAmount = 0;


                for (int i = 2; i < ReportHeadList.Count() - 1; i++)
                {
                    var HeadModel = ReportHeadList[i];
                    int ChargeSubjectId = Convert.ToInt32(HeadModel.Id);
                    if (ResourcesSubject.Any(o => o.ChargeSubjectId == ChargeSubjectId))
                    {
                        var ResourcesSubjectItem = ResourcesSubject.Where(o => o.ChargeSubjectId == ChargeSubjectId).FirstOrDefault();
                        ReportRowData RowDataSubject = new ReportRowData();
                        dr[ResourcesSubjectItem.ChargeSubjectId.ToString()] = ResourcesSubjectItem.Amount;

                        RowDataSubject.Text = ResourcesSubjectItem.Amount.ToString();

                        SumAmount += ResourcesSubjectItem.Amount.Value;
                        ReportRowDataList.Add(RowDataSubject);
                    }
                    else
                    {
                        ReportRowData RowDataSubject = new ReportRowData();
                        RowDataSubject.Text = "0";
                        ReportRowDataList.Add(RowDataSubject);
                    }
                }

                dr["ArrearsAmount"] = SumAmount;

                ReportRowData RowDataArrearsAmount = new ReportRowData();
                RowDataArrearsAmount.Text = SumAmount.ToString();
                ReportRowDataList.Add(RowDataArrearsAmount);
                dt.Rows.Add(dr);


                reportArrearsDTO.RowDataList = ReportRowDataList;

                ReportArrearsList.Add(reportArrearsDTO);
            }


            ReportModel.ReportHeadList = ReportHeadList;
            ReportModel.ReportArrearsDTOList = ReportArrearsList;
            return ReportModel;


        }

        //根据传入的条件返回查询条件
        public void GetAllArrearsReportQuery(ref Condition<ChargBill> PredicateHouse, ref Condition<ChargBill> PredicateCarPort, ReportArrearsSearchDTO search, ref List<DeptInfo> HouseDeptList, ref List<DeptInfo> OwnerDeptList)
        {
            string ComDeptId = search.ComDeptId.ToString();

            if (!string.IsNullOrEmpty(search.ResourceName))
            {//资源类型是空或者NULL
                Condition<SEC_Dept> condition = new Condition<SEC_Dept>(c => c.Name.Contains(search.ResourceName) && c.DeptType == (int)EDeptType.FangWu && c.Code.Contains(ComDeptId));
                HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoByQuery(condition.ExpressionBody).ToList();
                var DeptIdstr = HouseDeptList.Select(o => o.Id).ToArray();
                PredicateHouse = PredicateHouse & new Condition<ChargBill>(o => DeptIdstr.Contains(o.HouseDeptId));
                PredicateCarPort = PredicateCarPort & new Condition<ChargBill>(o => o.ResourcesName.Contains(search.ResourceName));
            }
            if (search.LouyuIdStr != null && search.LouyuIdStr.Length > 0)
            {

                var LouyuList = search.LouyuIdStr.Split(',').ToList().ConvertAll(i => int.Parse(i));
                var bulidList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildsInfoByBuildDeptId(LouyuList);
                string BuildCode = bulidList[0].Building_code;

                Condition<ChargBill> condition_bill_OR = new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(BuildCode));
                foreach (var c in bulidList)
                {
                    condition_bill_OR = condition_bill_OR | new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(c.Building_code));

                }
                PredicateHouse = PredicateHouse & condition_bill_OR;
                PredicateCarPort = PredicateCarPort & condition_bill_OR;

            }

            if (!string.IsNullOrEmpty(search.OwnerName))
            {
                OwnerDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptAndOwnerListByComDeptId(search.OwnerName, search.ComDeptId.ToString()).ToList();
                var OwenerDeptIdstr = OwnerDeptList.Select(o => o.Id).ToArray();
                PredicateHouse = PredicateHouse & new Condition<ChargBill>(o => OwenerDeptIdstr.Contains(o.HouseDeptId));
                PredicateCarPort = PredicateHouse & new Condition<ChargBill>(o => o.Id == null);
            }
        }

        private IQueryable<ArrearsReportData> GetArrearsUnionList(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, ReportArrearsSearchDTO search, ref List<DeptInfo> HouseDeptList, ref List<DeptInfo> OwnerDeptList)
        {

            Condition<ChargBill> ConditionHouse = new Condition<ChargBill>(c => true);
            Condition<ChargBill> ConditionCarPort = new Condition<ChargBill>(c => true);
            GetAllArrearsReportQuery(ref ConditionHouse, ref ConditionCarPort, search, ref HouseDeptList, ref OwnerDeptList);

           




            //房间号
            var query_HouseList = (from b in propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(o => o.ComDeptId == search.ComDeptId
                                  && o.EndDate <= search.ChargeDate && o.Status == (int)BillStatusEnum.NoPayment && o.IsDel == false
                                  && (o.HouseDeptId != null && o.HouseDeptId > 0)).Where(ConditionHouse.ExpressionBody)

                                   join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on b.ChargeSubjectId equals s.Id
                                   group new { b.HouseDeptId, s.Id, s.Name, b.BillAmount, b.ReceivedAmount, b.ReliefAmount }
                                   by new { b.HouseDeptId, s.Id, s.Name } into r
                                   select new ArrearsReportData
                                   {
                                       Amount = r.Sum(o => (o.BillAmount - o.ReceivedAmount - o.ReliefAmount)),
                                       ResourcesId = r.Key.HouseDeptId,
                                       RefType = (int)ReourceTypeEnum.House,
                                       ChargeSubjectName = r.Key.Name,
                                       ChargeSubjectId = r.Key.Id,
                                       ResourcesName = ""


                                   });


            //没有房间号的科目
            var query_ResourcesList = (
                            from b in propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(o => o.ComDeptId == search.ComDeptId
                                      && o.EndDate <= search.ChargeDate && o.Status == (int)BillStatusEnum.NoPayment && o.IsDel == false
                                      && (o.HouseDeptId == null || o.HouseDeptId <= 0)).Where(ConditionCarPort.ExpressionBody)
                            join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on b.ChargeSubjectId equals s.Id
                            group new { b.HouseDeptId, s.Id, s.Name, b.BillAmount, b.ReceivedAmount, b.ReliefAmount }
                            by new { b.ResourcesId, b.RefType, s.Id, s.Name, b.ResourcesName } into r
                            select new ArrearsReportData
                            {
                                Amount = r.Sum(o => (o.BillAmount - o.ReceivedAmount - o.ReliefAmount)),
                                ResourcesId = r.Key.ResourcesId,
                                RefType = r.Key.RefType,
                                ChargeSubjectName = r.Key.Name,
                                ChargeSubjectId = r.Key.Id,
                                ResourcesName = r.Key.ResourcesName

                            }

                         );

            return query_HouseList.Concat(query_ResourcesList);


        }

        private class ArrearsReportData
        {
            public decimal? Amount { get; set; }
            public int? ResourcesId { get; set; }
            public int? RefType { get; set; }
            public string ChargeSubjectName { get; set; }
            public string ResourcesName { get; set; }
            public int? ChargeSubjectId { get; set; }


            //{
            //    get
            //    {
            //        return ResourcesId.Value.ToString() + "_" + RefType.ToString();
            //    }
            //}

        }

        private ReportArrearsDTO GetArrearsReportDataSumList(IQueryable<ArrearsReportData> UnionList)
        {



            var ChargeSubjectList = UnionList.GroupBy(o => new { o.ChargeSubjectId, o.ChargeSubjectName }).ToList();
            ReportArrearsDTO obj = new ReportArrearsDTO();
            List<ReportRowData> RowList = new List<ReportRowData>();
            //合计
            ReportRowData TotalName = new ReportRowData()
            {
                Text = "合计"
            };
            RowList.Add(TotalName);
            //欠费人数
            ReportRowData ArrearsCount = new ReportRowData()
            {
                Text = "欠费业主数：" + UnionList.GroupBy(o => o.ResourcesId).Count().ToString()
            };
            RowList.Add(ArrearsCount);

            decimal? SumAmount = 0;

            foreach (var subjectItem in ChargeSubjectList)
            {
                ReportRowData ArrearsSubjectSum = new ReportRowData();
                var sumMoney = (from r in UnionList
                                where r.ChargeSubjectId == subjectItem.Key.ChargeSubjectId
                                group new { r.ChargeSubjectId, r.Amount } by r.ChargeSubjectId into b
                                select new
                                {
                                    Money = b.Sum(c => c.Amount)
                                }
                                );
                ArrearsSubjectSum.Text = sumMoney.FirstOrDefault() == null ? "0.00" : sumMoney.FirstOrDefault().Money.ToString();
                SumAmount += sumMoney.FirstOrDefault() == null ? 0 : sumMoney.FirstOrDefault().Money;
                RowList.Add(ArrearsSubjectSum);
            }
            ReportRowData ArrearsSumAmount = new ReportRowData()
            {
                Text = SumAmount.ToString()
            };
            RowList.Add(ArrearsSumAmount);
            obj.RowDataList = RowList;


            return obj;

        }

        public ReportArrearsExportModel GetArrearsReportExport(ReportArrearsSearchDTO search)
        {

            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                ReportArrearsExportModel Model = new ReportArrearsExportModel();
                List<DeptInfo> HouseDeptList = new List<DeptInfo>();
                List<DeptInfo> OwnerDeptList = new List<DeptInfo>();
                var UnionList = GetArrearsUnionList(propertyMgrUnitOfWork, search, ref HouseDeptList, ref OwnerDeptList);
                var BasePageList = UnionList.GroupBy(o => new { o.ResourcesId, o.RefType, o.ResourcesName }).OrderBy(o => o.Key.ResourcesName);
                var allList = (from a in UnionList
                               join p in BasePageList on new { ResourcesId = a.ResourcesId, RefType = a.RefType } equals new { ResourcesId = p.Key.ResourcesId, RefType = p.Key.RefType }
                               select a).OrderBy(o => o.ResourcesId).ThenBy(o => o.RefType).ToList();

                if (string.IsNullOrEmpty(search.ResourceName) || string.IsNullOrEmpty(search.OwnerName))
                {
                    var ArrHouseId = allList.Where(o => o.RefType == (int)ReourceTypeEnum.House).Select(p => p.ResourcesId).ToList();
                    OwnerDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptAndOwnerListByIdArr(ArrHouseId).ToList();

                    Condition<SEC_Dept> condition = new Condition<SEC_Dept>(o => ArrHouseId.Contains(o.Id));
                    HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoByQuery(condition.ExpressionBody).ToList();
                }
                //返回数据
                DataTable dt = new DataTable();

                //TEMP
                List<TemplateModel> TemplateModelList = new List<TemplateModel>();

                //收费对象列表
                var ResourcesList = allList.GroupBy(o => new { o.ResourcesId, o.RefType }).ToList();
                var ChargeSubjectList = UnionList.GroupBy(o => new { o.ChargeSubjectId, o.ChargeSubjectName }).ToList();

                dt.Columns.Add("ResourcesName", typeof(string));
                int seq = 1;
                TemplateModelList.Add(new TemplateModel()
                {
                    CnName = "资源名称",
                    EnName = "ResourcesName",
                    Seq = seq++,
                    Type = "string",
                    IsExport = true
                });
                dt.Columns.Add("OwnerName", typeof(string));
                TemplateModelList.Add(new TemplateModel()
                {
                    CnName = "业主姓名",
                    EnName = "OwnerName",
                    Seq = seq++,
                    Type = "string",
                    IsExport = true
                });



                foreach (var ChargeSubjectItem in ChargeSubjectList)
                {

                    dt.Columns.Add(ChargeSubjectItem.Key.ChargeSubjectId.ToString(), typeof(decimal));

                    TemplateModelList.Add(new TemplateModel()
                    {
                        CnName = ChargeSubjectItem.Key.ChargeSubjectName,
                        EnName = ChargeSubjectItem.Key.ChargeSubjectId.ToString(),
                        Seq = seq++,
                        Type = "Decimal",
                        IsExport = true
                    });

                }

                dt.Columns.Add("ArrearsAmount", typeof(decimal));

                TemplateModelList.Add(new TemplateModel()
                {
                    CnName = "欠费金额",
                    EnName = "ArrearsAmount",
                    Seq = seq++,
                    Type = "Decimal",
                    IsExport = true
                });



                foreach (var a in ResourcesList)
                {


                    //赋值
                    DataRow dr = dt.NewRow();
                    var ResourcesSubject = allList.Where(o => o.ResourcesId == a.Key.ResourcesId && o.RefType == a.Key.RefType).ToList();
                    var ArrearsReportDataObj = ResourcesSubject[0];
                    //取出姓名

                    if (ArrearsReportDataObj.RefType == (int)ReourceTypeEnum.House)
                    {
                        dr["ResourcesName"] = HouseDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault() == null ? "" : HouseDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault().Name;
                        dr["OwnerName"] = OwnerDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault() == null ? "" : OwnerDeptList.Where(o => o.Id == ArrearsReportDataObj.ResourcesId).FirstOrDefault().OwnerUserName;
                    }
                    else if (ArrearsReportDataObj.RefType == (int)ReourceTypeEnum.CarPark)
                    {
                        dr["ResourcesName"] = ArrearsReportDataObj.ResourcesName;
                        dr["OwnerName"] = "";
                    }
                    decimal SumAmount = 0;
                    foreach (var HeadList in ChargeSubjectList)
                    {
                        int ChargeSubjectId = Convert.ToInt32(HeadList.Key.ChargeSubjectId);
                        var ResourcesSubjectItem = ResourcesSubject.Where(o => o.ChargeSubjectId == ChargeSubjectId).FirstOrDefault();
                        if (ResourcesSubject.Any(o => o.ChargeSubjectId == ChargeSubjectId))
                        {
                            ReportRowData RowDataSubject = new ReportRowData();
                            dr[ResourcesSubjectItem.ChargeSubjectId.ToString()] = ResourcesSubjectItem.Amount;
                            SumAmount += ResourcesSubjectItem.Amount.Value;
                        }
                        else
                        {
                            dr[HeadList.Key.ChargeSubjectId.ToString()] = 0;
                        }
                    }

                    dr["ArrearsAmount"] = SumAmount;
                    dt.Rows.Add(dr);
                }

                var ReportArrearsSum = GetArrearsReportDataSumList(UnionList);
                DataRow drsum = dt.NewRow();
                int i = 0;
                foreach (var sumobj in ReportArrearsSum.RowDataList)
                {

                    drsum[i] = sumobj.Text;
                    i++;
                }
                dt.Rows.Add(drsum);

                Model.ExportData = dt;
                Model.TemPlateList = TemplateModelList;
                return Model;
            }





        }



        #endregion

        #region 2.5版本预交费明细表

        public List<PrePaymentDetailReportDTO> PrePaymentDetailReport(PrePaymentDetailSearchDTO search, out int totalCount, out decimal SumAmount, bool IsExport)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                totalCount = 0;
                List<DeptInfo> HouseDeptList = new List<DeptInfo>();
                List<DeptInfo> OwnerDeptList = new List<DeptInfo>();
                var UnionList = GetPrePayDetailDataList(propertyMgrUnitOfWork, search, ref HouseDeptList, ref OwnerDeptList);
                var BasePageList = UnionList.OrderByDescending(o => o.ChargePayDate);
                var pageList = new List<PrePaymentDetailReportDTO>();  // BasePageList.Skip((search.PageIndex - 1) * search.PageSize).Take(search.PageSize);
                if (IsExport)
                {
                    pageList = BasePageList.ToList();
                }
                else
                {
                    pageList = BasePageList.Skip((search.PageIndex - 1) * search.PageSize).Take(search.PageSize).ToList();
                }

                var dataList = pageList.ToList();
                totalCount = BasePageList.Count();

                var ArrHouseId = dataList.Where(o => o.RefType == (int)ReourceTypeEnum.House).Select(p => p.RescourcesId).Distinct().ToList();
                OwnerDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptAndOwnerListByIdArr(ArrHouseId).ToList();

                Condition<SEC_Dept> condition = new Condition<SEC_Dept>(o => ArrHouseId.Contains(o.Id));
                HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoByQuery(condition.ExpressionBody).ToList();


                //资源名称+客户名称
                var PrepayDetailReportList = HandlePrePaymentNameAttributes(dataList, HouseDeptList, OwnerDeptList);

                if (BasePageList.Count() > 0)
                    SumAmount = BasePageList.Sum(o => o.Amount).Value;
                else
                    SumAmount = 0;
                //导出加入合计行
                if (IsExport)
                {
                    PrePaymentDetailReportDTO summodel = new PrePaymentDetailReportDTO();
                    summodel.ResourcesName = "合计金额";
                    summodel.Amount = BasePageList.Sum(o => o.Amount);
                    PrepayDetailReportList.Add(summodel);
                }

                return PrepayDetailReportList;

            }

        }

        private List<PrePaymentDetailReportDTO> HandlePrePaymentNameAttributes(List<PrePaymentDetailReportDTO> List, List<DeptInfo> HouseDeptList, List<DeptInfo> OwnerDeptList)
        {





            var list = (from p in List.Where(o => o.RefType == (int)ReourceTypeEnum.House)
                        join h in HouseDeptList on p.RescourcesId equals h.Id into h_join
                        from ph in h_join.DefaultIfEmpty()
                        join o in OwnerDeptList on p.RescourcesId equals o.Id into o_join
                        from po in o_join.DefaultIfEmpty()
                        select new PrePaymentDetailReportDTO
                        {

                            ReceiptNum = p.ReceiptNum,
                            ChargePayDate = p.ChargePayDate,
                            ChargePayDateStr = p.ChargePayDate.Value.ToString("yyyy-MM-dd HH:mm"),
                            Amount = p.Amount,
                            RefType = p.RefType,
                            RescourcesId = p.RescourcesId,
                            SubjectType = p.SubjectType,
                            ResourcesName = (ph == null ? "" : ph.Name),
                            BeginDate = p.BeginDate,
                            EndDate = p.EndDate,
                            ChargeSubjectName = p.ChargeSubjectName,
                            PreType = (p.SubjectType == (int)SubjectTypeEnum.SystemPreset ? "预存" : "预收"),
                            CustomerName = (po == null ? "" : po.OwnerUserName),
                            Remark = p.Remark == null ? "" : p.Remark

                        }

                ).ToList();
            var NoHouseList = List.Where(o => o.RefType != (int)ReourceTypeEnum.House).Select(o => new PrePaymentDetailReportDTO
            {
                ReceiptNum = o.ReceiptNum,
                ChargePayDate = Convert.ToDateTime(o.ChargePayDate.Value.ToString("yyyy-MM-dd HH:mm")),
                ChargePayDateStr = o.ChargePayDate.Value.ToString("yyyy-MM-dd HH:mm"),
                Amount = o.Amount,
                RefType = o.RefType,
                RescourcesId = o.RescourcesId,
                SubjectType = o.SubjectType,
                ResourcesName = o.ResourcesName,
                BeginDate = o.BeginDate,
                EndDate = o.EndDate,
                ChargeSubjectName = o.ChargeSubjectName,
                PreType = (o.SubjectType == (int)SubjectTypeEnum.SystemPreset ? "预存" : "预收"),
                CustomerName = o.CustomerName,
                Remark = (o.SubjectType == (int)SubjectTypeEnum.SystemPreset ? o.BeginDate.Value.ToString("yyyy-MM-dd HH:mm") + "到" + o.EndDate.Value.ToString("yyyy-MM-dd HH:mm") : o.Remark)
            }).ToList();
            return list.Concat(NoHouseList).ToList();


        }


        private IQueryable<PrePaymentDetailReportDTO> GetPrePayDetailDataList(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, PrePaymentDetailSearchDTO search, ref List<DeptInfo> HouseDeptList, ref List<DeptInfo> OwnerDeptList)
        {
            int ComDeptId = search.ComDeptId.Value;

            Condition<ChargBill> ConditionHouse = new Condition<ChargBill>(c => c.ComDeptId == ComDeptId && c.IsDel == false && (c.HouseDeptId != null && c.HouseDeptId > 0));
            Condition<ChargBill> ConditionCarPort = new Condition<ChargBill>(c => c.ComDeptId == ComDeptId && c.IsDel == false && (c.HouseDeptId == null || c.HouseDeptId <= 0));
            Condition<Receipt> ConditionReceipt = new Condition<Receipt>(c => true);//票据
            Condition<ChargeRecord> ConditionRecord = new Condition<ChargeRecord>(c => c.ChargeType != (int)ChargeTypeEnum.BalanceTransfer && c.PayMthodId != (int)PayTypeEnum.InternalTransfer);//收费记录
            GetPrePayDetailReportQuery(ref ConditionHouse, ref ConditionCarPort, ref ConditionReceipt, ref ConditionRecord, search, ref HouseDeptList, ref OwnerDeptList);

            //有房间的资源产生的预交费记录
            var Query_HousePrePaymentList = (from c in propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(ConditionHouse.ExpressionBody)
                                             join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on c.Id equals m.ChargeBillId
                                             join cr in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(ConditionRecord.ExpressionBody) on m.ChargeRecordId equals cr.Id
                                             join r in propertyMgrUnitOfWork.ReceiptRepository.GetAll().Where(ConditionReceipt.ExpressionBody) on cr.ReceiptId equals r.Id
                                             join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on c.ChargeSubjectId equals s.Id
                                             where (s.SubjectType == (int)SubjectTypeEnum.SystemPreset || c.BeginDate > cr.PayDate)
                                             //  where (cr.ChargeType != (int)ChargeTypeEnum.Refund)
                                             select new PrePaymentDetailReportDTO
                                             {
                                                 ReceiptNum = r.Number,
                                                 ChargePayDate = cr.PayDate,
                                                 Amount = cr.ChargeType == (int)ChargeTypeEnum.Refund ? -m.Amount : m.Amount,
                                                 RefType = (int)ReourceTypeEnum.House,
                                                 RescourcesId = c.HouseDeptId,
                                                 SubjectType = s.SubjectType,
                                                 ResourcesName = cr.ResourcesNames,
                                                 BeginDate = c.BeginDate,
                                                 EndDate = c.EndDate,
                                                 ChargeSubjectName = s.Name,
                                                 PreType = "",
                                                 CustomerName = "",
                                                 Remark = c.Remark
                                             }
                                             );


            //没有房间的车位产生的预交费记录

            var Query_CarPortPrePaymentList = (from c in propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(ConditionCarPort.ExpressionBody)
                                               join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on c.Id equals m.ChargeBillId
                                               join cr in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(ConditionRecord.ExpressionBody) on m.ChargeRecordId equals cr.Id
                                               join r in propertyMgrUnitOfWork.ReceiptRepository.GetAll().Where(ConditionReceipt.ExpressionBody) on cr.ReceiptId equals r.Id
                                               join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on c.ChargeSubjectId equals s.Id
                                               where (s.SubjectType == (int)SubjectTypeEnum.SystemPreset || c.BeginDate > cr.PayDate)
                                               //   where (cr.ChargeType != (int)ChargeTypeEnum.Refund)
                                               select new PrePaymentDetailReportDTO
                                               {
                                                   ReceiptNum = r.Number,
                                                   ChargePayDate = cr.PayDate,
                                                   Amount = cr.ChargeType == (int)ChargeTypeEnum.Refund ? -m.Amount : m.Amount,
                                                   RefType = (int)ReourceTypeEnum.CarPark,
                                                   RescourcesId = c.ResourcesId,
                                                   SubjectType = s.SubjectType,
                                                   ResourcesName = cr.ResourcesNames,
                                                   BeginDate = c.BeginDate,
                                                   EndDate = c.EndDate,
                                                   ChargeSubjectName = s.Name,
                                                   PreType = "",
                                                   CustomerName = cr.CustomerName,
                                                   Remark = c.Remark


                                               }
                                            );




            return Query_HousePrePaymentList.Concat(Query_CarPortPrePaymentList);
        }


        private void GetPrePayDetailReportQuery(ref Condition<ChargBill> PredicateHouse, ref Condition<ChargBill> PredicateCarPort, ref Condition<Receipt> ConditionReceipt, ref Condition<ChargeRecord> ConditionRecord, PrePaymentDetailSearchDTO search, ref List<DeptInfo> HouseDeptList, ref List<DeptInfo> OwnerDeptList)
        {
            string ComDeptId = search.ComDeptId.ToString();

            if (!string.IsNullOrEmpty(search.ResourceName))
            {//资源类型是空或者NULL
                Condition<SEC_Dept> condition = new Condition<SEC_Dept>(c => c.Name.Contains(search.ResourceName) && c.DeptType == (int)EDeptType.FangWu && c.Code.Contains(ComDeptId));
                HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoByQuery(condition.ExpressionBody).ToList();
                var DeptIdstr = HouseDeptList.Select(o => o.Id).ToArray();
                PredicateHouse = PredicateHouse & new Condition<ChargBill>(o => DeptIdstr.Contains(o.HouseDeptId));
                PredicateCarPort = PredicateCarPort & new Condition<ChargBill>(o => o.ResourcesName.Contains(search.ResourceName));
            }

            if (!string.IsNullOrEmpty(search.OwnerName))
            {
                OwnerDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptAndOwnerListByComDeptId(search.OwnerName, search.ComDeptId.ToString()).ToList();
                var OwenerDeptIdstr = OwnerDeptList.Select(o => o.Id).ToArray();
                PredicateHouse = PredicateHouse & new Condition<ChargBill>(o => OwenerDeptIdstr.Contains(o.HouseDeptId));
                PredicateCarPort = PredicateHouse & new Condition<ChargBill>(o => o.Id == null);
            }

            //收费项目
            if (search.ChargeSubjectId.Value > 0)
            {
                PredicateHouse = PredicateHouse & new Condition<ChargBill>(o => o.ChargeSubjectId == search.ChargeSubjectId.Value);
                PredicateCarPort = PredicateCarPort & new Condition<ChargBill>(o => o.ChargeSubjectId == search.ChargeSubjectId.Value);
            }
            //收费开始日期
            if (search.ChargeBeginDate != null && search.ChargeBeginDate.Value > DateTime.MinValue)
            {
                ConditionRecord = ConditionRecord & new Condition<ChargeRecord>(o => o.PayDate >= search.ChargeBeginDate.Value);

            }
            //收费结束日期
            if (search.ChargeBeginDate != null && search.ChargeEndDate.Value > DateTime.MinValue)
            {
                var EndDate = search.ChargeEndDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
                ConditionRecord = ConditionRecord & new Condition<ChargeRecord>(o => o.PayDate <= EndDate);

            }

            //票据号
            if (!string.IsNullOrEmpty(search.ReceiptNum))
            {
                ConditionReceipt = ConditionReceipt & new Condition<Receipt>(o => o.Number == search.ReceiptNum);
            }
            if (search.LouyuIdStr != null && search.LouyuIdStr.Length > 0)
            {

                var LouyuList = search.LouyuIdStr.Split(',').ToList().ConvertAll(i => int.Parse(i));
                var bulidList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildsInfoByBuildDeptId(LouyuList);
                string BuildCode = bulidList[0].Building_code;

                Condition<ChargBill> condition_bill_OR = new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(BuildCode));
                foreach (var c in bulidList)
                {
                    condition_bill_OR = condition_bill_OR | new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(c.Building_code));

                }
                PredicateHouse = PredicateHouse & condition_bill_OR;
                PredicateCarPort = PredicateCarPort & condition_bill_OR;

            }




        }

        #endregion

        #region 预交费抵扣明细表
        public List<PrePaymentdeductionDetailReportDTO> PrePaymentdeductionDetailReport(PrePaymentdeductionDetailSearchDTO search, out int totalCount, bool IsExport)
        {
            //条件
            Condition<PrePaymentdeductionDetailReportDTO> condotion = new Condition<PrePaymentdeductionDetailReportDTO>(c => c.PayMthodId == (int)PayTypeEnum.InternalTransfer);
            Condition<ChargBill> condotions = new Condition<ChargBill>(o=>true);
            //小区
            condotion = condotion & new Condition<PrePaymentdeductionDetailReportDTO>(c => c.ComDeptId == search.ComDeptId);
            //资源名称
            if (!string.IsNullOrEmpty(search.ResourceName))
            {
                condotion = condotion & new Condition<PrePaymentdeductionDetailReportDTO>(c => c.ResourcesName.Contains(search.ResourceName));
            };
            //业主姓名
            if (!string.IsNullOrEmpty(search.OwnerName))
            {
                condotion = condotion & new Condition<PrePaymentdeductionDetailReportDTO>(c => c.OwnerName.Contains(search.OwnerName));
            }
            //票据号
            if (!string.IsNullOrEmpty(search.ReceiptNum))
            {
                condotion = condotion & new Condition<PrePaymentdeductionDetailReportDTO>(c => c.Number.Contains(search.ReceiptNum));
            }
            //收费项目
            if (search.ChargeSubjectId.Value > 0)
            {
                condotion = condotion & new Condition<PrePaymentdeductionDetailReportDTO>(c => c.ChargeSubjectId == search.ChargeSubjectId);
            }
            //收费开始时间
            if (search.ChargeBeginDate.HasValue)
            {
                condotion = condotion & new Condition<PrePaymentdeductionDetailReportDTO>(c => c.PayDate >= search.ChargeBeginDate);
            }
            //收费结束时间
            if (search.ChargeEndDate.HasValue)
            {
                search.ChargeEndDate = search.ChargeEndDate.Value.AddDays(1);
                condotion = condotion & new Condition<PrePaymentdeductionDetailReportDTO>(c => c.PayDate < search.ChargeEndDate);
            }
            if (search.LouyuIdStr != null && search.LouyuIdStr.Length > 0)
            {

                var LouyuList = search.LouyuIdStr.Split(',').ToList().ConvertAll(i => int.Parse(i));
                var bulidList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildsInfoByBuildDeptId(LouyuList);
                string BuildCode = bulidList[0].Building_code;

                Condition<ChargBill> condition_bill_OR = new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(BuildCode));
                foreach (var c in bulidList)
                {
                    condition_bill_OR = condition_bill_OR | new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(c.Building_code));

                }
                condotions = condotions & condition_bill_OR;


            }




            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = from b in pmUnitWork.ChargBillRepository.GetAll().Where(condotions.ExpressionBody)
                            join m in pmUnitWork.ChargeBillRecordMatchingRepository.GetAll()
                            on b.Id equals m.ChargeBillId
                            select new PrePaymentdeductionDetailReportDTO()
                            {
                                ChargeSubjectId = b.ChargeSubject.Id,
                                ComDeptId = b.ComDeptId,
                                ResourcesName = b.ResourcesName,
                                HouseDoorNo = b.HouseDoorNo,
                                ChargeSubjectName = b.ChargeSubject.Name,
                                ChargeType = m.ChargeRecord.ChargeType,
                                BeginDate = b.BeginDate,
                                EndDate = b.EndDate,
                                PayDate = m.ChargeRecord.PayDate,
                                BillAmount = b.BillAmount,
                                ArrearsAmount = m.Amount,
                                Number = m.ChargeRecord.Receipt.Number,
                                OperatorName = m.ChargeRecord.OperatorName,
                                OwnerName = m.ChargeRecord.CustomerName,
                                PayMthodId = m.ChargeRecord.PayMthodId,
                                Remark = b.Remark
                            };
                totalCount = query.Where(condotion.ExpressionBody).Count();
                //如果导出
                if (IsExport)
                {
                    return query.Where(condotion.ExpressionBody).OrderByDescending(o => o.PayDate).ToList();
                }
                return query.Where(condotion.ExpressionBody).OrderByDescending(o => o.PayDate).Skip(search.PageStart).Take(search.PageSize).ToList();
            };
        }
        #endregion

        #region 2.6版本欠费明细报表
        public List<ReportArrearsDetailDTO> GetArrearsReportDetailList(int PageIndex, int PageSize, ReportArrearsSearchDTO search, out int totalCount, bool IsExport)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                totalCount = 0;
                List<DeptInfo> HouseDeptList = new List<DeptInfo>();
                List<DeptInfo> OwnerDeptList = new List<DeptInfo>();
                var UnionList = GetArrearDetailDataList(propertyMgrUnitOfWork, search, ref HouseDeptList, ref OwnerDeptList);
                var BasePageList = UnionList.OrderBy(o => o.ResourcesName).ThenBy(o => o.ChargeSubjectName).ThenBy(o => o.BeginDate);
                var pageList = new List<ArrearsReportDataDetail>();  // BasePageList.Skip((search.PageIndex - 1) * search.PageSize).Take(search.PageSize);
                if (IsExport)
                {
                    pageList = BasePageList.ToList();
                }
                else
                {
                    pageList = BasePageList.Skip((search.PageIndex - 1) * search.PageSize).Take(search.PageSize).ToList();

                }

                var dataList = pageList.ToList();
                totalCount = BasePageList.Count();


                if (string.IsNullOrEmpty(search.OwnerName))
                {
                    var ArrHouseId = dataList.Where(o => o.RefType == (int)ReourceTypeEnum.House).Select(p => p.ResourcesId).Distinct().ToList();
                    OwnerDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptAndOwnerListByIdArr(ArrHouseId).ToList();
                }





                //资源名称+客户名称
                var ArrearsReportDetailList = ExChangeArrearsReportDataDetailToReportArrearsDetailDTO(dataList, OwnerDeptList);


                //导出加入合计行
                if (IsExport)
                {
                    ReportArrearsDetailDTO summodel = new ReportArrearsDetailDTO();
                    summodel.ResourcesName = "合计金额";
                    summodel.BillAmount = BasePageList.Sum(o => o.BillAmount);
                    summodel.ArrearsAmount = BasePageList.Sum(o => o.Amount);
                    ArrearsReportDetailList.Add(summodel);
                }
                return ArrearsReportDetailList;
            }
        }

        private List<ReportArrearsDetailDTO> ExChangeArrearsReportDataDetailToReportArrearsDetailDTO(List<ArrearsReportDataDetail> List, List<DeptInfo> OwnerDeptList)
        {

            var Houselist = (from p in List.Where(o => o.RefType == (int)ReourceTypeEnum.House)

                             join o in OwnerDeptList on p.ResourcesId equals o.Id into o_join
                             from po in o_join.DefaultIfEmpty()
                             select new ReportArrearsDetailDTO
                             {
                                 ResourcesName = p.ResourcesName == null ? "" : p.ResourcesName,
                                 OwnerName = (po == null ? "" : po.OwnerUserName),
                                 ChargeSubjectName = p.ChargeSubjectName,
                                 BeginDate = p.BeginDate,
                                 EndDate = p.EndDate,
                                 BillAmount = p.BillAmount,
                                 BeginDateStr = p.BeginDate.Value.ToString("yyyy-MM-dd"),
                                 EndDateStr = p.EndDate.Value.ToString("yyyy-MM-dd"),
                                 CreateTimeStr = p.CreateTime == null ? p.BeginDate.Value.ToString("yyyy-MM-dd") : p.CreateTime.Value.ToString("yyyy-MM-dd"),
                                 ArrearsAmount = p.Amount,
                                 CreateTime = p.CreateTime,
                                 Remark = p.Remark == null ? "" : p.Remark

                             }

               ).ToList();
            var NoHouseList = List.Where(o => o.RefType != (int)ReourceTypeEnum.House)
                .Select(o => new ReportArrearsDetailDTO
                {

                    ResourcesName = o.ResourcesName == null ? "" : o.ResourcesName,
                    OwnerName = o.OwnerName == null ? "" : o.ResourcesName,
                    ChargeSubjectName = o.ChargeSubjectName,
                    BeginDate = o.BeginDate,
                    EndDate = o.EndDate,
                    BillAmount = o.BillAmount,
                    BeginDateStr = o.BeginDate.Value.ToString("yyyy-MM-dd"),
                    EndDateStr = o.EndDate.Value.ToString("yyyy-MM-dd"),
                    CreateTimeStr = o.CreateTime == null ? o.BeginDate.Value.ToString("yyyy-MM-dd") : o.CreateTime.Value.ToString("yyyy-MM-dd"),
                    ArrearsAmount = o.Amount,
                    CreateTime = o.CreateTime,
                    Remark = o.Remark == null ? "" : o.Remark
                }).ToList();
            return Houselist.Concat(NoHouseList).ToList();


        }


        private IQueryable<ArrearsReportDataDetail> GetArrearDetailDataList(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, ReportArrearsSearchDTO search, ref List<DeptInfo> HouseDeptList, ref List<DeptInfo> OwnerDeptList)
        {
            int ComDeptId = search.ComDeptId.Value;
            Condition<ChargBill> ConditionHouse = new Condition<ChargBill>(c => c.ComDeptId == ComDeptId
                                                        && c.IsDel == false
                                                        && (c.HouseDeptId != null && c.HouseDeptId > 0)
                                                        && c.Status == (int)BillStatusEnum.NoPayment);
            if (!string.IsNullOrEmpty(search.ResourceName))
            {
                ConditionHouse = ConditionHouse & new Condition<ChargBill>(c => c.ResourcesName.Contains(search.ResourceName));
            }
            Condition<ChargBill> ConditionCarPort = new Condition<ChargBill>(c => c.ComDeptId == ComDeptId
                                                        && c.IsDel == false
                                                        && (c.HouseDeptId == null || c.HouseDeptId <= 0)
                                                        && c.Status == (int)BillStatusEnum.NoPayment);

           

            GetArrearDetailReportQuery(ref ConditionHouse, ref ConditionCarPort, search, ref OwnerDeptList);

            //有房间的资源产生的预交费记录
            var Query_HousePrePaymentList = (from c in propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(ConditionHouse.ExpressionBody)
                                             join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on c.ChargeSubjectId equals s.Id
                                             select new ArrearsReportDataDetail
                                             {
                                                 BillAmount = c.BillAmount,
                                                 Amount = (c.BillAmount - c.ReceivedAmount - c.ReliefAmount),
                                                 ResourcesId = c.HouseDeptId,
                                                 RefType = (int)ReourceTypeEnum.House,
                                                 ChargeSubjectName = s.Name,
                                                 ResourcesName = c.ResourcesName,
                                                 BeginDate = c.BeginDate,
                                                 EndDate = c.EndDate,
                                                 CreateTime = c.CreateTime,
                                                 Remark = c.Remark,
                                                 OwnerName = ""


                                             }
                                            );
            var Query_CarPortPrePaymentList =
                          (from c in propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(ConditionCarPort.ExpressionBody)
                           join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on c.ChargeSubjectId equals s.Id
                           where (c.HouseDeptId == null || c.HouseDeptId <= 0)
                           select new ArrearsReportDataDetail
                           {
                               BillAmount = c.BillAmount,
                               Amount = (c.BillAmount - c.ReceivedAmount - c.ReliefAmount),
                               ResourcesId = c.HouseDeptId,
                               RefType = (int)ReourceTypeEnum.CarPark,
                               ChargeSubjectName = s.Name,
                               ResourcesName = c.ResourcesName,
                               BeginDate = c.BeginDate,
                               EndDate = c.EndDate,
                               CreateTime = c.CreateTime,
                               Remark = c.Remark,
                               OwnerName = ""
                           }
                           );

            return Query_HousePrePaymentList.Concat(Query_CarPortPrePaymentList);


        }

        /// <summary>
        /// 查询条件的处理
        /// </summary>
        /// <param name="PredicateHouse"></param>
        /// <param name="PredicateCarPort"></param>
        /// <param name="search"></param>
        /// <param name="HouseDeptList"></param>
        /// <param name="OwnerDeptList"></param>
        private void GetArrearDetailReportQuery(ref Condition<ChargBill> PredicateHouse, ref Condition<ChargBill> PredicateCarPort, ReportArrearsSearchDTO search, ref List<DeptInfo> OwnerDeptList)
        {
            string ComDeptId = search.ComDeptId.ToString();

            if (!string.IsNullOrEmpty(search.ResourceName))
            {//资源类型是空或者NULL
             //Condition<SEC_Dept> condition = new Condition<SEC_Dept>(c => c.Name.Contains(search.ResourceName) && c.DeptType == (int)EDeptType.FangWu && c.Code.Contains(ComDeptId));
             //var  HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetDeptInfoByQuery(condition.ExpressionBody).ToList();
             //var DeptIdstr = HouseDeptList.Select(o => o.Id).ToArray();
             //PredicateHouse = PredicateHouse & new Condition<ChargBill>(o => DeptIdstr.Contains(o.HouseDeptId));
                PredicateCarPort = PredicateCarPort & new Condition<ChargBill>(o => o.ResourcesName.Contains(search.ResourceName));
            }
            //业主姓名
            if (!string.IsNullOrEmpty(search.OwnerName))
            {
                OwnerDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptAndOwnerListByComDeptId(search.OwnerName, search.ComDeptId.ToString()).ToList();
                var OwenerDeptIdstr = OwnerDeptList.Select(o => o.Id).ToArray();
                PredicateHouse = PredicateHouse & new Condition<ChargBill>(o => OwenerDeptIdstr.Contains(o.HouseDeptId));
                PredicateCarPort = PredicateHouse & new Condition<ChargBill>(o => o.Id == null);
            }

            //收费结束日期
            if (search.ChargeDate != null && search.ChargeDate.Value > DateTime.MinValue)
            {
                var EndDate = search.ChargeDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
                PredicateHouse = PredicateHouse & new Condition<ChargBill>(o => o.EndDate <= EndDate);
                PredicateCarPort = PredicateCarPort & new Condition<ChargBill>(o => o.EndDate <= EndDate);

            }

            if (search.LouyuIdStr != null && search.LouyuIdStr.Length > 0)
            {

                var LouyuList = search.LouyuIdStr.Split(',').ToList().ConvertAll(i => int.Parse(i));
                var bulidList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetBuildsInfoByBuildDeptId(LouyuList);
                string BuildCode = bulidList[0].Building_code;

                Condition<ChargBill> condition_bill_OR = new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(BuildCode));
                foreach (var c in bulidList)
                {
                    condition_bill_OR = condition_bill_OR | new Condition<ChargBill>(o => o.HouseDoorNo.StartsWith(c.Building_code));

                }
                PredicateHouse = PredicateHouse & condition_bill_OR;
                PredicateCarPort = PredicateCarPort & condition_bill_OR;

            }



        }

        private class ArrearsReportDataDetail
        {
            public decimal? BillAmount { get; set; }
            public decimal? Amount { get; set; }
            public int? ResourcesId { get; set; }
            public int? RefType { get; set; }
            public string ChargeSubjectName { get; set; }
            public string ResourcesName { get; set; }
            public string OwnerName { get; set; }
            public DateTime? BeginDate { get; set; }
            public DateTime? EndDate { get; set; }
            public DateTime? CreateTime { get; set; }
            public string Remark { get; set; }




        }

        #endregion

        #region 2.8综合报表

        #region 收费项目汇总表
        /// <summary>
        /// 综合报表——收费项目汇总
        /// </summary>
        /// <returns></returns>
        public IList<ReportTableDTO> GetIntegratedReportChargeSubjectList(Condition<ChargBill> Predicate, Condition<ChargeRecord> PredicateRecord)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
               return  GetIntegratedReportChargeSubjectQuery(propertyMgrUnitOfWork,Predicate, PredicateRecord);
            }

              
        }

        private IList<ReportTableDTO> GetIntegratedReportChargeSubjectQuery(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, Condition<ChargBill> Predicate, Condition<ChargeRecord> PredicateRecord)
        {
            //预存费不计入综合报表
            var Query = (from b in propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(Predicate)
                         join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll().Where(o => o.SubjectType != (int)SubjectTypeEnum.SystemPreset) on b.ChargeSubjectId equals s.Id
                         group new { s.Id, s.Name, b.BillAmount, b.ReliefAmount, b.ReceivedAmount } by new { s.Id, s.Name } into r
                         select new ReportTableDTO
                         {
                             ChargeSubjectName = r.Key.Name,
                             ChargeSubjectId =r.Key.Id,
                             TotalRecAmount = r.Sum(o => o.BillAmount - o.ReliefAmount)
                        
                         }
                        ).ToList();

         


            var RecordQuery = (from r in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(PredicateRecord)
                               join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on r.Id equals m.ChargeRecordId
                               join b in propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(Predicate) on m.ChargeBillId equals b.Id
                               join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll().Where(o => o.SubjectType != (int)SubjectTypeEnum.SystemPreset) on b.ChargeSubjectId equals s.Id
                               select new ReportTableDTO
                               {
                                   ChargeSubjectName = s.Name,
                                   ChargeSubjectId = s.Id,
                                   RececiveTotal = r.ChargeType == (int)ChargeTypeEnum.Refund ? -m.Amount.Value : m.Amount.Value

                               }
                               ).ToList();
            var RecordQueryList = (from r in RecordQuery
                                   group new { r.ChargeSubjectId, r.ChargeSubjectName, r.RececiveTotal } by new { r.ChargeSubjectId, r.ChargeSubjectName } into rg
                                   select new ReportTableDTO
                                   {
                                       ChargeSubjectName = rg.Key.ChargeSubjectName,
                                       ChargeSubjectId = rg.Key.ChargeSubjectId,
                                       RececiveTotal = rg.Sum(o => o.RececiveTotal)
                                   }

                                  ).ToList();

            var ReturnList = (from c in Query
                              join r in RecordQueryList on c.ChargeSubjectId equals r.ChargeSubjectId into rtemple
                              from record in rtemple.DefaultIfEmpty()
                              select new ReportTableDTO
                              {
                                  ChargeSubjectName = c.ChargeSubjectName,
                                  ChargeSubjectId = c.ChargeSubjectId,
                                  TotalRecAmount = c.TotalRecAmount,
                                  RececiveTotal = record==null?0:record.RececiveTotal,
                                  UnPaidAmountTotal = (c.TotalRecAmount==null?0:c.TotalRecAmount.Value)-(record == null ? 0 : record.RececiveTotal.Value)
                                  
                              }
                              ).ToList();



            ReturnList.Add(new ReportTableDTO {
                ChargeSubjectName="合计",
                TotalRecAmount = ReturnList.Sum(o=>o.TotalRecAmount),
                RececiveTotal = ReturnList.Sum(o => o.RececiveTotal),
                UnPaidAmountTotal = ReturnList.Sum(o => o.UnPaidAmountTotal),
                GroupId=3
            });

            ReturnList.ForEach(o =>
            {
                if (o.TotalRecAmount > 0)
                    o.PayRate = Math.Round((decimal)(o.RececiveTotal / o.TotalRecAmount) * 100, 2, MidpointRounding.AwayFromZero).ToString() + "%";
                else
                    o.PayRate = string.Empty;
            });

            return ReturnList;
        }

        #endregion

        #region 房屋明细汇总表

        public IList<ReportTableDTO> GetIntegratedReportHouseDetaillList(Condition<ChargBill> Predicate, Condition<ChargeRecord> PredicateRecord, out int totalCount,int PageSize,int PageIndex,bool IsExport)
        {

            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var BaseQuery = GetIntegratedReportHouseDetaillQuery(propertyMgrUnitOfWork, Predicate, PredicateRecord);
               
                totalCount = BaseQuery.Count();
                var BaseList = new List<ReportTableDTO>();
                if (IsExport)
                {
                    BaseList = BaseQuery.OrderBy(o=>o.ResourceName).ToList();
                }
                else
                {

                    BaseList = BaseQuery.OrderBy(o => o.ResourceName).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                }
                //获取户主

                var ArrHouseId = BaseList.Where(o => o.RefType == (int)ReourceTypeEnum.House).Select(p => p.ResourcesId).ToList();
               

                var HouseDeptList = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouDeptAndOwnerListByIdArr(ArrHouseId).ToList();

                var ReturnList =( from b in BaseList
                                 join h in HouseDeptList on b.ResourcesId equals h.Id into hleft
                                 from htemp in hleft.DefaultIfEmpty()
                                 select new ReportTableDTO
                                 {
                                     ResourcesId = b.ResourcesId,
                                     RefType = b.RefType,
                                     ResourceName = b.ResourceName,
                                     TotalRecAmount = b.TotalRecAmount,
                                     RececiveTotal = b.RececiveTotal,
                                     OwnerUserName = htemp==null?"": htemp.OwnerUserName,
                                     UnPaidAmountTotal = (b.TotalRecAmount == null ? 0 : b.TotalRecAmount.Value) - b.RececiveTotal.Value,
                                     PayRate = b.TotalRecAmount==0?"0.00%": Math.Round((decimal)(b.RececiveTotal / b.TotalRecAmount) * 100, 2, MidpointRounding.AwayFromZero).ToString() + "%"

            }).ToList();
                if (IsExport)
                {
                    ReturnList.Add(new ReportTableDTO
                    {
                        ResourceName = "合计",
                        TotalRecAmount = ReturnList.Sum(o => o.TotalRecAmount),
                        RececiveTotal = ReturnList.Sum(o => o.RececiveTotal),
                        UnPaidAmountTotal = ReturnList.Sum(o => o.UnPaidAmountTotal),
                         
                       PayRate = ReturnList.Sum(o => o.TotalRecAmount)==0?"0.00%":Math.Round((decimal)(ReturnList.Sum(o => o.RececiveTotal) / ReturnList.Sum(o => o.TotalRecAmount)) * 100, 2, MidpointRounding.AwayFromZero).ToString() + "%"
                    });
                }


                return ReturnList;
            }




        }
        private IQueryable<ReportTableDTO> GetIntegratedReportHouseDetaillQuery(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, Condition<ChargBill> Predicate, Condition<ChargeRecord> PredicateRecord)
        {
            //预存费不计入综合报表
            var query = (from c in propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(Predicate)
                         join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll().Where(o => o.SubjectType != (int)SubjectTypeEnum.SystemPreset) on c.ChargeSubjectId equals s.Id

                         select new 
                         {
                             ResourcesId = c.HouseDeptId > 0 ? c.HouseDeptId : c.ResourcesId,
                             RefType = c.HouseDeptId > 0 ? (int)ReourceTypeEnum.House : c.RefType,
                             ResourcesName = c.HouseDeptId > 0 ? c.HouseDoorNo : c.ResourcesName,
                             BillAmount = (c.BillAmount - c.ReliefAmount),
                             Id = c.Id
                         });




            var RecordQuery = (from r in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(PredicateRecord)
                     join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on r.Id equals m.ChargeRecordId
                     join c in propertyMgrUnitOfWork.ChargBillRepository.GetAll().Where(Predicate) on m.ChargeBillId equals c.Id
                     join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll().Where(o => o.SubjectType != (int)SubjectTypeEnum.SystemPreset) on c.ChargeSubjectId equals s.Id
                     select new 
                     {
                         ResourcesId = c.HouseDeptId > 0 ? c.HouseDeptId : c.ResourcesId,
                         RefType = c.HouseDeptId > 0 ? (int)ReourceTypeEnum.House : c.RefType,
                         ResourceName = c.HouseDeptId > 0 ? c.HouseDoorNo : c.ResourcesName,
                         TotalRecAmount = (c.BillAmount - c.ReliefAmount),
                         RececiveTotal = r.ChargeType == (int)ChargeTypeEnum.Refund ? -m.Amount.Value : m.Amount.Value,
                         ChargeBillId = c.Id
                        
                     });

            var RecordQueryDD = from r in RecordQuery
                                group new { r.ChargeBillId, r.RececiveTotal } by new { r.ChargeBillId } into rgroup
                                select new 
                                {
                                    Id = rgroup.Key.ChargeBillId,
                                    Amount = rgroup.Sum(o=>o.RececiveTotal)
                                };




            var ResultQuery = (from t in (from b in query
                                          join r in RecordQueryDD on b.Id equals r.Id into rleft
                                          from rtemp in rleft.DefaultIfEmpty()
                                          select new 
                                          {
                                              ResourcesId = b.ResourcesId,
                                              RefType = b.RefType,
                                              ResourcesName = b.ResourcesName,
                                              BillAmount = b.BillAmount,
                                              ReceivedAmount = rtemp.Amount

                                          })
                               group new { t.ResourcesId, t.RefType, t.ResourcesName, t.BillAmount, t.ReceivedAmount } by new { t.ResourcesId, t.RefType, t.ResourcesName } into tgroup
                               select new ReportTableDTO
                               {
                                 
                                   ResourcesId = tgroup.Key.ResourcesId,
                                   RefType = tgroup.Key.RefType,
                                   ResourceName = tgroup.Key.ResourcesName,
                                   TotalRecAmount = tgroup.Sum(o => o.BillAmount)==null?0: tgroup.Sum(o => o.BillAmount),
                                   RececiveTotal = tgroup.Sum(o => o.ReceivedAmount),
                               }).OrderBy(o=>o.ResourceName);
            return ResultQuery;
        }

        #endregion

        #endregion

        #region 2.9临时收费报表

        public ReportArrearsModels GetTemporaryChargesChargeSubjectReport()
        {



            return null;

        }



        #endregion


    }


}
