using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
   public class ReportExternalchargeDetailDTO
    {
        /// <summary>
        /// 收费对象
        /// </summary>
        public string ResourcesName { get; set; }
        /// <summary>
        /// 收费项目
        /// </summary>
        public string ChargeSubjectName { get; set; }
        /// <summary>
        /// 收费类型
        /// </summary>
        public int? ChargeType { get; set; }
        /// <summary>
        /// 账单开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }

        public int? SubjectType { get; set; }
        public string BeginDateFormat
        {
            get
            {
                if (BeginDate.HasValue)
                {
                    return BeginDate.Value.ToString("yyyy-MM-dd");
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 账单结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
        public string EndDateFormat
        {
            get
            {
                if (EndDate.HasValue)
                {
                    return EndDate.Value.ToString("yyyy-MM-dd");
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 账单金额
        /// </summary>
        public decimal? BillAmount { get; set; }
        /// <summary>
        /// 收费金额
        /// </summary>
        public decimal? ArrearsAmount { get; set; }
        /// <summary>
        /// 收费时间
        /// </summary>
        public DateTime? PayDate { get; set; }

        public string PayDateFormat
        {
            get
            {
                if (PayDate.HasValue)
                {
                    return PayDate.Value.ToString("yyyy-MM-dd HH:mm");
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 票据号
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public int? PayMthodId { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public string BeginDateStr { get; set; }

        public string EndDateStr { get; set; }

        public string CreateTimeStr { get; set; }

        public int? ComDeptId { get; set; }
    }
}
