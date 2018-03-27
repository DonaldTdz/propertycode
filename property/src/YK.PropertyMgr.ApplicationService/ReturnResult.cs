using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationService
{
    public class ReturnResult
    {
        public bool IsSuccess { get; set; }
        public bool IsResult { get; set; }//兼容旧接口 2017-05-23
        public string Msg { get; set; }
        public object Data { get; set; }
    }
}
