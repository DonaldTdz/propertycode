using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.Repository
{
    public partial class SEC_AdminUserRepository: BackgroundMgrRepository<SEC_AdminUser>, ISEC_AdminUserRepository
    {
        public SEC_AdminUserRepository(BackgroundMgrDataBaseContext context)
            : base(context)
        {
        }

        public override string TableName
        {
            get
            {
                return "SEC_AdminUser";
            }
        }
    }
}

