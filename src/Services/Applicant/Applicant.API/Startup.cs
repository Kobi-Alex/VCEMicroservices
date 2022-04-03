using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Applicant.Infrasructure;
using Applicant.Domain.Repositories;
using Applicant.API.Application.Services;
using Applicant.API.Application.Configurations;
using Applicant.API.Application.Services.Interfaces;
using Applicant.Infrasructure.Persistance.Repositories;



namespace Applicant.API
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
            if (_env.IsStaging())
            {
                Console.WriteLine("\n---> Staging");
                Console.WriteLine("\n---> Using SQL Server Staging\n");

                services.AddDbContext<AppDbContext>(opt =>
                     opt.UseSqlServer(Configuration.GetConnectionString("UsersConnection")));
            }

            if (_env.IsProduction())
            {
                Console.WriteLine("\n---> Production");
                Console.WriteLine("\n---> Using InMem Db Production\n");

                services.AddDbContext<AppDbContext>(opt =>
                   opt.UseInMemoryDatabase("InMem"));

                //services.AddDbContext<AppDbContext>(opt =>
                //    opt.UseSqlServer(Configuration.GetConnectionString("UsersConnection")));
            }

            if (_env.IsDevelopment())
            {
                Console.WriteLine("\n---> Development");
                Console.WriteLine("\n---> Using SqlServer Db Development\n");
                Console.WriteLine();

                services.AddDbContext<AppDbContext>(opt =>
                    opt.UseSqlServer(Configuration.GetConnectionString("UsersConnection")));

                //services.AddDbContext<AppDbContext>(opt =>
                //   opt.UseInMemoryDatabase("InMem"));
            }

            //add service CustomAuthentication
            services.AddCustomAuthentication(Configuration);

            //add service EmailConfiguration
            services.AddSingleton(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());

            services.AddCors(c => 
            { 
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            //add service ServiceManager
            services.AddScoped<IServiceManager, ServiceManager>();

            //add service RepositoryManager
            services.AddScoped<IRepositoryManager, RepositoryManager>();


            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Applicant.API", Version = "v1" });
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Applicant.API v1"));
            }

            app.UseRouting();
            app.UseCors("AllowOrigin");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PrepDb.PrepPopulation(app, env.IsProduction());
        }
    }
}


static class CustomExtensionsMethods
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
        var key = Encoding.ASCII.GetBytes(configuration["JwtConfig:Secret"]);

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

        services.AddAuthentication(options =>
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

        return services;
    }

}