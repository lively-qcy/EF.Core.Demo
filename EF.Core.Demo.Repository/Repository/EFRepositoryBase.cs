using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EF.Core.Demo.Repository.Repository
{
    public class EFRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;
        protected bool _isEnableTransaction;
        public EFRepositoryBase(DbContext dbContext, bool isEnableTransaction = false)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<TEntity>();
            _isEnableTransaction = isEnableTransaction;  //默认不开启事物
        }

        public TEntity Find(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public IQueryable<TEntity> FromSql(string sql, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public int Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            return _isEnableTransaction ? 0 : SaveChanges();        // 如果使用了工作单元获取Repository（默认认为是在同一事物下），insert就不每次提交,可以通过参数覆盖
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
