using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repositories.Dto;
using Xunit;

namespace Repositories.Tests.Fakes
{
    public class FakeDbSet<T> : DbSet<T>
        where T : class, IEntity<int>, new()
    {
        IList<T> _data;

        public FakeDbSet()
        {
            _data = new List<T>();
        }

        public override T Find(params object[] keyValues)
        {
            return _data.SingleOrDefault(e => e.Id == (int)keyValues[0]);
        }

        public T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _data.AsQueryable().SingleOrDefault(predicate); 
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return _data.AsQueryable().Where(predicate); 
        }

        public override EntityEntry<T> Add(T item)
        {
            _data.Add(item);
            return null;
        }

        public void Remove(T item)
        {
            _data.Remove(item);
        }

        public void Attach(T item)
        {
            _data.Add(item);
        }

        // public void Detach(T item)
        // {
        //     _data.Remove(item);
        // }

        public int DeCounttach()
        {
            return _data.Count;
        }

        // public T Create()
        // {
        //     return Activator.CreateInstance<T>();
        // }

        // public TDerivedEntity Create<TDerivedEntity>() where TDerivedEntity : class, T
        // {
        //     return Activator.CreateInstance<TDerivedEntity>();
        // }

        // public ObservableCollection<T> Local
        // {
        //     get { return _data; }
        // }

        // Type IQueryable.ElementType
        // {
        //     get { return _query.ElementType; }
        // }

        // System.Linq.Expressions.Expression IQueryable.Expression
        // {
        //     get { return _query.Expression; }
        // }

        // IQueryProvider IQueryable.Provider
        // {
        //     get { return _query.Provider; }
        // }

        // System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        // {
        //     return _data.GetEnumerator();
        // }

        // IEnumerator<T> IEnumerable<T>.GetEnumerator()
        // {
        //     return _data.GetEnumerator();
        // }
    }
}