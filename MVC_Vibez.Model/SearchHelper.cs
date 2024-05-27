using System.Net.Http.Headers;
using System.Text;
using MVC_Vibez.Model;
using Newtonsoft.Json;

namespace MVC_Vibez.Models;

public class SearchHelper
{
	private const string ClientID = "1230a4d844d54c93b4ad47f2718afcc4";
	private const string ClientSecret = "3f58ee04292643a290023c19345ece2a";
	private static Token _token;
	private static DateTime _tokenExpirationTime;
	private static readonly HttpClient _httpClient = new();

	private static async Task<Token> GetTokenAsync()
	{
		if (_token != null && DateTime.Now < _tokenExpirationTime) return _token;

		var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(ClientID + ":" + ClientSecret));
		var args = new List<KeyValuePair<string, string>> { new("grant_type", "client_credentials") };
		var content = new FormUrlEncodedContent(args);

		using var client = new HttpClient();
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);

		var resp = await client.PostAsync("https://accounts.spotify.com/api/token", content);
		resp.EnsureSuccessStatusCode();
		var msg = await resp.Content.ReadAsStringAsync();
		_token = JsonConvert.DeserializeObject<Token>(msg);
		_tokenExpirationTime = DateTime.Now.AddSeconds(_token.expires_in);

		return _token;
	}

	public static async Task<Welcome> SearchAll(string searchWord)
	{
		var token = await GetTokenAsync();
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
		var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/search?q={searchWord}&type=artist,album,playlist,track,show,episode,audiobook");

		if (response.IsSuccessStatusCode)
		{
			var content = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<Welcome>(content);
		}

		throw new Exception($"Failed to search for '{searchWord}'. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
	}

	public static async Task<List<PlaylistsItem>> GetRandomPlaylistsAsync(int count)
	{
		var token = await GetTokenAsync();
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
		var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/browse/featured-playlists?limit={count}");

		if (response.IsSuccessStatusCode)
		{
			var content = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<FeaturedPlaylistsResponse>(content);
			return result.playlists.Items;
		}

		throw new Exception($"Failed to get the playlists. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
	}

    public static async Task<PlaylistsItem> GetPlaylistAsync()
    {
        var token = await GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
        var response = await _httpClient.GetAsync("https://api.spotify.com/v1/playlists/47ukgyNSavcfwMEr4bWbku");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PlaylistsItem>(content);
            return result;
        }

        throw new Exception($"Failed to get the playlist. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
    }

	public static async Task AddTrackToPlaylistAsync(string trackUri)
	{
		var token = await GetTokenAsync();
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);

		var addTracksContent = new StringContent(JsonConvert.SerializeObject(new { uris = new[] { trackUri } }), Encoding.UTF8, "application/json");
		var response = await _httpClient.PostAsync("https://api.spotify.com/v1/playlists/47ukgyNSavcfwMEr4bWbku/tracks", addTracksContent);
		response.EnsureSuccessStatusCode();
	}
}