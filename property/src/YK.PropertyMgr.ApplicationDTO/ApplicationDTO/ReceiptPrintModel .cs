using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class ReceiptPrintModel
    {
        public string BillId { get; set; }
        public string ProjectDesc { get; set; }
        public int? SubjectId { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Price { get; set; }
        public string Quantity { get; set; }
        public decimal? ReliefAmount { get; set; }
        public decimal? BillAmount { get; set; }
        public decimal? Amount { get; set; }
        public int? RefType { get; set; }
        public int? ResId { get; set; }
        public string BillRemark { get; set; }
        public string AbstractDesc
        {
            get
            {
                if (!string.IsNullOrEmpty(DiscountDesc))
                {
                    return DiscountDesc;
                }
                else
                {
                    string desc = string.Empty;
                   
                    if (RefType == (int)SubjectTypeEnum.SystemPreset)
                    {
                        desc = BillRemark;
                    }
                    else
                    {
                        desc = (BeginDate.HasValue ? BeginDate.Value.ToString("yyyy-MM-dd") + "至" : "")
                        + (EndDate.HasValue ? EndDate.Value.ToString("yyyy-MM-dd") : "")
                        + (RefType == (int)SubjectTypeEnum.Meter ? " 读数: " + Quantity : null);
                        if ((RefType == (int)SubjectTypeEnum.ParkingSpace || RefType == (int)SubjectTypeEnum.House) && !string.IsNullOrEmpty(Quantity))
                        {
                            desc += (" 面积: " + Quantity);
                        }
                            desc +=(PayMthodId == (int)PayTypeEnum.InternalTransfer ? " 预存抵扣" : "");
                    }
                 
                    return desc;
                }
            }
        }
        public int PayMthodId { get; set; }
        public string DiscountDesc { get; set; }
    }
}
