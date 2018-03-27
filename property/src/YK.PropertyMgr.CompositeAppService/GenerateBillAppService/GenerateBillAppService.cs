using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.Enums;
using YK.PropertyMgr.ApplicationMapper;
using YK.PropertyMgr.CompositeDomainService;
using YK.PropertyMgr.CompositeDomainService.GenerateBillService;
using YK.PropertyMgr.DomainEntity;

namespace YK.PropertyMgr.CompositeAppService
{
    public class GenerateBillAppService
    {
        #region 生成小区账单

        /// <summary>
        /// 生成指定小区账单
        /// 指定账单日：生成账单日收费项目账单 包括之前未生成的账单
        /// 不指定账单日：生成所有未生成的账单，所有账单日
        /// </summary>
        /// <param name="CommunityID">小区ID</param>
        /// <param name="BillDay">账单日 账单日=0时，为当前日前的所有账单</param>
        public static void GenerateCommunityChargBill(int CommunityID, int BillDay = 0)
        {
            GenerateChargBillService.Instance.GenerateCommunityChargBill(CommunityID, BillDay);
        }

        /// <summary>
        /// 生成指定小区账单日为今天的账单
        /// </summary>
        /// <param name="CommunityID">小区ID</param>
        public static void GenerateCommunityTodayChargBill(int CommunityID)
        {
            GenerateChargBillService.Instance.GenerateCommunityChargBill(CommunityID, DateTime.Today.Day);
        }

        #endregion

        #region 生成所有账单

        /// <summary>
        /// 生成所有账单
        /// </summary>
        /// <param name="BillDay">账单日</param>
        public static void GenerateAllChargBill(int BillDay = 0, bool IsTaskRun = true)
        {
            GenerateChargBillService.Instance.GenerateAllChargBill(BillDay, IsTaskRun);
        }

        /// <summary>
        /// 生成所有账单日为今天的账单
        /// </summary>
        public static void GenerateAllTodayChargBill()
        {
            GenerateChargBillService.Instance.GenerateAllChargBill(DateTime.Today.Day);
        }

        #endregion

        #region 开启关闭自动生成账单

        /// <summary>
        /// 自动循环生成账单
        /// 默认每天循环 时间指定
        /// </summary>
        /// <param name="StartDate">开始日期</param>
        /// <param name="Hour">小时</param>
        /// <param name="Minute">分钟</param>
        public static void AutomaticCycleGenerationBill(DateTime StartDate, int Hour, int Minute, bool IsTaskRun)
        {
            GenerateChargBillService.Instance.AutomaticCycleGenerationBill(StartDate, Hour, Minute, IsTaskRun);
        }

        /// <summary>
        /// 关闭自动生成账单
        /// </summary>
        public static void CloseAutomaticCycle()
        {
            GenerateChargBillService.Instance.CloseCycle();
        }

        #endregion

        #region 生成预缴费账单

        /// <summary>
        /// 生成预付款账单
        /// </summary>
        /// <param name="ComDeptId">小区ID</param>
        /// <param name="HouseDeptId">房屋ID</param>
        /// <param name="ResourcesId">资源ID</param>
        /// <param name="Amount">预付款金额</param>
        /// <param name="Remark">备注</param>
        /// <returns></returns>
        //public static ResultModel GeneratePrepaymentBill(int ComDeptId, int HouseDeptId, int ResourcesId, decimal Amount, string Remark, int Operator, string OperatorName)
        //{
        //    if (Operator == 0 && string.IsNullOrEmpty(OperatorName))
        //    {
        //        OperatorName = BillCommonService.SystemOperatorName;
        //    }
        //    return GenerateChargBillService.Instance.GeneratePrepaymentBill(ComDeptId, HouseDeptId, ResourcesId, Amount, Remark, Operator, OperatorName);
        //}

        /// <summary>
        /// 生成预付款账单
        /// </summary>
        /// <param name="ComDeptId">小区ID</param>
        /// <param name="HouseDeptId">房屋ID</param>
        /// <param name="ResourcesId">资源ID</param>
        /// <param name="Amount">预付款金额</param>
        /// <param name="Remark">备注</param>
        /// <returns></returns>
        public static ResultModel GenerateTempPrepaymentBill(ChargBillDTO billInfo, int Operator, string OperatorName)
        {
            if (Operator == 0 && string.IsNullOrEmpty(OperatorName))
            {
                OperatorName = BillCommonService.SystemOperatorName;
            }
            var result = GenerateChargBillService.Instance.GenerateTempPrepaymentBill(0, billInfo.HouseDeptId.Value, 
                billInfo.HouseDeptId.Value, billInfo.BillAmount.Value, billInfo.Remark, Operator, OperatorName, 
                billInfo.PreChargeSubjectId, billInfo.Months);
            if (result.IsSuccess)
            {
                var demoBill = (ChargBill)result.Data;
                var bill = ChargBillMappers.ChangeChargBillToDTO(demoBill);
                bill.ActionStatus = ActionStatusEnum.New;
                bill.HouseDoorNo = billInfo.HouseDoorNo;
                bill.ChargeSubjectName = bill.Description;
                bill.Months = billInfo.Months;                        //预存月份
                bill.PreChargeSubjectId = billInfo.PreChargeSubjectId;//预付收费项目Id
                bill.IsChecked = true;
                bill.IsShow = true;
                result.Data = bill;
            }
            return result;
        }

        #endregion

        #region 生成临时缴费账单

        public static ChargBillDTO GenerateTemporaryBill(int ComDeptId, int HouseDeptId, int ResourcesId, int ChargeSubjectId, DateTime? BeginDateTime, DateTime? EndDateTime, decimal Amount, string Remark, int ReourceType = (int)ReourceTypeEnum.House)
        {
            var bill = GenerateChargBillService.Instance.GenerateTempTemporaryBill(ComDeptId, HouseDeptId, ResourcesId, ChargeSubjectId, BeginDateTime, EndDateTime, Amount, Remark, ReourceType);
            var billdto = ChargBillMappers.ChangeChargBillToDTO(bill);
            billdto.ActionStatus = ActionStatusEnum.New;
            billdto.ChargeSubjectName = bill.Description;
            billdto.IsChecked = true;
            return billdto;
        }

        #endregion

        #region 计算收费项目金额

        //public decimal CalculateAmount(int ChargeSubjectId, int houseDeptId)
        //{
        //    return GenerateChargBillService.Instance.CalculateAmount(ChargeSubjectId, houseDeptId);
        //}

        #endregion

        #region 账单拆分

        public static ResultModel SplitBill(string billId, DateTime splitDate, string remark, int Operator, string OperatorName)
        {
            return GenerateChargBillService.Instance.SplitBill(billId, splitDate, remark, Operator, OperatorName);
        }
 

        public static ResultModel SplitTempBill(ChargBillDTO bill, int Operator, string OperatorName)
        {
            var result = GenerateChargBillService.Instance.SplitTempBill(bill.Id, bill.SplitDate.Value, bill.Remark, Operator, OperatorName, ChargBillMappers.ChangeDTOToChargBillNew(bill));
            if (result.IsSuccess)
            {
                ChargBill[] arr = result.Data as ChargBill[];
                var newBill = ChargBillMappers.ChangeChargBillToDTO(arr[0]);
                newBill.ChargeSubjectName = arr[1].ChargeSubject.Name;
                newBill.HouseDoorNo = bill.HouseDoorNo;
                newBill.IsChecked = true;
                newBill.ActionStatus = ActionStatusEnum.New;
                var oldbill = ChargBillMappers.ChangeChargBillToDTO(arr[1]);
                oldbill.ChargeSubjectName = arr[1].ChargeSubject.Name;
                oldbill.HouseDoorNo = bill.HouseDoorNo;
                oldbill.IsChecked = true;

                if (bill.ActionStatus == ActionStatusEnum.New)
                {
                    newBill.RefId = bill.RefId;
                    oldbill.RefId = bill.RefId;
                    oldbill.ActionStatus = ActionStatusEnum.New;
                }
                else
                {
                    oldbill.ActionStatus = ActionStatusEnum.Update;
                    newBill.RefId = oldbill.Id;
                }

                //判断是否是第二次拆分账单
                //oldbill.ActionStatus = (bill.ActionStatus == ActionStatusEnum.New? ActionStatusEnum.New : ActionStatusEnum.Update);
                result.Data = new { NewBill = newBill, Bill = oldbill };
            }
            return result;
        }
        public static ResultModel CheckSplitTempBill(ChargBillDTO bill)
        {
            return GenerateChargBillService.Instance.CheckSplitTempBill(ChargBillMappers.ChangeDTOToChargBillNew(bill), bill.SplitDate.Value);
        }




        #endregion

        #region 账单作废
        public static ResultModel DeleteBill(ChargBillDTO bill, List<ChargBillDTO> billList, DailyChargBillDTO deleteModel)
        {
            var result = new ResultModel() { IsSuccess=true,Msg="删除成功"};

            var RowList = billList.Where(o => o.RowType == RowTypeEnum.ChildRow).ToList();

            foreach (var DeleteModel in RowList)
            {
                var DeleteResult = GenerateChargBillService.Instance.DeleteBill(DeleteModel.Id, bill.Remark, deleteModel);
                if (!DeleteResult.IsSuccess)
                {
                    result.IsSuccess = false;
                    result.Msg +=  " 科目：" + DeleteModel.ChargeSubjectName + " 时间范围:" + DeleteModel.BeginDate.Value.ToShortDateString() + "-" + DeleteModel.EndDate.Value.ToShortDateString() + " 原因:" + DeleteResult.Msg;
                }


            }

            return result;
        }

        public static ResultModel CheckDeleteBill_new(List<ChargBillDTO> billList, DailyChargBillDTO deleteModel)
        {
       
                var result = GenerateChargBillService.Instance.CheckDeleteBill(billList,"check",deleteModel);
                return result;
          
        }

        public static ResultModel CheckDeleteBill(string bill, DailyChargBillDTO deleteModel)
        {

            var result = GenerateChargBillService.Instance.CheckDeleteBill(bill, "check", deleteModel);
            return result;

        }


        #endregion

        #region 账单刷新

        public static ResultModel RefreshChargBill(int HouseDeptID)
        {
            return GenerateChargBillService.Instance.RefreshChargBill(HouseDeptID);
        }

        #endregion

        #region 手动生成账单

        public static ResultModel ManualGenerationHouseBill(int HouseDeptID,int DeptType, string[] SubjectResourceIds, DateTime EndDate)
        {
            return GenerateChargBillService.Instance.ManualGenerationHouseBill(HouseDeptID, SubjectResourceIds, EndDate,DeptType);
        }

        public static ResultModel ManualBatchGenerationByComDeptId(int ComDeptId, int SubjectId, int?[] ResultIds, DateTime EndDate)
        {
            return GenerateChargBillService.Instance.ManualBatchGenerationByComDeptId(ComDeptId, SubjectId, ResultIds, EndDate);
        }

        #endregion

        #region 解绑手动生成账单

        public static IList<ResultModel> ManualUnbundlingGenerationBill(int ComDeptId, Dictionary<int, IEnumerable<UnbundlingDto>> UnbundlingList)
        {
            return GenerateChargBillService.Instance.ManualUnbundlingGenerationBill(ComDeptId, UnbundlingList);
        }

        #endregion
    }
}
