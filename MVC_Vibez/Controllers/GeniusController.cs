﻿using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Model;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers;

public class GeniusController : Controller
{
    private readonly LoginService _LoginService;

    public GeniusController(LoginService programService) { _LoginService = programService; }

    public Task<IActionResult> Index()
    {
        var user = _LoginService.GetUserByEmail(User.Identity.Name);
        var model = new ProgramPage { user = user, SearchPerformed = false };
        return Task.FromResult<IActionResult>(View(model));
    }

    public async Task<IActionResult> SearchResults(string searchTerm)
    {
        var user = _LoginService.GetUserByEmail(User.Identity.Name);
        var model = new ProgramPage { user = user, SearchPerformed = true };

        if (string.IsNullOrEmpty(searchTerm)) return View("Index", model);
        model.Hits = await GeniusSearch.SearchSongs(searchTerm);

        return View("Index", model);
    }

    public async Task<IActionResult> SongDetails(string path, string title, string artist, int id)
    {
        var user = _LoginService.GetUserByEmail(User.Identity.Name);
        var model = new ProgramPage { user = user, SearchPerformed = false };

        if (string.IsNullOrEmpty(path) || id == 0) return View("Index", model);
        var lyrics = await GeniusSearch.GetLyrics(path);
        var songDetails = await GeniusSearch.GetSongDetails(id);
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
        model.Lyrics = lyrics;
        model.SelectedHit = selectedHit;

        return View("Index", model);
    }

    public IActionResult Search(string searchTerm) => RedirectToAction("SearchResults", new { searchTerm });
    public IActionResult Lyrics(string path, string title, string artist, int id) => RedirectToAction("SongDetails", new { path, title, artist, id });
}