using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public partial class ChargeRecordDTO
    {
        public ChargeRecordDTO()
        {
            PageType = RecordPageType.None;
            ValidationType = CRValidationEnum.None;
        }

        /// <summary>
        /// 票据号
        /// </summary>
        public string ReceiptNum { get; set; }
        /// <summary>
        ///// 收费类型名称
        ///// </summary>
        //public string ChargeTypeName { get; set; }
        ///// <summary>
        ///// 支付方式名称
        ///// </summary>
        //public string PayTypeName { get; set; }
        ///// <summary>
        /// 状态名称
        /// </summary>
        public int ReceiptStatus { get; set; }
        /// <summary>
        /// 房屋编号
        /// </summary>
        public string HouseDoorNo { get; set; }

        public string HouseDoorNoFormat { get; set; }


        public string ResourcesNamesFormat { get; set; }

        public string ReceiptId { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string ChargeSubjectName { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        public string ComDeptName { get; set; }

        public RecordPageType PageType { get; set; }

        /// <summary>
        /// 退款原因
        /// 
        /// </summary>
        public string RefundReason { get; set; }

        public CRValidationEnum ValidationType { get; set;}

        public IList<PaymentDiscountInfoDTO> PaymentDiscountDTOList { get; set; }

        public string PayTypeName { get; set; }
    }

    public enum RecordPageType
    {
        None = 0,
        BillDetail = 1//账单详情
    }

    public enum CRValidationEnum
    {
        None = 0,
        Update = 1
    }
}
