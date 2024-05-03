using System.Text;
using Newtonsoft.Json;
using RestSharp;

namespace MVC_Vibez.Models;

public class SearchHelper
{
    private const string ClientID = "1230a4d844d54c93b4ad47f2718afcc4";
    private const string ClientSecret = "3f58ee04292643a290023c19345ece2a";
    private static Token _token;
    private static DateTime _tokenExpirationTime;

    public static async Task<Token> GetTokenAsync()
    {
        if (_token != null && DateTime.Now < _tokenExpirationTime) return _token;

        var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(ClientID + ":" + ClientSecret));
        var args = new List<KeyValuePair<string, string>> { new("grant_type", "client_credentials") };
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Basic {auth}");
        HttpContent content = new FormUrlEncodedContent(args);

        var resp = await client.PostAsync("https://accounts.spotify.com/api/token", content);
        var msg = await resp.Content.ReadAsStringAsync();
        _token = JsonConvert.DeserializeObject<Token>(msg)!;
        _tokenExpirationTime = DateTime.Now.AddSeconds(_token!.expires_in);

        return _token;
    }

    public static async Task<SpotifySearch.Welcome> SearchAll(string searchWord)
    {
        var token = await GetTokenAsync();
        var client = new RestClient("https://api.spotify.com/v1/search");
        client.AddDefaultHeader("Authorization", $"Bearer {token.access_token}");
        var request = new RestRequest($"?q={searchWord}&type=artist,album,playlist,track,show,episode,audiobook");
        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful) return JsonConvert.DeserializeObject<SpotifySearch.Welcome>(response.Content);

        throw new Exception($"Failed to search for '{searchWord}'. Status code: {response.StatusCode}, Error: {response.ErrorMessage}");
    }

    public static async Task<List<Playlist>> GetRandomPlaylistsAsync(int count)
    {
        var token = await GetTokenAsync();
        var client = new RestClient("https://api.spotify.com/v1/browse/featured-playlists");
        client.AddDefaultHeader("Authorization", $"Bearer {token.access_token}");
        var request = new RestRequest($"?limit={count}");
        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            var result = JsonConvert.DeserializeObject<FeaturedPlaylistsResponse>(response.Content);
            return result.playlists.items;
        }

        throw new Exception($"Failed to get the playlists. Status code: {response.StatusCode}, Error: {response.ErrorMessage}");
    }
}

public class Playlist
{
    public string id { get; set; }
    public string name { get; set; }
    public List<Image> images { get; set; }
    public Owner owner { get; set; }
    public Tracks tracks { get; set; }
}

public class Owner
{
    public string display_name { get; set; }
}

public class Tracks
{
    public List<PlaylistItem> items { get; set; }
}

public class PlaylistItem
{
    public Track track { get; set; }
}

public class Track
{
    public string name { get; set; }
    public List<Artist> artists { get; set; }
}

public class Artist
{
    public string name { get; set; }
}

public class Image
{
    public string url { get; set; }
}

public class FeaturedPlaylistsResponse
{
    public Playlists playlists { get; set; }
}

public class Playlists
{
    public List<Playlist> items { get; set; }
}