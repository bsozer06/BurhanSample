//using BurhanSample.API.Service.Abstract;
//using BurhanSample.API.Service.Concrete;
using BurhanSample.Core.Services.Abstract;
using BurhanSample.Core.Services.Concrete;

using BurhanSample.Business.Abstract;
using BurhanSample.Business.Concrete;
using BurhanSample.DAL.Abstract;
using BurhanSample.DAL.Concrete.EntityFramework;
using BurhanSample.DAL.Concrete.EntityFramework.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using System.Collections.Generic;
using AspNetCoreRateLimit;
using BurhanSample.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BurhanSample.API.Extensions
{
    public static class ServiceExtensions
    {
        #region CORS Config

        /// <summary>
        /// Configure CORS options as the extension.  
        /// Author=Burhan Sözer
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicyByBurhan", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                );
            });
        }

        #endregion

        #region IIS Config

        /// <summary>
        /// Configure IIS options.  
        /// Author=Burhan Sözer 
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureIISIntegration(this IServiceCollection services) =>
           services.Configure<IISOptions>(opt =>
           {
               opt.AutomaticAuthentication = true;
           });

        #endregion

        //#region Logging / Serilog

        /// Core içerisine tasindi...

        //public static void ConfigureLoggerService(this IServiceCollection services) =>
        // services.AddScoped<ILoggerManager, LoggerManager>();

        //#endregion

        #region Sql Connection

        /// <summary>
        /// Configure MSSQL context.
        /// Author=Burhan Sözer 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<RepositoryContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SqlConnection")
                    //, x => x.MigrationsAssembly("BurhanSample.DAL")
                ));

        #endregion

        #region Repository

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryCollection, RepositoryCollection>();
        }

        #endregion

        #region Business Logic Layer/ Manager

        public static void ConfigureBusinessManager(this IServiceCollection services)
        {
            services.AddScoped<ICompanyManager, CompanyManager>();
            services.AddScoped<IEmployeeManager, EmployeeManager>();
        }

        #endregion

        #region Service Versioning

        public static void ConfiguringVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(option =>
            {
                option.ReportApiVersions = true;
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.DefaultApiVersion = new ApiVersion(1, 0);
                //option.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }

        #endregion

        #region Rate limiting and Throttling

        /// <summary>
        /// X-Rate-Limit-Limit : rate limit period.
        /// X-Rate-Limit-Remaining : number of remaining requests.
        /// X-Rate-Limit-Reset : date/time information about resetting the request limit.
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Limit = 30,
                    Period = "5m"
                }
            };

            services.Configure<IpRateLimitOptions>(options =>
            {
                options.GeneralRules = rateLimitRules;
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        }


        #endregion

        #region Identity

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 5;
                options.User.RequireUniqueEmail = true;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);

            builder.AddEntityFrameworkStores<RepositoryContext>().AddDefaultTokenProviders();
        }

        #endregion

        #region JWT Configs

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            /// aslında bu saglikli degil !! Secretkey configde olmamalı !!1
            var secretKey = configuration["JwtSettings:secretKey"];     

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => 
            {
                options.TokenValidationParameters = new TokenValidationParameters 
                { 
                    ValidateIssuer = true, 
                    ValidateAudience = true, 
                    ValidateLifetime = true, 
                    ValidateIssuerSigningKey = true, 
                    ValidIssuer = jwtSettings.GetSection("validIssuer").Value, 
                    ValidAudience = jwtSettings.GetSection("validAudience").Value, 
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)) 
                }; 
            });
        }

        #endregion



    }
}
