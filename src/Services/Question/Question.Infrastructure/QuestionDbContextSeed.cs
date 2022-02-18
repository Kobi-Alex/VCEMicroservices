using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Question.Domain.Entities;
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

                context.Categories.AddRange(
                    new QuestionCategory()
                    {
                        Name = "AspDotNET",
                        QuestionItems = (ICollection<QuestionItem>)GetPreconfiguredQuestionItemTest1()
                    },

                     new QuestionCategory()
                     {
                         Name = "WPF",
                         QuestionItems = (ICollection<QuestionItem>)GetPreconfiguredQuestionItemTest2()
                     }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We alredy have data!!");
            }
        }

        private static IEnumerable<QuestionAnswer> GetPreconfiguredQuestionAnswerTest1()
        {
            return new List<QuestionAnswer>()
            {
                new QuestionAnswer() { CharKey = "A", Context = "This is a design pattern used to implement IoC.", IsCorrectAnswer = true},
                new QuestionAnswer() { CharKey = "B", Context = "This is a middleware", IsCorrectAnswer = false},
            };
        }

        private static IEnumerable<QuestionAnswer> GetPreconfiguredQuestionAnswerTest2()
        {
            return new List<QuestionAnswer>()
            {
                new QuestionAnswer() { CharKey = "A", Context = "This is a pattern",   IsCorrectAnswer = false},
                new QuestionAnswer() { CharKey = "B", Context = "This is a injection", IsCorrectAnswer = false},
                new QuestionAnswer() { CharKey = "C", Context = "This is a banding",   IsCorrectAnswer = false},
                new QuestionAnswer() { CharKey = "D", Context = "This is a Windows Presentation Fundation",   IsCorrectAnswer = true},
                new QuestionAnswer() { CharKey = "E", Context = "This is a test",   IsCorrectAnswer = true},
            };
        }

        private static IEnumerable<QuestionItem> GetPreconfiguredQuestionItemTest1()
        {
            return new List<QuestionItem>()
            {
                new QuestionItem() 
                { 
                    Context = "What is the dependency injection?", 
                    ReleaseDate = new DateTimeOffset(new DateTime(2021, 12, 26)), 
                    QuestionAnswers = (ICollection<QuestionAnswer>)GetPreconfiguredQuestionAnswerTest1()
                }
            };
        }
        
        private static IEnumerable<QuestionItem> GetPreconfiguredQuestionItemTest2()
        {
            return new List<QuestionItem>()
            {
                new QuestionItem() 
                { 
                    Context = "Banding is it?", 
                    ReleaseDate = new DateTimeOffset(new DateTime(2021, 12, 28)), 
                    QuestionAnswers = (ICollection<QuestionAnswer>)GetPreconfiguredQuestionAnswerTest2()
                }
            };
        }
    }
}
