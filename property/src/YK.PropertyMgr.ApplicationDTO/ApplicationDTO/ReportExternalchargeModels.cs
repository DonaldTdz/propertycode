using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class ReportExternalchargeModels
    {
        public List<ReportHead> ReportHeadList { get; set; }
        
        public List<ReportExternalchargeDTO> ReportExternalchargeDTOList { get; set; }

        public ReportExternalchargeDTO ReportArrearsSum { get; set; }
    }
}
