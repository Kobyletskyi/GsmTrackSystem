using DbModels.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLayer.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity, int>
        where TEntity : class, IEntity<int>, new()
    {
        public Repository(DbContext context)
        {
            _dbContext = context;
            _DbSet = _dbContext.Set<TEntity>();
        }

        #region IRepository implementation

        public void Add(TEntity entity)
        {
            //entity.Id = Guid.NewGuid();
            //entity.CreatedUtcDate = DateTime.Now.ToUniversalTime();
            //entity.UpdatedUtcDate = DateTime.Now.ToUniversalTime();
            _DbSet.Add(entity);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                //entity.Id = Guid.NewGuid();
                //entity.CreatedUtcDate = DateTime.Now.ToUniversalTime();
                //entity.UpdatedUtcDate = DateTime.Now.ToUniversalTime();
                _DbSet.Add(entity);
            }
        }

        public void Update(TEntity entity)
        {
            //entity.UpdatedUtcDate = DateTime.Now.ToUniversalTime();
            _DbSet.Attach(entity);
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                //entity.UpdatedUtcDate = DateTime.Now.ToUniversalTime();
                _DbSet.Attach(entity);
            }
        }

        public void Remove(TEntity entity)
        {
            _DbSet.Remove(entity);
        }

        public void Remove(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Remove(entity);
            }
        }

        public void Remove(int key)
        {
            var entity = FindByKey(key);
            Remove(entity);
        }

        public void Remove(IEnumerable<int> keys)
        {
            foreach (var key in keys)
            {
                Remove(key);
            }
        }

        public TEntity FindByKey(int key)
        {
            return _DbSet.Find(key);
        }

        public TEntity FindByKeys(params Object[] keyValues)
        {
            return _DbSet.Find(keyValues);
        }

        public TEntity FindSingle(Expression<Func<TEntity, bool>> predicate)
        {
            return _DbSet.SingleOrDefault(predicate);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _DbSet.Where(predicate);
        }

        public IQueryable<TEntity> FindAll()
        {
            return _DbSet;
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _DbSet.Where(predicate).Count();
        }

        public int CountAll()
        {
            return _DbSet.Count();
        }

        #endregion

        protected DbSet<TEntity> _DbSet;
        protected DbContext _dbContext;
    }
}
