using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.PropertyMgr.CompositeDomainService.NoticeService;

namespace YK.PropertyMgr.CompositeAppService.NoticeService
{
    public static class NoticeTaskAppService
    {
        public static void FullPointRun()
        {
            NoticeTaskService.Instance.FullPointRun();
        }
    }
}
