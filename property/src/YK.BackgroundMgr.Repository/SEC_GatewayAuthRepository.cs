using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.Repository
{
    public partial class SEC_GatewayAuthRepository: BackgroundMgrRepository<SEC_GatewayAuth>, ISEC_GatewayAuthRepository
    {
        public SEC_GatewayAuthRepository(BackgroundMgrDataBaseContext context)
            : base(context)
        {
        }

        public override string TableName
        {
            get
            {
                return "SEC_GatewayAuth";
            }
        }
    }
}

