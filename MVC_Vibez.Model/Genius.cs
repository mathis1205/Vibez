using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Vibez.Model
{
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
    }

    public class GeniusArtist
    {
        public string name { get; set; }
    }
}

