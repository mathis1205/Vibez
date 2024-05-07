using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Models;
using System.Diagnostics;

namespace MVC_Vibez.Controllers
{
    public class HomeController : Controller
    {
        //create instance of the logger
        private readonly ILogger<HomeController> _logger;
        //Initialize the instance of logger
        public HomeController(ILogger<HomeController> logger) => _logger = logger;

        public IActionResult Index() => View();

        //create a result if the code would error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
