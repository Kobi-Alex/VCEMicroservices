using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
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
                    new Review("Report from exam #1", new Guid().ToString()),
                    new Review("Report from exam #2", new Guid().ToString()),
                    new Review("Report from exam #3", new Guid().ToString())
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
