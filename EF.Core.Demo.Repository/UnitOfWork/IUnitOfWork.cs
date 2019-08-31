using EF.Core.Demo.Repository.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EF.Core.Demo.Repository.UnitOfWork
{

    /// <summary>
    /// 引入单元操作，主要是为了给各个实体维护一个共同的DbContext上下文对象，保证所有的操作都是在共同的上下文中进行的。EF的操作提交 context.SaveChanged() 默认就是事务性的，只要保证了当前的所有实体的操作都是在一个共同的上下文中进行的，就实现了事务操作了。
    ///在业务层中，各个实体的增删改操作都是通过各个实体的Repository进行的，只需要提供一个提交保存的功能作为最后调用，即可保证当前的提交是事务性的。因此定义给业务层引用的单元操作接口如下
    /// </summary>
    public interface IUnitOfWork : IRepositoryFactory, IDisposable
    {
        int Commit();

        void BeginTransaction();

        void RollBackTransaction();

        void CommitTransaction();
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork
        where TContext : DbContext
    {
        TContext DbContext { get; }
    }
}
