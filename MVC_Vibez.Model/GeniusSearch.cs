using System.Text;
using MVC_Vibez.Models;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;


namespace MVC_Vibez.Model
{
    public class GeniusSearch
    {
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string redirectUri;

        public GeniusSearch(string clientId, string clientSecret, string redirectUri)
        {
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.redirectUri = redirectUri;
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

                HttpResponseMessage response = await client.PostAsync("oauth/token", content);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsAsync<Dictionary<string, string>>();
                    return data["access_token"];
                }
                else
                {
                    throw new Exception("Failed to get access token");
                }
            }
        }
    }

}
