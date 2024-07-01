using RepositoriesLayer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DataBase
{
    public class DbLogic : IDbLogic
    {
        public void DbInitialization()
        {
            DB.DbInitialization("");
        }
    }
}
