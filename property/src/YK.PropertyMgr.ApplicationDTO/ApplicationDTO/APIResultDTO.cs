using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    [Serializable]
    public class APIResultDTO
    {
        /// <summary>
        /// 返回code代码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 返回的描述信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 返回的数据
        /// </summary>
        public object Data { get; set; }
    }
}
