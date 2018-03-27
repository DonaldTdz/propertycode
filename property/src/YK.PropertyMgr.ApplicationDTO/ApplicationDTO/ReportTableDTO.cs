using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class ReportTableDTO
    {

        /// <summary>
        /// 科目名称
        /// </summary>
        public string ChargeSubjectName { get; set; }
         
        /// <summary>
        /// 科目Id
        /// </summary>
        public int? ChargeSubjectId { get; set; }

        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal? TotalRecAmount { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        public string CommunityName { get; set; }


        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 房屋ID
        /// </summary>
        public int? HouseDeptId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime CreCreateTime { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate { get; set; }


        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime PayDate { get; set; }


        /// <summary>
        /// 支付方式
        /// </summary>
        public int PayMthodId { get; set; }

        /// <summary>
        /// 实收金额合计
        /// </summary>
        public decimal? RececiveTotal { get; set; }


        /// <summary>
        /// 账单金额合计
        /// </summary>
        public decimal BillMoneyTotal { get; set; }

        /// <summary>
        /// 减免/优惠 金额
        /// </summary>
        public decimal ReliefAmountTotal { get; set; }


        /// <summary>
        /// 未缴金额
        /// </summary>
        public decimal UnPaidAmountTotal { get; set; }

        /// <summary>
        /// 收费率
        /// </summary>
        public string PayRate{get;set;}
        /// <summary>
        /// 字符串数据
        /// </summary>
        public Array MoneyList { get; set; }

        public string OwnerUserName { get; set; }

        public int? GroupId { get; set; }

        public int? RefType { get; set; }

        public int ? ResourcesId { get; set; }
        /// <summary>
        /// 账单Id
        /// </summary>
        public string ChargeBillId { get; set; }


    }
}
