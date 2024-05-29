using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Model;
using MVC_Vibez.Models;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers;

public class LibraryController : Controller
{
    private readonly LoginService _LoginService;

    public LibraryController(LoginService ProgramService) { _LoginService = ProgramService; }

    public async Task<IActionResult> Index()
    {
        var user = _LoginService.GetUserByEmail(User.Identity.Name);
        var playlists = await SearchHelper.GetRandomPlaylistsAsync(35);
        var favoritSongs = await SearchHelper.GetPlaylistAsync();
        return View(new ProgramPage { user = user, playlists = playlists, FavoritSongs = favoritSongs});
    }

    public IActionResult RemoveFromFavorites(string itemId)
    {
        return RedirectToAction("Index");
    }
}