using System;
using System.Linq;
using System.Text;
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

            builder.HasOne(eq => eq.ExamItem)
                .WithMany()
                .HasForeignKey(eq => eq.ExamItemId);
        }
    }
}
