using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Model;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers;

public class LibraryController : Controller
{
    private readonly ProgramService _ProgramService;
    public LibraryController(ProgramService programService) => _ProgramService = programService;
    public IActionResult Index() => View(_ProgramService.GetUserByEmail(User.Identity.Name));
}