using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Model;
using MVC_Vibez.Services;
using System.Net.Http.Headers;

namespace MVC_Vibez.Controllers;

public class GeniusController : Controller
{
    private readonly GeniusSearch _geniusSearch;
    private readonly ProgramService _programService;

    public GeniusController(GeniusSearch geniusSearch, ProgramService programService)
    {
        _geniusSearch = geniusSearch;
        _programService = programService;
    }

    public IActionResult Index()
    {
        var user = _programService.GetUserByEmail(User.Identity.Name);
        return View(new ProgramPage { user = user });
    }

    public async Task<IActionResult> Search(string searchTerm)
    {
        var user = _programService.GetUserByEmail(User.Identity.Name);
        var hits = await _geniusSearch.SearchSongs(searchTerm);
        return View("Index", new ProgramPage { user = user, Hits = hits });
    }

    public async Task<IActionResult> Lyrics(string path)
    {
        var user = _programService.GetUserByEmail(User.Identity.Name);
        var lyrics = await _geniusSearch.GetLyrics(path);
        var hits = await _geniusSearch.SearchSongs(""); // Keep the previous search results
        return View("Index", new ProgramPage { user = user, Hits = hits, Lyrics = lyrics });
    }

}





