using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.ApplicationDTO.ApplicationDTO;
using YK.PropertyMgr.CompositeDomainService;

namespace YK.PropertyMgr.CompositeAppService
{
    public class DataInitCompositeAppService
    {
        #region 欠费信息导入

        public static ResultModel ImportArrearage(int ComDeptId, IList<ArrearageInfo> ArrearageInfoList, int Operator, string OperatorName)
        {
            return DataInitService.Instance.ImportArrearage(ComDeptId, ArrearageInfoList, Operator, OperatorName);
        }

        #endregion

        #region 历史缴费信息导入

        public static ResultModel ImportHistoryCost(int ComDeptId, IList<HistoryCostInfo> HistoryCostInfoList, int Operator, string OperatorName)
        {
            return DataInitService.Instance.ImportHistoryCost(ComDeptId, HistoryCostInfoList, Operator, OperatorName);
        }

        #endregion
    }
}
