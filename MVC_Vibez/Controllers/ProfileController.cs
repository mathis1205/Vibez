using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Core;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers;

public class ProfileController : Controller
{
    //Create an instance of the database as a readonly 
    private readonly VibezDbContext _dbContext;

    //Create a constructor to get the database
    public ProfileController(VibezDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    //Create a view of the index 
    [HttpGet]
    public IActionResult Index()
    {
        //Foreach to check whitch user is logged in and return the view with the user
        foreach (var user in _dbContext.Users)
            if (user.loggedin)
                return View(user);
        return View();
    }
}