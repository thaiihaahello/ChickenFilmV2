using Microsoft.AspNetCore.Mvc;
using ChickenFilmV2.Models;
using ChickenFilmV2.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ChickenFilmV2.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly MovieDbContext _context;

        public BookingController(MovieDbContext context)
        {
            _context = context;
        }


        public IActionResult Booking()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today); // Lấy ngày hôm nay

            var model = new BookingViewModel
            {
                Theaters = _context.Theaters
                    .Where(t => t.Location.Contains("Đà Nẵng")) // Lọc rạp ở Đà Nẵng
                    .ToList(),

                // Lấy phim đang chiếu (chỉ phim với trạng thái "playing")
                DangChieu = _context.Movies
                    .Where(m => m.Showtimes
                        .Any(s => s.ShowDate.HasValue
                                  && s.ShowDate.Value == today
                                  && s.Status == "Đang chiếu")) // Lọc phim đang chiếu
                    .Include(m => m.Showtimes)
                    .ToList()
            };



            return View(model);
        }
        

        // Ajax: lấy phim theo rạp
        [HttpGet]
        public JsonResult GetMovies(int theaterId)
        {
            var movies = _context.Showtimes
                .Where(s => s.Auditorium.TheaterId == theaterId && s.Status == "Đang chiếu")
                .Join(_context.Movies, s => s.MovieId, m => m.MovieId, (s, m) => new
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    PosterUrl = m.PosterUrl
                })
                .Distinct()
                .ToList();

            return Json(movies);
        }


        // Ajax: lấy suất chiếu theo phim và rạp
        [HttpGet]
        public JsonResult GetShowtimes(int movieId, int theaterId)
        {
            var showtimes = _context.Showtimes
                .Where(s => s.MovieId == movieId && s.Auditorium.TheaterId == theaterId && s.Status == "Đang chiếu")
                .Select(s => new
                {
                    ShowtimeId = s.ShowtimeId,
                    ShowDate = s.ShowDate,
                    StartTime = s.ShowTime1
                })
                .ToList();

            return Json(showtimes);
        }


        // GET: /Booking/SeatSelection/5
        public IActionResult SelectSeats(int showtimeId)
        {
            var showtime = _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Auditorium)
                .ThenInclude(a => a.Theater)
                .FirstOrDefault(s => s.ShowtimeId == showtimeId);

            if (showtime == null)
            {
                return NotFound();
            }
            // Truyền thông tin về phim, suất chiếu, rạp vào ViewBag
            ViewBag.MovieTitle = showtime.Movie.Title;
            ViewBag.MoviePoster = showtime.Movie.PosterUrl;
            ViewBag.Showtime = showtime.ShowTime1;
            ViewBag.TheaterName = showtime.Auditorium.Theater.TheaterName;
            var seats = (from seat in _context.Seats
                         where seat.AuditoriumId == showtime.AuditoriumId
                         join pricing in _context.AuditoriumSeatPricings
                             on new { seat.AuditoriumId, seat.SeatType } equals new { pricing.AuditoriumId, pricing.SeatType }
                         select new SeatViewModel
                         {
                             SeatId = seat.SeatId,
                             SeatNumber = seat.SeatNumber,
                             SeatType = seat.SeatType,
                             IsAvailable = !seat.SeatBookings.Any(sb => sb.Booking.ShowtimeId == showtimeId),
                             Price = pricing.Price
                         }).ToList();

            ViewBag.ShowtimeId = showtimeId;
            return View(seats);
        }


        [HttpGet]
        public async Task<IActionResult> GetSeatPrices(int auditoriumId)
        {
            // Lấy giá ghế theo auditoriumId
            var seatPrices = await _context.AuditoriumSeatPricings
                .Where(p => p.AuditoriumId == auditoriumId)
                .ToListAsync();

            // Trả về giá ghế dưới dạng JSON
            return Json(seatPrices);
        }

        [HttpPost]
        public IActionResult ApplyPromoCode(string promoCode, decimal totalAmount)
        {
            // Kiểm tra mã khuyến mãi có hợp lệ không
            var today = DateOnly.FromDateTime(DateTime.Now); // Chuyển DateTime thành DateOnly để so sánh

            var promotion = _context.Promotions
                .FirstOrDefault(p => p.Code == promoCode && p.IsActive == true
                                     && p.StartDate <= today && p.EndDate >= today);

            if (promotion != null)
            {
                return Json(new { success = false, message = "Mã khuyến mãi không hợp lệ." });
            }

            // Kiểm tra nếu tổng đơn hàng đủ điều kiện áp dụng khuyến mãi
            if (promotion.MinOrderValue <= totalAmount)
            {
                return Json(new { success = false, message = "Đơn hàng không đủ giá trị để áp dụng mã khuyến mãi." });
            }

            // Tính toán giảm giá
            decimal discountAmount = totalAmount * (promotion.Discount / 100);
            decimal finalAmount = totalAmount - discountAmount;

            // Cập nhật số lần sử dụng mã khuyến mãi
            promotion.UsedCount += 1;
            _context.SaveChanges();

            return Json(new { success = true, message = "Áp dụng mã khuyến mãi thành công.", discount = discountAmount, finalAmount = finalAmount });
        }

        [HttpPost]
        public IActionResult Payment(int showtimeId, string selectedSeats, int totalAmount)
        {
            // Lấy thông tin suất chiếu
            var showtime = _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Auditorium)
                .ThenInclude(a => a.Theater)
                .FirstOrDefault(s => s.ShowtimeId == showtimeId);

            if (showtime == null)
            {
                return NotFound();
            }

            // Xử lý danh sách ghế đã chọn
            var seatNumbers = selectedSeats.Split(','); // Tách danh sách ghế
            var auditoriumId = showtime.AuditoriumId;

            var seats = _context.Seats
                .Where(s => s.AuditoriumId == auditoriumId && seatNumbers.Contains(s.SeatNumber))
                .ToList();


            // Truyền thông tin vào ViewBag
            ViewBag.MovieTitle = showtime.Movie.Title;
            ViewBag.MoviePoster = showtime.Movie.PosterUrl;
            ViewBag.Showtime = showtime.ShowTime1;
            ViewBag.TheaterName = showtime.Auditorium.Theater.TheaterName;
            ViewBag.Seats = seats;
            ViewBag.TotalAmount = totalAmount;
            ViewBag.PromoMessage = string.Empty; 

            
            return View();
        }


        


        public IActionResult VNPayReturn()
        {
            

            return View();
        }

    }

}
