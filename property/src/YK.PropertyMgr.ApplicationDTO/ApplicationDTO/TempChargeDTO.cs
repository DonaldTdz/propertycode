using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class TempChargeDTO
    {
        /// <summary>
        /// 房屋编号
        /// </summary>
        public int HouseDeptId { get; set; }
        /// <summary>
        /// 科目ID
        /// </summary>
        public int SubjectId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 账单金额
        /// </summary>
        public decimal Money { get; set; }
        /// <summary>
        /// 小区ID
        /// </summary>
        public int ComVillageDeptId { get; set; }

        public string Remark { get; set; }


        public string CustomerName { get; set; }



        public int ResourcesId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RefType { get; set; }

    }
}
