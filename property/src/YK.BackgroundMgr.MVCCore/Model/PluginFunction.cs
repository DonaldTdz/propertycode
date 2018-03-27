using System.Runtime.Serialization;
using System.Collections.Generic;

namespace YK.BackgroundMgr.MVCCore
{
    public class PluginFunction
    {
        /// <summary>
        /// 功能名
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string Action { get; set; }
    }
}
