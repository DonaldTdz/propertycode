using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class PrePaymentDetailReportDTO
    {
        public string ResourcesName { get; set; }
        public string CustomerName { get; set; }
        public string ReceiptNum { get; set; }
        public string ChargeSubjectName { get; set; }
        public DateTime? ChargePayDate { get; set; }
        public string Remark { get; set; }
        public string PreType { get; set; }
        public decimal? Amount { get; set; }

        //判断条件
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? SubjectType { get; set; }

        public int? RefType { get; set; }

        public int? RescourcesId { get; set; }


        public string ChargePayDateStr
        {
            get;
            set;
            
        }

        public string BillAbstract
        {
            get
            {
                if (PreType == "预存")
                {
                        return Remark;
                }
                else
                {
                    if (BeginDate.HasValue && EndDate.HasValue)
                    {
                        return BeginDate.Value.ToString("yyyy-MM-dd") + "到" + EndDate.Value.ToString("yyyy-MM-dd");
                    }
                    
                }
                return string.Empty;
            }

        }
    }
}
