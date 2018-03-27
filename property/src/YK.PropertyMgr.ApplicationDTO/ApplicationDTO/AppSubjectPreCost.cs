using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    [Serializable]
    public class AppSubjectPreCost
    {
        public AppSubjectPreCost()
        {
            ManualSubjectPreCostList = new List<ManualSubjectPreCost>();
        }

        /// <summary>
        /// 小区deptId
        /// </summary>
        public int ComDeptId { get; set; }
        
        /// <summary>
        /// 房屋deptId
        /// </summary>
        public int HouseDeptId { get; set; }

        /// <summary>
        /// 预存月数
        /// </summary>
        public int? Month { get; set; }

        /// <summary>
        /// 收费项目Ids
        /// </summary>
        public int[] SubjectIds { get; set; }

        /// <summary>
        /// 支付金额 = 预存月数 * 收费项目合计金额 - 优惠金额
        /// </summary>
        public decimal? PaymentAmount { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public PayTypeEnum PayType { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 优惠信息
        /// </summary>
        public PaymentDiscountInfoDTO DiscountInfo { get; set; }

        /// <summary>
        /// 手动预存费列表
        /// </summary>
        public List<ManualSubjectPreCost> ManualSubjectPreCostList { get; set; }
    }

    [Serializable]
    public class ManualSubjectPreCost
    {
        /// <summary>
        /// 手动输入收费项目Id
        /// </summary>
        public int? SubjectId { get; set; }

        /// <summary>
        /// 手动输入预存费
        /// </summary>
        public decimal? PreCost { get; set; }
    }
}
