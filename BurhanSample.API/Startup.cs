using AspNetCoreRateLimit;
using BurhanSample.API.Extensions;
using BurhanSample.Business.Filters;
using BurhanSample.Business.ValidationRules;
using BurhanSample.Core.Extensions;
using BurhanSample.Core.Utilities.Models.DataShaping;
using BurhanSample.Core.Utilities.Models.RequestFeatures;
using BurhanSample.DataAccess.Mapping.Profiles;
using BurhanSample.Entities.Dto;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BurhanSample.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            /// Serilog, default adiyla  appsettings'den al�n�yor. Default parametre adi "Serilog" olarak tanimlanmali !!
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region for ModelState : Bad request'ler 500 hatas� olarak gozukmemesi icin

            //services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = true;
            //});

            #endregion


            services.AddAutoMapper(typeof(MappingProfile));         /// **** typeof(Startup) olmuyor. !

            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureLoggerService();
            services.ConfigureSqlContext(Configuration);
            services.ConfigureRepositoryManager();
            services.ConfigureBusinessManager();
            services.ConfiguringVersioning();

            // filters
            services.AddScoped<ValidationFilterAttribute>();

            // Identity
            services.AddAuthentication();
            services.ConfigureIdentity();
            services.ConfigureJWT(Configuration);

            // JWT service
            services.AddCustomAuthenticationService();

            // rate limiting
            services.AddMemoryCache();
            services.ConfigureRateLimitingOptions();
            //services.AddHttpContextAccessor();

            services.AddControllers();

            #region HttpContext config

            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #endregion

            #region Data shaping config

            services.AddScoped<IDataShaper<EmployeeDto>, DataShaper<EmployeeDto>>();

            #endregion


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "My API", 
                    Version = "v1",
                    Description = "CompanyEmployees API by CodeMaze",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact 
                    { 
                        Name = "Burhan", 
                        Email = "bsozer06@gmail.com", 
                        Url = new Uri("https://twitter.com/johndoe")
                    },
                    License = new OpenApiLicense 
                    {
                        Name = "CompanyEmployees API LICX", 
                        Url = new Uri("https://example.com/license")
                    } 
            });

                c.SwaggerDoc("v2", new OpenApiInfo { Title = "My API", Version = "v2" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer",
                        },
                        new List<string>()
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            #region Content type => XML
            /* Accept : text/xml olmal� */

            //services.AddControllers(config =>
            //{
            //    config.RespectBrowserAcceptHeader = true;
            //    config.ReturnHttpNotAcceptable = true;
            //}).AddXmlDataContractSerializerFormatters();

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "My API v2");
            });

            #region Custom middleware from my "Core" project

            app.ConfigureCustomExceptionMiddleware();

            #endregion

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseCors("CorsPolicyByBurhan");
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All
            });

            app.UseIpRateLimiting();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
