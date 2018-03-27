using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class PreAccountORSearchDTO : BaseSearchDTO
    {
        /// <summary>
        /// 内容 描述
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 操作开始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 操作结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}
