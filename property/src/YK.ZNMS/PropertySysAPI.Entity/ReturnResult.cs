using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertySysAPI.Entity
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class ReturnResult
    {
        public bool IsResult { get; set; }
        public string Msg { get; set; }
        public object Data { get; set; }
    }
}
