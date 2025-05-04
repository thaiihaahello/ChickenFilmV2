using ChickenFilmV2.Contacts;
using ChickenFilmV2.Models;
using ChickenFilmV2.ViewModelManager;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChickenFilmV2.Controllers
{
    public class ShowtimesController : Controller
    {
        private readonly IShowtimesServices _showtimesServices;
        private readonly IAuditoriumServices _auditoriumServices;
        private readonly IMoviesServices _movieServices;
        public ShowtimesController(MovieDbContext context, IShowtimesServices showtimesServices, IAuditoriumServices auditoriumServices, IMoviesServices movieServices)
        {
            _showtimesServices = showtimesServices;
            _auditoriumServices = auditoriumServices;
            _movieServices = movieServices;
        }


        [HttpGet]
        public IActionResult IndexShowtime(string? searchAuditoriumName, DateOnly? searchShowDate)
        {
            var auditoriums = _auditoriumServices.GetAllAuditoriums();
            var showtimes = _showtimesServices.SearchShowtimes(searchAuditoriumName, searchShowDate);

            var createModel = new ShowtimeViewModel
            {
                MovieList = _movieServices.GetAllMovies().Select(m => new SelectListItem
                {
                    Value = m.MovieId.ToString(),
                    Text = m.Title
                }).ToList(),

                AuditoriumList = _auditoriumServices.GetAllAuditoriums().Select(a => new SelectListItem
                {
                    Value = a.AuditoriumId.ToString(),
                    Text = a.AuditoriumName
                }).ToList(),

                AuditoriumId = auditoriums.FirstOrDefault(a => a.AuditoriumName == searchAuditoriumName)?.AuditoriumId ?? 0,
                ShowDate = searchShowDate
            };

            var viewModel = new FilterShowtimeViewModel
            {
                //Showtime = showtimes,
                SearchAuditoriumName = searchAuditoriumName,
                SearchShowDate = searchShowDate,
                AuditoriumName = _auditoriumServices.GetAuditoriumNames(),
                createModel = createModel
            };


            return View(viewModel);
        }

        [HttpGet]
        public IActionResult CreateShowtime(string? searchAuditoriumName, DateOnly? searchShowDate)
        {
            var showtimes = _showtimesServices.SearchShowtimes(searchAuditoriumName, searchShowDate);

            var movies = _movieServices.GetAllMovies();
            var auditoriums = _auditoriumServices.GetAllAuditoriums();
            var selectedAuditorium = auditoriums.FirstOrDefault(a => a.AuditoriumName == searchAuditoriumName);

            var createModel = new ShowtimeViewModel
            {
                MovieList = movies.Select(m => new SelectListItem
                {
                    Value = m.MovieId.ToString(),
                    Text = m.Title
                }).ToList(),

                AuditoriumList = auditoriums.Select(a => new SelectListItem
                {
                    Value = a.AuditoriumId.ToString(),
                    Text = a.AuditoriumName
                }).ToList(),

                AuditoriumId = selectedAuditorium?.AuditoriumId ?? 0,
                ShowDate = searchShowDate
            };

            var viewModel = new FilterShowtimeViewModel
            {
                Showtime = showtimes,
                SearchAuditoriumName = searchAuditoriumName,
                SearchShowDate = searchShowDate,
                AuditoriumName = _auditoriumServices.GetAuditoriumNames(),
                createModel = createModel  // Truyền model tạo mới cho view
            };

            return PartialView("CreateShowtime", createModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateShowtime(ShowtimeViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                // Nếu không hợp lệ, thêm dữ liệu cho dropdown
                viewmodel.MovieList = _movieServices.GetAllMovies().Select(m => new SelectListItem
                {
                    Value = m.MovieId.ToString(),
                    Text = m.Title
                }).ToList();

                viewmodel.AuditoriumList = _auditoriumServices.GetAllAuditoriums().Select(a => new SelectListItem
                {
                    Value = a.AuditoriumId.ToString(),
                    Text = a.AuditoriumName
                }).ToList();

                return View(viewmodel); // Trả về view nếu ModelState không hợp lệ
            }

            // Tạo Showtime mới
            var showtime = new Showtime
            {
                AuditoriumId = viewmodel.AuditoriumId,
                MovieId = viewmodel.MovieId,
                ShowDate = viewmodel.ShowDate ?? default,
                ShowTime1 = viewmodel.Showtimes
            };

            _showtimesServices.AddShowtime(showtime); // Thêm Showtime vào cơ sở dữ liệu

            return RedirectToAction(nameof(IndexShowtime));  // Quay lại trang danh sách showtimes
        }
    }
}