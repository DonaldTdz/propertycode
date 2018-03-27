using YK.ParkingSys.DomainEntity;
using YK.ParkingSys.RepositoryContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.ParkingSys.Repository
{
    public partial class ParkingRepository: ParkingSysRepository<Parking>, IParkingRepository
    {
        public ParkingRepository(ParkingSysDataBaseContext context)
            : base(context)
        {
        }

        public override string TableName
        {
            get
            {
                return "Parking";
            }
        }
    }
}

