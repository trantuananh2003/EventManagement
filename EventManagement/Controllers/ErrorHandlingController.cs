using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ErrorHandlingController : ControllerBase
    {
        [HttpGet("ProcessError")]
        public IActionResult ProcessError([FromServices] IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment.IsDevelopment())
            {
                //Custom logic
                var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
                if(feature != null)
                {
                    return Problem(
                        detail: feature.Error.StackTrace,
                        title: feature.Error.Message,
                        instance: hostEnvironment.EnvironmentName);
                }
            }

            return Problem();
        }
    }
}
