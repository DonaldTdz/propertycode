using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.PresentationService;

namespace YK.PropertyMgr.ApplicationDTO
{
    public partial class PrepayAccountShowDTO:BaseSearchDTO
    {
        public  EDeptType DeptType { get; set; }
    }
}
