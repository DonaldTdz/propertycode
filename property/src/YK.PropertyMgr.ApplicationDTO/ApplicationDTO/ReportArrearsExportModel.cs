﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.PresentationService;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    public class ReportArrearsExportModel
    {
     public DataTable ExportData { get; set; }

     public IEnumerable<TemplateModel> TemPlateList { get; set; }


    }
}
