using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.ParkingSys.Repository
{
    public abstract class ParkingSysRepository<TAggregateRoot> : Repository<ParkingSysDataBaseContext, TAggregateRoot>
     where TAggregateRoot : class, IAggregateRoot
    {
        protected ParkingSysRepository(ParkingSysDataBaseContext context)
            : base(context)
        {
        }

        public abstract override string TableName
        {
            get;
        }
    }
}
