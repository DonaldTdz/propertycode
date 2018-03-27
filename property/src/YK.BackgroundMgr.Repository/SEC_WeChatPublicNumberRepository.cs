using YK.BackgroundMgr.DomainEntity;
using YK.BackgroundMgr.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.Repository
{
    public partial class SEC_WeChatPublicNumberRepository: BackgroundMgrRepository<SEC_WeChatPublicNumber>, ISEC_WeChatPublicNumberRepository
    {
        public SEC_WeChatPublicNumberRepository(BackgroundMgrDataBaseContext context)
            : base(context)
        {
        }

        public override string TableName
        {
            get
            {
                return "SEC_WeChatPublicNumber";
            }
        }
    }
}

