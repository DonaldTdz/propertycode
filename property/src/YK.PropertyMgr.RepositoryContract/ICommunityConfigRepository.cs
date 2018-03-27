using KW.Sprite.Common.Repository;
using YK.PropertyMgr.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.RepositoryContract
{
    public partial interface ICommunityConfigRepository : IRepository<CommunityConfig> ,IPaging<CommunityConfig>
    {
    }
}
