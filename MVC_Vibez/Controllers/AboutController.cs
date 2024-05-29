using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers
{
    public class AboutController : Controller
    {
        private readonly LoginService _LoginService;
        public AboutController(LoginService programService) => _LoginService = programService;
        public IActionResult Index() => View(_LoginService.GetUserByEmail(User.Identity.Name));
    }
}
