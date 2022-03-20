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
using Report.API.Application.Contracts.Infrastructure;


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


            // Review queries configuration
            services.AddScoped<IReviewQueries, ReviewQueries>(provider => new ReviewQueries
            (Configuration.GetConnectionString("ReportsConnection")));


            // Review repository configuration
            services.AddScoped<IReviewRepository, ReviewRepository>();


            // Request manager configuration (Idempotency)
            services.AddScoped<IRequestManager, RequestManager>();


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
