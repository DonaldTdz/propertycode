using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.CompositeDomainService.NoticeService
{
    public interface INoticeService
    {
        void SendNoticeMsg(NoticeMsg msg);
    }
}
