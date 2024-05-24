using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Model;
using MVC_Vibez.Models;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers;

public class LibraryController : Controller
{
    private readonly ProgramService _ProgramService;

    public LibraryController(ProgramService ProgramService)
    {
        _ProgramService = ProgramService;
    }

    public async Task<IActionResult> Index()
    {
        var user = _ProgramService.GetUserByEmail(User.Identity.Name);
        var playlists = await SearchHelper.GetRandomPlaylistsAsync(36);
        var favoritSongs = await SearchHelper.GetPlaylistAsync();
        return View(new ProgramPage { user = user, playlists = playlists, FavoritSongs = favoritSongs});
    }

    public IActionResult RemoveFromFavorites(string itemId)
    {
        return RedirectToAction("Index");
    }
}