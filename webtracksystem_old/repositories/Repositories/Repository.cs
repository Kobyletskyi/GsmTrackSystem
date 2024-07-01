using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Repositories.DataBase;
using Repositories.Dto;

namespace Repositories
{
    public class Repository<TEntity> : IRepository<TEntity, int>
        where TEntity : class, IEntity<int>, new()
    {
        public Repository(DbScheme context)
        {
            _dbContext = context;
            _DbSet = _dbContext.Set<TEntity>();
        }

        #region IRepository implementation

        public void Add(TEntity entity)
        {
            //entity.Id = Guid.NewGuid();
            // var now = DateTime.UtcNow;
            // entity.CreatedUtcDate = now;
            // entity.UpdatedUtcDate = now;
            _DbSet.Add(entity);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Add(entity);
            }
        }

        public void Update(TEntity entity)
        {
            //entity.UpdatedUtcDate = DateTime.UtcNow;
            _DbSet.Attach(entity);
        }

        public bool ExistByKey(int key){
            return _DbSet.Count(e => e.Id == key) > 0;
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
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

        public TEntity FindByKey(int key, Func<IQueryable<TEntity>, IQueryable<TEntity>> eagerLoading = null)
        {
            if(eagerLoading == null){
                return _DbSet.Find(key);
            }else{
                return loadRelatedData(eagerLoading).SingleOrDefault(e => e.Id == key);
            }
        }

        // public TEntity FindByKeys(params Object[] keyValues)
        // {
        //     return _DbSet.Find(keyValues);
        // }

        public TEntity FindSingle(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> eagerLoading = null)
        {
            return loadRelatedData(eagerLoading).SingleOrDefault(predicate);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> eagerLoading = null)
        {
            return loadRelatedData(eagerLoading).Where(predicate);
        }

        public IQueryable<TEntity> FindAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> eagerLoading = null)
        {
            return loadRelatedData(eagerLoading);
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _DbSet.Where(predicate).Count();
        }

        public int CountAll()
        {
            return _DbSet.Count();
        }
        public void Load<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> propertyExpression) where TProperty : class
        {
            _dbContext.Entry(entity).Reference(propertyExpression).Load(); 
        }

        public void Load<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionExpression) where TProperty : class
        {
            _dbContext.Entry(entity).Collection(collectionExpression).Load(); 
        }

        private IQueryable<TEntity> loadRelatedData(Func<IQueryable<TEntity>, IQueryable<TEntity>> eagerLoading){
            IQueryable<TEntity> result = null;
            if(eagerLoading != null){
                result = eagerLoading(_DbSet);
            }else{
                result = _DbSet;
            }
            return result;
        }

        #endregion

        protected DbSet<TEntity> _DbSet;
        protected DbScheme _dbContext;
    }
}