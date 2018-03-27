using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;
using YK.PropertyMgr.DomainEntity;

namespace YK.PropertyMgr.CompositeDomainService.Model
{
    /// <summary>
    /// 支付参数列表
    /// </summary>
    public class PaymentModel
    {
        public PaymentModel()
        {
            OperatorName = "";
            CustomerName = "";
            Remark = "";
        }
        /// <summary>
        /// 需要付款的已存在账单Id集合
        /// </summary>
        public string[] BillIDs { get; set; }
        /// <summary>
        /// 需要付款的新账单集合
        /// </summary>
        public IList<ChargBill> NewBillList { get; set; }
        /// <summary>
        /// 需要付款的更新账单集合
        /// </summary>
        public IList<ChargBill> UpdateBillList { get; set; }
        /// <summary>
        /// 缴费金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 是否需要验证票据号
        /// </summary>
        public bool IsCheckReceiptNum { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public PayTypeEnum PayType { get; set; }
        /// <summary>
        /// 收费类型
        /// </summary>
        public ChargeTypeEnum ChargeType { get; set; }
        /// <summary>
        /// 是否找零预存
        /// </summary>
        public bool IsChangeStore { get; set; }
        /// <summary>
        /// 零钱转存的收费项目Id
        /// </summary>
        public int SmallToPrepaySubjectId { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public int Operator { get; set; }
        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 未来账单列表
        /// </summary>
        public List<ChargBill> OnlyFutureBillList { get; set; }
        /// <summary>
        /// 是否是线上支付（APP支付）
        /// </summary>
        public bool IsOnline { get; set; }
        /// <summary>
        /// 优惠信息
        /// </summary>
        public PaymentDiscountInfo DiscountInfo { get; set; }

        /// <summary>
        /// 预存抵扣金额 v2.9
        /// </summary>
        public decimal PreDeductibleAmount { get; set; }

        /// <summary>
        /// 是否要预存抵扣 v2.9
        /// </summary>
        public bool IsPreDeductible { get; set; }

        /// <summary>
        /// 是否打印票据 v2.9
        /// </summary>
        public bool? IsPrintReceipt { get; set; }

        /// <summary>
        /// 打印的票据号 v2.9
        /// </summary>
        public string ReceiptNo { get; set; }
    }
}
