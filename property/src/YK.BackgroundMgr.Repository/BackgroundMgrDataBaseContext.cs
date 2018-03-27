using KW.Sprite.Common.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace YK.BackgroundMgr.Repository
{
    public class BackgroundMgrDataBaseContext : DataBaseContext
    {
        //public FrameworkDataBaseContext()
        //    : base(new SqlConnection(DataEncrypt.DecryptFromDB(ConfigurationManager.ConnectionStrings["FrameworkConnection"].ToString())),
        //        false, new FrameworkMapperCollection())
        //{
        //    Database.SetInitializer(new FrameworkDatabaseInitializer());
        //    Configuration.LazyLoadingEnabled = true;
        //}

        public BackgroundMgrDataBaseContext()
            : base("BackgroundMgrConnection", new BackgroundMgrMapperCollection())
        {
            Database.SetInitializer(new BackgroundMgrDatabaseInitializer());
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
