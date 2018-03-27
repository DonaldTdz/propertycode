using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.PresentationService;

namespace YK.PropertyMgr.ApplicationDTO
{
    public class DailyChargBillDTO
    {
        public DailyChargBillDTO()
        {
            IsPreDeductible = true;
        }

        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal AmountShouldTotal { get; set; }

        /// <summary>
        /// 应收所有收费项目合计金额
        /// </summary>
        public decimal AmountShouldAllTotal { get; set; }

        /// <summary>
        /// 票据号·
        /// </summary>
        public string ReceiptNum { get; set; }

        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal ReceivedAmountTotal { get; set; }


        /// <summary>
        /// 付款方式
        /// </summary>
        public int PayTypeId { get; set; }

        /// <summary>
        /// 找零
        /// </summary>
        public string SmallChange { get; set; }


        /// <summary>
        /// 找零存入预付款
        /// </summary>
        public bool IsSmallToPrepay { get; set; }

        /// <summary>
        /// 找零预存到收费项目的ID
        /// </summary>
        public int SmallToPrepaySubjectId { get; set; }

        /// <summary>
        /// 显示开发商代缴项
        /// </summary>
        public bool IsShowDevPay { get; set; }

        /// <summary>
        /// 选取账单Id
        /// </summary>
        public string IdStr { get; set; }

        /// <summary>
        /// 保存的账单ID集合
        /// </summary>
        public string[] BillIds { get; set; }

        /// <summary>
        /// 新增账单
        /// </summary>
        public IList<ChargBillDTO> NewBillList { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public List<DictionaryModel> PayTypeList { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 收费项目Id 
        /// </summary>
        public int ChargeSubjectId { get; set; }

        /// <summary>
        /// 房屋DeptId
        /// </summary>
        public int HouseDeptId { get; set; }


        public int ResourcesId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RefType { get; set; }

        /// <summary>
        /// 收费对象
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 预存抵扣金额 v2.9
        /// </summary>
        public decimal PreDeductibleAmount { get; set; }

        /// <summary>
        /// 是否要预存抵扣 v2.9
        /// </summary>
        public bool IsPreDeductible { get; set; }

        /// <summary>
        /// 预存抵扣账户信息 v2.9
        /// </summary>
        public string PreAmountInfo { get; set; }

        /// <summary>
        /// 是否打印票据 v2.9
        /// </summary>
        public bool IsPrintReceipt { get; set; }

        /// <summary>
        /// 打印的票据号 v2.9
        /// </summary>
        public string ReceiptNo { get; set; }

    }
}
