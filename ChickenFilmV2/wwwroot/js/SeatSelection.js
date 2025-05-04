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
        progressBar.style.width = `${percent}%`; // ✅ Sửa cú pháp template literal
    }

    updateProgressStep(1); // Bắt đầu tại bước 0

    // Lấy các phần tử cần thiết từ DOM
    const seats = document.querySelectorAll(".seat:not(.unavailable)"); // Tất cả ghế có thể chọn
    const selectedSeatsInput = document.getElementById("selectedSeatsInput"); // Input ẩn để gửi ghế đã chọn
    const selectedSeatsSpan = document.getElementById("selectedSeats"); // Hiển thị ghế đã chọn
    const totalPriceSpan = document.getElementById("totalPrice"); // Hiển thị tổng tiền
    const totalAmountSpan = document.getElementById("totalAmount"); // Hiển thị tổng tiền (VNĐ)

    // Giá vé cho từng loại ghế
    const seatPrices = {
        "seat-standard": 50000,
        "seat-vip": 70000,
        "seat-deluxe": 90000,
        "seat-couple": 120000
    };

    // Lắng nghe sự kiện click trên các ghế
    seats.forEach(seat => {
        seat.addEventListener("click", function () {
            if (!seat.classList.contains("unavailable")) { // Kiểm tra ghế có sẵn không
                this.classList.toggle("selected"); // Thêm hoặc bỏ lớp "selected" khi click
                updateSelectedSeats(); // Cập nhật danh sách ghế đã chọn
            }
        });
    });

    // Hàm cập nhật ghế đã chọn và tính tổng tiền
    function updateSelectedSeats() {
        const selectedIds = [];
        let totalAmount = 0;

        // Định nghĩa đối tượng chứa ghế theo loại
        const seatCategories = {
            "seat-standard": [],
            "seat-vip": [],
            "seat-deluxe": [],
            "seat-couple": []
        };

        // Duyệt qua tất cả ghế đã chọn
        document.querySelectorAll(".seat.selected").forEach(seat => {
            selectedIds.push(seat.dataset.seatNumber); // Thêm ID ghế vào danh sách đã chọn

            // Tìm class loại ghế của ghế được chọn
            const seatClass = Array.from(seat.classList).find(c => c.startsWith("seat-"));
            if (seatClass) {
                seatCategories[seatClass].push(seat.dataset.seatNumber); // Phân loại ghế
                totalAmount += seatPrices[seatClass] || 0; // Cộng giá ghế vào tổng tiền
            }
        });

        // Cập nhật thông tin ghế đã chọn
        selectedSeatsInput.value = selectedIds.join(","); // Gửi danh sách ID ghế đã chọn
        displaySelectedSeats(seatCategories); // Hiển thị thông tin ghế đã chọn
        totalPriceSpan.innerText = totalAmount.toLocaleString("vi-VN"); // Cập nhật tổng tiền
        totalAmountSpan.innerText = totalAmount.toLocaleString("vi-VN"); // Cập nhật tổng tiền (VNĐ)
        document.getElementById("totalAmountInput").value = totalAmount;
    }

    // Hàm hiển thị thông tin ghế đã chọn
    function displaySelectedSeats(seatCategories) {
        let seatInfo = "";

        // Kiểm tra và hiển thị từng loại ghế
        for (let category in seatCategories) {
            if (seatCategories[category].length > 0) {
                const seatCategoryName = getSeatCategoryName(category); // Lấy tên loại ghế
                seatInfo += `${seatCategoryName}: ${seatCategories[category].join(", ")} <br />`; // ✅ Sửa cú pháp
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

});
