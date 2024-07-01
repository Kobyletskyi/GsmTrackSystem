using DbModels.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLayer.UnitOfWork
{
    public interface IUoWRepository<TEntity, TKey>
        where TKey : struct
        where TEntity : class, IEntity<TKey>
    {
        void RunCreation(TEntity entity, IDbTransaction transaction);
        void RunUpdate(TEntity entity, IDbTransaction transaction);
        void RunDeletion(TEntity entity, IDbTransaction transaction);
    }
}
