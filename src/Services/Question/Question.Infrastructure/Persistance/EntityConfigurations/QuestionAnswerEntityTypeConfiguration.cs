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
    internal sealed class QuestionAnswerEntityTypeConfiguration
        : IEntityTypeConfiguration<QuestionAnswer>
    {
        public void Configure(EntityTypeBuilder<QuestionAnswer> builder)
        {
            builder.ToTable("Answers");

            builder.HasKey(answer => answer.Id);

            builder.Property(answer => answer.Id).ValueGeneratedOnAdd();

            builder.Property(answer => answer.Context)
               .IsRequired(true).HasMaxLength(400);

            builder.Property(answer => answer.CorrectAnswerCoefficient)
                .IsRequired(true)
                .HasPrecision(5, 2);
        }
    }
}
