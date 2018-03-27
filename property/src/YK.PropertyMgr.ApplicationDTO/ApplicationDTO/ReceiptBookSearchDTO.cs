using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
     public  class ReceiptBookSearchDTO:BaseSearchDTO
    {
        public string Name { get; set; } 

        /// <summary>
        /// 票据类型/操作类型
        /// </summary>
        public int? RceciptType { get; set; }

        public int? Status { get; set; }

        /// <summary>
        /// 票据日志、修改历史
        /// </summary>
        public int? ReceiptBookHistoryType { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>
        public string OperatorContent { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime MinDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime MaxDate { get; set; }

      
    }
}
