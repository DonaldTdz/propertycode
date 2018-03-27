using KW.Sprite.Common.Repository;
using YK.ParkingSys.DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.ParkingSys.RepositoryContract
{
    public partial interface IParkingRepository : IRepository<Parking>
    {
    }
}
