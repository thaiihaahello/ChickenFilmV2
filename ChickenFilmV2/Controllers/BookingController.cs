using Microsoft.AspNetCore.Mvc;
using ChickenFilmV2.Models;
using ChickenFilmV2.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
            var model = new BookingViewModel
            {
                Theaters = _context.Theaters
                    .Where(t => t.Location.Contains("Đà Nẵng"))
                    .ToList(),

                DangChieu = _context.Movies
                    .Where(m => m.Showtimes
                        .Any(s => s.ShowDate.HasValue
                                  && s.ShowDate.Value.Date == DateTime.Today
                                  && s.Status == "Đang chiếu"))
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


        // GET: /Booking/SeatSelection
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
            ViewBag.AuditoriumId = showtime.Auditorium.AuditoriumId;  // Thêm AuditoriumId vào ViewBag

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
            // Lấy giá ghế theo loại và auditoriumId (lọc bỏ trùng lặp)
            var seatPrices = await _context.AuditoriumSeatPricings
                .Where(p => p.AuditoriumId == auditoriumId)
                .GroupBy(p => p.SeatType) // Nhóm theo loại ghế
                .Select(g => new
                {
                    SeatType = g.Key,
                    Price = g.FirstOrDefault().Price // Lấy giá của một loại ghế (không bị trùng)
                })
                .ToListAsync();

            return Json(seatPrices); // Trả về giá ghế dưới dạng JSON
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmBooking(int showtimeId, List<int> selectedSeatIds)
        {
            // Lấy UserId từ Claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToAction("Login", "Account"); // Nếu chưa đăng nhập, chuyển đến trang đăng nhập
            }

            // Chuyển UserId từ string sang int
            var userIdInt = int.Parse(userId);

            // Lấy thông tin suất chiếu từ cơ sở dữ liệu
            var showtime = await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Auditorium)
                .ThenInclude(a => a.Theater)
                .FirstOrDefaultAsync(s => s.ShowtimeId == showtimeId);

            if (showtime == null)
            {
                return NotFound(); // Nếu không tìm thấy suất chiếu
            }

            Console.WriteLine($"ShowtimeId: {showtimeId}");
            Console.WriteLine("Selected Seat IDs: " + string.Join(", ", selectedSeatIds));


            // Kiểm tra ghế đã chọn hợp lệ
            if (selectedSeatIds == null || !selectedSeatIds.Any())
            {
                return BadRequest("Chưa chọn ghế.");
            }

            // Kiểm tra xem ghế đã được đặt chưa
            foreach (var seatId in selectedSeatIds)
            {
                var seat = await _context.Seats.FirstOrDefaultAsync(s => s.SeatId == seatId);
                if (seat == null)
                {
                    return BadRequest("Ghế không hợp lệ.");
                }

                // Kiểm tra xem ghế có bị trùng với các ghế đã đặt chưa
                var existingBooking = await _context.SeatBookings
                    .FirstOrDefaultAsync(sb => sb.SeatId == seatId && sb.Booking.ShowtimeId == showtimeId);

                if (existingBooking != null)
                {
                    return BadRequest($"Ghế {seat.SeatNumber} đã được đặt.");
                }
            }

            // Tạo một đối tượng Booking mới
            var booking = new Booking
            {
                UserId = userIdInt,
                ShowtimeId = showtimeId,
                CreatedAt = DateTime.Now
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();  // Lưu booking để có BookingId

            // Tiến hành lưu SeatBooking (mối quan hệ giữa booking và seat)
            foreach (var seatId in selectedSeatIds)
            {
                var seatBooking = new SeatBooking
                {
                    BookingId = booking.BookingId,
                    SeatId = seatId
                };

                _context.SeatBookings.Add(seatBooking);
            }

            await _context.SaveChangesAsync();  // Lưu SeatBooking

            // Chuyển hướng đến trang xác nhận đặt vé
            return RedirectToAction("BookingConfirmation", new { bookingId = booking.BookingId });
        }



        public IActionResult BookingConfirmation(int bookingId)
        {
            // Lấy thông tin đơn đặt vé từ database để hiển thị
            var booking = _context.Bookings
                .Include(b => b.SeatBookings).ThenInclude(sb => sb.Seat)
                .Include(b => b.Showtime).ThenInclude(s => s.Movie)
                .Include(b => b.Showtime).ThenInclude(s => s.Auditorium)
                .FirstOrDefault(b => b.BookingId == bookingId);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking); // Truyền dữ liệu sang View xác nhận
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
            ViewBag.PromoMessage = string.Empty; // Không có thông báo mã khuyến mãi

            
            return View();
        }


        


        public IActionResult VNPayReturn()
        {
            

            return View();
        }

    }

}
