using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Repositories.Dto;

namespace Repositories.DataBase
{
    public interface IDbScheme
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class, IEntity<int>, new();
        int SaveChanges();
    }
}
