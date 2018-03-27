using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;

namespace YK.PropertyMgr.CompositeDomainService.GenerateBillService.Interface
{
    public interface IGenerateChargBill
    {
        /// <summary>
        /// 生成某小区下面收费项目的账单
        /// </summary>
        /// <param name="CommunityID">小区ID</param>
        /// <param name="BillDay">账单日 默认0为当前日期</param>
        void GenerateCommunityChargBill(int CommunityID, int BillDay = 0, int houseDeptId = 0);

        /// <summary>
        /// 生成系统平台下面所有小区的账单
        /// </summary>
        /// <param name="BillDay">账单日</param>
        void GenerateAllChargBill(int BillDay = 0, bool IsTaskRun = true);

        /// <summary>
        /// 自动循环生成账单
        /// </summary>
        /// <param name="StartDate">开始时间</param>
        /// <param name="Hour">小时</param>
        /// <param name="Minute">分钟</param>
        void AutomaticCycleGenerationBill(DateTime StartDate, int Hour, int Minute, bool IsTaskRun);

        /// <summary>
        /// 关闭循环
        /// </summary>
        void CloseCycle();

        /// <summary>
        /// 生成预付款账单
        /// </summary>
        /// <param name="ComDeptId">小区ID</param>
        /// <param name="HouseDeptId">房屋ID</param>
        /// <param name="ResourcesId">资源ID</param>
        /// <param name="Amount">预付款金额</param>
        /// <param name="Remark">备注</param>
        /// <returns></returns>

        //ResultModel GeneratePrepaymentBill(int ComDeptId, int HouseDeptId, int ResourcesId, decimal Amount, string Remark, int Operator, string OperatorName);

        /// <summary>
        /// 生成临时收费账单
        /// </summary>
        /// <param name="ComDeptId">小区ID</param>
        /// <param name="HouseDeptId">房屋ID</param>
        /// <param name="ResourcesId">资源ID</param>
        /// <param name="Amount">预付款金额</param>
        /// <param name="Remark">备注</param>
        /// <returns></returns>

        ChargBill GenerateTemporaryBill(int ComDeptId, int HouseDeptId, int ResourcesId,int ChargeSubjectId, DateTime? BeginDateTime, DateTime? EndDateTime, decimal Amount, string Remark);
        /// <summary>
        /// 计算收费项计费金额
        /// </summary>
        /// <param name="chargeSubject">收费项目</param>
        /// <param name="houseDeptId">房屋DeptId</param>
        /// <returns></returns>
        //decimal CalculateAmount(int ChargeSubjectId, int houseDeptId);

        /// <summary>
        /// 拆分账单
        /// </summary>
        ResultModel SplitBill(string billId, DateTime splitDate, string remark, int Operator, string OperatorName);

        /// <summary>
        /// 刷新房屋账单
        /// </summary>
        ResultModel RefreshChargBill(int HouseDeptID);

        #region 手动生成账单

        ResultModel ManualGenerationHouseBill(int HouseDeptId, string[] SubjectResourceIds, DateTime EndDate, int DeptType);

        ResultModel ManualBatchGenerationByComDeptId(int ComDeptId, int SubjectId, int?[] ResultIds, DateTime EndDate);

        #endregion

        #region 解绑手动生成账单

        IList<ResultModel> ManualUnbundlingGenerationBill(int ComDeptId, Dictionary<int, IEnumerable<UnbundlingDto>> UnbundlingList);

        #endregion
    }
}
