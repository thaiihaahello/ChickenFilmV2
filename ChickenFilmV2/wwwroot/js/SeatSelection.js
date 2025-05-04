document.addEventListener("DOMContentLoaded", function () {

    // Hàm cập nhật tiến trình bước
    function updateProgressStep(currentStep) {
        const steps = document.querySelectorAll(".step");
        const progressBar = document.getElementById("progressBar");

        steps.forEach((step, index) => {
            if (index < currentStep) {
                step.classList.add("done");
                step.classList.remove("active");
            } else if (index === currentStep) {
                step.classList.add("active");
                step.classList.remove("done");
            } else {
                step.classList.remove("active", "done");
            }
        });

        const percent = ((currentStep + 1) / steps.length) * 100;
        progressBar.style.width = `${percent}%`; // Cập nhật thanh tiến trình
    }

    updateProgressStep(0); // Bắt đầu tại bước 0

    // Lấy các phần tử cần thiết từ DOM
    const selectedSeatsInput = document.getElementById("selectedSeatsInput"); // Input ẩn để gửi ghế đã chọn
    const selectedSeatsSpan = document.getElementById("selectedSeats"); // Hiển thị ghế đã chọn
    const totalPriceSpan = document.getElementById("totalPrice"); // Hiển thị tổng tiền
    const totalAmountSpan = document.getElementById("totalAmount"); // Hiển thị tổng tiền (VNĐ)

    // Biến lưu trữ giá ghế theo loại
    let seatPrices = {};

    // Hàm lấy giá ghế từ API
    async function fetchSeatPrices(auditoriumId) {
        try {
            // Gọi API để lấy giá ghế
            const response = await fetch(`/Booking/GetSeatPrices?auditoriumId=${auditoriumId}`);

            // Kiểm tra nếu response không thành công
            if (!response.ok) {
                throw new Error(`Failed to fetch seat prices: ${response.statusText}`);
            }

            // Chuyển đổi dữ liệu từ JSON
            const data = await response.json();

            // Kiểm tra nếu dữ liệu trả về trống
            if (!Array.isArray(data) || data.length === 0) {
                console.warn("No seat prices found.");
                return;
            }

            console.log("Seat prices data:", data);

            // Cập nhật giá ghế vào seatPrices
            seatPrices = data.reduce((acc, curr) => {
                if (curr.seatType && curr.price) {  // Sử dụng seatType và price với chữ thường
                    acc[curr.seatType] = curr.price;  // Lưu giá ghế theo loại ghế
                } else {
                    console.warn("Invalid seat data:", curr);
                }
                return acc;
            }, {});

            console.log("Seat prices after fetching:", seatPrices);

            // Sau khi dữ liệu ghế được tải, cập nhật lại thông tin ghế đã chọn
            updateSelectedSeats();

            // Lắng nghe sự kiện click trên các ghế sau khi lấy giá ghế
            setupSeatSelection();

        } catch (error) {
            console.error("Error fetching seat prices:", error);
        }
    }

    // Lắng nghe sự kiện click trên các ghế
    function setupSeatSelection() {
        const seats = document.querySelectorAll(".seat:not(.unavailable)"); // Tất cả ghế có thể chọn
        seats.forEach(seat => {
            seat.addEventListener("click", function () {
                if (!seat.classList.contains("unavailable")) { // Kiểm tra ghế có sẵn không
                    this.classList.toggle("selected"); // Thêm hoặc bỏ lớp "selected" khi click
                    updateSelectedSeats(); // Cập nhật danh sách ghế đã chọn
                }
            });
        });
    }

    // Hàm cập nhật ghế đã chọn và tính tổng tiền
    function updateSelectedSeats() {
        const selectedIds = [];
        let totalAmount = 0;

        const seatTypeMap = {
            "seat-standard": "Standard",
            "seat-vip": "VIP",
            "seat-deluxe": "Deluxe",
            "seat-couple": "Couple"
        };

        const seatCategories = {
            "seat-standard": [],
            "seat-vip": [],
            "seat-deluxe": [],
            "seat-couple": []
        };

        document.querySelectorAll(".seat.selected").forEach(seat => {
            selectedIds.push(seat.dataset.seatNumber);

            const seatClass = Array.from(seat.classList).find(c => c.startsWith("seat-"));
            if (seatClass) {
                seatCategories[seatClass].push(seat.dataset.seatNumber);

                const seatType = seatTypeMap[seatClass];
                const price = seatPrices[seatType] || 0;
                totalAmount += price;

                console.log(`Ghế: ${seat.dataset.seatNumber}, Loại: ${seatType}, Giá: ${price}`);
            }
        });

        console.log("Tổng tiền hiện tại:", totalAmount);

        selectedSeatsInput.value = selectedIds.join(",");
        displaySelectedSeats(seatCategories);
        totalPriceSpan.innerText = totalAmount.toLocaleString("vi-VN");
        totalAmountSpan.innerText = totalAmount.toLocaleString("vi-VN");
        document.getElementById("totalAmountInput").value = totalAmount;
    }


    // Hàm hiển thị thông tin ghế đã chọn
    function displaySelectedSeats(seatCategories) {
        let seatInfo = "";

        // Kiểm tra và hiển thị từng loại ghế
        for (let category in seatCategories) {
            if (seatCategories[category].length > 0) {
                const seatCategoryName = getSeatCategoryName(category); // Lấy tên loại ghế
                seatInfo += `${seatCategoryName}: ${seatCategories[category].join(", ")} <br />`;
            }
        }

        // Nếu không có ghế nào được chọn, hiển thị thông báo
        if (seatInfo === "") {
            seatInfo = "Chưa chọn ghế";
        }

        selectedSeatsSpan.innerHTML = seatInfo; // Cập nhật phần hiển thị ghế đã chọn
    }

    // Hàm trả về tên loại ghế dựa trên class
    function getSeatCategoryName(seatClass) {
        switch (seatClass) {
            case "seat-vip":
                return "<strong>Ghế VIP</strong>";  // In đậm
            case "seat-deluxe":
                return "<strong>Ghế Deluxe</strong>";  // In đậm
            case "seat-couple":
                return "<strong>Ghế Couple</strong>";  // In đậm
            default:
                return "<strong>Ghế thường</strong>";  // In đậm
        }
    }

    // Lấy giá ghế khi trang tải (cần truyền auditoriumId vào)
    const auditoriumId = parseInt(document.getElementById("auditoriumId").value);
    fetchSeatPrices(auditoriumId); // Gọi hàm lấy giá ghế khi tải trang

});
document.getElementById("seatForm").addEventListener("submit", function (event) {
    console.log("Selected Seats:", selectedSeatsInput.value); // In ra giá trị ghế đã chọn
    if (selectedSeatsInput.value === "") {
        alert("Vui lòng chọn ghế trước khi tiếp tục.");
        event.preventDefault(); // Ngừng gửi form nếu không có ghế nào được chọn.
    }
});