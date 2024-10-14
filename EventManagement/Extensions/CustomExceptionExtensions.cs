using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;

namespace EventManagement.Extensions
{
    public static class CustomExceptionExtensions
    {
        public static void HandleError(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    Console.WriteLine("In Extensions");
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    if (feature != null)
                    {
                        if(feature.Error is BadImageFormatException badImageFormatException)
                        {
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new {
                                statusCode = 776,
                                ErrorMessage = "Hello, From Custome Exception Handler",

                            }));
                            
                        }
                        else
                        {
                            var errorResponse = new
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = feature.Error.Message,
                                StackTrace = feature.Error.StackTrace
                            };
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorResponse));
                        }
                    }
                });
            });
        }
    }
}
