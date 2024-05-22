using System.Net.Http.Headers;
using Microsoft.Extensions.Options;

namespace MVC_Vibez.Model;

public class GeniusSearch
{
    private readonly string clientId;
    private readonly string clientSecret;
    private readonly string redirectUri;

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

    public async Task<List<GeniusHit>> SearchSongs(string accessToken, string searchTerm)
    {
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri("https://api.genius.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync($"search?q={Uri.EscapeDataString(searchTerm)}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<GeniusSearchResult>();
                // Return the list of hits
                return data.response.hits;
            }

            throw new Exception("Failed to search songs");
        }
    }
}