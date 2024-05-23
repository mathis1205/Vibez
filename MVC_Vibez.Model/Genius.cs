public class GeniusSearchResult
{
    public GeniusSearchResponse response { get; set; }
}

public class GeniusSearchResponse
{
    public List<GeniusHit> hits { get; set; }
}

public class GeniusHit
{
    public GeniusResult result { get; set; }
}

public class GeniusResult
{
    public string title { get; set; }
    public GeniusArtist primary_artist { get; set; }
    public string url { get; set; }
    public int id { get; set; }
    public string path { get; set; }
    public string release_date { get; set; }
    public string song_art_image_thumbnail_url { get; set; }
}


public class GeniusArtist
{
    public string name { get; set; }
}

public class GeniusSongResult
{
    public GeniusSongResponse response { get; set; }
}

public class GeniusSongResponse
{
    public GeniusSong song { get; set; }
}

public class GeniusSong
{
    public string lyrics { get; set; }
    public string title { get; set; }
    public GeniusArtist primary_artist { get; set; }
    public string release_date_for_display { get; set; }
}