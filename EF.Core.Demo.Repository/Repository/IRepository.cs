using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EF.Core.Demo.Repository.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Find(params object[] keyValues);

        int Insert(TEntity entity);

        IQueryable<TEntity> FromSql(string sql, params object[] parameters);

        int SaveChanges();
    }
}
