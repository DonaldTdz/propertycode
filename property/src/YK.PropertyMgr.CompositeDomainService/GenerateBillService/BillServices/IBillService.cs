using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.ApplicationDTO;

namespace YK.PropertyMgr.CompositeDomainService.GenerateBillService.BillServices
{
    public interface IBillService
    {
        SubjectTypeEnum SubjectType { get; }

        void GenerateBills();
    }
}
