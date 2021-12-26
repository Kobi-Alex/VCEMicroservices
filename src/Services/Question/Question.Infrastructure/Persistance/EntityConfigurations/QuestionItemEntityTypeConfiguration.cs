using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Question.Domain.Domain.Entities;

namespace Question.Infrastructure.Persistance.EntityConfigurations
{
    internal sealed class QuestionItemEntityTypeConfiguration
        :IEntityTypeConfiguration<QuestionItem>
    {
        public void Configure(EntityTypeBuilder<QuestionItem> builder)
        {
            builder.ToTable("Questions");

            builder.HasKey(qi => qi.Id);

            builder.Property(qi => qi.Category)
                .IsRequired(true).HasMaxLength(30);

            builder.Property(qi => qi.Question)
               .IsRequired(true).HasMaxLength(200);

            builder.Property(qi => qi.QuestionOptionA)
                .IsRequired(false).HasMaxLength(200);

            builder.Property(qi => qi.QuestionOptionB)
                .IsRequired(false).HasMaxLength(200);

            builder.Property(qi => qi.QuestionOptionC)
                .IsRequired(false).HasMaxLength(200);

            builder.Property(qi => qi.QuestionOptionD)
                .IsRequired(false).HasMaxLength(200);

            builder.Property(qi => qi.Answer)
               .IsRequired().HasMaxLength(200);

            builder.Property(qi => qi.AnswerOptionA)
                .IsRequired(false).HasMaxLength(200);

            builder.Property(qi => qi.AnswerOptionB)
                .IsRequired(false).HasMaxLength(200);

            builder.Property(qi => qi.AnswerOptionC)
                .IsRequired(false).HasMaxLength(200);

            builder.Property(qi => qi.AnswerOptionD)
                .IsRequired(false).HasMaxLength(200);

            builder.Property(qi => qi.ReleaseDate)
               .IsRequired(true);
        }
    }
}