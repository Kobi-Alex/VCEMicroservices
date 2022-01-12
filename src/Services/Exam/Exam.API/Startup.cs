using Exam.API.Application.Services;
using Exam.API.Application.Services.Abstractions;
using Exam.API.Middleware;
using Exam.Domain.Repositories;
using Exam.Infrastructure.Persistance;
using Exam.Infrastructure.Persistance.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;


namespace Exam.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ExamDbContext>(opt =>
                opt.UseInMemoryDatabase("InMem"));


            //add service ServiceManager
            services.AddScoped<IServiceManager, ServiceManager>();

            //add service RepositoryManager
            services.AddScoped<IRepositoryManager, RepositoryManager>();


            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Exam.API", Version = "v1" });
            });

            services.AddTransient<ExceptionHandlingMiddleware>();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exam.API v1"));
            }

            //add ExceptionHandlingMiddleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Seeding data
            ExamDbContextSeed.PrepPopulation(app);
        }
    }
}