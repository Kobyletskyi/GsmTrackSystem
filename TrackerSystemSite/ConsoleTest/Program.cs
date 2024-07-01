using RepositoriesLayer.DataBase;
using RepositoriesLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DB.DbInitialization("");
            var repositories = new Repositories();
            //var result = repositories.Devices.FindAll().ToList();
        }
    }
}
