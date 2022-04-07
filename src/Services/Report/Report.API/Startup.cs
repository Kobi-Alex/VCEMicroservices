using System;
using System.Reflection;

using MediatR;
using GrpcExam;
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
using Report.API.Application.Models;
using Report.API.Application.Behaviours;
using Report.API.Application.Services.Mail;
using Report.API.Application.Features.Queries;
using Report.Infrastructure.Persistance.Idempotency;
using Report.Domain.AggregatesModel.ReviewAggregate;
using Report.Infrastructure.Persistance.Repositories;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Report.API.Application.Contracts.Infrastructure;
using Report.API.Application.Services.Interfaces;
using Report.API.Application.Services;

namespace Report.API
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

            // Database connection

            if (_env.IsDevelopment())
            {
                Console.WriteLine("\n---> Using SqlServer Db Development\n");
                services.AddDbContext<ReportDbContext>(opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString("ReportsConnection")));
            }

            if(_env.IsStaging())
            {
                Console.WriteLine("\n---> Using SqlServer Db Staging\n");
                services.AddDbContext<ReportDbContext>(opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString("ReportsConnection")));
            }

            if (_env.IsProduction())
            {
                Console.WriteLine("\n---> Using InMem Db Production\n");

                services.AddDbContext<ReportDbContext>(opt =>
                   opt.UseInMemoryDatabase("InMem"));
            }


            services.AddScoped<IServiceManager, ServiceManager>();

            // Review queries configuration
            services.AddScoped<IReviewQueries, ReviewQueries>(provider => new ReviewQueries(Configuration.GetConnectionString("ReportsConnection")));



            // Review repository configuration
            services.AddScoped<IReviewRepository, ReviewRepository>();


            // Request manager configuration (Idempotency)
            services.AddScoped<IRequestManager, RequestManager>();

            services.AddGrpc();

            // gRPC configuration (Question Service)
            services.AddGrpcClient<QuestionGrpc.QuestionGrpcClient>
                        (o => o.Address = new Uri(Configuration["GrpcQuestionSettings:QuestionUrl"]));
            services.AddScoped<QuestionGrpcService>();


            // gRPC configuration (Exam Service)
            services.AddGrpcClient<ExamGrpc.ExamGrpcClient>
                        (o => o.Address = new Uri(Configuration["GrpcExamSettings:ExamUrl"]));
            services.AddScoped<ExamGrpcService>();




            // CQRS configuration
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));


            // Email configuration
            services.Configure<EmailSettings>(c => Configuration.GetSection("EmailSettings").Bind(c));
            services.AddTransient<IEmailService, EmailService>();

            // gRPC configuration
            services.AddGrpc();

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
            app.UseCors("AllowOrigin");
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<ReportGrpcService>();
                endpoints.MapGrpcService<ExamGrpcService>();
            });
        }

    }
}
