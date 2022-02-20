using System;
using Microsoft.EntityFrameworkCore;
using Question.Domain.Entities;


namespace Question.Infrastructure
{
    public sealed class QuestionDbContext : DbContext
    {
        public DbSet<QuestionCategory> Categories{ get; set; }
        public DbSet<QuestionItem> Questions { get; set; }
        public DbSet<QuestionAnswer> Answers { get; set; }

        public QuestionDbContext(DbContextOptions options) 
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuestionDbContext).Assembly);

    }
}