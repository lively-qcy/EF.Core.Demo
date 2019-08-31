using System;
using System.Collections.Generic;
using System.Text;

namespace EF.Core.Demo.Service.Base
{
    public interface IBaseAppService<TEntity>
        where TEntity : class, new()
    {
        TEntity Find(string id);
    }
}
