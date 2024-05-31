using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Model;
using MVC_Vibez.Models;
using MVC_Vibez.Services;

namespace MVC_Vibez.Controllers;

public class ProgramController : Controller
{
	private readonly LoginService _LoginService;

	public ProgramController(LoginService programService) => _LoginService = programService;

	public IActionResult Index() => View(new ProgramPage { user = _LoginService.GetUserByEmail(User.Identity.Name) });

	[HttpPost]
	public async Task<ActionResult> Autocomplete(string searchText)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(searchText)) return Json(new { success = false });

			var result = await SearchHelper.SearchAll(searchText);
			if (result == null) return Json(new { success = false });

			var artists = result.Artists.Items.Select(item => new Spotify
			{
				ID = item.Id,
				Image = item.Images.Count != 0 ? item.Images[0].Url.ToString() : "https://user-images.githubusercontent.com/24848110/33519396-7e56363c-d79d-11e7-969b-09782f5ccbab.png",
				Name = item.Name
			}).ToList();
			var songs = result.Tracks.Items.Select(item => new Spotify
			{
				ID = item.Id,
				Image = item.Album.Images.Count != 0 ? item.Album.Images[0].Url.ToString() : "https://user-images.githubusercontent.com/24848110/33519396-7e56363c-d79d-11e7-969b-09782f5ccbab.png",
				Name = item.Name,
				Uri = item.Uri,
				Artist = item.Artists[0].Name
			}).ToList();
			var albums = result.Albums.Items.Select(item => new Spotify
			{
				ID = item.Id,
				Image = item.Images.Count != 0 ? item.Images[0].Url.ToString() : "https://user-images.githubusercontent.com/24848110/33519396-7e56363c-d79d-11e7-969b-09782f5ccbab.png",
				Name = item.Name
			}).ToList();
			var playlists = result.Playlists.Items.Select(item => new Spotify
			{
				ID = item.Id,
				Image = item.Images.Count != 0 ? item.Images[0].Url.ToString() : "https://user-images.githubusercontent.com/24848110/33519396-7e56363c-d79d-11e7-969b-09782f5ccbab.png",
				Name = item.Name
			}).ToList();
			return Json(new { success = true, artists, songs, albums, playlists });
		}
		catch (Exception ex)
		{
			return Json(new { success = false, message = ex.Message });
		}
	}

	[HttpPost]
	public Task<IActionResult> AddToFavoriteAsync(PlaylistsItem song)
	{
		try
		{
			_ = SearchHelper.AddTrackToPlaylistAsync(song.Uri);
			return Task.FromResult<IActionResult>(RedirectToAction("Index"));
		}
		catch (Exception ex)
		{
			return Task.FromResult<IActionResult>(RedirectToAction("Index", new { error = ex.Message }));
		}
	}
}