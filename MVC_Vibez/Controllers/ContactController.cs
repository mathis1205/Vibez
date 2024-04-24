using Microsoft.AspNetCore.Mvc;

namespace MVC_Vibez.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
