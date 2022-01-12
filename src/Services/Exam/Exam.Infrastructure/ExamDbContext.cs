using Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Infrastructure.Persistance
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
