using System;
using Applicant.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Applicant.Infrasructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserExams> UserExams { get; set; }
        public DbSet<AccessCode> AccessCodes { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasMany(x => x.RefreshTokens).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<UserExams>().HasKey(x => new { x.ExamId, x.UserId });

            base.OnModelCreating(builder);
        }
    }

}
