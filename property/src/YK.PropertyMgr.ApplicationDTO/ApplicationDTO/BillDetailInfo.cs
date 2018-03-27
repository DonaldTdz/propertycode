using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO.Enums;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class BillDetailInfo
    {
        /// <summary>
        /// 账单Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 收费记录Id
        /// </summary>
        public string ChargeRecordId { get; set; }

        public string ResourcesName { get; set; }
        /// <summary>
        /// 交易项
        /// </summary>
        public string BillDesc { get; set; }

        public string ReceiptNum { get; set; }

        public string CustomerName { get; set; }

        public string OperatorName { get; set; }

        public int? ChargeSubjectId { get; set; }

        public int? ChargeType { get; set; }

        public string ChargeTypeName { get; set; }

        public int? BillStatus { get; set; }

        public int? ReceiptStatus { get; set; }

        public string BillStatusName
        {
            get
            {
                if (ReceiptStatus.HasValue)
                {
                    switch ((ReceiptStatusEnum)ReceiptStatus)
                    {
                        case ReceiptStatusEnum.Refunded:
                            return "退款";
                    }
                }
                if (BillStatus.HasValue)
                {
                    switch ((BillStatusEnum)BillStatus)
                    {
                        case BillStatusEnum.NoPayment:
                            return "未付款";
                        case BillStatusEnum.Paid:
                            return "已付款";
                        case BillStatusEnum.Refunded:
                            return "退款";
                        default:
                            return "";
                    }
                }
                return "";
            }
        }

        public int? PayType { get; set; }

        public string PayTypeName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 收费日期
        /// </summary>
        public DateTime? ChargeDate { get; set; }
        /// <summary>
        /// 生成日期
        /// </summary>
        public DateTime? GenerationDate { get; set; }

        public decimal? BillAmount { get; set; }

        public decimal? Amount { get; set; }

        public string SerialNumber { get; set; }

        public int? AccountingStatus { get; set; }

        public string AccountingStatusName { get; set; }

        public string RefundReason { get; set; }

        public string Remark { get; set; }

        public int? DeptId { get; set; }
    }
}
