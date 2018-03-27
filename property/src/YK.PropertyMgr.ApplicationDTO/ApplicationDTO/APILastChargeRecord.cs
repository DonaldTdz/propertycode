using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    [Serializable]
    public class APILastChargeRecord
    {
        public string Id { get; set; }

        public DateTime? LastTime { get; set; }

        public decimal? Amount { get; set; }

    }
}
