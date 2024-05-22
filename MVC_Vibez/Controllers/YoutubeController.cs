using Microsoft.AspNetCore.Mvc;
using MVC_Vibez.Model;
using MVC_Vibez.Services;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVC_Vibez.Controllers
{
    public class YoutubeController : Controller
    {
        private readonly ProgramService _ProgramService;
        private readonly string _apiKey = "AIzaSyBQ1G_Kei2BSW3veNJaWpWyiRLTD7ORqNk";
        public YoutubeController(ProgramService ProgramService)
        {
            _ProgramService = ProgramService;
        }
        public IActionResult Index()
        {
            var user = _ProgramService.GetUserByEmail(User.Identity.Name);
            return View(new ProgramPage { user = user });
        }

        [HttpGet]
        [Route("/YoutubeApi/SearchVideos")]
        public async Task<IActionResult> SearchVideos(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query is required.");
            }

            try
            {
                var videos = await FetchVideosFromYouTube(query);
                return Json(videos);
            }
            catch (Exception ex)
            {
                // Log the exception for diagnosis
                Console.WriteLine($"Error fetching YouTube API: {ex.Message}");
                return StatusCode(500, $"Failed to fetch videos. Error: {ex.Message}");
            }
        }

        private async Task<List<YoutubeVideo>> FetchVideosFromYouTube(string query)
        {
            var apiUrl = $"https://www.googleapis.com/youtube/v3/search?part=snippet&maxResults=10&q={query}&type=video&key={_apiKey}";
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(apiUrl);
                var jsonResponse = JObject.Parse(response);
                var videos = new List<YoutubeVideo>();

                foreach (var item in jsonResponse["items"])
                {
                    videos.Add(new YoutubeVideo
                    {
                        VideoId = item["id"]["videoId"].ToString()
                    });
                }

                return videos;
            }
        }
    }

    public class YoutubeVideo
    {
        public string VideoId { get; set; }
    }
}