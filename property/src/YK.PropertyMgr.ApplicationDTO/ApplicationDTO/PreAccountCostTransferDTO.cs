using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class PreAccountCostTransferDTO
    {
        /// <summary>
        /// 转入收费项目
        /// </summary>
        public int? ChargeSubjectId { get; set; }

        public string OutChargeSubjectName { get; set; }

        public string InChargeSubjectName { get; set; }

        /// <summary>
        /// 被转移账户
        /// </summary>
        public int? PrepayAccountId { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        public int Operator { get; set; }

        /// <summary>
        /// 操作者Id
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
