namespace MVC_Vibez.Models;

public class SpotifySearch
{
    //Create a class tot be later used in the class
    public class ExternalUrls { public string spotify { get; set; } }

    //create a class with the variables for the followers
    public class Followers { 
        public object href { get; set; }        
        public int total { get; set; }
    }
    //create a class with the variables for the images that you get out of the json file
    public class ImageSP
    {
        public int width { get; set; }
        public string url { get; set; }
        public int height { get; set; }
    }
    //create a class with the variables for an item that you get out of the json file
    public class Item
    {
        public ExternalUrls external_urls { get; set; }
        public Followers followers { get; set; }
        public List<string> genres { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<ImageSP> images { get; set; }
        public string name { get; set; }
        public int popularity { get; set; }

        public string type { get; set; }
        public string uri { get; set; }
    }
    //create a class with the variables for the artists that you get out of the json file
    public class Artists
    {
        public string id { get; set; }  
        public string name { get; set; }
        public string url { get; set; }
        public string href { get; set; }
        public List<Item> items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }
    }
    //create a class with the variables that u use to save a certain amount of artist
    public class SpotifyResult
    {
        public Artists artists { get; set; }
    }
}