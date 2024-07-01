
using Repositories.Dto;

namespace Repositories.UnitOfWork
{
    public interface IUnitOfWork<TEntity, TKey>
        where TKey : struct
        where TEntity : class, IEntity<TKey>
    {

        void PutNew(TEntity entity, IUoWRepository<TEntity, TKey> repository);
        void PutUpdated(TEntity entity, IUoWRepository<TEntity, TKey> repository);
        void PutDeleted(TEntity entity, IUoWRepository<TEntity, TKey> repository);

        //IDbConnection DbConnection { get; }

        void SaveChanges();
    }
}
