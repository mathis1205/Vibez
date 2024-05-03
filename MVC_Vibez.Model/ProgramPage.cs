using MVC_Vibez.Models;
using SpotifySearch;

namespace MVC_Vibez.Model;

public class ProgramPage
{
    public User user { get; set; }
    public List<PlaylistsItem> playlists { get; set; }
}