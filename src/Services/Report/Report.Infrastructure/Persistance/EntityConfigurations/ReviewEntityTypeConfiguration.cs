using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.AggregatesModel.ReviewAggregate;


namespace Report.Infrastructure.Persistance.EntityConfigurations
{
    internal sealed class ReviewEntityTypeConfiguration
        : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> reviewConfiguration)
        {
            reviewConfiguration.ToTable("reviews", ReportDbContext.DEFAULT_SCHEMA);

            reviewConfiguration.HasKey(r => r.Id);

            //examId
            reviewConfiguration
               .Property<int>("_examId")
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .HasColumnName("ExamId")
               .IsRequired();

            //applicationId
            reviewConfiguration
                .Property<string>("_applicantId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("ApplicantId")
                .IsRequired();

            //totalScore
            reviewConfiguration
                .Property<decimal>("_totalScore")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("TotalScore")
                .IsRequired()
                .HasPrecision(5, 2);

            //persentScore
            reviewConfiguration
                .Property<decimal>("_persentScore")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("PersentScore")
                .IsRequired()
                .HasPrecision(5, 2);

            //grade
            reviewConfiguration
                .Property<string>("_grade")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Grade")
                .IsRequired(false);

            //reportData
            reviewConfiguration
                .Property<DateTime>("_reportDate")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("ReportDate")
                .IsRequired();

            reviewConfiguration.HasMany(category => category.QuestionUnits)
             .WithOne(c => c.Review)
             .OnDelete(DeleteBehavior.Cascade);


            var navigation = reviewConfiguration.Metadata.FindNavigation(nameof(Review.QuestionUnits));

            // DDD Patterns comment:
            //Set as field (New since EF 1.1) to access the ReviewItem collection property through its field
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            //var questionUnitsConfiguration = reviewConfiguration.OwnsMany(x => x.QuestionUnits);
            //reviewConfiguration.Navigation(x => x.QuestionUnits).Metadata.SetField("_questionUnits");
        }
    }
}
