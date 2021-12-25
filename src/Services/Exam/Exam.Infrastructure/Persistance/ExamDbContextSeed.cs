using Exam.Domain.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Infrastructure.Persistance
{
    public static class ExamDbContextSeed
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ExamDbContext>());
            }
        }

        private static void SeedData(ExamDbContext context)
        {
            if (!context.Exams.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                context.Exams.AddRange(
                    new ExamItem()
                    {
                        Title = "Entity Framework Core",
                        Category = "Dot net core",
                        Description = "Microsoft EF Core, migrations, seedings data;",
                        DurationTime = 1800,
                        PassingScore = 70.00m,
                        DateExam = new DateTime(2022, 3, 12),
                        Status = "not avalible"
                    },
                     new ExamItem()
                     {
                         Title = "Begin in Dosker",
                         Category = "Docker",
                         Description = "Docker and containerization",
                         DurationTime = 1000,
                         PassingScore = 68.00m,
                         DateExam = new DateTime(2022, 02, 24),
                         Status = "not avalible"
                     },
                     new ExamItem()
                     {
                         Title = "Dosker and Kibernetis",
                         Category = "Docker",
                         Description = "Docker with kibernatis",
                         DurationTime = 1000,
                         PassingScore = 80.00m,
                         DateExam = new DateTime(2022, 01, 30),
                         Status = "avalible"
                     }
                );

                context.SaveChanges();
            }   
            else
            {
                Console.WriteLine("--> We alredy have data!!");
            }
        }
    }
}
