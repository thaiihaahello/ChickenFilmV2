document.addEventListener("DOMContentLoaded", function () {
    // ---- XỬ LÝ CHUYỂN TAB ----
    document.querySelectorAll(".tab-button").forEach((tab) => {
        tab.addEventListener("click", function () {
            // Loại bỏ lớp "active" của tất cả tab
            document.querySelectorAll(".tab-button").forEach((t) => t.classList.remove("active"));
            // Thêm lớp "active" vào tab hiện tại
            this.classList.add("active");

            // Lấy giá trị filter từ thuộc tính data-filter
            let filter = this.getAttribute("data-filter");

            // Ẩn tất cả danh sách phim
            document.querySelectorAll(".movie-list").forEach((list) => {
                list.classList.add("hidden");
            });

            // Hiển thị danh sách phim theo tab được chọn
            if (filter === "all") {
                document.querySelector(".movie-list.default").classList.remove("hidden");
            } else {
                document.getElementById(filter).classList.remove("hidden");
            }
        });
    });

    // ---- XỬ LÝ MUA VÉ ----
    document.querySelectorAll(".buy-button").forEach(button => {
        button.addEventListener("click", function (e) {
            e.stopPropagation();  // Ngăn không cho sự kiện tiếp tục bubling lên
            const movieName = this.closest(".movie-item").querySelector(".movie-name").textContent;
            window.location.href = "/BOOKING/Booking?ten=" + encodeURIComponent(movieName);
        });
    });

    // ---- XỬ LÝ XEM CHI TIẾT PHIM ----
    document.querySelectorAll(".chitietphim-button").forEach(button => {
        button.addEventListener("click", function (e) {
            e.stopPropagation(); // Ngăn sự kiện lan ra ngoài
            const movieId = this.getAttribute("data-id"); // ✅ Lấy giá trị data-id
            window.location.href = "/Movie/MovieDetails?id=" + movieId;
        });
    });

    // ---- CLICK VÀO POSTER -> CHI TIẾT PHIM ----
    document.querySelectorAll(".poster-container").forEach(poster => {
        poster.addEventListener("click", function (e) {
            // Kiểm tra nếu không phải là click vào nút mua vé
            if (!e.target.classList.contains("buy-button")) {
                const movieName = this.closest(".movie-item").querySelector(".movie-name").textContent;
                window.location.href = "/Home/Chitietphim?ten=" + encodeURIComponent(movieName);
            }
        });
    });
});
