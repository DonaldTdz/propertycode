using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.ParkingSys.Repository
{
    public class ParkingSysDatabaseInitializer : CreateDatabaseIfNotExists<ParkingSysDataBaseContext>
    {
        protected override void Seed(ParkingSysDataBaseContext context)
        {
            base.Seed(context);
            //InitFrameWorkVersion(context);
        }
    }
}
