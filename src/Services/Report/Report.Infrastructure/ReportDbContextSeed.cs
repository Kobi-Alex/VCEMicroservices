using System;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Report.Domain.AggregatesModel.ReviewAggregate;


namespace Report.Infrastructure
{
    public static class ReportDbContextSeed
    {
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ReportDbContext>());
            }
        }

        private static void SeedData(ReportDbContext context)
        {
            if (!context.Reviews.Any())
            {
                Console.WriteLine("--> Seeding Report Data...");

                context.Reviews.AddRange
                (
                    new Review(1, "a1875c21-b82e-4e87-962b-9777c351f989"),
                    new Review(1, "a1875c21-b82e-4e87-962b-9777c351f990"),
                    new Review(1, "a1875c21-b82e-4e87-962b-9777c351f991"),
                    new Review(2, "a1875c21-b82e-4e87-962b-9777c351f994"),
                    new Review(2, "a1875c21-b82e-4e87-962b-9777c351f995")
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
