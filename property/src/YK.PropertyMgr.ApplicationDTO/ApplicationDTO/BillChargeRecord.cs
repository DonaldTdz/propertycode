using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class BillChargeRecord
    {


        public string Id { get; set; }

        /// <summary>
        /// 房屋DeptID
        /// </summary>
        public int? HouseDeptId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public int? ResourcesId { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public int? RefType { get; set; }


        /// <summary>
        /// 小区DeptId
        /// </summary>
        public int? ComDeptId { get; set; }
        /// <summary>
        /// 交易项描述
        /// </summary>
        public string TransactionDesc { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Amount { get; set; }
        /// <summary>
        /// 交易日期
        /// </summary>
        public DateTime? PayDate { get; set; }

        /// <summary>
        /// 账单开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 账单结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 票据号
        /// </summary>
        public string ReceiptNum { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 收费类型
        /// </summary>
        public int? ChargeType { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public int? PayType { get; set; }

        public string Remark { get; set; }
    }
}
