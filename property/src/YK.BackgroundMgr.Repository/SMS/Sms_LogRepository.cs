using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.RepositoryContract;

namespace YK.BackgroundMgr.Repository
{
    public partial class Sms_LogRepository : BackgroundMgrRepository<Sms_Log>, ISms_LogRepository
    {
        public Sms_LogRepository(BackgroundMgrDataBaseContext context)
            : base(context)
        {
        }

        public override string TableName
        {
            get
            {
                return "Sms_Log";
            }
        }
    }
}

