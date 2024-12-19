using MailKit.Security;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Net;

namespace EventManagement.Middleware
{
    //Tự custom 1 hàm bắt lỗi
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
                //Đây là hàm thực thi request khi có lỗi
                await _requestDelegate(context);
            }
            catch (Exception ex)
            {
                await ProcessException(context, ex);
            }
        }

        private async Task ProcessException(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            if (ex is BadImageFormatException badImageFormatException)
            {
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    statusCode = 400,
                    ErrorMessage = "Invalid image format",
                }));
            }
            else if (ex is UnauthorizedAccessException unauthorizedAccessException)
            {
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    statusCode = 403,
                    ErrorMessage = "You don't have Permission",
                }));
            }
            else if (ex is AuthenticationException authenticationException)
            {
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    statusCode = 401,
                    ErrorMessage = "You need login",
                }));
            }
            else if (ex is SqlException sqlException)
            {
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    statusCode = 500,
                    ErrorMessage = "An unexpected error occurred." + ex,
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
