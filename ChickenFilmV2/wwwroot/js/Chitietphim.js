function playTrailer() {
    let trailerContainer = document.getElementById("trailer-container");
    trailerContainer.innerHTML = `
        <iframe 
            width="100%" 
            height="500" 
            src="https://www.youtube.com/embed/GOeKx7L8yyk?autoplay=1"
            title="Bí Kíp Luyện Rồng - Official Trailer"
            frameborder="0"
            allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" 
            allowfullscreen>
        </iframe>
    `;
}