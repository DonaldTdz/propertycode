using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    /// <summary>
    /// 预存抵扣结果
    /// </summary>
    public class PrepayDeductionResult
    {
        public PrepayDeductionResult()
        {
            DeductionDetailList = new List<DeductionDetail>();
        }
        /// <summary>
        /// 判断收费项目预存是否为空
        /// </summary>
        public int SubjectAccountIsnull { get; set; }
        /// <summary>
        /// 划账金额
        /// </summary>
        public decimal TotalDeductionAmount { get; set; }

        /// <summary>
        /// 账单Id
        /// </summary>
        public string ChargeBillId { get; set; }

        /// <summary>
        /// 房屋Id
        /// </summary>
        public int? HouseDeptId { get; set; }

        /// <summary>
        /// 抵扣明细列表
        /// </summary>
        public List<DeductionDetail> DeductionDetailList { get; set; }
    }

    public class DeductionDetail
    {
        /// <summary>
        /// 收费项目ID
        /// </summary>
        public int? SubjectId { get; set; }

        /// <summary>
        ///  预存账户Id
        /// </summary>
        public int? PrepayAccountId { get; set; }

        /// <summary>
        /// 预存账户抵扣金额
        /// </summary>
        public decimal? DeductionAmount { get; set; }
    }
}
