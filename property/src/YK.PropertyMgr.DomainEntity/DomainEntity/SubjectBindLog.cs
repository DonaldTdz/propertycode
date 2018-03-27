using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.DomainEntity.DomainEntity
{
    public class SubjectBindLog
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public string OperateName { get; set; }
        /// <summary>
        /// 资源
        /// </summary>
        public string ResourceName { get; set; }
        /// <summary>
        /// 绑定科目
        /// </summary>
        public string SubjectName { get; set; }
        /// <summary>
        /// 操作
        /// </summary>
        public string OperateTypeName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperateTime { get; set; }
    }
}
