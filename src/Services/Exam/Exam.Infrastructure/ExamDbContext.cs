using System;
using Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Exam.Infrastructure
{
    public sealed class ExamDbContext : DbContext
    {
        public ExamDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<ExamItem> Exams { get; set; }
        public DbSet<ExamQuestion> Questions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExamDbContext).Assembly);
    }
}
