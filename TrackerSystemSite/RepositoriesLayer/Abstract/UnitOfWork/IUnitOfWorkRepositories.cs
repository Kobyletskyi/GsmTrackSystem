using RepositoriesLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLayer.UnitOfWork
{
    public interface IUnitOfWorkRepositories : IRepositories
    {
        void SaveChanges();
    }
}
