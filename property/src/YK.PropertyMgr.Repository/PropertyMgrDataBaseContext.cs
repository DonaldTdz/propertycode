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

namespace YK.PropertyMgr.Repository
{
    public class PropertyMgrDataBaseContext : DataBaseContext
    {
        public PropertyMgrDataBaseContext()
            : base(new SqlConnection(ConfigurationManager.ConnectionStrings["PropertyMgrConnection"].ToString()),
                false, new PropertyMgrMapperCollection())
        {
            //Database.SetInitializer(new PropertyMgrDatabaseInitializer());
            //Configuration.LazyLoadingEnabled = true;

            ///获得数据库最后一个版本
            Database.SetInitializer<PropertyMgrDataBaseContext>(new MigrateDatabaseToLatestVersion<PropertyMgrDataBaseContext, PropertyConfiguration>());

            ///删除原来数据库 重新创建数据库
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ContextHelper>());
            // Database.SetInitializer<ContextHelper>(new DropCreateDatabaseIfModelChanges<ContextHelper>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}
