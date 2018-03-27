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
    public partial class ChargeRecordDomainService
    {
        public IList<ChargeRecord> GetChargeRecordList(int startRowIndex, int PageSize, Expression<Func<ChargeRecord, bool>> predicate, string sortExpressions, int houseDeptId, out int totalCount)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //var dataList = propertyMgrUnitOfWork.ChargeRecordRepository.Paging(PageIndex, PageSize, predicate, sortExpressions, out totalCount).ToList();
                var query = from c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(predicate)
                                   join  m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on c.Id equals m.ChargeRecordId
                                   join   b in propertyMgrUnitOfWork.ChargBillRepository.GetAll() on m.ChargeBillId equals b.Id
                                  where c.ChargeBillRecordMatchingList.Any(m => m.HouseDeptId == houseDeptId)&& c.ChargeType !=(int)ChargeTypeEnum.ForeignCharge
                                 select c;
                var dataList = query.Distinct().SortingAndPaging(sortExpressions, startRowIndex, PageSize, out totalCount).ToList();
                foreach (var item in dataList)
                {
                    item.ReceiptId = item.Receipt.Id;
                }
                return dataList;
            }
        }


        public IList<ChargeRecord> GetChargeRecordList(int startRowIndex, int PageSize, Expression<Func<ChargeRecord, bool>> predicate, string sortExpressions, int ResourcesId, int RefTypeId, out int totalCount)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = from c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(predicate)
                            join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on c.Id equals m.ChargeRecordId
                            join b in propertyMgrUnitOfWork.ChargBillRepository.GetAll() on m.ChargeBillId equals b.Id
                            where b.ResourcesId == ResourcesId && b.RefType == RefTypeId
                            
                            select c;
                var dataList = query.Distinct().SortingAndPaging(sortExpressions, startRowIndex, PageSize, out totalCount).ToList();
                foreach (var item in dataList)
                {
                    item.ReceiptId = item.Receipt.Id;
                }
                return dataList;
            }
        }







        public IList<ChargeRecord> GetChargeRecordList(int startRowIndex, int PageSize, Expression<Func<ChargeRecord, bool>> predicate, string sortExpressions, bool isDeveloper, out int totalCount)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //var dataList = propertyMgrUnitOfWork.ChargeRecordRepository.Paging(PageIndex, PageSize, predicate, sortExpressions, out totalCount).ToList();
                //先查出是开发商的账单
                var billQuery = (from m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll()
                                 join b in propertyMgrUnitOfWork.ChargBillRepository.GetAll() on m.ChargeBillId equals b.Id
                                 where b.IsDevPay == isDeveloper
                                 select m);
                var query = propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(predicate).Where(c => billQuery.Any(b => b.ChargeRecordId == c.Id));
                var dataList = query.SortingAndPaging(sortExpressions, startRowIndex, PageSize, out totalCount).ToList();
                foreach (var item in dataList)
                {
                    item.ReceiptId = item.Receipt.Id;
                }
                return dataList;
            }
        }

        public ChargeRecord GetChargeRecordById(string Id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var record = propertyMgrUnitOfWork.ChargeRecordRepository.GetByKey(Id);
                record.ReceiptId = record.Receipt.Id;
                return record;
            }
        }

        public ChargeRecord GetChargeRecordDiscountById(string Id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var record = propertyMgrUnitOfWork.ChargeRecordRepository.GetByKey(Id);
                record.ReceiptId = record.Receipt.Id;
                record.PaymentDiscountList.ToList();
                
                return record;
            }
        }

        /// <summary>
        /// 退款记录引用 优惠信息
        /// </summary>
        /// <param name="record"></param>
        public void RefPaymentDiscountList(ChargeRecord record)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var refRecord = propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(p => p.ReceiptId == record.ReceiptId && p.Id != record.Id).FirstOrDefault();
                if (refRecord != null)
                {
                    record.PaymentDiscountList = refRecord.PaymentDiscountList.ToList();
                }
            }
        }

        public bool Update(ChargeRecord domainChargeRecord)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var chargeRecord = propertyMgrUnitOfWork.ChargeRecordRepository.GetByKey(domainChargeRecord.Id);
                chargeRecord.UpdateTime = DateTime.Now;
                chargeRecord.Operator = domainChargeRecord.Operator;
                chargeRecord.OperatorName = domainChargeRecord.OperatorName;
                chargeRecord.PayMthodId = domainChargeRecord.PayMthodId;
                chargeRecord.Remark = domainChargeRecord.Remark;
                propertyMgrUnitOfWork.ChargeRecordRepository.Update(chargeRecord);
                propertyMgrUnitOfWork.Commit();
                return true;
            }
        }

        public ResultModel ReceiptPrint(ChargeRecord domainChargeRecord, string number)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var chargeRecord = propertyMgrUnitOfWork.ChargeRecordRepository.GetByKey(domainChargeRecord.Id);
                //chargeRecord.UpdateTime = DateTime.Now;
                //chargeRecord.Operator = domainChargeRecord.Operator;
                //chargeRecord.OperatorName = domainChargeRecord.OperatorName;
                chargeRecord.Remark = domainChargeRecord.Remark;
                bool IsReceiptbook = false;
                string ReciptNumber = string.Empty;
                if (string.IsNullOrEmpty(chargeRecord.Receipt.Number))
                {
                    //chargeRecord.Receipt.Number = number;
                    //票据号修改为自动生成

                    if (BillCommonService.Instance.CheckHasReceiptBook(propertyMgrUnitOfWork, chargeRecord.ComDeptId.Value))
                    {
                        try
                        {
                            ReciptNumber = BillCommonService.Instance.GenerateReceiptBookNumber(propertyMgrUnitOfWork, chargeRecord.ComDeptId.Value);

                            int i = 1;
                            while (PaymentService.Instance.CheckReceiptBookNumRepeat(ReciptNumber, chargeRecord.ComDeptId.Value, ""))
                            {
                                ReciptNumber = BillCommonService.Instance.GenerateReceiptBookNumber(propertyMgrUnitOfWork, chargeRecord.ComDeptId.Value,i);
                                i++;
                                if (i == 11)
                                {
                                    break;
                                }
                            }
                            if (i == 11)
                            {
                                 return new ResultModel() { IsSuccess = false, ErrorCode = "820", Msg = "票据号生成失败，请联系管理员" };
                            }
                            IsReceiptbook = true;

                        }
                        catch (Exception ReceiptEx)
                        {
                            return new ResultModel() { IsSuccess = false, ErrorCode = "820", Msg = ReceiptEx.Message };
                        }
                    }
                    else
                    {
                        return new ResultModel() { IsSuccess = false, ErrorCode = "820", Msg = "未能找到该小区有可用票据本，请检查票据信息或者联系管理员" };

                        //TicketSerialNumber tnumber = BillCommonService.Instance.CreateTicketSerialNumber(propertyMgrUnitOfWork, chargeRecord.ComDeptId.Value);
                        //int i = 0;
                        ////如果重复 就重新生成
                        //while (PaymentService.Instance.CheckReceiptNumRepeat(tnumber.CompleteSerialValue, chargeRecord.ComDeptId.Value, ""))
                        //{
                        //    tnumber = BillCommonService.Instance.CreateTicketSerialNumber(propertyMgrUnitOfWork, chargeRecord.ComDeptId.Value);
                        //    i++;
                        //    if (i == 10)
                        //    {
                        //        break;
                        //    }
                        //}
                        //if (i == 10)
                        //{
                        //    return new ResultModel() { IsSuccess = false, ErrorCode = "820", Msg = "票据号生成失败，请联系管理员" };
                        //}
                        //propertyMgrUnitOfWork.TicketSerialNumberRepository.Update(tnumber);
                        //ReciptNumber = tnumber.CompleteSerialValue;
                    }
                    chargeRecord.Receipt.Number = ReciptNumber;
                    chargeRecord.Receipt.UpdateTime = DateTime.Now;
                    chargeRecord.Receipt.Operator = domainChargeRecord.Operator;
                    chargeRecord.Receipt.OperatorName = domainChargeRecord.OperatorName;
                    if (IsReceiptbook)
                        BillCommonService.Instance.GenerateReceiptBookDetail(propertyMgrUnitOfWork, chargeRecord.ComDeptId.Value, chargeRecord, chargeRecord.Receipt);
                }
                propertyMgrUnitOfWork.ChargeRecordRepository.Update(chargeRecord);
                propertyMgrUnitOfWork.Commit();
                return new ResultModel() { IsSuccess = true, ErrorCode = "0", Msg = "补打成功", Data = chargeRecord.Id }; ;
            }
        }

        public IList<BillChargeRecord> GetBillChargeRecordList(int StartRowIndex, int PageSize, Expression<Func<BillChargeRecord, bool>> predicate, string sortExpressions, out int totalCount)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = (from b in propertyMgrUnitOfWork.ChargBillRepository.GetAll()
                             join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on b.Id equals m.ChargeBillId
                             join c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll() on m.ChargeRecordId equals c.Id
                             where 
                             //b.IsDel == false  &&  //注释 账单作废了 也需要显示
                             c.IsDel == false
                             //where b.ReceivedAmount != 0//已收金额>0
                             select new BillChargeRecord()
                             {
                                 Id = c.Id,
                                 ComDeptId = c.ComDeptId,
                                 HouseDeptId = b.HouseDeptId,
                                 ResourcesId =b.ResourcesId,
                                 RefType =b.RefType,
                                 TransactionDesc = b.Description,
                                 BeginDate = b.BeginDate,
                                 EndDate = b.EndDate,
                                 Amount = m.Amount,//交易金额
                                 OperatorName = c.OperatorName,
                                 PayDate = c.PayDate,
                                 PayType = c.PayMthodId,
                                 ReceiptNum = c.Receipt.Number,
                                 ChargeType = c.ChargeType,
                                 Remark = b.Remark
                             });
                //totalCount = query.Count();
                //return query.Where(predicate).OrderByDescending(b => b.PayDate).Take(PageSize).Skip(StartRowIndex).ToList();
                return query.Where(predicate).SortingAndPaging(sortExpressions, StartRowIndex, PageSize, out totalCount).ToList();
            }
        }


        public IList<BillChargeRecord> GetPaymentTasksBillChargeRecordList(int StartRowIndex, int PageSize, Expression<Func<BillChargeRecord, bool>> predicate, string sortExpressions, out int totalCount)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var pquery = propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll().Where(o => o.IsDel == false);
                var query = (from b in propertyMgrUnitOfWork.ChargBillRepository.GetAll()
                             join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on b.Id equals m.ChargeBillId
                             join c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll() on m.ChargeRecordId equals c.Id

                             where b.IsDel == false && c.IsDel == false&& c.ChargeType!= (int)ChargeTypeEnum.BalanceTransfer
                             //   where b.ReceivedAmount != 0 //已收金额>0
                             where !pquery.Any(p => p.ChargeRecordId == c.Id)
                             select new BillChargeRecord()
                             {
                                 Id = c.Id,
                                 HouseDeptId = b.HouseDeptId,
                                 ComDeptId = b.ComDeptId,
                                 TransactionDesc = b.Description,
                                 Amount = c.Amount,
                                 OperatorName = c.OperatorName,
                                 PayDate = c.PayDate,
                                 PayType = c.PayMthodId,
                                 ReceiptNum = c.Receipt.Number,
                             });

                //totalCount = query.Count();
                //return query.Where(predicate).OrderByDescending(b => b.PayDate).Take(PageSize).Skip(StartRowIndex).ToList();
                return query.Where(predicate).SortingAndPaging(sortExpressions, StartRowIndex, PageSize, out totalCount).ToList();
            }
        }



        public IList<ChargeRecord> GetPaymentTasksDetailList(int StartRowIndex, int PageSize, Expression<Func<ChargeRecord, bool>> predicate, string sortExpressions, out int totalCount, int PaymentTaskId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {

                var query = (
                            from d in propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll()
                            join r in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll() on d.ChargeRecordId equals r.Id
                            where r.IsDel == false && d.IsDel == false
                            where r.ChargeType!=(int)ChargeTypeEnum.BalanceTransfer
                            where d.PaymentTaskID == PaymentTaskId
                            select r);

                //totalCount = query.Count();
                //return query.Where(predicate).OrderByDescending(b => b.PayDate).Take(PageSize).Skip(StartRowIndex).ToList();
                var dataList = query.Where(predicate).SortingAndPaging(sortExpressions, StartRowIndex, PageSize, out totalCount).ToList();

                foreach (var item in dataList)
                {
                    item.ReceiptId = item.Receipt.Id;
                }
                return dataList;
            }
        }

        public IList<ChargeRecord> GetPaymentTasksDetailListById(int PaymentTaskId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {

                var query = (
                            from d in propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll()
                            join r in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll() on d.ChargeRecordId equals r.Id
                            where r.IsDel == false && d.IsDel == false&& r.ChargeType!=(int)ChargeTypeEnum.BalanceTransfer

                            where d.PaymentTaskID == PaymentTaskId
                            select r);


                var dataList = query.ToList();

                foreach (var item in dataList)
                {
                    item.ReceiptId = item.Receipt.Id;
                }
                return dataList;
            }
        }
        public IList<ChargeRecord> GetPaymentTasksDetailListBySubject(int ComDeptId, DateTime PayDateMax)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {

                var pquery = propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll().Where(o => o.IsDel == false);
                var query = (from c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll()
                                 //from d in propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll() 
                             where c.IsDel == false&&c.ChargeType!= (int)ChargeTypeEnum.BalanceTransfer
                             where c.PayDate <= PayDateMax
                             where c.ComDeptId == ComDeptId
                             //   where b.ReceivedAmount != 0 //已收金额>0
                             where !pquery.Any(p => p.ChargeRecordId == c.Id)
                             select c);

                var dataList = query.ToList();


                return dataList;
            }
        }


        public IList<ChargeRecord> GetPaymentTasksDetailList_Add(int StartRowIndex, int PageSize, Expression<Func<ChargeRecord, bool>> predicate, string sortExpressions, out int totalCount, DateTime PaymentDateMax, int? DeptId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {

                var pquery = propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll().Where(o => o.IsDel == false);
                var query = (from c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(predicate)
                                 //from d in propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll() 
                             where c.IsDel == false&& c.ChargeType!=(int)ChargeTypeEnum.BalanceTransfer
                             where c.PayDate <= PaymentDateMax
                             where c.ComDeptId == DeptId
                             //   where b.ReceivedAmount != 0 //已收金额>0
                             where !pquery.Any(p => p.ChargeRecordId == c.Id)
                             select c);

                var dataList = query.SortingAndPaging(sortExpressions, StartRowIndex, PageSize, out totalCount).ToList();

                foreach (var item in dataList)
                {
                    item.ReceiptId = item.Receipt.Id;

                }
                return dataList;
            }
        }



        public IList<BillChargeRecord> GetPaymentTasksDetailList_Save(DateTime PaymentDateMax, int DeptId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {

                var pquery = propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll().Where(o => o.IsDel == false);
                var query = (from c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll()
                                 //from d in propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll() 
                             where c.IsDel == false&& c.ChargeType!= (int)ChargeTypeEnum.BalanceTransfer
                             where c.PayDate <= PaymentDateMax
                             where c.ComDeptId == DeptId
                             //   where b.ReceivedAmount != 0 //已收金额>0
                             where !pquery.Any(p => p.ChargeRecordId == c.Id)
                             select new BillChargeRecord()
                             {
                                 Id = c.Id,
                                 Amount = c.Amount,
                                 OperatorName = c.OperatorName,
                                 PayDate = c.PayDate,
                                 PayType = c.PayMthodId,
                                 ReceiptNum = c.Receipt.Number,
                             });


                return query.ToList();
            }
        }

        public string GetBalanceAmountByHouseDeptId(int HouseDeptId,string Separator)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                string balanceDesc = string.Empty;
                //全部收费项目 余额
                var pEntity = propertyMgrUnitOfWork.PrepayAccountRepository
                    .GetAll()
                    .Where(p => p.HouseDeptId == HouseDeptId && p.ChargeSubjectID == 0 && p.IsDel == false)
                    .FirstOrDefault();

                if (pEntity != null && pEntity.Balance > 0)
                {
                    balanceDesc += ("全部收费项目" + Separator + pEntity.Balance);
                }

                //收费项目 余额
                var balanceList = (from a in propertyMgrUnitOfWork.PrepayAccountRepository.GetAll()
                                   join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll()
                                   on a.ChargeSubjectID equals s.Id
                                   where s.IsDel == false
                                   where a.HouseDeptId == HouseDeptId && a.Balance > 0 && a.IsDel == false
                                   select new
                                   {
                                       s.Name,
                                       a.Balance
                                   }).ToList();
                foreach (var item in balanceList)
                {
                    balanceDesc += (" " + item.Name + Separator + item.Balance);
                }

                if (balanceDesc.Length > 0)
                {
                    return balanceDesc;
                }
                return "无";
            }
        }

        /// <summary>
        /// 票据打印获取信息
        /// </summary>
        public List<ReceiptPrintModel> GetChargBillListByChargeRecordId(string chargeRecordId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                try
                {

                var billList = from b in propertyMgrUnitOfWork.ChargBillRepository.GetAll()
                               join bc in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on b.Id equals bc.ChargeBillId
                               join c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll() on bc.ChargeRecordId equals c.Id
                               join s in propertyMgrUnitOfWork.ChargeSubjectSnaRepository.GetAll() on b.Id equals s.ChargeBillId
                               where b.IsDel == false && c.IsDel == false
                               where c.Id == chargeRecordId
                               select new ReceiptPrintModel
                               {
                                   BillId = bc.ChargeBillId,
                                   ProjectDesc = b.Description,
                                   SubjectId = b.ChargeSubjectId,
                                   BeginDate = b.BeginDate,
                                   EndDate = b.EndDate,
                                   Price = s.Price,
                                   Quantity = b.Quantity,
                                   BillAmount = b.BillAmount,
                                   ReliefAmount = b.ReliefAmount,
                                   Amount = bc.Amount,
                                   RefType = b.RefType,
                                   ResId = b.ResourcesId,
                                   BillRemark = b.Remark
                               };
                DataTable dt = new DataTable();
                var list = billList.ToList();

                    foreach (var o in list)
                    {
                        if (o.RefType == (int)SubjectTypeEnum.Meter)
                        {
                            if (o.Amount > 0 && !string.IsNullOrEmpty(o.Quantity))
                            {
                                //double quantity = Convert.ToDouble(dt.Compute(o.Quantity, "false").ToString());
                                //double amount = Convert.ToDouble(o.Amount);
                                if (o.Quantity.IndexOf("*") > 0 || o.Quantity.IndexOf("-") > 0 || o.Quantity.IndexOf("+") > 0 || o.Quantity.IndexOf("/") > 0)
                                {
                                    o.Quantity = Convert.ToDouble(o.Quantity.Split('*', '-', '+', '/')[1]).ToString();
                                    //double useQuantity = Convert.ToDouble(o.Quantity.Split('*', '-', '+', '/')[1]);
                                    //if (quantity > 0)
                                    //{
                                    //    o.Quantity = (Math.Round((amount / quantity) * useQuantity, 2)).ToString();
                                    //}
                                }
                            }
                            else
                            {
                                o.Quantity = "无";
                            }
                            
                        }

                        if (o.RefType == (int)SubjectTypeEnum.House || o.RefType == (int)SubjectTypeEnum.ParkingSpace)
                        {
                            if (o.Amount > 0 && !string.IsNullOrEmpty(o.Quantity))
                            {
                                if (o.Quantity.IndexOf("*") > 0)
                                {
                                    o.Quantity = Convert.ToDouble(o.Quantity.Split('*')[1]).ToString();
                                }
                                else
                                {
                                    o.Quantity = null;
                                }
                            }
                            else
                            {
                                o.Quantity = null;
                            }
                        }
                    }
                //list.ForEach(o =>
                //{
                   
                //});
                //优惠信息部分 2017-03-31
                var discountList = propertyMgrUnitOfWork.PaymentDiscountInfoRepository.GetAll().Where(p => p.ChargeRecordId == chargeRecordId).ToList();
                foreach (var item in discountList)
                {
                    switch (item.DiscountType)
                    {
                        case (int)DiscountTypeEnum.Coupon:
                        {
                                list.Add(new ReceiptPrintModel() { DiscountDesc = "优惠券："+item.DiscountDesc
                                    , ReliefAmount = item.DiscountAmount, Amount = item.DiscountAmount*-1
                                });
                            };break;
                        default:
                            break;
                    }
                    
                }
                return list;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public ChargeRecord GetChargeRecordAndReceiptByKey(object id)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var entity = propertyMgrUnitOfWork.ChargeRecordRepository.GetByKey(id);
                entity.ReceiptId = entity.Receipt.Id;
                return entity;
            }
        }


        /// <summary>
        /// 收费记录 对外收费
        /// </summary>
        /// <param name="startRowIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="predicate"></param>
        /// <param name="sortExpressions"></param>
        /// <param name="ComDeptId"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public IList<ChargeRecord> GetForeigChargeRecordList(int startRowIndex, int PageSize, Expression<Func<ChargeRecord, bool>> predicate, string sortExpressions, int ComDeptId, out int totalCount)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //var dataList = propertyMgrUnitOfWork.ChargeRecordRepository.Paging(PageIndex, PageSize, predicate, sortExpressions, out totalCount).ToList();
                var query = from c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(predicate)
                            join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on c.Id equals m.ChargeRecordId
                            join b in propertyMgrUnitOfWork.ChargBillRepository.GetAll() on m.ChargeBillId equals b.Id
                            join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on b.ChargeSubjectId equals s.Id
                            where b.ComDeptId == ComDeptId && s.SubjectType == (int)SubjectTypeEnum.Foreig
                            select c;
                var dataList = query.Distinct().SortingAndPaging(sortExpressions, startRowIndex, PageSize, out totalCount).ToList();
                foreach (var item in dataList)
                {
                    item.ReceiptId = item.Receipt.Id;
                }
                return dataList;
            }
        }


        

        /// <summary>
        /// 判断是否是
        /// </summary>
        /// <param name="RecrordId"></param>
        /// <returns></returns>
        public bool CheckChargeReocordIsForegi(string RecrordId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = from c in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll()
                            join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on c.Id equals m.ChargeRecordId
                            join b in propertyMgrUnitOfWork.ChargBillRepository.GetAll() on m.ChargeBillId equals b.Id
                            join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on b.ChargeSubjectId equals s.Id
                            where c.Id == RecrordId && s.SubjectType == (int)SubjectTypeEnum.Foreig
                            select c;
                if (query.Count() > 0)
                {
                    return true;
                }
            }


            return false;
        }


        public IList<ChargeRecordDTO> GetFullChargeRecordList(Expression<Func<ChargeRecordDTO, bool>> predicate, string expressions, out int totalCount, int PageStart, int PageSize)
        {
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //费用记录信息
                var query = from c in pmUnitWork.ChargeRecordRepository.GetAll()
                            join r in pmUnitWork.RefundRecordRepository.GetAll()
                            on c.Id equals r.ChargeRecordId into temp from lr in temp.DefaultIfEmpty()
                             select new ChargeRecordDTO
                             {
                                 ResourcesNames = c.ResourcesNames,
                                 ChargeType = c.ChargeType,
                                 Amount = c.Amount,
                                 DiscountAmount = c.DiscountAmount,
                                 PayDate = c.PayDate,
                                 ReceiptNum = c.Receipt.Number,
                                 CustomerName = c.CustomerName,
                                 OperatorName = c.OperatorName,
                                 ReceiptStatus = (int)c.Receipt.Status,
                                 PayMthodId = c.PayMthodId,
                                 RefundReason = lr == null? "" : lr.Reason,
                                 Remark = c.Remark,
                                 ComDeptId = c.ComDeptId
                             };
             
                var dataList = query.Where(predicate).Sorting(expressions);
                totalCount = query.Where(predicate).Count();
                var list = dataList.Skip(PageStart).Take(PageSize);
                return list.ToList();
            }
        }

        #region 获取最后一次缴费记录

        public ChargeRecord GetLastChargeRecord(int? houseDeptId)
        {
            using (var pmUnitWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = pmUnitWork.ChargeRecordRepository.GetAll()
                                        .Where(c => c.HouseDeptId == houseDeptId
                                        && c.IsDel == false
                                        && c.IsOnline == true)
                                        .OrderByDescending(c => c.PayDate);
                return query.FirstOrDefault();
            }
        }

        #endregion

    }
}
