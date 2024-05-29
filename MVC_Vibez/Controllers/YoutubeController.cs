using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Model;
using MVC_Vibez.Services;
using Newtonsoft.Json.Linq;

namespace MVC_Vibez.Controllers;

public class YoutubeController : Controller
{
    private readonly string _apiKey = "AIzaSyBQ1G_Kei2BSW3veNJaWpWyiRLTD7ORqNk";
    private readonly LoginService _LoginService;

    public YoutubeController(LoginService ProgramService) => _LoginService = ProgramService;

    public IActionResult Index() => View(new ProgramPage { user = _LoginService.GetUserByEmail(User.Identity.Name) });

    [HttpGet, Route("/YoutubeApi/SearchVideos")]
    public async Task<IActionResult> SearchVideos(string query)
    {
        if (string.IsNullOrEmpty(query)) return BadRequest("Query is required.");

        try { return Json(await FetchVideosFromYouTube(query)); }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching YouTube API: {ex.Message}");
            return StatusCode(500, $"Failed to fetch videos. Error: {ex.Message}");
        }
    }

    private async Task<List<YoutubeVideo>> FetchVideosFromYouTube(string query)
    {
        var apiUrl = $"https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=24&q={query}&type=video&key={_apiKey}";
        using var httpClient = new HttpClient();
        var jsonResponse = JObject.Parse(await httpClient.GetStringAsync(apiUrl));

        return jsonResponse["items"].Select(item => new YoutubeVideo { VideoId = item["id"]["videoId"].ToString() }).ToList();
    }
}

public class YoutubeVideo { public string VideoId { get; set; } }