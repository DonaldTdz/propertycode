using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using YK.BackgroundMgr.PresentationService;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class ReportMeterExportModel
    {
        public DataTable ExportData { get; set; }

        public IEnumerable<TemplateModel> TemPlateList { get; set; }
    }
}
