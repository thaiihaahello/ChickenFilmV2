document.addEventListener("DOMContentLoaded", function () {
    const dropdowns = document.querySelectorAll(".custom-dropdown");
    const theaterButtons = document.querySelectorAll(".location-button");
    const theaterText = document.getElementById("selectedTheaterText");
    const theaterInput = document.getElementById("theaterSelect");
    const movieText = document.getElementById("selectedMovieText");
    const movieInput = document.getElementById("movieSelect");
    const moviePoster = document.querySelector(".movie-info img");
    const movieTitle = document.getElementById("selectedMovie");
    const showtimeDropdown = document.getElementById("showtimeDropdown");

    // Toggle dropdowns
    dropdowns.forEach(dropdown => {
        const trigger = dropdown.querySelector(".dropdown-trigger");
        trigger.addEventListener("click", function (e) {
            e.stopPropagation();
            closeAllDropdowns();
            dropdown.classList.toggle("active");
        });
    });

    // Close all dropdowns when clicking anywhere outside
    document.addEventListener("click", function () {
        closeAllDropdowns();
    });

    function closeAllDropdowns() {
        dropdowns.forEach(d => d.classList.remove("active"));
    }

    // Select Theater
    theaterButtons.forEach(button => {
        button.addEventListener("click", function () {
            const theaterId = this.getAttribute("data-theater-id");
            const theaterName = this.textContent;

            theaterText.textContent = theaterName;
            theaterInput.value = theaterId;
            document.getElementById("selectedTheater").textContent = theaterName;

            // Reset movie and showtime fields
            movieInput.value = "";
            movieText.textContent = "Chọn phim";
            moviePoster.src = "/images/placeholder.png";
            movieTitle.textContent = "-";
            showtimeDropdown.innerHTML = "";
            document.getElementById("selectedShowtimeText").textContent = "Chọn suất chiếu";

            closeAllDropdowns();
            loadMovies(theaterId); // Load movies for selected theater
        });
    });

    // Load Movies by Theater
    function loadMovies(theaterId) {
        fetch(`/Booking/GetMovies?theaterId=${theaterId}`)
            .then(response => response.json())
            .then(data => {
                const movieDropdown = dropdowns[1].querySelector(".dropdown-content");
                movieDropdown.innerHTML = "";  // Xoá danh sách cũ

                if (data.length === 0) {
                    const note = createNoteElement("Không có phim nào.");
                    movieDropdown.appendChild(note);
                    return;
                }

                data.forEach(movie => {
                    const btn = createMovieButton(movie, theaterId);
                    movieDropdown.appendChild(btn);
                });
            })
            .catch(error => {
                console.error("Lỗi khi lấy dữ liệu phim:", error);
            });
    }

    // Create a movie button for dropdown
    function createMovieButton(movie, theaterId) {
        const btn = document.createElement("button");
        btn.classList.add("movie-button");
        btn.setAttribute("data-movie-id", movie.movieId);
        btn.setAttribute("data-title", movie.title);
        btn.setAttribute("data-poster", movie.posterUrl);
        btn.innerHTML = `<img src="${movie.posterUrl}" class="movie-poster" /> ${movie.title}`;

        btn.addEventListener("click", function () {
            const movieId = this.getAttribute("data-movie-id");
            const title = this.getAttribute("data-title");
            const poster = this.getAttribute("data-poster");

            movieText.textContent = title;
            movieInput.value = movieId;
            moviePoster.src = poster;
            movieTitle.textContent = title;

            document.getElementById("selectedShowtimeText").textContent = "Chọn suất chiếu";
            showtimeDropdown.innerHTML = "";

            closeAllDropdowns();
            loadShowtimes(theaterId, movieId);
        });

        return btn;
    }

    // Load Showtimes by Movie and Theater
    function loadShowtimes(theaterId, movieId) {
        showtimeDropdown.innerHTML = ""; // Clear previous showtimes

        if (theaterId && movieId) {
            fetch(`/Booking/GetShowtimes?theaterId=${theaterId}&movieId=${movieId}`)
                .then(response => response.json())
                .then(data => {
                    if (data.length === 0) {
                        const note = createNoteElement("Không có suất chiếu.");
                        showtimeDropdown.appendChild(note);
                        return;
                    }

                    data.forEach(st => {
                        const btn = createShowtimeButton(st);
                        showtimeDropdown.appendChild(btn);
                    });
                })
                .catch(error => {
                    console.error("Lỗi khi lấy dữ liệu suất chiếu:", error);
                });
        } else {
            const note = createNoteElement("Vui lòng chọn rạp và phim.");
            showtimeDropdown.appendChild(note);
        }
    }

    // Create a showtime button for dropdown
    function createShowtimeButton(st) {
        const btn = document.createElement("button");
        btn.classList.add("location-button");
        const date = new Date(st.showDate).toLocaleDateString("vi-VN");
        btn.textContent = `${date} - ${st.startTime}`;
        btn.setAttribute("data-showtime-id", st.showtimeId);

        btn.addEventListener("click", function () {
            document.getElementById("selectedShowtimeText").textContent = this.textContent;
            document.getElementById("selectedShowtime").textContent = this.textContent;
            document.getElementById("showtimeSelect").value = st.showtimeId;
            closeAllDropdowns();
        });

        return btn;
    }

    // Create a note element for displaying "No data"
    function createNoteElement(text) {
        const note = document.createElement("div");
        note.textContent = text;
        note.style.padding = "10px";
        note.style.color = "#888";
        return note;
    }
});

// Xử lý nút Tiếp tục
const continueBtn = document.querySelector(".continue-btn");

continueBtn.addEventListener("click", function () {
    const showtimeId = document.getElementById("showtimeSelect").value;

    if (!showtimeId) {
        alert("Vui lòng chọn suất chiếu trước khi tiếp tục.");
        return;
    }

    // Điều hướng sang trang chọn ghế với showtimeId
    window.location.href = `/Booking/SelectSeats?showtimeId=${showtimeId}`;
});
