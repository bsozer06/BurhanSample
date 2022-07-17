using BurhanSample.Core.Services.Abstract;
using BurhanSample.Core.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurhanSample.Core.Extensions
{
    public static class ServiceCoreExtensions
    {
        #region Logging / Serilog

        public static void ConfigureLoggerService(this IServiceCollection services) =>
         services.AddScoped<ILoggerManager, LoggerManager>();

        #endregion

        #region JWT Service

        public static void AddCustomAuthenticationService(this IServiceCollection services) =>
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();

        #endregion
    }
}
