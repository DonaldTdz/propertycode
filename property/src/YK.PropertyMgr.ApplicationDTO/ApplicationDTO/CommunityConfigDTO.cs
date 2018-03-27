using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.DomainEntity;

namespace YK.PropertyMgr.ApplicationDTO
{
    public partial class CommunityConfigDTO
    {
        public CommunityConfigDTO()
        {
            ActionType = ActionTypeEnum.None;
            ChargBillList = new List<ChargBill>();
        }

        public ActionTypeEnum ActionType { get; set; }

        /// <summary>
        /// 收费记录Id
        /// </summary>
        public string ChargeRecordId { get; set; }

        public List<ChargBill> ChargBillList { get; set; }
    }

    public enum ActionTypeEnum
    {
        None = 0,
        /// <summary>
        /// 手动生成账单
        /// </summary>
        ManualGenerationBill = 1,
        /// <summary>
        /// 预存费批量抵扣
        /// </summary>
        PreCostBatchDeduction = 2

    }
}
