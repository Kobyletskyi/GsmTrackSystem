using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Repositories.DataBase;
using Repositories.Dto;
using Xunit;

namespace Repositories.Tests.Fakes
{
    public class FakeDbScheme : IDbScheme
    {
        private Dictionary<string, dynamic> _sets;
        public FakeDbScheme()
        {
            _sets = new Dictionary<string, dynamic>();
        }

        public int SaveChanges()
        {
            return 0;
        }
        // public DbSet<TEntity> Set<TEntity>()
        //     where TEntity : class, IEntity<int>, new()
        // {
        //     return new FakeDbSet<TEntity>();
        // }

        int IDbScheme.SaveChanges()
        {
            return 0;
        }

        public DbSet<TEntity> Set<TEntity>()
             where TEntity : class, IEntity<int>, new()
        {
            dynamic set;
            if(_sets.TryGetValue(typeof(TEntity).FullName,out set)){
                return set;
            }else{
                var newSet = new FakeDbSet<TEntity>();
                _sets.Add(typeof(TEntity).FullName, newSet);
                return  newSet;
            }           
        }
    }
}