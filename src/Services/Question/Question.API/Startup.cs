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
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Question.API.Grpc;


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
                Console.WriteLine("--> Using SQL DB Production");

                // SQL DB configuration
                //services.AddDbContext<QuestionDbContext>(opt =>
                //    opt.UseSqlServer(Configuration.GetConnectionString("QuestionsConnection")));

                services.AddDbContext<QuestionDbContext>(opt =>
                   opt.UseInMemoryDatabase("InMem"));

            }
            else
            {
                Console.WriteLine("\n---> Using SqlServer Db Development\n");
                services.AddDbContext<QuestionDbContext>(opt =>
                   opt.UseSqlServer(Configuration.GetConnectionString("QuestionConnection")));
                //add service InMemory DB
                //services.AddDbContext<QuestionDbContext>(opt =>
                //    opt.UseInMemoryDatabase("InMem"));

                //add service InMemory DB
                //services.AddDbContext<QuestionDbContext>(opt =>
                //    opt.UseInMemoryDatabase("InMem"));
            }


            //Auth <------------------------------------------------------------------------------------------------>

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

            //add service ServiceManager
            services.AddScoped<IServiceManager, ServiceManager>();

            // RepositoryManager configuration
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            
            // gRPC configuration
            services.AddGrpc();

            // MassTransit-RabbitMQ ñonfiguration
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

            app.UseCors("AllowOrigin");

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
