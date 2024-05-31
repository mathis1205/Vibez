using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;

namespace MVC_Vibez.Model;

public class GeniusSearch
{
    private const string AccessToken = "ic7vpBFBv8hoVHpnUSSOfRPUDBHNLn4pgWgvtcfu6A0VrsS9IJXUfkqFFlqJEJoA";
    private static readonly HttpClient Client;
    private readonly string clientId;
    private readonly string clientSecret;
    private readonly string redirectUri;

    static GeniusSearch()
    {
        Client = new HttpClient { BaseAddress = new Uri("https://api.genius.com/") };
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
    }

    public GeniusSearch(IOptions<GeniusSearchOptions> options)
    {
        clientId = options.Value.ClientId;
        clientSecret = options.Value.ClientSecret;
        redirectUri = options.Value.RedirectUri;
    }

    public static async Task<List<GeniusHit>> SearchSongs(string searchTerm)
    {
        var response = await Client.GetAsync($"search?q={Uri.EscapeDataString(searchTerm)}");
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<GeniusSearchResult>();
        return data?.response.hits ?? [];
    }

    public static async Task<GeniusSong> GetSongDetails(int id)
    {
        var response = await Client.GetAsync($"songs/{id}");
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<GeniusSongResult>();
        return data?.response.song ?? throw new Exception("Song details not found");
    }

    public static async Task<string> GetLyrics(string path)
    {
        HttpResponseMessage response;
        try
        {
            response = await Client.GetAsync($"https://genius.com{path}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to make GET request", ex);
        }

        string pageContent;
        try
        {
            pageContent = await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to read response content", ex);
        }

        var pageDocument = new HtmlDocument();
        pageDocument.LoadHtml(pageContent);

        var lyricsDivs = pageDocument.DocumentNode.SelectNodes("//div[starts-with(@class, 'Lyrics__Container')]");
        if (lyricsDivs == null || lyricsDivs.Count == 0) throw new Exception("Failed to get lyrics");

        var lyrics = new StringBuilder();
        foreach (var lyricsDiv in lyricsDivs) lyrics.AppendLine(lyricsDiv.InnerText + "<br>" + "<br>");

        var lyricsText = lyrics.ToString();
        return lyricsText.Replace("&#x27;", "'").Replace("[", " [").Replace("&quot;", "\"").Replace("&amp;", "&");
    }
}