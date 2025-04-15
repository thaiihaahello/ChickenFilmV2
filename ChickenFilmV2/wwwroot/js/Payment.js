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
