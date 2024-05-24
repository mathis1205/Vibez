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
        var hits = await _geniusSearch.SearchSongs(searchTerm ?? string.Empty);
        return View("Index", new ProgramPage { user = user, Hits = hits, SearchPerformed = true });
    }

    public async Task<IActionResult> Lyrics(string path, string title, string artist, int id)
    {
        var user = _programService.GetUserByEmail(User.Identity.Name);
        var lyrics = await _geniusSearch.GetLyrics(path);
        var songDetails = await _geniusSearch.GetSongDetails(id);
        var selectedHit = new GeniusHit
        {
            result = new GeniusResult
            {
                title = title,
                primary_artist = new GeniusArtist { name = artist },
                SongArtImageUrl = songDetails.song_art_image_url,
                ReleaseDateForDisplay = songDetails.release_date_for_display,
                FeaturedArtists = songDetails.featured_artists
            }
        };
        var model = new ProgramPage { user = user, Lyrics = lyrics, SelectedHit = selectedHit, SearchPerformed = false };
        return View("Index", model);
    }
}





