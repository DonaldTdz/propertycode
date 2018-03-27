using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.CompositeDomainService.NoticeService
{
    public abstract class NoticeDecorator : INoticeService
    {
        protected INoticeService _service;

        public NoticeDecorator(INoticeService _service)
        {
            this._service = _service;
        }
        public NoticeDecorator()
        {
        }

        public abstract void SendNoticeMsg(NoticeMsg msg);
    }
}
