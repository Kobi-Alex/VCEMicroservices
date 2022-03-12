﻿// <auto-generated />
using Exam.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Exam.Infrastructure.Migrations
{
    [DbContext(typeof(ExamDbContext))]
    [Migration("20220310150446_AddColumnQuestionItemId")]
    partial class AddColumnQuestionItemId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Exam.Domain.Entities.ExamItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int>("DurationTime")
                        .HasColumnType("int");

                    b.Property<decimal>("PassingScore")
                        .HasPrecision(5, 2)
                        .HasColumnType("decimal(5,2)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Exams");
                });

            modelBuilder.Entity("Exam.Domain.Entities.ExamQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ExamItemId")
                        .HasColumnType("int");

                    b.Property<string>("Question")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuestionItemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ExamItemId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Exam.Domain.Entities.ExamQuestion", b =>
                {
                    b.HasOne("Exam.Domain.Entities.ExamItem", "ExamItem")
                        .WithMany("ExamQuestions")
                        .HasForeignKey("ExamItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExamItem");
                });

            modelBuilder.Entity("Exam.Domain.Entities.ExamItem", b =>
                {
                    b.Navigation("ExamQuestions");
                });
#pragma warning restore 612, 618
        }
    }
}