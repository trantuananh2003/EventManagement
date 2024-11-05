using Microsoft.AspNetCore.Mvc;

namespace EventManagement.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
