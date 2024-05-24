using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;
using HtmlAgilityPack;

namespace MVC_Vibez.Model;

public class GeniusSearch
{
    private readonly string clientId;
    private readonly string clientSecret;
    private readonly string redirectUri;
    private const string AccessToken = "5zMOXvjfUgpx2H0zHmI01-xEkgWRRvS3rZGV09oV_hpJinMRVLj_q3k1Wm0jtxg3";


    public GeniusSearch(IOptions<GeniusSearchOptions> options)
    {
        clientId = options.Value.ClientId;
        clientSecret = options.Value.ClientSecret;
        redirectUri = options.Value.RedirectUri;
    }

    public async Task<string> GetAccessToken(string code)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("https://api.genius.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("redirect_uri", redirectUri),
                new KeyValuePair<string, string>("response_type", "code"),
                new KeyValuePair<string, string>("grant_type", "authorization_code")
            });

            var response = await client.PostAsync("oauth/token", content);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<Dictionary<string, string>>();
                return data["access_token"];
            }

            throw new Exception("Failed to get access token");
        }
    }

    public async Task<List<GeniusHit>> SearchSongs(string searchTerm)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("https://api.genius.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            var response = await client.GetAsync($"search?q={Uri.EscapeDataString(searchTerm)}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<GeniusSearchResult>();
                return data.response.hits;
            }

            throw new Exception("Failed to search songs");
        }
    }
    public async Task<GeniusSong> GetSongDetails(int id)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("https://api.genius.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

            var response = await client.GetAsync($"songs/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<GeniusSongResult>();
                return data.response.song;
            }

            throw new Exception("Failed to get song details");
        }
    }

    public async Task<string> GetLyrics(string path)
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync($"https://genius.com{path}");
            if (response.IsSuccessStatusCode)
            {
                var pageContent = await response.Content.ReadAsStringAsync();
                var pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(pageContent);

                var lyricsDivs = pageDocument.DocumentNode.SelectNodes("//div[contains(@class, 'Lyrics__Container-sc-1ynbvzw-1') and contains(@class, 'kUgSbL')]");
                if (lyricsDivs == null || !lyricsDivs.Any())
                {
                    throw new Exception("Failed to get lyrics");
                }

                var lyrics = new StringBuilder();
                foreach (var lyricsDiv in lyricsDivs)
                {
                    lyrics.AppendLine(lyricsDiv.InnerText + "<br>" +"<br>");

                    //var spans = lyricsDiv.SelectNodes(".//span");
                    //if (spans != null)
                    //{
                    //    foreach (var span in spans)
                    //    {
                    //        lyrics.AppendLine(span.InnerText + "<br>");
                    //    }
                    //}
                }


                var lyricsText = lyrics.ToString();
                lyricsText = lyricsText.Replace("&#x27;", "'").Replace("[", " [").Replace("&quot;", "'' ").Replace("&amp;", "&");
                return lyricsText;
                
            }

            throw new Exception("Failed to get lyrics");
        }
    }
}