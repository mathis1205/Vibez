﻿using Microsoft.AspNetCore.Mvc;

namespace MVC_Vibez.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            //returns the view of the action
            return View("~/Views/About/Index.cshtml");
        }
    }
}
