using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
   public class ReceiptBookDetailSeachDTO:BaseSearchDTO
    {
        /// <summary>
        /// 票据号
        /// </summary>
        public string ReceiptNum { get; set; }
        /// <summary>
        /// 收费开始日期
        /// </summary>
        public DateTime? ChargeStartDate { get; set; }
        /// <summary>
        /// 收费结束日期
        /// </summary>
        public DateTime? ChargeEndDate { get; set; }
        /// <summary>
        /// 收费资源
        /// </summary>
        public string ResourcesName { get; set; }
        /// <summary>
        /// 票据号Id
        /// </summary>
        public int? ReceiptBookId { get; set; }
    }
}
