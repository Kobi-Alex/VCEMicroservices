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
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
            var key = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);


            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false
            };
            services.AddSingleton(tokenValidationParams);

            services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenValidationParams;
            });

            //Auth <------------------------------------------------------------------------------------------------>

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            //Console.WriteLine("--> Using InMem DB");
            //services.AddDbContext<ExamDbContext>(opt =>
            //    opt.UseSqlServer(Configuration.GetConnectionString("ExamsConnection")));


            if (_env.IsProduction())
            {
                //add service InMemory DB
                Console.WriteLine("\n---> Using InMem Db Production\n");
                services.AddDbContext<ExamDbContext>(opt =>
                    opt.UseInMemoryDatabase("InMem"));
            }
            if (_env.IsDevelopment())
            {
                Console.WriteLine("\n---> Using SqlServer Db Development\n");
                services.AddDbContext<ExamDbContext>(opt =>
                   opt.UseSqlServer(Configuration.GetConnectionString("ExamsConnection")));
            }

            if(_env.IsStaging())
            {
                Console.WriteLine("\n---> Using SQL Server Db Staging\n");

                services.AddDbContext<ExamDbContext>(opt =>
                   opt.UseSqlServer(Configuration.GetConnectionString("ExamsConnection")));
            }

            //add service ServiceManager
            services.AddScoped<IServiceManager, ServiceManager>();

            //add service RepositoryManager
            services.AddScoped<IRepositoryManager, RepositoryManager>();

            services.AddScoped<IExamIntegrationEventService, ExamIntegrationEventService>();

            // gRPC configuration
            services.AddGrpc();

            // MassTransit-RabbitMQ Configuration
            services.AddMassTransit(config =>
            {
                config.AddConsumer<ExamIntegrationEventService>();

                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(Configuration["EventBusSettings:HostAddress"]);
                    cfg.ReceiveEndpoint(EventBusConstants.QuestionItemDeleteQueue, c =>
                    {
                        c.ConfigureConsumer<ExamIntegrationEventService>(ctx);
                    });
                });
            });
            services.AddMassTransitHostedService();


            services.AddControllers();

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

            app.UseCors("AllowOrigin");


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