using EF.Core.Demo.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EF.Core.Demo.Repository.UnitOfWork
{
    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext> where TDbContext : DbContext
    {
        public TDbContext DbContext { get; }
        private bool disposed = false;
        private Dictionary<Type, object> repositories;
        private bool isEnableTransaction = false;
        /// <summary>
        /// 工作单元管理DbContext上下文
        /// DbContext上下文通过注入
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWork(TDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public int Commit()
        {
            return DbContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (repositories != null)
                    {
                        repositories.Clear();
                    }
                    DbContext.Dispose();
                }
            }
            disposed = true;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }
            var type = typeof(TEntity);
            isEnableTransaction = true;
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new EFRepositoryBase<TEntity>(DbContext, isEnableTransaction); // 如果使用了工作单元获取Repository（默认认为是在同一事物下），insert就不每次提交,可以通过参数覆盖
            }
            return (IRepository<TEntity>)repositories[type];
        }

        public void BeginTransaction()
        {
            DbContext.Database.BeginTransaction();
            isEnableTransaction = true;
        }

        public void RollBackTransaction()
        {
            DbContext.Database.RollbackTransaction();
            isEnableTransaction = false;
        }

        public void CommitTransaction()
        {
            DbContext.Database.CommitTransaction();
            isEnableTransaction = false;
        }
    }
}
