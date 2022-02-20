using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Question.Domain.Entities;

namespace Question.Infrastructure.Persistance.EntityConfigurations
{
    internal sealed class QuestionItemEntityTypeConfiguration
        :IEntityTypeConfiguration<QuestionItem>
    {
        public void Configure(EntityTypeBuilder<QuestionItem> builder)
        {
            builder.ToTable("Questions");

            builder.HasKey(question => question.Id);
           
            builder.Property(question => question.Context)
               .IsRequired(true).HasMaxLength(400);

            builder.Property(question => question.ReleaseDate)
                .IsRequired();

            builder.Property(question => question.AnswerType)
                .IsRequired(true);

            //builder.HasOne(question => question.QuestionCategory)
            //    .WithMany()
            //    .HasForeignKey(question => question.QuestionCategoryId)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(question => question.QuestionAnswers)
                .WithOne(answer => answer.QuestionItem)
                .HasForeignKey(answer => answer.QuestionItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}