using EF.Core.Demo.Data;
using EF.Core.Demo.Data.Domain;
using EF.Core.Demo.Repository.Repository;
using EF.Core.Demo.Repository.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace EF.Core.Demo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _userRep;
        private readonly IRepository<Role> _roleRep;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfWork<TestDbContext> _testDbUnitOfWork;

        public UserController(IUnitOfWork unitOfWork,
            IUnitOfWork<TestDbContext> testDbUnitOfWork,
            IRepository<User> userRep,
            IRepository<Role> roleRep)
        {
            _userRep = userRep;
            _roleRep = roleRep;
            _unitOfWork = unitOfWork;
            _testDbUnitOfWork = testDbUnitOfWork;
        }
        [HttpGet("{id}")]
        public User GetById(string id)
        {
            #region 非事物
            _roleRep.Insert(new Role
            {
                Name = "test"
            });
            _userRep.Insert(new User
            {
                Id = "1",
                Name = "test",
                Account = "test",
                Password = "test",
                Status = "A"
            });
            #endregion

            #region 事物
            //_unitOfWork.BeginTransaction();  使用Uow GetRepository默认就是同一个DbContext。默认是同一个事物提交

            //_unitOfWork.GetRepository<Role>().Insert(new Role
            //{
            //    Name = "test"
            //});
            //_unitOfWork.GetRepository<User>().Insert(new User
            //{
            //    Id = "1",
            //    Name = "test",
            //    Account = "test",
            //    Password = "test",
            //    Status = "A"
            //});
            //_unitOfWork.Commit();

            //_unitOfWork.CommitTransaction(); 
            #endregion

            return _unitOfWork.GetRepository<User>().Find(id);
        }
    }
}