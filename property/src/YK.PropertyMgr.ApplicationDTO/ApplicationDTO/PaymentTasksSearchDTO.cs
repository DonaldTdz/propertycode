using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class PaymentTasksSearchDTO:BaseSearchDTO
    {

        /// <summary>
        /// 交款单Id
        /// </summary>
        public string PaymentTaskId { get; set; }


        /// <summary>
        /// 交款人
        /// </summary>
        public string ApplicantName { get; set; }

        /// <summary>
        ///交款时间开始
        /// </summary>
        public DateTime? PaymentDateMin { get; set; }

        /// <summary>
        ///交款时间结束
        /// </summary>
        public DateTime? PaymentDateMax { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public int Status { get; set; }


        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }
    }
}
