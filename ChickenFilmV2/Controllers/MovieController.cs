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
                    s.ShowDate.HasValue && s.ShowDate.Value <= today && s.Status == "Đang chiếu"))
                .ToList();

            // Phim sắp chiếu: ShowDate > hôm nay và Status là "scheduled"
            var sapChieu = allMovies
                .Where(m => m.Showtimes.Any(s =>
                    s.ShowDate.HasValue && s.ShowDate.Value > today && s.Status == "Sắp chiếu"))
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
                .GroupBy(s => s.ShowDate.Value.ToDateTime(TimeOnly.MinValue).Date)  // Nhóm theo ngày
                .Select(g => new ShowtimeByDateViewModel
                {
                    Date = g.Key,
                    TimeSlots = g.OrderBy(x => x.ShowTime1)  // Sắp xếp theo giờ chiếu
                                 .Select(x => x.ShowTime1.ToString(@"HH\:mm"))  // Định dạng giờ chiếu
                                 .ToList()
                })
                .OrderBy(g => g.Date)  // Sắp xếp theo ngày
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
                ReleaseDate = movie.ReleaseDate.ToDateTime(TimeOnly.MinValue),
                Description = movie.Description,

                ShowDates = groupedShowtimes.Select(g => g.Date).ToList(),
                GroupedShowtimesByDate = groupedShowtimes
            };

            return View("MovieDetails", viewModel);
        }

    }
}
