using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class PrePaymentdeductionDetailSearchDTO : BaseSearchDTO
    {
        /// <summary>
        ///  收费开始日期
        /// </summary>
        public DateTime? ChargeBeginDate { get; set; }
        /// <summary>
        ///  收费结束日期
        /// </summary>
        public DateTime? ChargeEndDate { get; set; }
        public int? DefaultComDeptId { get; set; }
        public int? DefaultChargeSubjectId { get; set; }

        public string ComDeptIdStr { get; set; }
        /// <summary>
        /// 业主姓名
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }
        /// <summary>
        /// 票据号
        /// </summary>
        public string ReceiptNum { get; set; }

        /// <summary>
        /// 收费项目Id
        /// </summary>
        public string ChargeSubjectIdStr { get; set; }

        public string LouyuIdStr { get; set; }

        public int? ChargeSubjectId
        {
            get
            {
                if (!string.IsNullOrEmpty(ChargeSubjectIdStr))
                {
                    return int.Parse(ChargeSubjectIdStr.Replace("number:", ""));
                }
                return DefaultChargeSubjectId;
            }
        }

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
