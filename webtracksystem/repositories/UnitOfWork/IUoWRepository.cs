using System.Data;
using Repositories.Dto;

namespace Repositories.UnitOfWork
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