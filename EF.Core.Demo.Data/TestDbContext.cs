using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EF.Core.Demo.Data
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
            
        }

        public DbSet<TestUser> TestUsers { get; set; }
    }

    [Table("Users")]
    public class TestUser
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Account { get; set; }

        [MaxLength(50)]
        public string Password { get; set; }
    }
}
