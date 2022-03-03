using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            Database.EnsureCreated();
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<UserExams> UserExams { get; set; }

        public DbSet<AccessCode> AccessCodes { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasMany(x => x.RefreshTokens).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<UserExams>().HasKey(x=> new { x.ExamId, x.UserId });



            base.OnModelCreating(builder);
        }
    }
}
