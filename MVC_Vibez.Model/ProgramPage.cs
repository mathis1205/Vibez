using MVC_Vibez.Models;

namespace MVC_Vibez.Model;

public class ProgramPage
{
    public User user { get; set; }
    public List<PlaylistsItem> playlists { get; set; }
    public ContactFormSubmission contactForm { get; set; }
    public List<Spotify> favoriteSongs { get; set; }
    public List<GeniusHit> Hits { get; set; }
    public string Lyrics { get; set; }
    public GeniusResult CurrentSong { get; set; }
    public bool SearchPerformed { get; set; }

}