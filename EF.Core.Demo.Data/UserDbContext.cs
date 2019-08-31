using EF.Core.Demo.Data.Domain;
using Microsoft.EntityFrameworkCore;
using System;

namespace EF.Core.Demo.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
