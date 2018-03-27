using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.BackgroundMgr.Repository
{

    public abstract class BackgroundMgrRepository<TAggregateRoot> : Repository<BackgroundMgrDataBaseContext, TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        protected BackgroundMgrRepository(BackgroundMgrDataBaseContext context)
            : base(context)
        {
        }

        public abstract override string TableName
        {
            get;
        }
    }
}
