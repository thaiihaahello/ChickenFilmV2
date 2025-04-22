using ChickenFilmV2.Contacts;
using ChickenFilmV2.Models;
using ChickenFilmV2.ViewModelManager;
using Microsoft.AspNetCore.Mvc;

namespace ChickenFilmV2.Controllers
{
    public class SchedulesController : Controller
    {
        private readonly MovieDbContext _context;
        private readonly IMoviesServices _moviesServices;

        public SchedulesController(MovieDbContext context, IMoviesServices moviesServices)
        {
            _context = context;
            _moviesServices = moviesServices;
        }
        [HttpGet]
        public IActionResult IndexSchedule()
        {
            var movies = _moviesServices.GetAllMovies();
            return View(movies);
        }
        [HttpGet]
        public IActionResult CreateSchedule()
        {
            var viewmodel = new ScheduleViewModel();
            return View(viewmodel);
        }

        [HttpPost]
        public IActionResult CreateSchedule(ScheduleViewModel viewmodel)
        {
            var success = _moviesServices.CreateSchedule(viewmodel);

            if (!success)
            {
                viewmodel.Movies = _moviesServices.GetAllMovies();
                return View("IndexSchedule", viewmodel);
            }

            return RedirectToAction("IndexSchedule", "Schedules");
        }
        [HttpGet]
        public IActionResult EditSchedule(int id)
        {
            var movie = _moviesServices.GetMovieById(id);
            if (movie != null)
            {
                var viewmodel = new ScheduleViewModel
                {
                    MovieId = movie.MovieId,
                    Title = movie.Title,
                    ReleaseDate = movie.ReleaseDate,
                    EndDate = movie.EndDate
                };
                return View(viewmodel);
            }
            return RedirectToAction("IndexSchedule", "Schedules");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditSchedule(IFormCollection form)
        {
            var movieId = Convert.ToInt32(form["MovieId"]);
            var reDateString = form["ReleaseDate"];
            DateOnly reDate = DateOnly.Parse(reDateString);
            var endDateString = form["EndDate"];
            DateOnly endDate = DateOnly.Parse(endDateString);

            var movie = _moviesServices.GetMovieById(movieId);
            if (movie != null)
            {
                movie.ReleaseDate = reDate;
                movie.EndDate = endDate;
                _moviesServices.UpdateMovie(movie);
                return RedirectToAction("IndexSchedule", "Schedules");
            }
            return RedirectToAction("IndexSchedule", "Schedules");
        }
    }
}
