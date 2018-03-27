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
using YK.PropertyMgr.ApplicationDTO;
using System.Data.Entity.Infrastructure;
using YK.BackgroundMgr.PresentationService;
using YK.BackgroundMgr.Crosscuting;
using System.Data;
using System.Text.RegularExpressions;
using YK.BackgroundMgr.ApplicationDTO.ApplicationDTO;
using YK.BackgroundMgr.DomainInterface;
using YK.PropertyMgr.CompositeDomainService;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.FrameworkTools.ExcelService;

namespace YK.PropertyMgr.DomainService
{
    public partial class PrepayAccountDomainService
    {

        private IEnumerable<DeptInfo> limitHouseDeptInfos;
        private IEnumerable<PrepayAccount> limitPrepayAccountByComDept;
        private int CommDeptId = 0;

        private IEnumerable<PrepayRefDeptinfo> ValidatePrepayAccountList;
        #region 获取余额管理列表

        /// <summary>
        ///  获取余额管理列表
        /// </summary>
        /// <param name="HouseNumber"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="expressions"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<PrepayAccountShowDTO> GetPrepayAccountList(PrepayAccountShowDTO model, int PageStart, int PageSize, string expressions, out int totalCount)
        {
            Condition<PrepayAccountShowDTO> condition = new Condition<PrepayAccountShowDTO>(f => f.ComDeptId == model.DeptId);
            if (!string.IsNullOrEmpty(model.HouseNumber))
            {
                int?[] houseIds = DomainInterfaceHelper.LookUp<IPropertyDomainService>().GetHouseDeptIdsByHouseNum(model.DeptId.Value, model.HouseNumber);
                if (houseIds.Count() > 0)
                {
                    condition = condition & new Condition<PrepayAccountShowDTO>(f => houseIds.Contains(f.HouseDeptId));
                }
                else
                {
                    condition = condition & new Condition<PrepayAccountShowDTO>(f => false);
                }
            }
            using (var propertyMgrUnitOfWork = Crosscuting.UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //int PageIndex, int PageSize, string expressions, out int totalCount
                //totalCount = GetAll().Where(predicate).Count();
                //var dataList = GetAll().Where(predicate).Sorting(expressions).Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                //string Pstr = string.Empty;

                //Pstr = model.ComDeptId.ToString();

                //if (string.IsNullOrEmpty(model.HouseNumber))
                //{
                //    Pstr += ", null";

                //}
                //else
                //{
                //    Pstr += ", '" + model.HouseNumber + "'";

                //}


                //string sql = "[dbo].[p_PrepayAccountShow] "+ Pstr;
                //DbRawSqlQuery<PrepayAccountShowDTO> query =    propertyMgrUnitOfWork.PrepayAccountRepository.DatabaseContext.Database.SqlQuery<PrepayAccountShowDTO>(sql);
                var query = from p in propertyMgrUnitOfWork.PrepayAccountRepository.GetAll()
                            join s in propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll()
                            on p.ChargeSubjectID equals s.Id into temp
                            from ls in temp.DefaultIfEmpty()
                            where p.IsDel == false
                            select new PrepayAccountShowDTO()
                            {
                                HouseDeptId = p.HouseDeptId,
                                Balance = p.Balance,
                                ChargeSubjectId = p.ChargeSubjectID,
                                ComDeptId = p.CommDeptID,
                                ChargeSubjectName = (ls == null ? "全部收费项目" : ls.Name),
                                CreateTime = p.CreateTime,
                                Remark = p.Remark
                            };
                totalCount = query.Where(condition.ExpressionBody).Count();
                var datalist = query.Where(condition.ExpressionBody).Sorting(expressions).Skip(PageStart).Take(PageSize).ToList();

                foreach (var item in datalist)
                {
                    var record = propertyMgrUnitOfWork.ChargeRecordRepository.GetAll().Where(r => r.ComDeptId == model.DeptId && r.HouseDeptId == item.HouseDeptId).FirstOrDefault();
                    if (record != null)
                    {
                        item.HouseNumber = record.HouseDeptNos;
                    }
                }

                return datalist;
            }
        }

        #endregion

        private void GetListByComDeptId(int CommunityID)
        {
            using (var propertyMgrUnitOfWork = Crosscuting.UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                limitPrepayAccountByComDept = propertyMgrUnitOfWork.PrepayAccountRepository.GetAll()
                    .Where(o => o.CommDeptID == CommunityID).ToList();

            }

        }

        private void CreateValidatePrepayAccountContain(int CommunityID)
        {
            var query = from prepayaccount in limitPrepayAccountByComDept
                        join CommDeptinfo in limitHouseDeptInfos on prepayaccount.HouseDeptId equals CommDeptinfo.Id
                        where prepayaccount.CommDeptID == CommunityID
                        select new PrepayRefDeptinfo()
                        {

                            PrepayAccountId = prepayaccount.Id,
                            ChargeSubjectId = prepayaccount.ChargeSubjectID,
                            DoorNum = CommDeptinfo.Name
                        };



            ValidatePrepayAccountList = query.ToList();
        }

        #region  导入余额
        public ImportResult ImportPrepayAccounts(string filePath, IEnumerable<TemplateModel> templateModels, int CommDeptId, int Operator, string OperatorName)
        {

            limitHouseDeptInfos = DomainInterfaceHelper
            .LookUp<IPropertyDomainService>().GetHouDeptListByCommunityDeptId
            (CommDeptId.ToString());//获取该小区房屋

            GetListByComDeptId(CommDeptId);
            CreateValidatePrepayAccountContain(CommDeptId);
            this.CommDeptId = CommDeptId;
            var PrepayAccountImportResult = ExcelHelper.ImportFromExcels(filePath, templateModels, this.ValidateHouseColumn, null);


            IList<BalanceInfo> list = new List<BalanceInfo>();

            foreach (DataRow successRow in PrepayAccountImportResult.SuccessTable.Rows)
            {
                BalanceInfo bi = new BalanceInfo()
                {
                    ComDeptId = CommDeptId,
                    HouseDeptId = (int)limitHouseDeptInfos.Where(o => o.Name == successRow["DoorNumber"].ToString()).FirstOrDefault().Id,
                    ResourcesId = (int)limitHouseDeptInfos.Where(o => o.Name == successRow["DoorNumber"].ToString()).FirstOrDefault().Id,
                    ChargeSubjectName = successRow["ChargeSubjectName"].ToString(),
                    Amount = decimal.Parse(successRow["Balance"].ToString()),
                    Remark = successRow["Remark"].ToString()

                };
                list.Add(bi);


            }
            ResultModel result = new ResultModel();
            if (list.Count > 0)
            {
                result = BalanceService.Instance.BalanceInitialization(list, Operator, OperatorName);
                if (result.IsSuccess == false)
                {
                    PrepayAccountImportResult.ErrorMsg = "导入房屋数据出错，请与系统管理员联系。" + result.Msg;
                    PrepayAccountImportResult.ErrorTable = null;
                    PrepayAccountImportResult.IsSuccess = false;
                }

            }
            return PrepayAccountImportResult;
        }





        private CustomerValidateResult ValidateHouseColumn(DataRow validateRow, DataTable importTable, DataTable succesTable)
        {
            CustomerValidateResult customerValidateResult = new CustomerValidateResult();


            if (false == ValidateFangwuFormat(validateRow["房屋编号"].ToString()))
            {
                customerValidateResult.IsSuccess = false;
                customerValidateResult.ErrorMsg = "房屋编号格式错误“楼宇-单元-楼层-房号”，如“1-2-12-1203”;";

                return customerValidateResult;
            }

            if (false == ValidateFangwuContain(validateRow["房屋编号"].ToString()))
            {
                customerValidateResult.IsSuccess = false;
                customerValidateResult.ErrorMsg = "在该小区下未找到房间号：" + validateRow["房屋编号"].ToString();

                return customerValidateResult;
            }
            int sujId = 0;
            if (!ValidateChargeSubjectContain(validateRow["收费项目"].ToString(), out sujId))
            {
                customerValidateResult.IsSuccess = false;
                customerValidateResult.ErrorMsg = "该小区收费项目不存在";

                return customerValidateResult;
            }

            if (ValidatePrepayAccountContain(validateRow["房屋编号"].ToString(), sujId))
            {
                customerValidateResult.IsSuccess = false;
                customerValidateResult.ErrorMsg = "在该小区下房间号：" + validateRow["房屋编号"].ToString() + "已有收费数据，不能导入。";

                return customerValidateResult;
            }

            if (false == ValidateBlanceFormat(validateRow["余额"].ToString()))
            {
                customerValidateResult.IsSuccess = false;
                customerValidateResult.ErrorMsg = "余额格式导入错误！";

                return customerValidateResult;
            }



            customerValidateResult.IsSuccess = true;
            return customerValidateResult;
        }


        private bool ValidateFangwuFormat(string strDoorNo)
        {
            return Regex.IsMatch(strDoorNo, @"^([\u4e00-\u9fa5a-zA-Z0-9]+\-){3}([\u4e00-\u9fa5a-zA-Z0-9]+)$");
        }


        private bool ValidateFangwuContain(string strDoorNo)
        {
            return limitHouseDeptInfos.Any(r => r.Name == strDoorNo);
        }

        private bool ValidatePrepayAccountContain(string strDoorNo, int sujId)
        {
            return ValidatePrepayAccountList.Any(r => r.DoorNum == strDoorNo && r.ChargeSubjectId == sujId);
        }

        private bool ValidateChargeSubjectContain(string ChargeSubjectName, out int ChargeSubjectId)
        {
            ChargeSubjectId = 0;
            if (string.IsNullOrEmpty(ChargeSubjectName))
            {
                return true;
            }
            using (var propertyMgrUnitOfWork = Crosscuting.UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var suj = propertyMgrUnitOfWork.ChargeSubjectRepository.GetAll()
                    .Where(r => r.Name == ChargeSubjectName
                        && r.ComDeptId == this.CommDeptId
                        && r.IsDel == false).FirstOrDefault();
                if (suj == null)
                {
                    return false;
                }
                ChargeSubjectId = suj.Id.Value;
                return true;
            }
        }


        private bool ValidateBlanceFormat(string Blance)
        {
            return Regex.IsMatch(Blance, @"^(([1-9]\d{0,9})|0)(\.\d{1,2})?$");
        }


        #endregion

        #region 预存账户费用管理

        /// <summary>
        /// 获取预存费转移列表
        /// </summary>
        public IList<PrepayAccountTransferDTO> GetPrepayAccountTransferList(int houseDeptId)
        {
            using (var pUnitWork = Crosscuting.UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                List<PrepayAccountTransferDTO> dataList = new List<PrepayAccountTransferDTO>();
                //全部收费项目
                var allSubject = pUnitWork.PrepayAccountRepository.GetAll()
                    .Where(p => p.HouseDeptId == houseDeptId && p.ChargeSubjectID == 0 && p.IsDel == false).FirstOrDefault();
                if (allSubject != null)
                {
                    dataList.Add(new PrepayAccountTransferDTO()
                    {
                        Id = allSubject.Id,
                        ChargeSubjectID = allSubject.ChargeSubjectID,
                        ChargeSubjectName = "全部收费项目",
                        Balance = allSubject.Balance
                    });
                }
                var query = from p in pUnitWork.PrepayAccountRepository.GetAll()
                            join s in pUnitWork.ChargeSubjectRepository.GetAll()
                            on p.ChargeSubjectID equals s.Id
                            where p.HouseDeptId == houseDeptId
                            && p.IsDel == false
                            select new PrepayAccountTransferDTO()
                            {
                                Id = p.Id,
                                ChargeSubjectID = p.ChargeSubjectID,
                                ChargeSubjectName = s.Name,
                                Balance = p.Balance
                            };
                dataList.AddRange(query.ToList());
                return dataList;
            }
        }

        /// <summary>
        /// 获取批量抵扣账单列表
        /// </summary>
        public List<BatchDeductionBillSumDTO> GetBatchDeductionBillList(int pageStart, int pageSize,
            Expression<Func<BatchDeductionBillDTO, bool>> predicate, out int outCount)
        {
            using (var pUnitWork = Crosscuting.UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                //预存费大于0
                var preQuery = pUnitWork.PrepayAccountRepository.GetAll().Where(p => p.Balance > 0);
                //收费项目预存费账单
                var projectQuery = from b in pUnitWork.ChargBillRepository.GetAll()//.Where(c => c.Status == (int)BillStatusEnum.NoPayment )
                                   join p in preQuery on new { HouseDeptId = b.HouseDeptId, SubjectId = b.ChargeSubjectId }
                                   equals new { HouseDeptId = p.HouseDeptId, SubjectId = p.ChargeSubjectID }
                                   into ptemp
                                   from lp in ptemp.DefaultIfEmpty()
                                   join cp in preQuery on new { HouseDeptId = b.HouseDeptId, SubjectId = (int?)0 }
                                   equals new { HouseDeptId = cp.HouseDeptId, SubjectId = cp.ChargeSubjectID }
                                   into cptemp
                                   from lcp in cptemp.DefaultIfEmpty()
                                   where lp != null || lcp != null
                                   where b.Status == (int)BillStatusEnum.NoPayment //未付款
                                   where b.IsDevPay == false                       //排除开发商代缴
                                   where b.IsDel == false                          //排除作废账单
                                   select new BatchDeductionBillDTO()
                                   {
                                       ComDeptId = b.ComDeptId,
                                       BeginDate = b.BeginDate,
                                       EndDate = b.EndDate,
                                       HouseDeptId = b.HouseDeptId,
                                       ResourceName = b.ResourcesName,
                                       SubjectId = b.ChargeSubjectId,
                                       SubjectName = b.ChargeSubject.Name,
                                       PreAmount = (lp == null ? 0 : lp.Balance),        //收费项目余额
                                       CommonPreAmount = (lcp == null ? 0 : lcp.Balance),//所有收费项目余额
                                       Amount = (b.BillAmount - b.ReceivedAmount),
                                       HouseDoorNo = b.HouseDoorNo
                                   };
                var query = from p in projectQuery.Where(predicate)
                            group p
                            by new { p.ComDeptId, p.HouseDeptId, p.ResourceName, p.SubjectId, p.SubjectName, p.PreAmount, p.CommonPreAmount } into g
                            select new BatchDeductionBillSumDTO()
                            {
                                ComDeptId = g.Key.ComDeptId,
                                HouseDeptId = g.Key.HouseDeptId,
                                ResourceName = g.Key.ResourceName,
                                SubjectId = g.Key.SubjectId,
                                SubjectName = g.Key.SubjectName,
                                CommonPreAmount = g.Key.CommonPreAmount,//全部收费项目预存
                                PreAmount = g.Key.PreAmount,            //收费项目预存
                                Amount = g.Sum(s => s.Amount)           //欠费金额合计
                            };
                outCount = query.Count();
                var dataList = query.OrderBy(f => f.ResourceName).ThenBy(f => f.SubjectName).Skip(pageStart).Take(pageSize).ToList();
                return dataList;
            }
        }

        /// <summary>
        /// 根据房屋deptId和收费项目的账单进行预存抵扣
        /// </summary>
        /// <param name="houseDeptSubjectIds"></param>
        /// <returns></returns>
        public bool PreCostBatchDeduction(string[] houseDeptSubjectIds, int Operator, string OperatorName)
        {
            List<ChargBill> deducitonBillList = new List<ChargBill>();
            List<PrepayDeductionResult> presult = new List<PrepayDeductionResult>();
            try
            {
                using (var pUnitWork = Crosscuting.UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
                {
                    PrepayDeductionResult result = new PrepayDeductionResult();
                    foreach (var item in houseDeptSubjectIds)
                    {
                        var hsIds = item.Split(',');
                        var hid = int.Parse(hsIds[0]);
                        var sid = int.Parse(hsIds[1]);
                        //满足条件的账单
                        var billList = pUnitWork.ChargBillRepository
                                                .GetAll()
                                                .Where(b => b.Status == (int)BillStatusEnum.NoPayment
                                                && b.IsDevPay == false //排除开发商代缴
                                                && b.HouseDeptId == hid
                                                && b.IsDel ==false
                                                && b.ChargeSubjectId == sid)
                                                .ToList();
                        deducitonBillList.AddRange(billList);
                    }

                    //v2.9 预存抵扣收费记录合并 2017-9-13
                    var config = BillCommonService.Instance.GetCommunityConfig(pUnitWork, deducitonBillList.First().ComDeptId);
                    string chargeRecordId = string.Empty;          //合并收费记录Id
                    Dictionary<int?, string> groupChargeRecordId = new Dictionary<int?, string>();
                    List<ChargBill> preDeductionBillList = new List<ChargBill>();   //被预存费抵扣了的账单列表

                    //先抵扣账单开始日最小的
                    var orderbyBillList = deducitonBillList.OrderBy(d => d.BeginDate).ToList();

                    //定义抵扣日志列表 2017-7-28
                    List<DeductionLogInfo> deductionLogList = new List<DeductionLogInfo>();
                    //按照 账单开始日最小的 循环抵扣
                    foreach (var bill in orderbyBillList)
                    {
                        decimal deductionAmount = bill.BillAmount.Value + bill.PenaltyAmount.Value - bill.ReceivedAmount.Value - bill.ReliefAmount.Value;
                        //v2.9 配置是否合并收费记录
                        if (!config.IsPreMergeChargeRecord.Value)
                        {
                            chargeRecordId = Guid.NewGuid().ToString();
                        }
                        else
                        {
                            //如果不存在组， 就添加组
                            if (!groupChargeRecordId.Keys.Any(g => g == bill.HouseDeptId))
                            {
                                groupChargeRecordId.Add(bill.HouseDeptId, Guid.NewGuid().ToString());
                            }
                            chargeRecordId = groupChargeRecordId[bill.HouseDeptId];
                        }
                        
                        //已交金额 += 抵扣金额
                        result = BillCommonService.Instance.AutomaticDeduction(pUnitWork, bill, deductionAmount, chargeRecordId);
                        //日志列表 2017-7-28
                        foreach (var item in result.DeductionDetailList)
                        {
                            DeductionLogInfo logInfo = new DeductionLogInfo();
                            logInfo.HouseDeptId = bill.HouseDeptId;
                            logInfo.HouseDoorNo = bill.HouseDoorNo;
                            logInfo.SubjectId = bill.ChargeSubjectId;
                            logInfo.SubjectName = bill.ChargeSubject.Name;
                            logInfo.DeductionAccountSubjectId = item.SubjectId;
                            logInfo.DeductionAmount = item.DeductionAmount;
                            deductionLogList.Add(logInfo);
                        }

                        //如果抵扣金额为0 就不需要更新账单和生成费用流水
                        if (result.TotalDeductionAmount == 0)
                        {
                            continue;
                        }
                        bill.ReceivedAmount += result.TotalDeductionAmount;
                        if (bill.BillAmount == bill.ReceivedAmount)//如果已交完，更新账单状态
                        {
                            bill.Status = BillStatusEnum.Paid.GetHashCode();
                        }
                        else
                        {
                            bill.Status = BillStatusEnum.NoPayment.GetHashCode();
                        }
                        //v2.9 配置是否合并收费记录 2017-9-13
                        if (config.IsPreMergeChargeRecord.Value)
                        {
                            preDeductionBillList.Add(bill);
                        }
                        else
                        {
                            //生成费用记录明细
                            var chargeRecord = BillCommonService.Instance.GenerateChargeRecordByBill(pUnitWork, bill, chargeRecordId, result.TotalDeductionAmount, "批量预存抵扣", Operator, OperatorName);
                        }
                        pUnitWork.ChargBillRepository.Update(bill);
                        presult.Add(result);
                    }
                    //v2.9 跟进配置合并收费记录 2017-9-13
                    if (config.IsPreMergeChargeRecord.Value && preDeductionBillList.Count() > 0)
                    {
                        //多房屋 需要以房屋分组生成 收费记录
                        var groupHouseDeptIds = preDeductionBillList.GroupBy(p => p.HouseDeptId).Select(g => g.Key).ToList();
                        foreach (var houseDeptId in groupHouseDeptIds)
                        {
                            //账单划账明细
                            var billDictionary = presult.Where(p => p.HouseDeptId == houseDeptId).ToDictionary(key => key.ChargeBillId, value => value.TotalDeductionAmount);
                            var preHouseBillList = preDeductionBillList.Where(b => b.HouseDeptId == houseDeptId).ToList();
                            BillCommonService.Instance.GenerateChargeRecordByBillList(pUnitWork, preHouseBillList, groupChargeRecordId[houseDeptId], billDictionary, "批量预存抵扣收费记录合并", Operator, OperatorName);
                        }
                    }

                    //记录日志 2017-7-28
                    PrepayAccountLog(pUnitWork, deductionLogList, Operator,OperatorName, orderbyBillList.First().ComDeptId);
                    pUnitWork.Commit();  
                }
                return true;
            }
            catch (Exception ex)
            {
                LogProperty.WriteLoginToFile(string.Format("[批量抵扣]houseDeptSubjectIds:{0} Exception:{1}", houseDeptSubjectIds, ex), "PreCostBatchDeduction", FileLogType.Exception);
                return false;
            }

        }

        #endregion

        #region 记录抵扣日志

        private void PrepayAccountLog(IPropertyMgrUnitOfWork pUnitWork, List<DeductionLogInfo> deductionLogList, int Operator, string OperatorName, int? ComDeptId)
        {
            //1.抵扣账户分组
            var groupLogList = from d in deductionLogList
                                where d.DeductionAmount > 0
                                group d by new { d.HouseDeptId, d.HouseDoorNo, d.SubjectId, d.SubjectName, d.DeductionAccountSubjectId }
                                into gd
                                select new DeductionLogInfo()
                                {
                                    HouseDeptId = gd.Key.HouseDeptId,
                                    HouseDoorNo = gd.Key.HouseDoorNo,
                                    SubjectId = gd.Key.SubjectId,
                                    SubjectName = gd.Key.SubjectName,
                                    DeductionAccountSubjectId = gd.Key.DeductionAccountSubjectId,
                                    DeductionAmount = gd.Sum(g => g.DeductionAmount)
                                };
          //2.记录日志收费项目分组
           var subjectGroup = groupLogList.Select(g => new { g.HouseDeptId,g.HouseDoorNo, g.SubjectId, g.SubjectName }).Distinct().ToList();
            //3.循环记录日志
            foreach (var item in subjectGroup)
            {
                PrepayAccountLog log = new PrepayAccountLog();
                log.ComDeptId = ComDeptId;
                log.HouseDeptId = item.HouseDeptId;
                log.ResourcesId = item.HouseDeptId;
                log.PrepayAccountId = 0;
                log.Operator = OperatorName;
                log.OperatorId = Operator;
                var logList = groupLogList.Where(g => g.HouseDeptId == item.HouseDeptId && g.SubjectId == item.SubjectId).ToList();
                log.Desc = string.Format("{0} 预存费 抵扣 {1} {2}（", item.HouseDoorNo, item.SubjectName, logList.Sum(l => l.DeductionAmount));
                int count = logList.Count();
                foreach (var itmeLog in logList)
                {
                    //全部收费项目预存
                    if (itmeLog.DeductionAccountSubjectId == 0)
                    {
                        log.Desc += ("全部收费项目预存 " + itmeLog.DeductionAmount);
                    }
                    //收费项目预存
                    else
                    {
                        log.Desc += ("收费项目预存 " + itmeLog.DeductionAmount + (count == 1 ? "" : "，"));
                    }

                }
                log.Desc += "）";
                log.OperationTime = DateTime.Now;
                log.Remark = "";
                pUnitWork.PrepayAccountLogRepository.Add(log);
            }
            
        }

        #endregion

        #region 公共服务

        public IList<PrepayAccount> GetPrepayAccountListByHouseDeptId(int? houseDeptId)
        {
            using (var pmUnitWork = Crosscuting.UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = pmUnitWork.PrepayAccountRepository.GetAll().Where(p => p.HouseDeptId == houseDeptId && p.Balance > 0 && p.IsDel == false);
                return query.ToList();
            }
        }

        public IList<DailyChargPrepayAccountDTO> GetailyChargPrepayAccountList(int? houseDeptId)
        {
            using (var pmUnitWork = Crosscuting.UnityHelper.UnityContainerInstance.Resolve<IPropertyMgrUnitOfWork>())
            {
                var query = pmUnitWork.PrepayAccountRepository.GetAll()
                    .Where(p => p.HouseDeptId == houseDeptId && p.Balance > 0 && p.IsDel == false)
                    .Select(p => new DailyChargPrepayAccountDTO() { SubjectId = p.ChargeSubjectID, PreAmount = p.Balance });
                return query.ToList();
            }
        }

        #endregion
    }



    class PrepayRefDeptinfo
    {
        public int? PrepayAccountId { get; set; }
        public int? ChargeSubjectId { get; set; }
        public string DoorNum { get; set; }
    }


    class PrepayAccountExcelModel
    {
        public string DoorNum { get; set; }

    }
    class PrepayAccountLogInfo
    {
        public int? HouseDeptId { get; set; }
        public int? ComDeptId { get; set; }
        public int? ResourcesId { get; set; }
        public string ResourcesName { get; set; }

        public int? Operator { get; set; }
        public string OperatorName { get; set; }
        public string Remark { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// 抵扣日志
    /// </summary>
    public class DeductionLogInfo
    {
        /// <summary>
        /// 房屋ID
        /// </summary>
        public int? HouseDeptId { get; set; }

        public string HouseDoorNo { get; set; }
        /// <summary>
        /// 收费项目
        /// </summary>
        public int? SubjectId { get; set; }

        public string SubjectName { get; set; }
        /// <summary>
        /// 抵扣的收费项目账户ID
        /// </summary>
        public int? DeductionAccountSubjectId { get; set; }
        /// <summary>
        /// 抵扣金额
        /// </summary>
        public decimal? DeductionAmount { get; set; }
    }

}
