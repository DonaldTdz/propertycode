using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class ReportArrearsSearchDTO : BaseSearchDTO
    {
        /// <summary>
        /// 欠费截止日期
        /// </summary>
        public DateTime? ChargeDate { get; set; }

        public string ComDeptIdStr { get; set; }

        public int? DefaultComDeptId { get; set; }

        /// <summary>
        /// 业主姓名
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 楼宇
        /// </summary>
        public string LouyuIdStr { get; set; }

        public int? ComDeptId
        {
            get
            {
                if (!string.IsNullOrEmpty(ComDeptIdStr))
                {
                    return int.Parse(ComDeptIdStr.Replace("number:", ""));
                }
                return DefaultComDeptId;
            }
        }
    }
}
