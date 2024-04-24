using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Models;
using System.Diagnostics;

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
