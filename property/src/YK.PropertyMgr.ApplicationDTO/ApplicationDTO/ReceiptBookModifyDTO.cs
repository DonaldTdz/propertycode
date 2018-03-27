using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.ApplicationDTO.ApplicationDTO
{
 public    class ReceiptBookModifyDTO
    {
        public int ReceiptBookType { get; set; }
        public string OldReceiptNum { get; set; }
        public string NewReceiptNum { get; set; }
        public string Remark { get; set; }
        public int ComDeptId { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyOperator { get; set; }
  
        public bool IsModify { get; set; }

        public bool IsExChange { get; set; }

        public bool IsCurrent { get; set; }



    }
}
