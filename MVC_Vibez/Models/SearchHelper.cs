using Newtonsoft.Json;
using RestSharp;
using System.Text;

namespace MVC_Vibez.Models
{
    public class SearchHelper
    {
        //create the variables for the token creation and validation
	    private static Token _token;
	    private static DateTime _tokenExpirationTime;

        //fill in the clientID and clientSecret from one of the development account
        private const string ClientID = "1230a4d844d54c93b4ad47f2718afcc4";
        private const string ClientSecret = "3f58ee04292643a290023c19345ece2a";

        //create task for getting the token of the website
        public static async Task<Token> GetTokenAsync()
	    {
            //check if there isnt already a token and check if the token isnt expired yet if that isnt the case he just returns the token back
            if (_token != null && DateTime.Now < _tokenExpirationTime) return _token;
            //fill in the different variables  to use in a later state
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes(ClientID + ":" + ClientSecret));
            var args = new List<KeyValuePair<string, string>> { new("grant_type", "client_credentials") };
            //create a http client so we can request the token
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {auth}");
            HttpContent content = new FormUrlEncodedContent(args);

            var resp = await client.PostAsync("https://accounts.spotify.com/api/token", content);
            var msg = await resp.Content.ReadAsStringAsync();
            //get the token from the reply of the request
            _token = JsonConvert.DeserializeObject<Token>(msg)!;
            //extract the expiration date and change it so we can use it longer then 60min
            _tokenExpirationTime = DateTime.Now.AddSeconds(_token!.expires_in);

            //return the token 
            return _token;
	    }

        //create a task to show searchresults using the given word
	    public static async Task<SpotifySearch.SpotifyResult> SearchAll(string searchWord)
	    {
            //get the token from the above class
		    var token = await GetTokenAsync();
            //get the api link and add the neccesary extra items like the token and other filters
		    var client = new RestClient("https://api.spotify.com/v1/search");
		    client.AddDefaultHeader("Authorization", $"Bearer {token.access_token}");
		    var request = new RestRequest($"?q={searchWord}&type=artist,album,playlist,track,show,episode,audiobook", Method.Get);
		    var response = await client.ExecuteAsync(request);
            //returns the given results if the reponse is succesfull
		    if (response.IsSuccessful) return JsonConvert.DeserializeObject<SpotifySearch.SpotifyResult>(response.Content);
            //if the response is not succesfull he will throw a error
		    throw new Exception($"Failed to search for '{searchWord}'. Status code: {response.StatusCode}, Error: {response.ErrorMessage}");
	    }
	}
}
