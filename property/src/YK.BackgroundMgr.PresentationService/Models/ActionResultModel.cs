using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.PresentationService
{
    /// <summary>
    /// Action操作返回信息
    /// </summary>
    public class ActionResultModel
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ActionInfo { get; set; }

        /// <summary>
        /// 其他需要返回的信息
        /// </summary>
        public string DataInfo { get; set; }
    }
}
