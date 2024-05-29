async function searchVideos() {
    const query = document.getElementById("searchQuery").value.trim();
    if (!query) {
        alert("Query required");
        return;
    }

    const videosContainer = document.getElementById("videos");
    const loadingIndicator = document.getElementById("loading");

    videosContainer.innerHTML = "";
    loadingIndicator.style.display = "block";

    try {
        const response = await fetch(`/YoutubeApi/SearchVideos?query=${encodeURIComponent(query)}`);
        const data = await response.json();
        displayVideos(data);
    } catch (error) {
        console.error("Error fetching YouTube API:", error);
        videosContainer.innerHTML = "<p>Failed to fetch videos. Please try again later.</p>";
    } finally {
        loadingIndicator.style.display = "none";
        document.getElementById("searchQuery").value = "";
    }
}

function displayVideos(videos) {
    const videosContainer = document.getElementById("videos");
    videosContainer.innerHTML = "";

    if (videos.length === 0) {
        videosContainer.innerHTML = "<p>No videos found.</p>";
        return;
    }

    videos.forEach(video => {
        const videoId = video.videoId;
        const iframe = document.createElement("iframe");
        iframe.src = `https://www.youtube.com/embed/${videoId}`;
        iframe.frameBorder = "0";
        iframe.allow = "accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture";
        iframe.allowFullscreen = true;
        videosContainer.appendChild(iframe);
    });
}