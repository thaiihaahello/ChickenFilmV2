using ChickenFilmV2.Contacts;
using ChickenFilmV2.Models;
using ChickenFilmV2.ViewModelManager;
using Microsoft.EntityFrameworkCore;
namespace ChickenFilmV2.Services
{
    public class ShowtimesServices : IShowtimesServices
    {
        private readonly MovieDbContext _context;

        public ShowtimesServices(MovieDbContext context)
        {
            _context = context;
        }

        public Showtime GetShowtimeById(int id)
        {
            return _context.Showtimes.Find(id);
        }

        public void AddShowtime(Showtime showtime)
        {
            _context.Showtimes.Add(showtime);
            _context.SaveChanges();
        }

        public void UpdateShowtime(Showtime showtime)
        {
            _context.Showtimes.Update(showtime);
            _context.SaveChanges();
        }

        public void DeleteShowtime(int id)
        {
            var showtime = GetShowtimeById(id);
            if (showtime != null)
            {
                _context.Showtimes.Remove(showtime);
                _context.SaveChanges();
            }
        }

        public List<ShowtimeViewModel> SearchShowtimes(string? AuditoriumName, DateOnly? showDate)
        {
            var query = _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Auditorium)
                .AsQueryable();
            if (showDate != null)
            {
                query = query.Where(s => s.ShowDate == showDate);
            }
            if (!string.IsNullOrEmpty(AuditoriumName))
            {
                query = query.Where(s => s.Auditorium.AuditoriumName.Contains(AuditoriumName));
            }
            var showtimeViewModels = query.Select(s => new ShowtimeViewModel
            {
                ShowtimeId = s.ShowtimeId,
                MovieId = s.MovieId,
                MovieTitle = s.Movie.Title,
                AuditoriumId = s.AuditoriumId,
                Language = s.Movie.Language,
                Format = s.Movie.Format,
                AuditoriumName = s.Auditorium.AuditoriumName,
                ShowDate = s.ShowDate,
                Showtimes = s.ShowTime1,
                Status = s.Status
            }).ToList();
            return showtimeViewModels;
        }
    }
}