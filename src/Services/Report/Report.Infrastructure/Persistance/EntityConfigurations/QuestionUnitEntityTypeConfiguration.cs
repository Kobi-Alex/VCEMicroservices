using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Domain.AggregatesModel.ReviewAggregate;


namespace Report.Infrastructure.Persistance.EntityConfigurations
{
    internal sealed class QuestionUnitEntityTypeConfiguration
        : IEntityTypeConfiguration<QuestionUnit>
    {
        public void Configure(EntityTypeBuilder<QuestionUnit> questionUnitConfiguration)
        {
            questionUnitConfiguration.ToTable("questionUnits", ReportDbContext.DEFAULT_SCHEMA);

            questionUnitConfiguration.HasKey(o => o.Id);
            questionUnitConfiguration.Property(o => o.Id).ValueGeneratedOnAdd();

            //name
            questionUnitConfiguration
                .Property<string>("_name")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("QuestionName")
                .IsRequired();
            
            //answerKeys
            questionUnitConfiguration
                .Property<string>("_answerKeys")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("AnswerKeys")
                .IsRequired();
            
            //currentKeys
            questionUnitConfiguration
                .Property<string>("_currentKeys")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("CurrentKeys")
                .IsRequired();

            //totalNumberAnswer
            questionUnitConfiguration
                .Property<int>("_totalNumberAnswer")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("TotalNumberAnswer")
                .IsRequired();

            //QuestionId
            questionUnitConfiguration
                .Property<int>("QuestionId")
                .IsRequired();
        }

    }
    
}
