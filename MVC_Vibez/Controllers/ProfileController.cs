using Microsoft.AspNetCore.Mvc;

namespace MVC_Vibez.Controllers
{
	public class ProfileController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
