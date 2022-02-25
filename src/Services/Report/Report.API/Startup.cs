using System;
using System.Reflection;

using MediatR;
using GrpcQuestion;
using FluentValidation;

using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Report.API.Grpc;
using Report.API.Middleware;
using Report.Infrastructure;
using Report.API.Application.Behaviours;
using Report.API.Application.Features.Queries;
using Report.Domain.AggregatesModel.ReviewAggregate;
using Report.Infrastructure.Persistance.Repositories;

namespace Report.API
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
            // Database connection
            services.AddDbContext<ReportDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetConnectionString("ReportsConnection")));

            // ReviewQueries configuration
            services.AddScoped<IReviewQueries, ReviewQueries>(provider => new ReviewQueries
            (Configuration.GetConnectionString("ReportsConnection")));

            // ReviewRepository configuration
            services.AddScoped<IReviewRepository, ReviewRepository>();

            // gRPC configuration
            services.AddGrpcClient<QuestionGrpc.QuestionGrpcClient>
                        (o => o.Address = new Uri(Configuration["GrpcSettings:QuestionUrl"]));
            services.AddScoped<QuestionGrpcService>();

            // CQRS configuration
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            // Controller configuration
            services.AddControllers();

            // Swagger configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Report.API", Version = "v1" });
            });


            // Middleware configuration
            services.AddTransient<ExceptionHandlingMiddleware>();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Report.API v1"));
            }

            //add ExceptionHandlingMiddleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
