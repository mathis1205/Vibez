$(document).ready(function() {
    var typingTimer;
    var doneTypingInterval = 100;

    $("#txtSearch").keyup(function() {
        clearTimeout(typingTimer);
        typingTimer = setTimeout(performSearch, doneTypingInterval);
    });

    function performSearch() {
        var searchText = $("#txtSearch").val();
        if (searchText.trim() === "") {
            $("#searchResults").empty();
            return;
        }
        $.ajax({
            type: "POST",
            url: "/Program/Autocomplete",
            data: { searchText: searchText },
            success: function(response) {
                if (response.success) {
                    var listArtistHtml = "";

                    listArtistHtml += "<h4>Songs</h4>";
                    $.each(response.songs.slice(0, 6),
                        function(index, song) {
                            listArtistHtml += '<div class="list-group-item list-group-item-action">';
                            listArtistHtml += '<div class="artist-info">';
                            listArtistHtml += '<img src="' + song.image + '" class="img-fluid" />';
                            listArtistHtml += "<p>" + song.name + "</p>";
                            listArtistHtml += '<div class="favorite-play-container">';
                            listArtistHtml +=
                                '<img src="/images/Favoritebtn.png" alt="plus icon" class="favorite-btn" data-song=\'' +
                                JSON.stringify(song) +
                                "'>";
                            listArtistHtml +=
                                '<img src="/images/playbutton.png" alt="Play icon" class="play-btn" data-song-id="' +
                                song.id +
                                '">';
                            listArtistHtml += "</div>";
                            listArtistHtml += "</div>";
                            listArtistHtml += "</div>";
                        });

                    listArtistHtml += "<h4>Artists</h4>";
                    $.each(response.artists.slice(0, 6),
                        function(index, artist) {
                            listArtistHtml += '<a href="#" class="list-group-item list-group-item-action">';
                            listArtistHtml += '<div class="artist-info">';
                            listArtistHtml += '<img src="' + artist.image + '" class="img-fluid" />';
                            listArtistHtml += "<p>" + artist.name + "</p>";
                            listArtistHtml += '<div class="favorite-play-container">';
                            listArtistHtml +=
                                '<img src="/images/playbutton.png" alt="Play icon" class="play-btn" data-artist-id="' +
                                artist.id +
                                '">';
                            listArtistHtml += "</div>";
                            listArtistHtml += "</div>";
                            listArtistHtml += "</a>";
                        });

                    listArtistHtml += "<h4>Albums</h4>";
                    $.each(response.albums.slice(0, 6),
                        function(index, albums) {
                            listArtistHtml += '<a href="#" class="list-group-item list-group-item-action">';
                            listArtistHtml += '<div class="artist-info">';
                            listArtistHtml += '<img src="' + albums.image + '" class="img-fluid" />';
                            listArtistHtml += "<p>" + albums.name + "</p>";
                            listArtistHtml += '<div class="favorite-play-container">';
                            listArtistHtml +=
                                '<img src="/images/playbutton.png" alt="Play icon" class="play-btn" data-album-id="' +
                                albums.id +
                                '">';
                            listArtistHtml += "</div>";
                            listArtistHtml += "</div>";
                            listArtistHtml += "</a>";
                        });

                    listArtistHtml += "<h4>Playlists</h4>";
                    $.each(response.playlists.slice(0, 6),
                        function(index, playlists) {
                            listArtistHtml += '<a href="#" class="list-group-item list-group-item-action">';
                            listArtistHtml += '<div class="artist-info">';
                            listArtistHtml += '<img src="' + playlists.image + '" class="img-fluid" />';
                            listArtistHtml += "<p>" + playlists.name + "</p>";
                            listArtistHtml += '<div class="favorite-play-container">';
                            listArtistHtml +=
                                '<img src="/images/playbutton.png" alt="Play icon" class="play-btn" data-playlist-id="' +
                                playlists.id +
                                '">';
                            listArtistHtml += "</div>";
                            listArtistHtml += "</div>";
                            listArtistHtml += "</a>";
                        });

                    $("#searchResults").html(listArtistHtml);
                } else {
                    console.error("An error occurred: " + response.error);
                }
            },
            error: function() {
                console.error("An error occurred while processing your request.");
            }
        });
    }

    $(document).on("click",
        ".favorite-btn",
        function() {
            var song = $(this).data("song");
            var songUri = song.Uri;
            $.ajax({
                type: "POST",
                url: "/Program/AddToFavorite",
                data: { song: song, songUri: songUri },
                success: function(data) {
                    $("#message").addClass("alert-visible").text("Song added to favorites!").show().delay(5000).fadeOut(
                        function() {
                            $(this).removeClass("alert-visible");
                        });
                },
                error: function(xhr, status, error) {
                    console.error("An error occurred while adding the song to favorites: " + error);
                }
            });
        });

    $(document).on("click",
        ".play-btn",
        function() {
            var songId = $(this).data("song-id");
            var artistId = $(this).data("artist-id");
            var albumId = $(this).data("album-id");
            var playlistId = $(this).data("playlist-id");
            var src = "";

            if (songId) {
                src = `https://open.spotify.com/embed/track/${songId}`;
            } else if (artistId) {
                src = `https://open.spotify.com/embed/artist/${artistId}`;
            } else if (albumId) {
                src = `https://open.spotify.com/embed/album/${albumId}`;
            } else if (playlistId) {
                src = `https://open.spotify.com/embed/playlist/${playlistId}`;
            }

            $("#mediaPlayer iframe").attr("src", src);
        });

    if (window.location.href) {
        var url = new URL(window.location.href);
        var playlistId = url.searchParams.get("playlistId");
        if (playlistId) {
            var src = `https://open.spotify.com/embed/playlist/${playlistId}`;
            $("#mediaPlayer iframe").attr("src", src);
        }
    }
});