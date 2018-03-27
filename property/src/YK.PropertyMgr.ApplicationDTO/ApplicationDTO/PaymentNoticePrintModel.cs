using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class PaymentNoticePrintModel
    {
        public string BillId { get; set; }
        public string Description { get; set; }
        public int? ChargeSubjectId { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Price { get; set; }
        public string Quantity { get; set; }
        /// <summary>
        /// 资源Id
        /// </summary>
        public int? HouseDeptId { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourcesName { get; set; }
        /// <summary>
        /// 账单金额
        /// </summary>
		public decimal? BillAmount { get; set; }
        /// <summary>
        /// 已收金额
        /// </summary>
        public decimal? ReceivedAmount { get; set; }
        /// <summary>
        /// 减免金额
        /// </summary>
        public decimal? ReliefAmount { get; set; }
        /// <summary>
        /// 滞纳金额
        /// </summary>
        public decimal? PenaltyAmount { get; set; }
        public int? RefType { get; set; }
        public string AbstractDesc
        {
            get
            {
                var desc = (BeginDate.HasValue ? BeginDate.Value.ToString("yyyy-MM-dd") + "至" : "")
                    + (EndDate.HasValue ? EndDate.Value.ToString("yyyy-MM-dd") : "")
               + (RefType == (int)SubjectTypeEnum.Meter ? " 读数: " + Quantity : null);
                return desc;
            }
        }
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal AmountShould
        {
            get
            {
                var bAmount = BillAmount.HasValue ? BillAmount.Value : 0;
                var rAmount = ReceivedAmount.HasValue ? ReceivedAmount.Value : 0;
                var rfAmount = ReliefAmount.HasValue ? ReliefAmount.Value : 0;
                var pAmount = PenaltyAmount.HasValue ? PenaltyAmount.Value : 0;
                //应收金额=计费金额+滞纳金-已交金额
                return bAmount + pAmount;
            }
        }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount
        {
            get
            {
                var bAmount = BillAmount.HasValue ? BillAmount.Value : 0;
                var rAmount = ReceivedAmount.HasValue ? ReceivedAmount.Value : 0;
                var rfAmount = ReliefAmount.HasValue ? ReliefAmount.Value : 0;
                var pAmount = PenaltyAmount.HasValue ? PenaltyAmount.Value : 0;
                //金额=计费金额+滞纳金-已交金额-减免金额
                return bAmount + pAmount - rAmount - rfAmount;
            }
        }
    }
}
