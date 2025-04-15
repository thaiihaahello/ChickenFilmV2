using ChickenFilmV2.Models;
using ChickenFilmV2.ViewModels;
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
            // ✅ Dùng DateTime thay vì DateOnly
            var today = DateTime.Today;

            // Lấy tất cả các phim có suất chiếu
            var allMovies = _context.Movies
    .Where(m => m.Showtimes.Any(s => s.Status == "playing" || s.Status == "scheduled"))
    .Include(m => m.Showtimes) // 🔥 thêm dòng này
    .ToList();


            // Phim đang chiếu: ShowDate <= hôm nay và Status là "playing"
            var dangChieu = allMovies
                .Where(m => m.Showtimes.Any(s =>
                    s.ShowDate.HasValue && s.ShowDate.Value.Date <= today && s.Status == "playing"))
                .ToList();

            // Phim sắp chiếu: ShowDate > hôm nay và Status là "scheduled"
            var sapChieu = allMovies
                .Where(m => m.Showtimes.Any(s =>
                    s.ShowDate.HasValue && s.ShowDate.Value.Date > today && s.Status == "scheduled"))
                .ToList();

            _logger.LogInformation("Danh sách phim sắp chiếu: " + string.Join(", ", sapChieu.Select(m => m.Title)));
            // Log thông tin về số lượng phim
            _logger.LogInformation($"Phim đang chiếu: {dangChieu.Count} phim");
            _logger.LogInformation($"Phim sắp chiếu: {sapChieu.Count} phim");

            // Tạo ViewModel với các danh sách phim
            var viewModel = new ListPhimViewModel
            {
                AllMovies = allMovies, // Hiển thị tất cả các phim
                DangChieu = dangChieu, // Hiển thị phim đang chiếu
                SapChieu = sapChieu    // Hiển thị phim sắp chiếu
            };

            return View(viewModel); // Trả về view với dữ liệu
        }

        public IActionResult MovieDetails(int id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.MovieId == id);
            ;
            if (movie == null) return NotFound();

            var viewModel = new MovieDetailViewModel
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                PosterUrl = movie.PosterUrl,
                TrailerUrl = movie.TrailerUrl,
                Genre = movie.Genre,
                Director = movie.Director,
                Cast = movie.Cast,
                Country = movie.Country,
                ReleaseDate = movie.ReleaseDate.ToDateTime(TimeOnly.MinValue),
                Description = movie.Description,

                ShowDates = _context.Showtimes
                    .Where(s => s.MovieId == id)
                    .Select(s => s.ShowDate.HasValue ? s.ShowDate.Value.ToString("dd'/'MM") : "")
                    .Distinct()
                    .ToList(),

                Showtimes = _context.Showtimes
                    .Where(s => s.MovieId == id)
                    .GroupBy(s => s.Format)
                    .Select(g => new ShowtimeViewModel
                    {
                        Format = g.Key,
                        TimeSlots = g.Select(x => x.ShowTime1.ToString(@"hh\:mm")).ToList()
                    }).ToList()
            };

            return View("MovieDetails", viewModel);

        }

    }
}