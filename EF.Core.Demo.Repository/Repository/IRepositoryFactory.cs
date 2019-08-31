using System;
using System.Collections.Generic;
using System.Text;

namespace EF.Core.Demo.Repository.Repository
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}
