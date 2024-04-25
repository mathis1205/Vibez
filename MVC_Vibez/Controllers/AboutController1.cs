using Microsoft.AspNetCore.Mvc;

namespace MVC_Vibez.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/About/Index.cshtml");
        }
    }
}
