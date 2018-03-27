using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.Resources
{
    public class TemplatePrintFormDetail
    {
        /// <summary>
        /// 科目名称
        /// </summary>
        public string ChargeSubjectName { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string SummaryStr { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public  string Amount { get; set; }

        public object GetValue(string propertyName)
        {
            return this.GetType().GetProperty(propertyName).GetValue(this, null);
        }

    }
}
