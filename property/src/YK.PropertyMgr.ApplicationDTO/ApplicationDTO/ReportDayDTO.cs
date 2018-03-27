using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class ReportDayDTO
    {
        public ReportDayDTO()
        {
            HasData = false;
            GroupId = 1;
        }
        /// <summary>
        /// 收费项目Id
        /// </summary>
        public int? Id { get; set; }

        public string SubjectName { get; set; }

        public int? SubjectType { get; set; }

        public decimal? BeforeMonthAmount { get; set; }

        public decimal? CurrentMonthAmount { get; set; }
        /// <summary>
        /// 预收金额
        /// </summary>
        public decimal? PreAmount { get; set; }
        /// <summary>
        /// 预存金额
        /// </summary>
        public decimal? PreStoreAmount { get; set; }
        /// <summary>
        /// 实收金额=“N月前期”+“当月”+“预收金额”+“预存金额”
        /// </summary>
        public decimal? ActualAmount
        {
            get
            {
                decimal temp = 0;
                if (BeforeMonthAmount.HasValue)
                {
                    temp += BeforeMonthAmount.Value;
                }
                if (CurrentMonthAmount.HasValue)
                {
                    temp += CurrentMonthAmount.Value;
                }
                if (PreAmount.HasValue)
                {
                    temp += PreAmount.Value;
                }
                if (PreStoreAmount.HasValue)
                {
                    temp += PreStoreAmount.Value;
                }
                return temp;
            }
        }

        public decimal? ShowActualAmount { get; set; }
        /// <summary>
        /// 应收退款
        /// </summary>
        public decimal? Refund { get; set; }
        /// <summary>
        /// 预收退款
        /// </summary>
        public decimal? PreRefund { get; set; }
        /// <summary>
        /// 预存退款
        /// </summary>
        public decimal? PreStoreRefund { get; set; }
        /// <summary>
        /// 合计
        /// </summary>
        public decimal? TotalAmount
        {
            get
            {
                var temp = ActualAmount;
                if (Refund.HasValue)
                {
                    temp -= Refund.Value;
                }
                if (PreRefund.HasValue)
                {
                    temp -= PreRefund.Value;
                }
                if (PreStoreRefund.HasValue)
                {
                    temp -= PreStoreRefund.Value;
                }
                return temp;
            }
        }
        public decimal? ShowTotalAmount { get; set; }
        /// <summary>
        /// 预存抵扣
        /// </summary>
        public decimal? PreStoreDeduction { get; set; }

        public bool HasData { get; set; }

        public int GroupId { get; set; }
    }
}
