using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    /// <summary>
    /// 余额信息
    /// </summary>
    [Serializable]
    public class BalanceInfo
    {
        /// <summary>
        /// 小区ID
        /// </summary>
        public int ComDeptId { get; set; }
        /// <summary>
        /// 房屋ID
        /// </summary>
        public int HouseDeptId { get; set; }
        /// <summary>
        /// 资源ID
        /// </summary>
        public int ResourcesId { get; set; }
        /// <summary>
        /// 金额 充值金额 或 初始化金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 导入错误信息
        /// </summary>
        public string ErrorMsg { get; set; }

        /// <summary>
        /// 收费项目名称
        /// </summary>
        public string ChargeSubjectName { get; set; }

        public int ChargeSubjectId { get; set; }

        public string Remark { get; set; }
    }
}
