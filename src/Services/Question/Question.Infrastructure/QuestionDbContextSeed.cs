using System;
using System.Linq;
using System.Collections.Generic;

using Question.Domain.Entities;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;


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
                     },


                     new QuestionCategory()
                     {
                         Name = "Home",
                         QuestionItems = (ICollection<QuestionItem>)GetPreconfiguredQuestionItemTest3()
                     },

                     new QuestionCategory()
                     {
                         Name = "Docker",
                         QuestionItems = (ICollection<QuestionItem>)GetPreconfiguredQuestionForDocker()
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
                new QuestionAnswer() { CharKey = "A", Context = "This is a pattern",   IsCorrectAnswer = false },
                new QuestionAnswer() { CharKey = "B", Context = "This is a injection", IsCorrectAnswer = false },
                new QuestionAnswer() { CharKey = "C", Context = "This is a banding",   IsCorrectAnswer = false },
                new QuestionAnswer() { CharKey = "D", Context = "This is a Windows Presentation Fundation",   IsCorrectAnswer = true }
                // new QuestionAnswer() { CharKey = "E", Context = "This is a test", IsCorrectAnswer = true},
            };
        }

        private static IEnumerable<QuestionAnswer> GetPreconfiguredQuestionAnswerTest3()
        {
            return new List<QuestionAnswer>()
            {
                new QuestionAnswer() { CharKey = "T", Context = "pet", IsCorrectAnswer = true},
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
                    AnswerType = AnswerType.Single,
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
                    AnswerType = AnswerType.Multiple,
                    QuestionAnswers = (ICollection<QuestionAnswer>)GetPreconfiguredQuestionAnswerTest2()
                }
            };
        }

        private static IEnumerable<QuestionItem> GetPreconfiguredQuestionItemTest3()
        {
            return new List<QuestionItem>()
            {
                new QuestionItem()
                {
                    Context = "Dog is it?",
                    ReleaseDate = new DateTimeOffset(new DateTime(2021, 12, 28)),
                    AnswerType = AnswerType.Text,
                    QuestionAnswers = (ICollection<QuestionAnswer>)GetPreconfiguredQuestionAnswerTest3()
                },

                new QuestionItem()
                {
                    Context = "Docker is it?",
                    ReleaseDate = new DateTimeOffset(new DateTime(2021, 12, 26)),
                    AnswerType = AnswerType.Single,
                    QuestionAnswers = (ICollection<QuestionAnswer>)GetPreconfiguredQuestionAnswerTest1()
                }
            };
        }

        private static IEnumerable<QuestionItem> GetPreconfiguredQuestionForDocker()
        {
            return new List<QuestionItem> 
            { 
                new QuestionItem()
                {
                    Context = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    ReleaseDate = new DateTimeOffset(DateTime.Now),
                    AnswerType = AnswerType.Text,
                    QuestionAnswers = new List<QuestionAnswer>
                    {
                       new QuestionAnswer() { CharKey = "T", Context = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",   IsCorrectAnswer = true },
                    }
                },
                new QuestionItem()
                {
                    Context = "Dui ut ornare lectus sit amet est placerat in.",
                    ReleaseDate = new DateTimeOffset(DateTime.Now),
                    AnswerType = AnswerType.Single,
                    QuestionAnswers = new List<QuestionAnswer>
                    {
                         new QuestionAnswer() { CharKey = "A", Context = "A iaculis at erat pellentesque adipiscing commodo elit at.",   IsCorrectAnswer = false },
                         new QuestionAnswer() { CharKey = "B", Context = "T->Ultricies integer quis auctor elit sed vulputate mi",   IsCorrectAnswer = true },
                         new QuestionAnswer() { CharKey = "C", Context = "Auctor urna nunc id cursus metus aliquam eleifend mi in.",   IsCorrectAnswer = false },
                         new QuestionAnswer() { CharKey = "D", Context = "Feugiat sed lectus vestibulum mattis ullamcorper velit sed ullamcorper.",   IsCorrectAnswer = false },
                         new QuestionAnswer() { CharKey = "E", Context = "Posuere sollicitudin aliquam ultrices sagittis orci a scelerisque purus.",   IsCorrectAnswer = false },
                    }
                },
                new QuestionItem()
                {
                    Context = "Donec enim diam vulputate ut pharetra. In nisl nisi scelerisque eu ultrices vitae auctor eu. Amet justo donec enim diam vulputate ut pharetra sit amet.",
                    ReleaseDate = new DateTimeOffset(DateTime.Now),
                    AnswerType = AnswerType.Text,
                    QuestionAnswers = new List<QuestionAnswer>
                    {
                       new QuestionAnswer() { CharKey = "T", Context = "Donec enim diam vulputate ut pharetra. In nisl nisi scelerisque eu ultrices vitae auctor eu. Amet justo donec enim diam vulputate ut pharetra sit amet.",   IsCorrectAnswer = true },
                    }
                },
                new QuestionItem()
                {
                    Context = "Donec enim diam vulputate ut pharetra. In nisl nisi scelerisque eu ultrices vitae auctor eu. Amet justo donec enim diam vulputate ut pharetra sit amet.",
                    ReleaseDate = new DateTimeOffset(DateTime.Now),
                    AnswerType = AnswerType.Text,
                    QuestionAnswers = new List<QuestionAnswer>
                    {
                       new QuestionAnswer() { CharKey = "T", Context = "Donec enim diam vulputate ut pharetra. In nisl nisi scelerisque eu ultrices vitae auctor eu. Amet justo donec enim diam vulputate ut pharetra sit amet.",   IsCorrectAnswer = true },
                    }
                },
                new QuestionItem()
                {
                    Context = "Iaculis at erat pellentesque adipiscing commodo elit at imperdiet dui.",
                    ReleaseDate = new DateTimeOffset(DateTime.Now),
                    AnswerType = AnswerType.Text,
                    QuestionAnswers = new List<QuestionAnswer>
                    {
                       new QuestionAnswer() { CharKey = "T", Context = "Iaculis at erat pellentesque adipiscing commodo elit at imperdiet dui.",   IsCorrectAnswer = true },
                    }
                },
                new QuestionItem()
                {
                    Context = "Adipiscing commodo elit at imperdiet dui. Viverra orci sagittis eu volutpat odio facilisis mauris.",
                    ReleaseDate = new DateTimeOffset(DateTime.Now),
                    AnswerType = AnswerType.Single,
                    QuestionAnswers = new List<QuestionAnswer>
                    {
                         new QuestionAnswer() { CharKey = "A", Context = "Magna etiam tempor orci eu.",   IsCorrectAnswer = false },
                         new QuestionAnswer() { CharKey = "B", Context = "Nibh mauris cursus mattis molestie a.",   IsCorrectAnswer = false },
                         new QuestionAnswer() { CharKey = "C", Context = "Quisque non tellus orci ac auctor augue mauris augue.",   IsCorrectAnswer = false },
                         new QuestionAnswer() { CharKey = "D", Context = "T->Volutpat lacus laoreet non curabitur gravida.s",   IsCorrectAnswer = true },
                    }
                },
                new QuestionItem()
                {
                    Context = "Dui ut ornare lectus sit amet est placerat in egestas.",
                    ReleaseDate = new DateTimeOffset(DateTime.Now),
                    AnswerType = AnswerType.Multiple,
                    QuestionAnswers = new List<QuestionAnswer>
                    {
                       new QuestionAnswer() { CharKey = "A", Context = "Curabitur vitae nunc sed velit dignissim sodales ut eu.",   IsCorrectAnswer = false },
                       new QuestionAnswer() { CharKey = "B", Context = "T->n aliquam sem fringilla ut morbi tincidunt augue.",   IsCorrectAnswer = true },
                       new QuestionAnswer() { CharKey = "C", Context = "T->Vitae turpis massa sed elementum. Nunc vel risus commodo viverra.",   IsCorrectAnswer = true },
                       new QuestionAnswer() { CharKey = "D", Context = "T->Mattis rhoncus urna neque viverra justo nec ultrices dui. Sed risus pretium quam vulputate dignissim suspendisse. Vel risus commodo viverra maecenas accumsan lacus vel.",   IsCorrectAnswer = true },
                       new QuestionAnswer() { CharKey = "E", Context = "Tempor orci eu lobortis elementum nibh tellus molestie.",   IsCorrectAnswer = false },
                    }
                },
                new QuestionItem()
                {
                    Context = "Sollicitudin nibh sit amet commodo nulla facilisi nullam.",
                    ReleaseDate = new DateTimeOffset(DateTime.Now),
                    AnswerType = AnswerType.Single,
                    QuestionAnswers = new List<QuestionAnswer> 
                    {
                         new QuestionAnswer() { CharKey = "A", Context = "Dui id ornare arcu odio ut sem. Tristique et egestas quis ipsum suspendisse ultrices.",   IsCorrectAnswer = false },
                         new QuestionAnswer() { CharKey = "B", Context = "Vivamus arcu felis bibendum ut.",   IsCorrectAnswer = false },
                         new QuestionAnswer() { CharKey = "C", Context = "Commodo sed egestas egestas fringilla phasellus faucibus scelerisque.",   IsCorrectAnswer = false },
                         new QuestionAnswer() { CharKey = "D", Context = "Mi bibendum neque egestas congue quisque egestas diam in. In nisl nisi scelerisque eu.",   IsCorrectAnswer = false },
                         new QuestionAnswer() { CharKey = "E", Context = "T->Eu scelerisque felis imperdiet proin fermentum leo vel orci.",   IsCorrectAnswer = true },
                    }
                },
                new QuestionItem()
                {
                    Context = "Tortor at risus viverra adipiscing at in tellus integer.",
                    ReleaseDate = new DateTimeOffset(DateTime.Now),
                    AnswerType = AnswerType.Single,
                    QuestionAnswers = new List<QuestionAnswer>
                    {
                         new QuestionAnswer() { CharKey = "A", Context = "T->Enim ut sem viverra aliquet eget sit amet tellus.",   IsCorrectAnswer = true },
                         new QuestionAnswer() { CharKey = "B", Context = "At lectus urna duis convallis convallis tellus id interdum velit.",   IsCorrectAnswer = false },
                         new QuestionAnswer() { CharKey = "C", Context = "Hac habitasse platea dictumst quisque sagittis purus.",   IsCorrectAnswer = false },
                         new QuestionAnswer() { CharKey = "D", Context = "Sit amet volutpat consequat mauris nunc congue nisi vitae",   IsCorrectAnswer = false },
                         new QuestionAnswer() { CharKey = "E", Context = "Id faucibus nisl tincidunt eget.",   IsCorrectAnswer = false },
                    }
                },
                new QuestionItem()
                {
                    Context = "Est pellentesque elit ullamcorper dignissim cras tincidunt lobortis feugiat vivamus.",
                    ReleaseDate = new DateTimeOffset(DateTime.Now),
                    AnswerType = AnswerType.Multiple,
                    QuestionAnswers = new List<QuestionAnswer>
                    {
                         new QuestionAnswer() { CharKey = "A", Context = "Convallis convallis tellus id interdum velit laoreet id.",   IsCorrectAnswer = false },
                         new QuestionAnswer() { CharKey = "B", Context = "T->Diam maecenas ultricies mi eget mauris.",   IsCorrectAnswer = true },
                         new QuestionAnswer() { CharKey = "C", Context = "Fusce id velit ut tortor pretium viverra suspendisse potenti.",   IsCorrectAnswer = false },
                         new QuestionAnswer() { CharKey = "D", Context = "T->Ipsum suspendisse ultrices gravida dictum fusce ut placerat orci.",   IsCorrectAnswer = true },
                         new QuestionAnswer() { CharKey = "E", Context = "At auctor urna nunc id cursus metus aliquam.",   IsCorrectAnswer = false },
                    }
                },
            };

        }
    }
}
