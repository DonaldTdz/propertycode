using KW.Sprite.Common.Repository;
using YK.ParkingSys.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.ParkingSys.Repository
{
    public class ParkingSysMapperCollection : IMapperCollection
    {
        public IEnumerable<IEntityMapper> Mappers { get; private set; }

        public ParkingSysMapperCollection()
        {
            Mappers = new List<IEntityMapper>
            {
                new ParkingMapper(),
                new CarportMapper(),
            };
        }
    }
}
