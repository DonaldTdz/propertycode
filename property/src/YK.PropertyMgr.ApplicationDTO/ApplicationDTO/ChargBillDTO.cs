using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO.Enums;

namespace YK.PropertyMgr.ApplicationDTO
{
    public partial class ChargBillDTO
    {
        public ChargBillDTO()
        {
            RowType = RowTypeEnum.ChildRow;
        }
        /// <summary>
        /// 科目名称
        /// </summary>
        public string ChargeSubjectName { get; set; }

        /// <summary>
        /// 小区名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 房屋编号
        /// </summary>
        //public string HouseDoorNo { get; set; }


        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal AmountShould
        {
            get
            {
                var bAmount = BillAmount.HasValue ? BillAmount.Value : 0;
                var rAmount = ReceivedAmount.HasValue ? ReceivedAmount.Value : 0;
                var rfAmount = ReliefAmount.HasValue ? ReliefAmount.Value : 0;
                var pAmount = PenaltyAmount.HasValue ? PenaltyAmount.Value : 0;
                //应收金额=计费金额+滞纳金-已交金额-减免金额
                return Math.Round(bAmount + pAmount - rAmount - rfAmount, 2);
            }
        }

        /// <summary>
        /// 收费周期
        /// </summary>
        public int? BillPeriod { get; set; }
        /// <summary>
        /// 科目类别
        /// </summary>
        public int? SubjectType { get; set; }

        /// <summary>
        /// 预交几月
        /// </summary>
        public double? Months { get; set; }

        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// 是否是页面临时账单
        /// </summary>
        public ActionStatusEnum ActionStatus { get; set; }

        public string BeginDateFormat
        {
            get
            {
                return this.BeginDate.HasValue ? this.BeginDate.Value.ToString("yyyy-MM-dd") : "";
            }
        }
        public string EndDateFormat
        {
            get
            {
                return this.EndDate.HasValue ? this.EndDate.Value.ToString("yyyy-MM-dd") : "";
            }
        }

        DateTime? _splitDate;

        public DateTime? SplitDate
        {
            get
            {
                if (!_splitDate.HasValue)
                {
                    return this.EndDate;
                }
                return _splitDate;
            }
            set
            {
                _splitDate = value;
            }
        }

        public int? ChargeSubjectId { get; set; }
        /// <summary>
        /// 预存收费项目ID
        /// </summary>
        public int? PreChargeSubjectId { get; set; }

        /// <summary>
        /// 引用Id 拆分账单使用
        /// </summary>
        public string RefId { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// 分组ID
        /// </summary>
        public string GroupId { get; set; }
        /// <summary>
        /// 分组类型()
        /// </summary>
        public RowTypeEnum RowType { get; set; }
        /// <summary>
        /// 分组数据主数据+ - 样式
        /// </summary>
        public string CccordionClass { get; set; }

        /// <summary>
        /// 客户名称·
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 预存费可抵扣金额
        /// </summary>
        public decimal PreAmount { get; set; }


    }
}
