namespace SpotifySearch
{

    public partial class Welcome
    {
        public Tracks Tracks { get; set; }
        public Artists Artists { get; set; }
        public Albums Albums { get; set; }
        public Playlists Playlists { get; set; }
    }

    public partial class Albums
    {
        public Uri Href { get; set; }
        public long Limit { get; set; }
        public Uri Next { get; set; }
        public long Offset { get; set; }
        public object Previous { get; set; }
        public long Total { get; set; }
        public List<AlbumElement> Items { get; set; }
    }

    public partial class AlbumElement
    {
        public string AlbumType { get; set; }
        public long TotalTracks { get; set; }
        public List<string> AvailableMarkets { get; set; }
        public ExternalUrls ExternalUrls { get; set; }
        public Uri Href { get; set; }
        public string Id { get; set; }
        public List<Image> Images { get; set; }
        public string Name { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public string ReleaseDatePrecision { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
        public List<Owner> Artists { get; set; }
    }

    public partial class Owner
    {
        public ExternalUrls ExternalUrls { get; set; }
        public Uri Href { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
        public string DisplayName { get; set; }
    }

    public partial class ExternalUrls
    {
        public Uri Spotify { get; set; }
    }

    public partial class Image
    {
        public Uri Url { get; set; }
        public long? Height { get; set; }
        public long? Width { get; set; }
    }

    public partial class Artists
    {
        public Uri Href { get; set; }
        public long Limit { get; set; }
        public Uri Next { get; set; }
        public long Offset { get; set; }
        public object Previous { get; set; }
        public long Total { get; set; }
        public List<ArtistsItem> Items { get; set; }
    }

    public partial class ArtistsItem
    {
        public ExternalUrls ExternalUrls { get; set; }
        public Followers Followers { get; set; }
        public List<string> Genres { get; set; }
        public Uri Href { get; set; }
        public string Id { get; set; }
        public List<Image> Images { get; set; }
        public string Name { get; set; }
        public long Popularity { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
    }

    public partial class Followers
    {
        public Uri Href { get; set; }
        public long Total { get; set; }
    }

    public partial class Playlists
    {
        public Uri Href { get; set; }
        public long Limit { get; set; }
        public Uri Next { get; set; }
        public long Offset { get; set; }
        public object Previous { get; set; }
        public long Total { get; set; }
        public List<PlaylistsItem> Items { get; set; }
    }

    public partial class PlaylistsItem
    {
        public bool Collaborative { get; set; }
        public string Description { get; set; }
        public ExternalUrls ExternalUrls { get; set; }
        public Uri Href { get; set; }
        public string Id { get; set; }
        public List<Image> Images { get; set; }
        public string Name { get; set; }
        public Owner Owner { get; set; }
        public object Public { get; set; }
        public string SnapshotId { get; set; }
        public Followers Tracks { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
        public object PrimaryColor { get; set; }
    }

    public partial class Tracks
    {
        public Uri Href { get; set; }
        public long Limit { get; set; }
        public Uri Next { get; set; }
        public long Offset { get; set; }
        public object Previous { get; set; }
        public long Total { get; set; }
        public List<TracksItem> Items { get; set; }
    }

    public partial class TracksItem
    {
        public AlbumElement Album { get; set; }
        public List<Owner> Artists { get; set; }
        public List<string> AvailableMarkets { get; set; }
        public long DiscNumber { get; set; }
        public long DurationMs { get; set; }
        public bool Explicit { get; set; }
        public ExternalIds ExternalIds { get; set; }
        public ExternalUrls ExternalUrls { get; set; }
        public Uri Href { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public long Popularity { get; set; }
        public object PreviewUrl { get; set; }
        public long TrackNumber { get; set; }
        public string Type { get; set; }
        public string Uri { get; set; }
        public bool IsLocal { get; set; }
    }

    public partial class ExternalIds
    {
        public string Isrc { get; set; }
    }
}
