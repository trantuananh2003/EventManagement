using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Net;

namespace EventManagement.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public CustomExceptionMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception ex)
            {
                await ProcessException(context, ex);
            }
        }

        private async Task ProcessException(HttpContext context, Exception ex)
        {
            Console.WriteLine("In Custom Middleware");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            if (ex is BadImageFormatException badImageFormatException)
            {
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    statusCode = 776,
                    ErrorMessage = "Hello, From Custom Exception Handler",
                }));
            }
            else if (ex is SqlException sqlException)
            {
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    statusCode = 776,
                    ErrorMessage = "Hello, From Custom Exception Handler",
                }));
            }
            else
            {
                var errorResponse = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace
                };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
            }
        }
    }
}
