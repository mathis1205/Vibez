using Microsoft.AspNetCore.Mvc;

namespace MVC_Vibez.Controllers
{
    public class ProgramController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
