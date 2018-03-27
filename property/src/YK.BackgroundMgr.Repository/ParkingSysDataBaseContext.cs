using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace YK.ParkingSys.Repository
{
    public class ParkingSysDataBaseContext : DataBaseContext
    {
        public ParkingSysDataBaseContext()
            : base("ParkingSysConnection", new ParkingSysMapperCollection())
        {
            //Database.SetInitializer(new ParkingSysDatabaseInitializer());
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

    }
}
