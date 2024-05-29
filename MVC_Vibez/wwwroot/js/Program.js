$(document).ready(function () {
    var typingTimer;
    var doneTypingInterval = 100;

    // Trigger search function after user stops typing
    $("#txtSearch").keyup(function () {
        clearTimeout(typingTimer);
        typingTimer = setTimeout(performSearch, doneTypingInterval);
    });

    // Trigger search function when a checkbox is checked or unchecked
    $(".search-options input[type='checkbox']").change(function () {
        performSearch();
    });

    // Update search results based on selected types and search text
    function performSearch() {
        var searchText = $("#txtSearch").val();
        var selectedTypes = [];
        if ($("#chkArtists").is(":checked")) selectedTypes.push("artist");
        if ($("#chkSongs").is(":checked")) selectedTypes.push("track");
        if ($("#chkAlbums").is(":checked")) selectedTypes.push("album");
        if ($("#chkPlaylists").is(":checked")) selectedTypes.push("playlist");

        // Treat zero selected checkboxes as all four selected
        if (selectedTypes.length === 0) {
            selectedTypes = ["artist", "track", "album", "playlist"];
        }

        // Clear search results if search text is empty
        if (searchText.trim() === '') {
            $("#searchResults").empty();
            return;
        }

        // Determine the number of items to display based on the number of selected checkboxes
        var itemsToShow = 6;
        if (selectedTypes.length === 1) itemsToShow = 24;
        else if (selectedTypes.length === 2) itemsToShow = 12;
        else if (selectedTypes.length === 3) itemsToShow = 8;
        else if (selectedTypes.length === 0 || selectedTypes.length === 4) itemsToShow = 6;

        // Send selected types, search text, and itemsToShow to the server
        $.ajax({
            type: "POST",
            url: "/Program/Autocomplete",
            data: { searchText: searchText, types: selectedTypes, itemsToShow: itemsToShow },
            success: function (response) {
                if (response.success) {
                    var listArtistHtml = "";

                    if (selectedTypes.includes("track")) {
                        listArtistHtml += "<h4>Songs</h4>";
                        $.each(response.songs.slice(0, itemsToShow), function (index, song) {
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
                        $.each(response.artists.slice(0, itemsToShow), function (index, artist) {
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
                        $.each(response.albums.slice(0, itemsToShow), function (index, album) {
                            listArtistHtml += '<a href="#" class="list-group-item list-group-item-action">';
                            listArtistHtml += '<div class="artist-info">';
                            listArtistHtml += '<img src="' + album.image + '" class="img-fluid" />';
                            listArtistHtml += '<p>' + album.name + '</p>';
                            listArtistHtml += '<div class="favorite-play-container">';
                            listArtistHtml += '<img src="/images/playbutton.png" alt="Play icon" class="play-btn" data-album-id="' + album.id + '">';
                            listArtistHtml += '</div>';
                            listArtistHtml += '</div>';
                            listArtistHtml += '</a>';
                        });
                    }

                    if (selectedTypes.includes("playlist")) {
                        listArtistHtml += "<h4>Playlists</h4>";
                        $.each(response.playlists.slice(0, itemsToShow), function (index, playlist) {
                            listArtistHtml += '<a href="#" class="list-group-item list-group-item-action">';
                            listArtistHtml += '<div class="artist-info">';
                            listArtistHtml += '<img src="' + playlist.image + '" class="img-fluid" />';
                            listArtistHtml += '<p>' + playlist.name + '</p>';
                            listArtistHtml += '<div class="favorite-play-container">';
                            listArtistHtml += '<img src="/images/playbutton.png" alt="Play icon" class="play-btn" data-playlist-id="' + playlist.id + '">';
                            listArtistHtml += '</div>';
                            listArtistHtml += '</div>';
                            listArtistHtml += '</a>';
                        });
                    }

                    $("#searchResults").html(listArtistHtml);
                } else {
                    console.error("An error occurred: " + response.message);
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
            url: "/Program/AddToFavorite",
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
