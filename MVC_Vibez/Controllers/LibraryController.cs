using Microsoft.AspNetCore.Mvc;

namespace MVC_Vibez.Controllers
{
    public class LibraryController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Library/Index.cshtml");
        }
    }
}
