using EF.Core.Demo.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EF.Core.Demo.Service.Base
{
    public class BaseAppService<TEntity> : IBaseAppService<TEntity>
        where TEntity : class,new()
    {
        private readonly UserDbContext _dbContext;
        public BaseAppService(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TEntity Find(string id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }
    }
}
