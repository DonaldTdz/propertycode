using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
   public class ReportMeterModels
    {
        public List<ReportHead> ReportHeadList { get; set; }

        public List<ReportMeterDTO> ReportMeterDTOList { get; set; }

        public ReportMeterDTO ReportArrearsSum { get; set; }
    }
}
