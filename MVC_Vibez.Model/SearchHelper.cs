using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MVC_Vibez.Models;
using Newtonsoft.Json;
using RestSharp;

namespace MVC_Vibez.Model
{
    public class SearchHelper
    {
        private const string ClientID = "1230a4d844d54c93b4ad47f2718afcc4";
        private const string ClientSecret = "3f58ee04292643a290023c19345ece2a";
        private static Token _token;
        private static DateTime _tokenExpirationTime;

        private static async Task<Token> GetTokenAsync()
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

        public static async Task<Welcome> SearchAll(string searchWord, List<string> types)
        {
            var token = await GetTokenAsync();
            var client = new RestClient("https://api.spotify.com/v1/search");

            // Construct query string based on selected types
            var queryString = new StringBuilder();
            foreach (var type in types)
            {
                queryString.Append($"{type},");
            }
            var request = new RestRequest($"?q={searchWord}&type={queryString.ToString().TrimEnd(',')}");
            client.AddDefaultHeader("Authorization", $"Bearer {token.access_token}");
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful) return JsonConvert.DeserializeObject<Welcome>(response.Content);

            throw new Exception($"Failed to search for '{searchWord}'. Status code: {response.StatusCode}, Error: {response.ErrorMessage}");
        }

        public static async Task<List<PlaylistsItem>> GetRandomPlaylistsAsync(int count)
        {
            var token = await GetTokenAsync();
            var client = new RestClient("https://api.spotify.com/v1/browse/featured-playlists");
            client.AddDefaultHeader("Authorization", $"Bearer {token.access_token}");
            var request = new RestRequest($"?limit={count}");
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                var result = JsonConvert.DeserializeObject<FeaturedPlaylistsResponse>(response.Content!);
                return result!.playlists.Items;
            }

            throw new Exception(
                $"Failed to get the playlists. Status code: {response.StatusCode}, Error: {response.ErrorMessage}");
        }
    }
}
