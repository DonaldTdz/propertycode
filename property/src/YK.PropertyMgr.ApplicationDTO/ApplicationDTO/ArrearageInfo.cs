﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
    /// <summary>
    /// 欠费信息
    /// </summary>
    [Serializable]
    public class ArrearageInfo : BaseImportBillInfo
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
    }
}
