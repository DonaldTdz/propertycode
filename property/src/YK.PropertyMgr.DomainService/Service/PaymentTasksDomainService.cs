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
using System.Data.Entity.Infrastructure;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.PresentationService;

namespace YK.PropertyMgr.DomainService
{
    public partial class PaymentTasksDomainService
    {


        #region 查询交款任务列表

        public IList<PaymentTasks> GetPaymentTasksList(int PageIndex, int PageSize, Expression<Func<PaymentTasks, bool>> predicate, string sortExpressions, out int totalCount)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var dataList = propertyMgrUnitOfWork.PaymentTasksRepository.Paging(PageIndex, PageSize, predicate, sortExpressions, out totalCount).ToList();

                return dataList;
            }
        }

        #endregion

        #region 过时的存储过程调用

        ///
        public List<PaymentTaskBySubjetDTO> GetPaymentTasksBySubjectList(string PaymentTaskId, out int totalCount)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                string sql = "[dbo].[p_PaymentTaksDetailBySubject] " + PaymentTaskId;
                DbRawSqlQuery<PaymentTaskBySubjetDTO> query = propertyMgrUnitOfWork.PaymentTasksRepository.DatabaseContext.Database.SqlQuery<PaymentTaskBySubjetDTO>(sql);

                var datalist = query.ToList();
                totalCount = datalist.Count();

                return datalist;
            }
        }


        public List<PaymentTaskBySubjetDTO> GetPaymentTasksBySubjectList_Add(DateTime PaymentDateMax, int ComDeptId, out int totalCount)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                string sql = "[dbo].[p_PaymentTaksDetailBySubject_Add] '" + PaymentDateMax.ToString("yyyy/MM/dd HH:mm") + "'," + ComDeptId.ToString();
                DbRawSqlQuery<PaymentTaskBySubjetDTO> query = propertyMgrUnitOfWork.PaymentTasksRepository.DatabaseContext.Database.SqlQuery<PaymentTaskBySubjetDTO>(sql);

                var datalist = query.ToList();
                totalCount = datalist.Count();

                return datalist;
            }
        }

        #endregion

        #region 收费项目汇总（支付方式行转列）
        public ReportArrearsModels GetPaymentTasksBySubjectList_All(DateTime PaymentDateMax, int ComDeptId, out int totalCount, int PaymentTaskId = 0)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                Condition<ChargeRecord> condition = new Condition<ChargeRecord>(c => true);
                ReportArrearsModels ReportModel = new ReportArrearsModels();
                var PaymentTaskChargeRecordList = GetPaymentTaskChargBillList(propertyMgrUnitOfWork, PaymentDateMax, ComDeptId, condition.ExpressionBody,PaymentTaskId).ToList();
                //获取收费项目的集合
                var ChargeSubjectIdNameList = PaymentTaskChargeRecordList.GroupBy(o => new { o.ChargeSubjectId, o.Name }).ToList();
                //表头
                List<ReportHead> ReportHeadList = new List<ReportHead>();

                //固定列--项目名称
                ReportHeadList.Add(new ReportHead() { Id = "ChargeSubjectName", Name = "项目名称" });

                //构建表头-付款方式

                var PayMthodIdArry = PaymentTaskChargeRecordList.GroupBy(o => o.PayMthodId).Select(o => o.Key.ToString()).ToList();
                var PayMthodList = GetPayTypeModelList().Where(o => PayMthodIdArry.Contains(o.Code)).ToList();
                foreach (var PayMthodObj in PayMthodList)
                {
                    // 构建表头
                    ReportHead reportHead = new ReportHead()
                    {
                        Id = PayMthodObj.EnName,
                        Name = PayMthodObj.CnName
                    };
                    ReportHeadList.Add(reportHead);
                }
                List<ReportArrearsDTO> ReportArrearsDTOList = new List<ReportArrearsDTO>();

                //构建数据
                foreach (var reportDataResource in ChargeSubjectIdNameList)
                {
                    ReportArrearsDTO reportArrearsDTO = new ReportArrearsDTO();
                    //构建数据
                    var PaymentTaskChargeSubjectList = PaymentTaskChargeRecordList.Where(o => o.ChargeSubjectId == reportDataResource.Key.ChargeSubjectId);
                    List<ReportRowData> ReportRowDataList = new List<ReportRowData>();
                    ReportRowDataList.Add(new ReportRowData() { Id = reportDataResource.Key.ChargeSubjectId.ToString(), Text = reportDataResource.Key.Name });
                    foreach (var rowHead in PayMthodList)
                    {
                        ReportRowData rowdata = new ReportRowData();
                        rowdata.Id = rowHead.EnName;
                        rowdata.Text = string.Empty;
                        var PayMthodData = PaymentTaskChargeSubjectList.Where(o => o.PayMthodId == Convert.ToInt32(rowHead.Code)).FirstOrDefault();
                        if (PayMthodData != null)
                        {
                            rowdata.Text = PayMthodData.Amount.ToString();
                        }
                        ReportRowDataList.Add(rowdata);
                    }

                    reportArrearsDTO.RowDataList = ReportRowDataList;

                    ReportArrearsDTOList.Add(reportArrearsDTO);
                }

                ReportModel.ReportHeadList = ReportHeadList;
                ReportModel.ReportArrearsDTOList = ReportArrearsDTOList;
                ReportModel.ReportArrearsSum = GetArrearsReportDataSumList(PaymentTaskChargeRecordList, PayMthodList);
                totalCount = ReportModel.ReportArrearsDTOList.Count();
                return ReportModel;

            }


        }

        private ReportArrearsDTO GetArrearsReportDataSumList(List<PaymentTaskChargeRecord> UnionList, List<DictionaryModel> PayMethodList)
        {
            ReportArrearsDTO obj = new ReportArrearsDTO();
            List<ReportRowData> RowList = new List<ReportRowData>();
            //合计
            ReportRowData TotalName = new ReportRowData()
            {
                Id = "TotalAmount",
                Text = "合计"
            };
            RowList.Add(TotalName);

            //开始动态项目加载
            foreach (var paymethod in PayMethodList)
            {
                ReportRowData TotalRow = new ReportRowData();
                TotalRow.Id = paymethod.EnName;
                TotalRow.Text = UnionList.Where(o => o.PayMthodId == Convert.ToInt32(paymethod.Code)).Sum(o => o.Amount).ToString();
                RowList.Add(TotalRow);
            }
            obj.RowDataList = RowList;
            return obj;
        }



        /// <summary>
        /// 获取支付方式列表
        /// </summary>
        /// <returns></returns>
        private IList<DictionaryModel> GetPayTypeModelList()
        {

            var propertyService = PresentationServiceHelper.LookUp<IPropertyService>();
            return propertyService.GetDictionaryModels(PropertyEnumType.PayType.ToString());

        }


        private IQueryable<PaymentTaskChargeRecord> GetPaymentTaskChargBillList(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, DateTime PaymentDateMax, int ComDeptId, Expression<Func<ChargeRecord, bool>> predicate,int PaymentTaskId = 0)
        {

            if (PaymentTaskId > 0)
            {//有交款记录的
                var AddPaymenTaskList = (from r in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(o => o.ChargeType != (int)ChargeTypeEnum.Refund&&o.ChargeType!= (int)ChargeTypeEnum.BalanceTransfer)
                                         join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on r.Id equals m.ChargeRecordId
                                         join c in propertyMgrUnitOfWork.ChargBillRepository.GetAll() on m.ChargeBillId equals c.Id
                                         join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on c.ChargeSubjectId equals s.Id
                                         join p in propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll().Where(o => o.IsDel == false) on r.Id equals p.ChargeRecordId
                                         where p.PaymentTaskID == PaymentTaskId
                                         group new { id = s.Id, Name = s.Name, r.PayMthodId, m.Amount }
                                         by new { id = s.Id, Name = s.Name, r.PayMthodId } into r
                                         select new PaymentTaskChargeRecord
                                         {
                                             ChargeSubjectId = r.Key.id.Value,
                                             Name = r.Key.Name,
                                             PayMthodId = r.Key.PayMthodId.Value,
                                             Amount = r.Sum(o => o.Amount).Value
                                         });

                var AddPaymenTaskListRefurd = (from r in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(o => o.ChargeType == (int)ChargeTypeEnum.Refund&& o.ChargeType != (int)ChargeTypeEnum.BalanceTransfer)
                                               join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on r.Id equals m.ChargeRecordId
                                               join c in propertyMgrUnitOfWork.ChargBillRepository.GetAll() on m.ChargeBillId equals c.Id
                                               join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on c.ChargeSubjectId equals s.Id
                                               join p in propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll().Where(o => o.IsDel == false) on r.Id equals p.ChargeRecordId
                                               where p.PaymentTaskID == PaymentTaskId
                                               group new { id = s.Id, Name = s.Name, r.PayMthodId, m.Amount }
                                               by new { id = s.Id, Name = s.Name, r.PayMthodId } into r
                                               select new PaymentTaskChargeRecord
                                               {
                                                   ChargeSubjectId = r.Key.id.Value,
                                                   Name = r.Key.Name,
                                                   PayMthodId = r.Key.PayMthodId.Value,
                                                   Amount = -r.Sum(o => o.Amount).Value
                                               });
                return AddPaymenTaskList.Union(AddPaymenTaskListRefurd);
            }

            else
            {//没有交款记录的


                var PaymentTaskDetails = propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll().Where(o => o.IsDel == false);
                var AddPaymenTaskList = (from r in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(o => o.PayDate <= PaymentDateMax && o.ComDeptId == ComDeptId && o.ChargeType != (int)ChargeTypeEnum.Refund && o.ChargeType != (int)ChargeTypeEnum.BalanceTransfer).Where(predicate)
                                         join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on r.Id equals m.ChargeRecordId
                                         join c in propertyMgrUnitOfWork.ChargBillRepository.GetAll() on m.ChargeBillId equals c.Id
                                         join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on c.ChargeSubjectId equals s.Id
                                         where !PaymentTaskDetails.Any(o => o.ChargeRecordId == r.Id)
                                         group new { id = s.Id, Name = s.Name, r.PayMthodId, m.Amount }
                                         by new { id = s.Id, Name = s.Name, r.PayMthodId } into r
                                         select new PaymentTaskChargeRecord
                                         {
                                             ChargeSubjectId = r.Key.id.Value,
                                             Name = r.Key.Name,
                                             PayMthodId = r.Key.PayMthodId.Value,
                                             Amount = r.Sum(o => o.Amount).Value
                                         });
                var AddPaymenTaskListRefurd = (from r in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(o => o.PayDate <= PaymentDateMax && o.ComDeptId == ComDeptId && o.ChargeType == (int)ChargeTypeEnum.Refund && o.ChargeType != (int)ChargeTypeEnum.BalanceTransfer).Where(predicate)
                                               join m in propertyMgrUnitOfWork.ChargeBillRecordMatchingRepository.GetAll() on r.Id equals m.ChargeRecordId
                                               join c in propertyMgrUnitOfWork.ChargBillRepository.GetAll() on m.ChargeBillId equals c.Id
                                               join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll() on c.ChargeSubjectId equals s.Id
                                               where !PaymentTaskDetails.Any(o => o.ChargeRecordId == r.Id)
                                               group new { id = s.Id, Name = s.Name, r.PayMthodId, m.Amount }
                                               by new { id = s.Id, Name = s.Name, r.PayMthodId } into r
                                               select new PaymentTaskChargeRecord
                                               {
                                                   ChargeSubjectId = r.Key.id.Value,
                                                   Name = r.Key.Name,
                                                   PayMthodId = r.Key.PayMthodId.Value,
                                                   Amount = -r.Sum(o => o.Amount).Value
                                               });
                return AddPaymenTaskList.Union(AddPaymenTaskListRefurd);


            }


        }

        #endregion

        #region 支付方式汇总
        public PaymentTaskBySubjetDTO GetPaymentTaskPayMthodIdList(DateTime PaymentDateMax, int ComDeptId, int? CheckAdminId, int PaymentTaskId = 0)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                ReportArrearsModels ReportModel = new ReportArrearsModels();
                var PaymentTaskChargeRecordList = GetPaymentTaskPayMthodChargBillList(propertyMgrUnitOfWork, PaymentDateMax, ComDeptId, CheckAdminId, PaymentTaskId).ToList();
                //区分线上和线下
                var OnlineList = PaymentTaskChargeRecordList.Where(o => o.IsOnline == true).ToList();
                var OfflineList = PaymentTaskChargeRecordList.Where(o => o.IsOnline == false).ToList();
                PaymentTaskBySubjetDTO paymentTaskBySubjetDTO = new PaymentTaskBySubjetDTO();
                var PayTypeList = GetPayTypeModelList().ToList();
                var CashPayType = PayTypeList.Where(o => o.EnName == "Cash").FirstOrDefault();//现金
                var InternalTransferPayType = PayTypeList.Where(o => o.EnName == "InternalTransfer").FirstOrDefault();//预存抵扣
                var BankCardPayType = PayTypeList.Where(o => o.EnName == "BankCard").FirstOrDefault();//银行卡
                var InternalDebitPayType = PayTypeList.Where(o => o.EnName == "InternalDebit").FirstOrDefault();//内部划账
                var OneNetcomPayType = PayTypeList.Where(o => o.EnName == "OneNetcom").FirstOrDefault();//一网通
                var WalletPayType = PayTypeList.Where(o => o.EnName == "Wallet").FirstOrDefault();//钱包抵扣
                var AlipayPayType = PayTypeList.Where(o => o.EnName == "Alipay").FirstOrDefault();//支付宝
                var WeChatPayType = PayTypeList.Where(o => o.EnName == "WeChat").FirstOrDefault();//微信

                paymentTaskBySubjetDTO.Name = "合计";
                //线下
                paymentTaskBySubjetDTO.Cash = GetAmountByPayType(CashPayType, OfflineList);//现金
                paymentTaskBySubjetDTO.BankCard = GetAmountByPayType(BankCardPayType, OfflineList);//银行卡
                paymentTaskBySubjetDTO.OffAlipay = GetAmountByPayType(AlipayPayType, OfflineList);//支付宝
                paymentTaskBySubjetDTO.OffWeChat = GetAmountByPayType(WeChatPayType, OfflineList);//微信
                paymentTaskBySubjetDTO.InternalDebit = GetAmountByPayType(InternalDebitPayType, OfflineList);//内部划账

                //线上
                paymentTaskBySubjetDTO.OnAlipay = GetAmountByPayType(AlipayPayType, OnlineList);//支付宝
                paymentTaskBySubjetDTO.OnWeChat = GetAmountByPayType(WeChatPayType, OnlineList);//微信
                paymentTaskBySubjetDTO.OneNetcom = GetAmountByPayType(OneNetcomPayType, OnlineList);//一网通
                paymentTaskBySubjetDTO.Wallet = GetAmountByPayType(WalletPayType, OnlineList);//一网通
                                                                                              //预存抵扣
                paymentTaskBySubjetDTO.InternalTransfer = GetAmountByPayType(InternalTransferPayType, PaymentTaskChargeRecordList);//预存抵扣
                paymentTaskBySubjetDTO.Coupon = GetChargeRecordCouponAmount(propertyMgrUnitOfWork, PaymentDateMax, ComDeptId, PaymentTaskId)==null?0: GetChargeRecordCouponAmount(propertyMgrUnitOfWork, PaymentDateMax, ComDeptId, PaymentTaskId).Value;//优惠券
                paymentTaskBySubjetDTO.Total = paymentTaskBySubjetDTO.Cash + paymentTaskBySubjetDTO.BankCard + paymentTaskBySubjetDTO.OffAlipay + paymentTaskBySubjetDTO.OffWeChat
                                               + paymentTaskBySubjetDTO.InternalDebit + paymentTaskBySubjetDTO.OnAlipay + paymentTaskBySubjetDTO.OnWeChat + paymentTaskBySubjetDTO.OneNetcom
                                               + paymentTaskBySubjetDTO.Wallet + paymentTaskBySubjetDTO.InternalTransfer + paymentTaskBySubjetDTO.Coupon;
                return paymentTaskBySubjetDTO;
            }
        }


        private decimal GetAmountByPayType(DictionaryModel DicModel,List<PaymentTaskChargeRecord> List)
        {
            var money = 0m;
            if (DicModel != null)
            {
                money = List.Where(o => o.PayMthodId == Convert.ToInt32(DicModel.Code)).Sum(o => o.Amount);
            }
            
            return money;
        }


        private IQueryable<PaymentTaskChargeRecord> GetPaymentTaskPayMthodChargBillList(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, DateTime PaymentDateMax, int ComDeptId, int? CheckAdminId,int PaymentTaskId = 0)
        {

            if (PaymentTaskId > 0)
            {//有交款记录的
                var AddPaymenTaskList = (from r in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(o =>o.IsDel==false && o.ChargeType != (int)ChargeTypeEnum.BalanceTransfer)
                                         join p in propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll().Where(o => o.IsDel == false) on r.Id equals p.ChargeRecordId
                                         where p.PaymentTaskID == PaymentTaskId
                                         group new { r.PayMthodId,Amount=(r.Amount-r.DiscountAmount),r.IsOnline }
                                         by new {   r.PayMthodId,r.IsOnline } into r
                                         select new PaymentTaskChargeRecord
                                         {
                                             PayMthodId = r.Key.PayMthodId.Value,
                                             Amount = r.Sum(o => o.Amount).Value,
                                             IsOnline =r.Key.IsOnline==null? false : r.Key.IsOnline.Value
                                         });
                return AddPaymenTaskList;
            }

            else
            {//没有交款记录的
                Condition<ChargeRecord> condition = new Condition<ChargeRecord>(c => true);
                if (CheckAdminId > 0)
                    condition = condition & new Condition<ChargeRecord>(c => c.Operator == CheckAdminId);

                      var PaymentTaskDetails = propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll().Where(o => o.IsDel == false  );
                var AddPaymenTaskList = (from r in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(condition.ExpressionBody).Where(o => o.PayDate <= PaymentDateMax && o.ComDeptId == ComDeptId && o.IsDel == false && o.ChargeType != (int)ChargeTypeEnum.BalanceTransfer)
                                         where !PaymentTaskDetails.Any(o => o.ChargeRecordId == r.Id)
                                         group new { r.PayMthodId, Amount = (r.Amount - r.DiscountAmount), r.IsOnline }
                                         by new { r.PayMthodId, r.IsOnline } into r
                                         select new PaymentTaskChargeRecord
                                         {
                                             PayMthodId = r.Key.PayMthodId.Value,
                                             Amount = r.Sum(o => o.Amount).Value,
                                             IsOnline = r.Key.IsOnline == null ? false : r.Key.IsOnline.Value
                                         });
                return AddPaymenTaskList;


            }


        }


        private decimal? GetChargeRecordCouponAmount(IPropertyMgrUnitOfWork propertyMgrUnitOfWork, DateTime PaymentDateMax, int ComDeptId, int PaymentTaskId = 0)
        {
            if (PaymentTaskId > 0)
            {//有交款记录的
                var AddPaymenTaskList = (from r in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(o => o.PayDate <= PaymentDateMax && o.ComDeptId == ComDeptId && o.IsDel == false && o.ChargeType != (int)ChargeTypeEnum.BalanceTransfer)
                                         join p in propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll().Where(o => o.IsDel == false) on r.Id equals p.ChargeRecordId
                                         where p.PaymentTaskID == PaymentTaskId
                                         select (r.DiscountAmount == null ? 0 : r.DiscountAmount)
                                       ).Sum(o => o);


                return AddPaymenTaskList;
            }
            else
            {//没有交款记录的


                var PaymentTaskDetails = propertyMgrUnitOfWork.PaymentTaskDetailRepository.GetAll().Where(o => o.IsDel == false);
                var AddPaymenTaskList = (from r in propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(o => o.PayDate <= PaymentDateMax && o.ComDeptId == ComDeptId && o.IsDel == false && o.ChargeType != (int)ChargeTypeEnum.BalanceTransfer)
                                         where !PaymentTaskDetails.Any(o => o.ChargeRecordId == r.Id)
                                         select (r.DiscountAmount == null ? 0 : r.DiscountAmount)
                                       ).Sum(o => o);

                return AddPaymenTaskList;


            }

        }
        #endregion

        #region 获取上一次交款日期
        /// <summary>
        /// 获取上一次交款日期
        /// </summary>
        /// <param name="ComDeptId"></param>
        /// <returns></returns>
        public string GetLastPaymentTaskDate(int ComDeptId)
        {
            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var paymentTaskObj = propertyMgrUnitOfWork.PaymentTasksRepository.GetAll().Where(o => o.ComDeptId == ComDeptId).OrderByDescending(o => o.PaymentDate).FirstOrDefault();
                if (paymentTaskObj != null && paymentTaskObj.Id > 0)
                {
                    return paymentTaskObj.PaymentDate.Value.ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        #region  收费项目汇总V2.5版本
        public List<PaymentTaskBySubjetDTO> GetPaymentTaskSubjectList(DateTime PaymentDateMax, int ComDeptId, Expression<Func<ChargeRecord, bool>> predicate, int PaymentTaskId = 0)
        {


            using (var propertyMgrUnitOfWork = UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {

                var PaymentTaskChargeRecordList = GetPaymentTaskChargBillList(propertyMgrUnitOfWork, PaymentDateMax, ComDeptId, predicate,PaymentTaskId).ToList();

                var list = (from p in PaymentTaskChargeRecordList
                            group new { p.ChargeSubjectId, p.Name, p.Amount }
                              by new { p.ChargeSubjectId, p.Name } into r
                            select new PaymentTaskBySubjetDTO
                            {
                                Name = r.Key.Name,
                                Amount = r.Sum(o => o.Amount),
                                GroupId = 1

                         }).ToList();

                PaymentTaskBySubjetDTO totalModel = new PaymentTaskBySubjetDTO();
                totalModel.Name = "合计";
                totalModel.Amount = list.Sum(o => o.Amount);
                totalModel.GroupId = 3;
                list.Add(totalModel);


                 return list;
            }
        }
       

        #endregion

        private class PaymentTaskChargeRecord
        {
            public int ChargeSubjectId { get; set; }
            public string Name { get; set; }
            public int PayMthodId { get; set; }
            public decimal Amount { get; set; }
            public bool IsOnline { get; set; }

        }




    }
}
