using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.CompositeDomainService.NoticeService
{
    public class NoticeMsg
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string[] UserPhones { get; set; }

        public int? ComDeptId { get; set; }

        public string ComDeptName { get; set; }

        public NoticeTypeEnum NoticeType { get; set; }
    }

    public enum NoticeTypeEnum
    {
        /// <summary>
        /// 欠费
        /// </summary>
        Arrears = 1
    }
}
