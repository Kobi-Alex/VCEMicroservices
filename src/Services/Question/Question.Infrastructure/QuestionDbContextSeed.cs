using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Question.Domain.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Question.Infrastructure
{
    public static class QuestionDbContextSeed
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<QuestionDbContext>());
            }
        }

        private static void SeedData(QuestionDbContext context)
        {
            if(!context.Questions.Any())
            {
                Console.WriteLine("--> Seeding Question Data...");

                context.Questions.AddRange(
                    new QuestionItem()
                    {
                        Category = "Asp Dot NET",
                        Question = "What is the dependency injection?",
                        Answer = "Is a design pattern used to implement IoC.",
                        ReleaseDate = new DateTimeOffset(new DateTime(2021, 12, 26))
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
