﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Report.Infrastructure;

namespace Report.Infrastructure.Migrations
{
    [DbContext(typeof(ReportDbContext))]
    partial class ReportDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Report.Domain.AggregatesModel.ReviewAggregate.QuestionUnit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<int?>("ReviewId")
                        .HasColumnType("int");

                    b.Property<string>("_answerKeys")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("AnswerKeys");

                    b.Property<string>("_currentKeys")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("CurrentKeys");

                    b.Property<string>("_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("QuestionName");

                    b.Property<int>("_totalNumberAnswer")
                        .HasColumnType("int")
                        .HasColumnName("TotalNumberAnswer");

                    b.HasKey("Id");

                    b.HasIndex("ReviewId");

                    b.ToTable("questionUnits", "report");
                });

            modelBuilder.Entity("Report.Domain.AggregatesModel.ReviewAggregate.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("_applicantId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("ApplicantId");

                    b.Property<int>("_examId")
                        .HasColumnType("int")
                        .HasColumnName("ExamId");

                    b.Property<string>("_grade")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Grade");

                    b.Property<decimal>("_persentScore")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)")
                        .HasColumnName("PersentScore");

                    b.Property<DateTime>("_reportDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("ReportDate");

                    b.Property<decimal>("_totalScore")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)")
                        .HasColumnName("TotalScore");

                    b.HasKey("Id");

                    b.ToTable("reviews", "report");
                });

            modelBuilder.Entity("Report.Infrastructure.Persistance.Idempotency.ClientRequest", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("requests", "report");
                });

            modelBuilder.Entity("Report.Domain.AggregatesModel.ReviewAggregate.QuestionUnit", b =>
                {
                    b.HasOne("Report.Domain.AggregatesModel.ReviewAggregate.Review", null)
                        .WithMany("QuestionUnits")
                        .HasForeignKey("ReviewId");
                });

            modelBuilder.Entity("Report.Domain.AggregatesModel.ReviewAggregate.Review", b =>
                {
                    b.Navigation("QuestionUnits");
                });
#pragma warning restore 612, 618
        }
    }
}
