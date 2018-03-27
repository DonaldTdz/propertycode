using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
    public partial class PrepayAccountLogDTO
    {
        public string OperationTimeDesc
        {
            get
            {
                if (OperationTime.HasValue)
                {
                    return OperationTime.Value.ToString("yyyy-MM-dd HH:mm");
                }
                return string.Empty;
            }
        }
    }
}
