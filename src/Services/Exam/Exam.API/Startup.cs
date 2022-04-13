using System;

using MassTransit;
using EventBus.Common;

using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Exam.API.Grpc;
using Exam.API.Middleware;
using Exam.Infrastructure;
using Exam.Domain.Repositories;
using Exam.API.Application.Services;
using Exam.API.Application.IntegrationEvents;
using Exam.API.Application.Services.Interfaces;
using Exam.Infrastructure.Persistance.Repositories;


namespace Exam.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment _env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsProduction())
            {
                Console.WriteLine("--> Using SQL DB");

                services.AddDbContext<ExamDbContext>(opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString("ExamsConnection")));
            }
            else
            {
                Console.WriteLine("--> Using InMem DB");

                services.AddDbContext<ExamDbContext>(opt =>
                    opt.UseInMemoryDatabase("InMem"));
            }

            //add service ServiceManager
            services.AddScoped<IServiceManager, ServiceManager>();

            //add service RepositoryManager
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            services.AddScoped<IExamIntegrationEventService, ExamIntegrationEventService>();

            // gRPC configuration
            services.AddGrpc();

            //// MassTransit-RabbitMQ Configuration
            //services.AddMassTransit(config => {

            //    config.AddConsumer<ExamIntegrationEventService>();

            //    config.UsingRabbitMq((ctx, cfg) => {
            //        cfg.Host(Configuration["EventBusSettings:HostAddress"]);
            //        cfg.ReceiveEndpoint(EventBusConstants.QuestionItemDeleteQueue, c =>
            //        {
            //            c.ConfigureConsumer<ExamIntegrationEventService>(ctx);
            //        });
            //    });
            //});
            //services.AddMassTransitHostedService();


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
                endpoints.MapGrpcService<ExamGrpcService>();
            });

            //Seeding data
            ExamDbContextSeed.PrepPopulation(app, env.IsProduction());
        }
    }
}