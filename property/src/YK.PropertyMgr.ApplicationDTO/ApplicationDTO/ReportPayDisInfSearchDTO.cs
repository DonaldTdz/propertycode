using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class ReportPayDisInfSearchDTO : BaseSearchDTO
    {
        /// <summary>
        ///　开始日期 
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        public string ComDeptIdStr { get; set; }
        /// <summary>
        /// 票据号
        /// </summary>
        public string ReceiptNum { get; set; }
        /// <summary>
        /// 优惠名称
        /// </summary>
        public string DiscountDesc { get; set; }
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
        /// 物业优惠记录状态
        /// </summary>
        public int Status { get; set; }
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

        /// <summary>
        /// 楼栋资源
        /// </summary>
        public string LouyuIdStr { get; set; }
    }
}
