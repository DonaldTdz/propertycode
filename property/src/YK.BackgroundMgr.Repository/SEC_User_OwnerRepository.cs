using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.Repository
{
    public partial class SEC_User_OwnerRepository: BackgroundMgrRepository<SEC_User_Owner>, ISEC_User_OwnerRepository
    {
        public SEC_User_OwnerRepository(BackgroundMgrDataBaseContext context)
            : base(context)
        {
        }

        public override string TableName
        {
            get
            {
                return "SEC_User_Owner";
            }
        }
    }
}

