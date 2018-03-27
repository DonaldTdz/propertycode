using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YK.PropertyMgr.Repository
{
    internal sealed class PropertyConfiguration : DbMigrationsConfiguration<PropertyMgrDataBaseContext>
    {
        /*
        * 数据库迁移配置
        */
        public PropertyConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        /*
         *数据库种子 
         */
        protected override void Seed(PropertyMgrDataBaseContext context)
        {

        }
    }
}
