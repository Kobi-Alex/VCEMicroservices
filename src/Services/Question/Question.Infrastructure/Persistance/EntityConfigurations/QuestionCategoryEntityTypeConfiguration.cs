using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Question.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Infrastructure.Persistance.EntityConfigurations
{
    internal sealed class QuestionCategoryEntityTypeConfiguration
        : IEntityTypeConfiguration<QuestionCategory>
    {
        public void Configure(EntityTypeBuilder<QuestionCategory> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(category => category.Id);

            builder.Property(question => question.Id).ValueGeneratedOnAdd();

            builder.Property(category => category.Name).HasMaxLength(60);

            builder.HasMany(category => category.QuestionItems)
                .WithOne()
                .HasForeignKey(question => question.QuestionCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
