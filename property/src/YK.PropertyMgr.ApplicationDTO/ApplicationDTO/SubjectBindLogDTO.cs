using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class SubjectBindLogDTO
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public string OperateName { get; set; }
        /// <summary>
        /// 绑定解绑
        /// </summary>
        public bool? IsDel { get; set; }

        public int? Operator { get; set; }

        public int? RelieveOperator { get; set; }
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

        public DateTime? OperateTime { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public string OperateTimeFormat
        {
            get
            {
                if (OperateTime.HasValue)
                {
                    return OperateTime.Value.ToString("yyyy-MM-dd HH:mm");
                }
                return string.Empty;
            }
        }
    }
}
