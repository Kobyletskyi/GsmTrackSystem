using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLayer.DataBase
{
    public interface IDB
    {
        void DbInitialization(string connectionString);
    }
}
