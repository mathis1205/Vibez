$(document).ready(function() {
    $(".play-btn").click(function() {
        var playlistId = $(this).data("playlist-id");
        window.location.href = "/Program/Index?playlistId=" + playlistId;
    });
});