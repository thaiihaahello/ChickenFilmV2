using ChickenFilmV2.Models;
using ChickenFilmV2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace ChickenFilmV2.Controllers
{
    public class MovieController : Controller
    {
        private readonly MovieDbContext _context;
        private readonly ILogger<MovieController> _logger;

        public MovieController(MovieDbContext context, ILogger<MovieController> logger)
        {
            _context = context;
            _logger = logger;
        }


        public IActionResult ListPhim()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            // Lấy tất cả các phim có suất chiếu
            var allMovies = _context.Movies
                .Where(m => m.Showtimes.Any(s => s.Status == "Đang chiếu" || s.Status == "Sắp chiếu"))
                .Include(m => m.Showtimes)
                .ToList();

            // Phim đang chiếu: ShowDate <= hôm nay và Status là "playing"
            var dangChieu = allMovies
    .Where(m => m.Showtimes.Any(s =>
        s.ShowDate.HasValue && DateOnly.FromDateTime(s.ShowDate.Value) <= today && s.Status == "Đang chiếu"))
    .ToList();

            // Phim sắp chiếu: ShowDate > hôm nay và Status là "scheduled"
            var sapChieu = allMovies
     .Where(m => m.Showtimes.Any(s =>
         s.ShowDate.HasValue && DateOnly.FromDateTime(s.ShowDate.Value) > today && s.Status == "Sắp chiếu"))
     .ToList();

            // Tạo ViewModel với các danh sách phim
            var viewModel = new ListPhimViewModel
            {
                AllMovies = allMovies.Select(m => new MovieViewModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Format = m.Format ?? "Unknown", // Nếu không có giá trị thì hiển thị "Unknown"
                    Language = m.Language ?? "Unknown", // Nếu không có giá trị thì hiển thị "Unknown"
                    PosterUrl = m.PosterUrl
                }).ToList(),

                DangChieu = dangChieu.Select(m => new MovieViewModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Format = m.Format ?? "Unknown",
                    Language = m.Language ?? "Unknown",
                    PosterUrl = m.PosterUrl
                }).ToList(),

                SapChieu = sapChieu.Select(m => new MovieViewModel
                {
                    MovieId = m.MovieId,
                    Title = m.Title,
                    Format = m.Format ?? "Unknown",
                    Language = m.Language ?? "Unknown",
                    PosterUrl = m.PosterUrl
                }).ToList()
            };

            return View(viewModel);
        }

        [Authorize]
        public IActionResult MovieDetails(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.MovieId == id);
            if (movie == null) return NotFound();

            // Lấy danh sách showtime theo ngày
            var showtimes = _context.Showtimes
                .Where(s => s.MovieId == id && s.ShowDate != null)
                .ToList();

            // Nhóm suất chiếu theo ngày và format
            var groupedShowtimes = showtimes
                .Where(s => s.ShowDate.HasValue) // tránh lỗi khi ShowDate bị null
                .GroupBy(s => s.ShowDate.Value.Date)  // Nhóm theo ngày (DateTime.Date)
                .Select(g => new ShowtimeByDateViewModel
                {
                    Date = g.Key,  // Kiểu DateTime
                    TimeSlots = g.OrderBy(x => x.ShowTime1)
                     .Select(x => x.ShowTime1.ToString(@"HH\:mm"))  // 24h format
                     .ToList()
                })
                    .OrderBy(g => g.Date)
    .ToList();


            // Tạo ViewModel và truyền dữ liệu cho View
            var viewModel = new MovieDetailViewModel
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                PosterUrl = movie.PosterUrl,
                BannerUrl = movie.BannerUrl,
                TrailerUrl = movie.TrailerUrl,
                Genre = movie.Genre,
                Director = movie.Director,
                Cast = movie.Cast,
                Country = movie.Country,
                ReleaseDate = movie.ReleaseDate,
                Description = movie.Description,

                ShowDates = groupedShowtimes.Select(g => g.Date).ToList(),
                GroupedShowtimesByDate = groupedShowtimes
            };

            return View("MovieDetails", viewModel);
        }

    }
}