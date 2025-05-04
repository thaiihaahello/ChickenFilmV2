document.querySelector(".pay-btn").addEventListener("click", function () {
    document.getElementById("ticketModal").classList.add("active");
});

// Đóng modal khi ấn nút X hoặc bấm ra ngoài modal
function closeModal() {
    document.getElementById("ticketModal").classList.remove("active");
}

window.onclick = function (event) {
    let modal = document.getElementById("ticketModal");
    if (event.target === modal) {
        closeModal();
    }
};
document.getElementById("applyPromoBtn").addEventListener("click", function () {
    var promoCode = document.getElementById("promoCode").value;
    var totalAmount = parseFloat(document.getElementById("totalAmount").innerText.replace(' VNĐ', '').replace('.', '').trim());

    // Gọi API kiểm tra mã khuyến mãi
    fetch('@Url.Action("ApplyPromoCode", "Booking")', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ promoCode: promoCode, totalAmount: totalAmount })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                document.getElementById("promoMessage").innerText = `Áp dụng mã khuyến mãi thành công! Giảm ${data.discount} VNĐ. Tổng cộng: ${data.finalAmount} VNĐ`;
                document.getElementById("totalAmount").innerText = data.finalAmount.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,') + ' VNĐ';
            } else {
                document.getElementById("promoMessage").innerText = data.message;
            }
        })
        .catch(error => {
            document.getElementById("promoMessage").innerText = "Có lỗi xảy ra. Vui lòng thử lại!";
        });
});

function redirectToPayment() {
    // Ví dụ chuyển hướng đến trang thanh toán mới
    window.location.href = "/Booking/VNPayReturn"; // Thay đổi URL theo đường dẫn bạn muốn
}
