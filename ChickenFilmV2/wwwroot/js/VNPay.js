// Đếm ngược 5 phút
let totalSeconds = 300;

function startCountdown() {
    const el = document.getElementById("countdown");
    const interval = setInterval(() => {
        let min = Math.floor(totalSeconds / 60);
        let sec = totalSeconds % 60;
        el.innerText =
            (min < 10 ? "0" : "") + min + ":" + (sec < 10 ? "0" : "") + sec;
        totalSeconds--;
        if (totalSeconds < 0) {
            clearInterval(interval);
            el.innerText = "00:00";
            alert("Hết thời gian thanh toán!");
            window.location.href = "/";
        }
    }, 1000);
}

window.onload = startCountdown;
