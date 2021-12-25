
using Exam.Domain.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exam.Infrastructure.Persistance.EntityConfigurations
{
    internal sealed class ExamItemEntityTypeConfiguration
        : IEntityTypeConfiguration<ExamItem>
    {
        public void Configure(EntityTypeBuilder<ExamItem> builder)
        {
            builder.ToTable("Exams");

            builder.HasKey(ei => ei.Id);

            builder.Property(ei => ei.Title)
                .IsRequired().HasMaxLength(60);

            builder.Property(ei => ei.Category)
                .IsRequired().HasMaxLength(60);

            builder.Property(ei => ei.Description)
                .IsRequired().HasMaxLength(100);

            builder.Property(ei => ei.DurationTime)
                .IsRequired();

            builder.Property(ei => ei.PassingScore)
                .IsRequired().HasColumnName("decimal(5, 2)");

            builder.Property(ei => ei.DateExam)
                .IsRequired();
            
            builder.Property(ei => ei.Status)
                .IsRequired().HasMaxLength(60);
        }
    }
}