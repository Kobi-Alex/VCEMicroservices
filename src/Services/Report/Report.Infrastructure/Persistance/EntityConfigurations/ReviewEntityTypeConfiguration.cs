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

            //_applicationId
            reviewConfiguration
                .Property<string>("_applicationId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("ApplicationId")
                .IsRequired();

            //_description
            reviewConfiguration
                .Property<string>("_description")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Description")
                .IsRequired(false);

            //_reportData
            reviewConfiguration
                .Property<DateTime>("_reportDate")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("ReportDate")
                .IsRequired();

            //_questionUnits
            var questionUnitsConfiguration = reviewConfiguration.OwnsMany(x => x.QuestionUnits);
            reviewConfiguration.Navigation(x => x.QuestionUnits).Metadata.SetField("_questionUnits");


            //var navigation = reviewConfiguration.Metadata.FindNavigation(nameof(Review.QuestionUnits));
            ////Set as field (New since EF 1.1) to access the OrderItem collection property through its field
            //navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
