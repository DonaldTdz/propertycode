using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
   public  class ReportArrearsDetailDTO
    {
        /// <summary>
        /// 资源名称 
        /// </summary>
        public string ResourcesName { get; set; }
        /// <summary>
        /// 业主姓名
        /// </summary>
        public string OwnerName { get; set; }
        /// <summary>
        /// 收费项目
        /// </summary>
        public string ChargeSubjectName { get; set; }
        /// <summary>
        /// 账单开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 账单结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 账单金额
        /// </summary>
        public decimal? BillAmount { get; set; }
        /// <summary>
        /// 欠费金额
        /// </summary>
        public decimal? ArrearsAmount { get; set; }
        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime ? CreateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public string BeginDateStr { get; set; }

        public string EndDateStr { get; set; }

        public string CreateTimeStr { get; set; }


    }
}
