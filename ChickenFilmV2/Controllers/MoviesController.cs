using ChickenFilmV2.Models.ViewModel;
using ChickenFilmV2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChickenFilmV2.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieDbContext _context;

        public MoviesController(MovieDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies.ToListAsync();
            return View(movies);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewmodel = new MovieViewModel();
            return View(viewmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                string? posterPath = "";
                string? bannerPath = "";

                if (viewmodel.PosterUrl != null && viewmodel.PosterUrl.Length > 0)
                {
                    var fileName = Path.GetFileName(viewmodel.PosterUrl.FileName);
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "movies");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewmodel.PosterUrl.CopyToAsync(stream);
                    }

                    posterPath = Path.Combine("image", "movies", fileName);
                }

                if (viewmodel.BannerUrl != null && viewmodel.BannerUrl.Length > 0)
                {
                    var fileName = Path.GetFileName(viewmodel.BannerUrl.FileName);
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "movies");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewmodel.BannerUrl.CopyToAsync(stream);
                    }

                    bannerPath = Path.Combine("image", "movies", fileName);
                }

                var movies = new Movie()
                {
                    Title = viewmodel.Title,
                    BannerUrl = bannerPath,
                    PosterUrl = posterPath,
                    AgeRating = viewmodel.AgeRating,
                    Genre = viewmodel.Genre,
                    Duration = viewmodel.Duration,
                    ReleaseDate = viewmodel.ReleaseDate,
                    Rating = viewmodel.Rating,
                    Status = viewmodel.Status,
                    Format = viewmodel.Format,
                    Language = viewmodel.Language,
                    Director = viewmodel.Director,
                    Cast = viewmodel.Cast,
                    Description = viewmodel.Description,
                    TrailerUrl = viewmodel.TrailerUrl,
                    Country = viewmodel.Country,
                };

                await _context.Movies.AddAsync(movies);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Movies");
            }
            return View(viewmodel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movies = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == id);
            if (movies is not null)
            {
               var viewmodel = new EditMovieViewModel()
               {
                   MovieId = movies.MovieId,
                   Title = movies.Title,
                   AgeRating = movies.AgeRating,
                   Genre = movies.Genre,
                   Duration = movies.Duration,
                   ReleaseDate = movies.ReleaseDate,
                   Rating = movies.Rating,
                   Status = movies.Status,
                   Format = movies.Format,
                   Language = movies.Language,
                   Director = movies.Director,
                   Cast = movies.Cast,
                   Description = movies.Description,
                   TrailerUrl = movies.TrailerUrl,
                   Country = movies.Country,
                   BannerPath = movies.BannerUrl,
                   PosterPath = movies.PosterUrl,

               };
                return View("Edit", viewmodel);
            }
            return RedirectToAction("Index", "Movies");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (EditMovieViewModel model)
        {
            var movie = await _context.Movies.FindAsync(model.MovieId);
            if (movie is not null)
            {
                movie.Title = model.Title;
                movie.AgeRating = model.AgeRating;
                movie.Genre = model.Genre;
                movie.Duration = model.Duration;
                movie.ReleaseDate = model.ReleaseDate;
                movie.Rating = model.Rating;
                movie.Status = model.Status;
                movie.Format = model.Format;
                movie.Language = model.Language;
                movie.Director = model.Director;
                movie.Cast = model.Cast;
                movie.Description = model.Description;
                movie.TrailerUrl = model.TrailerUrl;
                movie.Country = model.Country;

                if (model.PosterUrl != null && model.PosterUrl.Length > 0)
                {
                    var fileName = Path.GetFileName(model.PosterUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "movies", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.PosterUrl.CopyToAsync(stream);
                    }
                    movie.PosterUrl = Path.Combine("images", "movies", fileName);
                }
                if (model.BannerUrl != null && model.BannerUrl.Length > 0)
                {
                    var fileName = Path.GetFileName(model.BannerUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "movies", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.BannerUrl.CopyToAsync(stream);
                    }
                    movie.BannerUrl = Path.Combine("images", "movies", fileName);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Movies");
            }
            return RedirectToAction("Index", "Movies");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditMovieViewModel model)
        {
            var movies = await _context.Movies.FindAsync(model.MovieId);
            if (movies is not null)
            {
                _context.Movies.Remove(movies);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Movies");
            }
            return RedirectToAction("Index", "Movies");
        }
    }
}
