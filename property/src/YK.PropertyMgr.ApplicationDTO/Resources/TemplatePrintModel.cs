using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.Resources
{
    public  class TemplatePrintModel
    {



      
        /// <summary>
        /// 社区名称/项目名称
        /// </summary>
        public string CommunityName { get; set; }

        /// <summary>
        /// 资源编号/房间号/车位号
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 业主姓名
        /// </summary>
        public string OwnerName { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public string YearCode { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public string MonthCode { get; set; }

        /// <summary>
        /// 日
        /// </summary>
        public string DayCode { get; set; }

        /// <summary>
        /// 合计金额 人民币大写
        /// </summary>
        public string TotalAmountCN { get; set; }

        /// <summary>
        /// 合计金额 
        /// </summary>
        public string TotalAmount{ get; set; }

        /// <summary>
        /// 交款操作人
        /// </summary>
        public string OperatorName { get; set; }

        /// <summary>
        /// 表格对象集合
        /// </summary>
        public List<TemplatePrintFormDetail> FormList { get; set; }



        public object GetValue(string propertyName)
        {
            return this.GetType().GetProperty(propertyName).GetValue(this, null);
        }

    }
}
