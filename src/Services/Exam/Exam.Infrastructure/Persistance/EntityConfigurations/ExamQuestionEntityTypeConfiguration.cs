using System;
using Exam.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exam.Infrastructure.Persistance.EntityConfigurations
{
    internal sealed class ExamQuestionEntityTypeConfiguration
        : IEntityTypeConfiguration<ExamQuestion>
    {
        public void Configure(EntityTypeBuilder<ExamQuestion> builder)
        {
            builder.ToTable("Questions");

            builder.HasKey(question => question.Id);

            builder.Property(question => question.Question)
                .IsRequired(true);

            builder.Property(question => question.QuestionItemId)
                .IsRequired();

            builder.HasOne(eq => eq.ExamItem)
                .WithMany(question => question.ExamQuestions)
                .HasForeignKey(eq => eq.ExamItemId);
        }
    }
}
