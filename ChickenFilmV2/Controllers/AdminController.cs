using ChickenFilmV2.Contacts;
using ChickenFilmV2.Models;
using ChickenFilmV2.ViewModelManager;
using ChickenFilmV2.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChickenFilmV2.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMoviesServices _movieService;

        public AdminController(IMoviesServices movieService)
        {
            _movieService = movieService;
        }

        public IActionResult Index()
        {
            var movies = _movieService.GetAllMovies();
            var viewModel = new AdminDashboardViewModel
            {
                Movies = movies
            };
            return View(viewModel);
        }

        public IActionResult ManageFilm()
        {
            var movies = _movieService.GetAllMovies();
            var viewModel = new AdminDashboardViewModel
            {
                Movies = movies
            };
            return View(viewModel);
        }

        public IActionResult MovieDetails(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }

            var viewModel = new MovieDetailViewModel
            {
                Title = movie.Title,
                Genre = movie.Genre,
                Director = movie.Director,
                Cast = movie.Cast,
                ReleaseDate = movie.ReleaseDate,
                Country = movie.Country,
                Description = movie.Description,
                TrailerUrl = movie.TrailerUrl,
                PosterUrl = movie.PosterUrl,
                ShowDates = new List<DateTime>(), // Tạm để trống nếu chưa có dữ liệu từ ShowtimeService
                Showtimes = new List<ShowtimeViewModel>()
            };

            return View(viewModel);
        }

        public IActionResult CreateFilm()
        {
            ViewData["Title"] = "Add New Movie";
            return View(new Movie());
        }

        [HttpPost]
        public IActionResult CreateFilm(Movie movie)
        {
            if (ModelState.IsValid)
            {
                _movieService.AddMovie(movie);
                return RedirectToAction("ManageFilm");
            }
            ViewData["Title"] = "Add New Movie";
            return View(movie);
        }
    }
}
