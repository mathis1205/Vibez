using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace MVC_Vibez.Models
{
    public class SearchHelper
    {
	    private static Token _token;
	    private static DateTime _tokenExpirationTime;

        private const string ClientID = "1230a4d844d54c93b4ad47f2718afcc4";
        private const string ClientSecret = "3f58ee04292643a290023c19345ece2a";

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
            _tokenExpirationTime = DateTime.Now.AddSeconds(_token!.expires_in); // Set token expiration time

            return _token;
	    }

	    public static async Task<SpotifySearch.SpotifyResult> SearchArtistOrSong(string searchWord)
	    {
		    var token = await GetTokenAsync(); // Ensure token is valid
		    var client = new RestClient("https://api.spotify.com/v1/search");
		    client.AddDefaultHeader("Authorization", $"Bearer {token.access_token}");
		    var request = new RestRequest($"?q={searchWord}&type=artist", Method.Get);
		    var response = await client.ExecuteAsync(request);

		    if (response.IsSuccessful) return JsonConvert.DeserializeObject<SpotifySearch.SpotifyResult>(response.Content);
		    throw new Exception($"Failed to search for '{searchWord}'. Status code: {response.StatusCode}, Error: {response.ErrorMessage}");
	    }
	}
}
