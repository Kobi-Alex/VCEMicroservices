using System;

using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Question.API.Middleware;
using Question.Infrastructure;
using Question.Domain.Repositories;
using Question.API.Application.Services;
using Question.API.Application.Services.Interfaces;
using Question.Infrastructure.Persistance.Repositories;

using MassTransit;
using RabbitMQ.Client;
using Question.API.Grpc;
using GrpcReport;

namespace Question.API
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

                // SQL DB configuration
                services.AddDbContext<QuestionDbContext>(opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString("QuestionsConnection")));

            }
            else
            {
                Console.WriteLine("--> Using InMem DB");

                // InMemory DB configuration
                services.AddDbContext<QuestionDbContext>(opt =>
                    opt.UseInMemoryDatabase("InMem"));
            }


            // ServiceManager configuration
            services.AddScoped<IServiceManager, ServiceManager>();

            // RepositoryManager configuration
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            // gRPC configuration (ReportGrpcService)
            services.AddGrpcClient<ReportGrpc.ReportGrpcClient>
                        (o => o.Address = new Uri(Configuration["GrpcReportSettings:ReportUrl"]));
            services.AddScoped<ReportGrpcService>();


            // gRPC configuration
            services.AddGrpc();

            // MassTransit-RabbitMQ ņonfiguration
            services.AddMassTransit(config => {
                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(Configuration["EventBusSettings:HostAddress"]);
                });
            });
            services.AddMassTransitHostedService();


            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Question.API", Version = "v1" });
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Question.API v1"));
            }

            //add ExceptionHandlingMiddleware
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<QuestionGrpcService>();
            });

            //add Seeding data
            QuestionDbContextSeed.PrepPopulation(app);
        }

        //private void MassTransitConfigure(IServiceCollection services)
        //{
        //    var queueSettingsSection = Configuration.GetSection("RabbitMQ:QueueSettings");
        //    var queueSettings = queueSettingsSection.Get<QueueSettings>();


        //    services.AddMassTransit(config => {

        //        config.UsingRabbitMq((ctx, cfg) =>
        //        {
        //            //cfg.Host("amqp://<username>:<password>@<hostname>:<port>/");
        //            cfg.Host(queueSettings.HostName, queueSettings.Port, queueSettings.VirtualHost,
        //             h => {
        //                 h.Username(queueSettings.UserName);
        //                 h.Password(queueSettings.Password);
        //             });
        //            cfg.ExchangeType = ExchangeType.Direct;
        //        });


        //        services.AddMassTransitHostedService();
        //    });

        //}

    }
}
