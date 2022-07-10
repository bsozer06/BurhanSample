using BurhanSample.API.Service.Abstract;
using BurhanSample.API.Service.Concrete;
using BurhanSample.DAL.Abstract;
using BurhanSample.DAL.Concrete.EntityFramework;
using BurhanSample.DAL.Concrete.EntityFramework.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        #region Logging / Serilog

        public static void ConfigureLoggerService(this IServiceCollection services) =>
         services.AddScoped<ILoggerManager, LoggerManager>();

        #endregion

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
                    ,x => x.MigrationsAssembly("BurhanSample.DAL")
                ));

        #endregion

        #region Repository

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }

        #endregion
    }

}
