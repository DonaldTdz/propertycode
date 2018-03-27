using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class BaseImportBillInfo
    {
        /// <summary>
        /// 资源类型名称
        /// </summary>
        public string ResourceTypeName { get; set; }
        /// <summary>
        /// 资源编号
        /// </summary>
        public string ResourceNo { get; set; }
        /// <summary>
        /// 收费项目
        /// </summary>
        public string SubjectName { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? BeginDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 欠费金额
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 是否是开发商代缴
        /// 是 为开始代缴
        /// 不添加为 否
        /// </summary>
        public string IsDeveloper { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 行号
        /// </summary>
        public int RowNum { get; set; }
        /// <summary>
        /// 导入错误信息
        /// </summary>
        public string ErrorMsg { get; set; }

        public ImportDataType ImportType { get; set; }

        /// <summary>
        /// 收费类型
        /// </summary>
        public string ChargeTypeName { get; set; }

        public ChargeTypeEnum ChargeType
        {
            get
            {
                switch (ChargeTypeName)
                {
                    case "日常收费": return ChargeTypeEnum.DailyCharge;
                    case "临时收费": return ChargeTypeEnum.TemporaryCharge;
                    case "退款": return ChargeTypeEnum.Refund;
                    case "对外收费": return ChargeTypeEnum.ForeignCharge;
                    default:
                        break;
                }
                return ChargeTypeEnum.DailyCharge;
            }
        }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? PayDate { get; set; }
        /// <summary>
        /// 票据号
        /// </summary>
        public string ReceiptNo { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// 付款方式
        /// </summary>
        public string PayTypeName { get; set; }

        public PayTypeEnum PayType
        {
            get
            {
                switch (PayTypeName)
                {
                    case "支付宝": return PayTypeEnum.Alipay;
                    case "微信": return PayTypeEnum.WeChat;
                    case "现金": return PayTypeEnum.Cash;
                    default:
                        break;
                }
                return PayTypeEnum.Cash;
            }
        }
    }

    public enum ImportDataType
    {
        /// <summary>
        /// 欠费
        /// </summary>
        Arrearage = 1,
        /// <summary>
        /// 历史缴费
        /// </summary>
        HistoryCost = 2
    }
}
