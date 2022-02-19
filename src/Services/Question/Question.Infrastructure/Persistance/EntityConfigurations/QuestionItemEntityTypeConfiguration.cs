using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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