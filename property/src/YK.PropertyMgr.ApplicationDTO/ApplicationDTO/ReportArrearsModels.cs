using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
   public class ReportArrearsModels
    {

        public List<ReportHead> ReportHeadList { get; set; }

        public List<ReportArrearsDTO> ReportArrearsDTOList { get; set; }
        
        public ReportArrearsDTO  ReportArrearsSum{ get; set; }
        
     
    }
}
