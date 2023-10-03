using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace eathappy.order.api.Extensions
{
#pragma warning disable CS1591 // Unrecognized #pragma directive
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
                throw;
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var code = HttpStatusCode.InternalServerError;

            var result = JsonConvert.SerializeObject(new ProblemDetails
            {
                Title = exception.Message,
                Status = 500,
                Detail = exception.InnerException?.Message
            });

            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(result);
        }
    }
#pragma warning restore CS1591 // Unrecognized #pragma directive
}
