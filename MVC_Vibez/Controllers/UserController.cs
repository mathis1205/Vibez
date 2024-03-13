using Microsoft.AspNetCore.Mvc;

namespace MVC_Vibez.Controllers
{
    public class UserController : Controller
    { 
        public IActionResult Index()
        {
            return View();
        }
    }
}
