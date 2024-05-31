using System.Drawing;
using System.Net.Http.Headers;
using System.Text;
using MVC_Vibez.Model;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RestSharp;

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
        _token = JsonConvert.DeserializeObject<Token>(await resp.Content.ReadAsStringAsync())!;
        _tokenExpirationTime = DateTime.Now.AddSeconds(_token.expires_in);

        return _token;
    }

    public static async Task<Welcome> SearchAll(string searchWord)
    {
	    var token = await GetTokenAsync();
	    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);

	    var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/search?q={searchWord}&type=artist,album,playlist,track,show,episode,audiobook");
	    if (response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<Welcome>(await response.Content.ReadAsStringAsync())!;
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
            return JsonConvert.DeserializeObject<FeaturedPlaylistsResponse>(content)!.playlists.Items;
        }
        throw new Exception($"Failed to get the playlists. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
    }

    public static async Task<PlaylistsItem> GetPlaylistAsync()
    {
        var token = await GetTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.access_token);
        var response = await _httpClient.GetAsync("https://api.spotify.com/v1/playlists/3HYOR6qrtLpplRnInsgqyq");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PlaylistsItem>(content)!;
        }
        throw new Exception($"Failed to get the playlist. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
    }

    public static async Task AddTrackToPlaylistAsync(string trackUri)
    {
        await Task.Run(() =>
        {
            var options = new ChromeOptions();
            ChromeDriver driver = new(options);

            try
            {
                driver.Navigate().GoToUrl("https://accounts.spotify.com/login");
                driver.Manage().Window.Size = new Size(1920, 1080);
                Thread.Sleep(500);

                var emailInput = driver.FindElement(By.Id("login-username"));
                emailInput.SendKeys("vibes-customerservice@hotmail.com");

                var passwordInput = driver.FindElement(By.Id("login-password"));
                passwordInput.SendKeys("Vives2023!");

                var loginButton = driver.FindElement(By.Id("login-button"));
                loginButton.Click();
                Thread.Sleep(500);

                driver.Navigate().GoToUrl("https://developer.spotify.com/documentation/web-api/reference/add-tracks-to-playlist");
                Thread.Sleep(500);

                var playlistInput = driver.FindElement(By.CssSelector("input[data-encore-id='formInput']:nth-of-type(1)"));
                playlistInput.Clear();
                playlistInput.SendKeys("3HYOR6qrtLpplRnInsgqyq");

                var positionInput = driver.FindElement(By.CssSelector("input[data-encore-id='formInput']:nth-of-type(2)"));
                positionInput.Clear();
                positionInput.SendKeys("0");

                var trackInput = driver.FindElement(By.CssSelector("input[data-encore-id='formInput']:nth-of-type(3)"));
                trackInput.Clear();
                trackInput.SendKeys(trackUri);

                var addButton = driver.FindElement(By.XPath("//span[text()='Try it']"));
                addButton.Click();
                Thread.Sleep(2000);
            }
            finally
            {
                driver.Quit();
            }
        });
    }
}