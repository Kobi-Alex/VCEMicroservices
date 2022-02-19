using System;
using System.Collections.Generic;
using System.Linq;
using Exam.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Exam.Infrastructure
{
    public static class ExamDbContextSeed
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ExamDbContext>(), isProduction);
            }
        }

        private static void SeedData(ExamDbContext context, bool isProduction)
        {
            if (isProduction)
            {
                Console.WriteLine("--> Attemption to apply migrations...");

                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not migrations: {ex.Message}");
                }
            }


            if (!context.Exams.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                context.Exams.AddRange(
                    new ExamItem()
                    {
                        Title = "Entity Framework Core",
                        Description = "Microsoft EF Core, migrations, seedings data;",
                        DurationTime = 60,
                        PassingScore = 70,
                        Status = ExamStatus.NotAvailable,
                        ExamQuestions = (ICollection<ExamQuestion>)GetPreconfiguredExamQuestionTest()
                    },
                    new ExamItem()
                    {
                        Title = "Begin in Docker",
                        Description = "Docker and containerization",
                        DurationTime = 120,
                        PassingScore = 68,
                        Status = ExamStatus.Finished
                    },
                    new ExamItem()
                    {
                         Title = "Docker and Kibernetis",
                         Description = "Docker with kibernatis",
                         DurationTime = 100,
                         PassingScore = 80,
                         Status = ExamStatus.NotAvailable,
                         ExamQuestions = (ICollection<ExamQuestion>)GetPreconfiguredExamQuestionTest1()
                    }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We alredy have data!!");
            }
        }

        private static IEnumerable<ExamQuestion> GetPreconfiguredExamQuestionTest()
        {
            return new List<ExamQuestion>()
            {
                new ExamQuestion() { Question = "What is the dependency injection?", ExamItemId = 1, QuestionItemId = 1 },
            };
        } 
        private static IEnumerable<ExamQuestion> GetPreconfiguredExamQuestionTest1()
        {
            return new List<ExamQuestion>()
            {
                new ExamQuestion() { Question = "What is the dependency injection?", ExamItemId = 3, QuestionItemId = 1 },
            };
        }
    }
}
