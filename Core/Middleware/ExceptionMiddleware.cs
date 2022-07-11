using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BurhanSample.Core.Utilities.Models;
using BurhanSample.Core.Services.Abstract;
using Microsoft.AspNetCore.Diagnostics;

namespace BurhanSample.Core.Extensions
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ILoggerManager logger)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex, logger);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex, ILoggerManager logger)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

            logger.LogError("----------------------------------------------------------");
            logger.LogError($"Something went wrong: {ex.Message}");
            await httpContext.Response.WriteAsync(new ErrorModel()
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = ex.Message ?? ex.InnerException.ToString()
            }.ToString());

        }
    }
}
