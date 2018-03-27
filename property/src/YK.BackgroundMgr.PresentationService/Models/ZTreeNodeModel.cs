using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.PresentationService
{
    public class ZTreeNodeModel: BaseTreeNodeModel
    {
         public  string name { get; set; }
         public int? pId { get; set; }
         public bool isParent { get; set; }
         public bool open { get; set; }
    }
}
