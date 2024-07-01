using DbModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLayer.Repositories
{
    /// <summary>
    /// Repository that works with entities, TEntity type, and have TKey idetification field
    /// </summary>
    /// <typeparam name="TEntity">Entity type, should be reference type and be inherited from 'IEntity'</typeparam>
    /// <typeparam name="TKey">Idetification field type, should be inherited from 'struct'</typeparam>
    public interface IRepository<TEntity, TKey>
        where TKey : struct
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Add entity to Database
        /// </summary>
        /// <param name="entity">Entity to add</param>
        void Add(TEntity entity);
        /// <summary>
        /// Add entities to Database
        /// </summary>
        /// <param name="entities">Entities to add</param>
        void Add(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update entity in Database
        /// </summary>
        /// <param name="entity">Entity to update</param>
        void Update(TEntity entity);
        /// <summary>
        /// Update entities in Database
        /// </summary>
        /// <param name="entities">Entities to update</param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// Remove entity from Database
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        void Remove(TEntity entity);
        /// <summary>
        /// Remove entites from Database
        /// </summary>
        /// <param name="entities">Entities to remove</param>
        void Remove(IEnumerable<TEntity> entities);
        /// <summary>
        /// Remove entity from Database by identificator
        /// </summary>
        /// <param name="key">Identitfication key</param>
        void Remove(TKey key);
        /// <summary>
        /// Remove entities from Database by identificators
        /// </summary>
        /// <param name="keys">Identitfication keys</param>
        void Remove(IEnumerable<TKey> keys);

        /// <summary>
        /// Get single entity from Database by identificator
        /// </summary>
        /// <param name="key">Identification key</param>
        /// <returns>Entity or null if not found</returns>
        TEntity FindByKey(TKey key);

        /// <summary>
        /// Get single entity from Database by composite primary keys
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        TEntity FindByKeys(params Object[] keyValues);
        /// <summary>
        /// Get single entity from Database by condition
        /// </summary>
        /// <param name="expFunc">Search condition</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">If more than one entity corresponds to the condition</exception>
        TEntity FindSingle(Expression<Func<TEntity, bool>> expFunc);
        /// <summary>
        /// Get entities from Database by condition
        /// </summary>
        /// <param name="expFunc">Search condition</param>
        /// <returns>Set of enties</returns>
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expFunc);
        /// <summary>
        /// Get al entities
        /// </summary>
        /// <returns>Set of enties</returns>
        IQueryable<TEntity> FindAll();

        /// <summary>
        /// Get count of entities which corresponds to the condition
        /// </summary>
        /// <param name="expFunc">Search condition</param>
        /// <returns>Count number</returns>
        int Count(Expression<Func<TEntity, bool>> expFunc);
        /// <summary>
        /// Get count of all entities
        /// </summary>
        /// <returns>Count number</returns>
        int CountAll();

        //bool Exist(Expression<Func<TEntity, bool>> predicate = null);
        //bool ExistByKey(TKey key);

    }
}
