using Microsoft.EntityFrameworkCore;
using Question.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Infrastructure
{
    public sealed class QuestionDbContext :DbContext
    {
        public DbSet<QuestionCategory> Categories{ get; set; }
        public DbSet<QuestionItem> Questions { get; set; }
        public DbSet<QuestionAnswer> Answers { get; set; }

        public QuestionDbContext(DbContextOptions options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuestionDbContext).Assembly);

    }
}