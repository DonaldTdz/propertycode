using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    [Serializable]
    public class HistoryCostInfo: BaseImportBillInfo
    {
        public string BeginDateFormat
        {
            get
            {
                return BeginDate.HasValue ? BeginDate.Value.ToString("yyyy-MM-dd") : "";
            }
        }

        public string EndDateFormat
        {
            get
            {
                return EndDate.HasValue ? EndDate.Value.ToString("yyyy-MM-dd") : "";
            }
        }

        public string PayDateFormat
        {
            get
            {
                return PayDate.HasValue ? PayDate.Value.ToString("yyyy-MM-dd") : "";
            }
        }
    }
}
