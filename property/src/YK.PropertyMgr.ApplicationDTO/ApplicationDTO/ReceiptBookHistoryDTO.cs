using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO
{
   public partial class ReceiptBookHistoryDTO
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTimeStr
        {
            get
            {
                return CreateTime.Value.ToString("yyyy-MM-dd  HH:mm");
            }
        }
    }
}
