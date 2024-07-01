using DbModels.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLayer.UnitOfWork
{
    public interface IUnitOfWork<TEntity, TKey>
        where TKey : struct
        where TEntity : class, IEntity<TKey>
    {

        void PutNew(TEntity entity, IUoWRepository<TEntity, TKey> repository);
        void PutUpdated(TEntity entity, IUoWRepository<TEntity, TKey> repository);
        void PutDeleted(TEntity entity, IUoWRepository<TEntity, TKey> repository);

        IDbConnection DbConnection { get; }

        void SaveChanges();
    }
}
