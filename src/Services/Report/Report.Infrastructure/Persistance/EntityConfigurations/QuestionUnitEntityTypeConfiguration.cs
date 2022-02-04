﻿using System;
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

            // _name
            questionUnitConfiguration
                .Property<string>("_name")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("QuestionName")
                .IsRequired();
            
            // _answerKeys
            questionUnitConfiguration
                .Property<string>("_answerKeys")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("AnswerKeys")
                .IsRequired();
            
            // _currentKeys
            questionUnitConfiguration
                .Property<string>("_currentKeys")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("CurrentKeys")
                .IsRequired();

            // _totalNumberAnswer
            questionUnitConfiguration
                .Property<int>("_totalNumberAnswer")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("TotalNumberAnswers")
                .IsRequired();

            // QuestionId
            questionUnitConfiguration
                .Property<int>("QuestionId")
                .IsRequired();
        }

    }
    
}