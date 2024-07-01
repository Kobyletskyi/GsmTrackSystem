using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLayer.DataBase
{
    public static class DB
    {
        public static void DbInitialization(string connectionString)
        {
            Database.SetInitializer(new MyDbContextInitializerIfChanges());
            //Database.SetInitializer(new MyDbContextInitializerIfNotExists());
            //Database.SetInitializer<DbScheme>(new MyDbContextInitializerAlways());
        }
    }
}
