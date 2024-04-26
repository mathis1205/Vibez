using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Core;
using MVC_Vibez.Models;

namespace MVC_Vibez.Controllers
{
    public class ProfileController : Controller
    {
        private readonly VibezDbContext _context;

        public ProfileController(VibezDbContext context) => _context = context;

        [HttpGet] public IActionResult Index()
        {
            foreach (var user in _context.Users)
            {
                if(user.loggedin) return View(user);
            }
            return View();
        }
    }
}
