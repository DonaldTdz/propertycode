using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
   public class MeterBindSubjectHouse
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 仪表编号名称
        /// </summary>
        public string MeterNum { get; set; }
        /// <summary>
        /// 开发商代缴
        /// </summary>
        public bool? IsDevPay { get; set; }

        /// <summary>
        /// 开发商代缴开始时间
        /// </summary>
        public DateTime? DevBeginDate { get; set; }

        /// <summary>
        /// 开发商代缴结束时间
        /// </summary>
        public DateTime? DevEndDate { get; set; }

        public string MeterText
        {
            get
            {
                StringBuilder strMetertime = new StringBuilder();
                strMetertime.Append("<span class='treeLabel' onclick='fnTreeLabel(event,this)'>" + MeterNum + "</span>");
                strMetertime.Append(string.Format("{0}_{1}", string.Format("{0:yyyy-MM-dd}", DevBeginDate), string.Format("{0:yyyy-MM-dd}", DevEndDate)));
                return strMetertime.ToString();
            }
        }

    }
}
