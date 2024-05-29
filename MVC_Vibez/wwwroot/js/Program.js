@model MVC_Vibez.Model.ProgramPage
@{
    ViewData["Title"] = "Program";
}

@await Html.PartialAsync("_Navbar", Model.user)
    < div id = "message" class="alert" style = "margin-top: 20px;" ></div >
<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
<script src="https://open.spotify.com/embed/iframe-api/v1" async></script>
<link rel="stylesheet" href="css/Program.css" />

<body>
    <div class="sidebar bg-light">
	    <div class="search-box">
		    <input type="text" id="txtSearch" class="form-control mb-3" placeholder="Search">
	    </div>
	    <div class="search-options">
		    <label><input type="checkbox" id="chkArtists" checked> Artists</label>
		    <label><input type="checkbox" id="chkSongs" checked> Songs</label>
		    <label><input type="checkbox" id="chkAlbums" checked> Albums</label>
		    <label><input type="checkbox" id="chkPlaylists" checked> Playlists</label>
	    </div>
        <div id="searchResults" class="search-results list-group"></div>
    </div>

    <div class="main-content">
        <div id="mediaPlayer">
            <iframe allow="encrypted-media"></iframe>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            var typingTimer;
            var doneTypingInterval = 100;

            // Trigger search function after user stops typing
            $("#txtSearch").keyup(function () {
                clearTimeout(typingTimer);
                typingTimer = setTimeout(performSearch, doneTypingInterval);
            });

            // Update search results based on selected types
            function performSearch() {
                var searchText = $("#txtSearch").val();
                var selectedTypes = [];
                if ($("#chkArtists").is(":checked")) selectedTypes.push("artist");
                if ($("#chkSongs").is(":checked")) selectedTypes.push("track");
                if ($("#chkAlbums").is(":checked")) selectedTypes.push("album");
                if ($("#chkPlaylists").is(":checked")) selectedTypes.push("playlist");

                // Send selected types to the server
                $.ajax({
                    type: "POST",
                    url: "@Url.Action("Autocomplete", "Program")",
                    data: { searchText: searchText, types: selectedTypes },
                    success: function (response) {
                        if (response.success) {
                            var listArtistHtml = "";

                            if (selectedTypes.includes("track")) {
                                listArtistHtml += "<h4>Songs</h4>";
                                $.each(response.songs.slice(0, 6), function (index, song) {
                                    listArtistHtml += '<div class="list-group-item list-group-item-action">';
                                    listArtistHtml += '<div class="artist-info">';
                                    listArtistHtml += '<img src="' + song.image + '" class="img-fluid" />';
                                    listArtistHtml += '<p>' + song.name + '</p>';
                                    listArtistHtml += '<div class="favorite-play-container">';
                                    listArtistHtml += '<img src="/images/Favoritebtn.png" alt="plus icon" class="favorite-btn" data-song=\'' + JSON.stringify(song) + '\'>';
                                    listArtistHtml += '<img src="/images/playbutton.png" alt="Play icon" class="play-btn" data-song-id="' + song.id + '">';
                                    listArtistHtml += '</div>';
                                    listArtistHtml += '</div>';
                                    listArtistHtml += '</div>';
                                });
                            }

                            if (selectedTypes.includes("artist")) {
                                listArtistHtml += "<h4>Artists</h4>";
                                $.each(response.artists.slice(0, 6), function (index, artist) {

                                    listArtistHtml += '<a href="#" class="list-group-item list-group-item-action">';
                                    listArtistHtml += '<div class="artist-info">';
                                    listArtistHtml += '<img src="' + artist.image + '" class="img-fluid" />';
                                    listArtistHtml += '<p>' + artist.name + '</p>';
                                    listArtistHtml += '<div class="favorite-play-container">';
                                    listArtistHtml += '<img src="/images/playbutton.png" alt="Play icon" class="play-btn" data-artist-id="' + artist.id + '">';
                                    listArtistHtml += '</div>';
                                    listArtistHtml += '</div>';
                                    listArtistHtml += '</a>';
                                });
                            }

if (selectedTypes.includes("album")) {
    listArtistHtml += "<h4>Albums</h4>";
    $.each(response.albums.slice(0, 6), function (index, album) {
        listArtistHtml += '<a href="#" class="list-group-item list-group-item-action">';
        listArtistHtml += '<div class="artist-info">';
        listArtistHtml += '<img src="' + albums.image + '" class="img-fluid" />';
        listArtistHtml += '<p>' + albums.name + '</p>';
        listArtistHtml += '<div class="favorite-play-container">';
        listArtistHtml += '<img src="/images/playbutton.png" alt="Play icon" class="play-btn" data-album-id="' + albums.id + '">';
        listArtistHtml += '</div>';
        listArtistHtml += '</div>';
        listArtistHtml += '</a>';
    });
}

if (selectedTypes.includes("playlist")) {
    listArtistHtml += "<h4>Playlists</h4>";
    $.each(response.playlists.slice(0, 6), function (index, playlist) {
        listArtistHtml += '<a href="#" class="list-group-item list-group-item-action">';
        listArtistHtml += '<div class="artist-info">';
        listArtistHtml += '<img src="' + playlists.image + '" class="img-fluid" />';
        listArtistHtml += '<p>' + playlists.name + '</p>';
        listArtistHtml += '<div class="favorite-play-container">';
        listArtistHtml += '<img src="/images/playbutton.png" alt="Play icon" class="play-btn" data-playlist-id="' + playlists.id + '">';
        listArtistHtml += '</div>';
        listArtistHtml += '</div>';
        listArtistHtml += '</a>';
    });
}

$("#searchResults").html(listArtistHtml);
                        } else {
    console.error("An error occurred: " + response.error);
}
                    },
error: function () {
    console.error("An error occurred while processing your request.");
}
                });
            }

// Handle favorite button click
$(document).on('click', '.favorite-btn', function () {
    var song = $(this).data('song');
    $.ajax({
        type: "POST",
        url: "@Url.Action("AddToFavorite", "Program")",
        data: { song: song },
        success: function (data) {
            // Update UI to reflect the song being added to favorites
            $('#message').addClass('alert-visible').text("Song added to favorites!").show().delay(5000).fadeOut(function () {
                $(this).removeClass('alert-visible');
            });
        },
        error: function (xhr, status, error) {
            console.error("An error occurred while adding the song to favorites: " + error);
        }
    });
});

// Handle play button click
$(document).on('click', '.play-btn', function () {
    var songId = $(this).data('song-id');
    var artistId = $(this).data('artist-id');
    var albumId = $(this).data('album-id');
    var playlistId = $(this).data('playlist-id');
    var src = '';

    if (songId) {
        src = `https://open.spotify.com/embed/track/${songId}`;
    } else if (artistId) {
        src = `https://open.spotify.com/embed/artist/${artistId}`;
    } else if (albumId) {
        src = `https://open.spotify.com/embed/album/${albumId}`;
    } else if (playlistId) {
        src = `https://open.spotify.com/embed/playlist/${playlistId}`;
    }

    $('#mediaPlayer iframe').attr('src', src);
});

// Check URL for playlistId parameter and set iframe source
if (window.location.href) {
    var url = new URL(window.location.href);
    var playlistId = url.searchParams.get("playlistId");
    if (playlistId) {
        var src = `https://open.spotify.com/embed/playlist/${playlistId}`;
        $('#mediaPlayer iframe').attr('src', src);
    }
}
        });
    </script >
</body >
