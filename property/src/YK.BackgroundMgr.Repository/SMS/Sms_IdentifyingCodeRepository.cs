using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.RepositoryContract;

namespace YK.BackgroundMgr.Repository
{
    public partial class Sms_IdentifyingCodeRepository : BackgroundMgrRepository<Sms_IdentifyingCode>, ISms_IdentifyingCodeRepository
    {
        public Sms_IdentifyingCodeRepository(BackgroundMgrDataBaseContext context)
            : base(context)
        {
        }

        public override string TableName
        {
            get
            {
                return "Sms_IdentifyingCode";
            }
        }
    }
}

