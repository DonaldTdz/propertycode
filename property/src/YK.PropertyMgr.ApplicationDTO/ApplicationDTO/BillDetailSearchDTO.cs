using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class BillDetailSearchDTO : BaseSearchDTO
    {
        public string ResourcesName { get; set; }

        public string ReceiptNum { get; set; }

        public string CustomerName { get; set; }

        public string OperatorName { get; set; }

        public string ChargeSubjectIdName { get; set; }

        public int? ChargeSubjectId
        {
            get
            {
                if (!string.IsNullOrEmpty(ChargeSubjectIdName))
                {
                    return int.Parse(ChargeSubjectIdName.Replace("number:", ""));
                }
                return null;
            }
        }

        public int? ChargeType { get; set; }

        public int? BillStatus { get; set; }

        public int? PayType { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 用于控制查看结算列权限
        /// </summary>
        public bool SettleAccount { get; set; }
    }
}
