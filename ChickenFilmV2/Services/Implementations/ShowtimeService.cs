
    using global::ChickenFilmV2.Models;
    using global::ChickenFilmV2.Services.Interfaces.ChickenFilmV2.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;

    namespace ChickenFilmV2.Services
    {
        public class ShowtimeService : IShowtimeService
        {
            private readonly MovieDbContext _context;

            public ShowtimeService(MovieDbContext context)
            {
                _context = context;
            }

            // Lấy tất cả các suất chiếu của một bộ phim
            public async Task<IEnumerable<Showtime>> GetShowtimesByMovieIdAsync(int movieId)
            {
                return await _context.Showtimes
                    .Where(s => s.MovieId == movieId)
                    .Include(s => s.Auditorium)
                    .ToListAsync();
            }

            // Lấy tất cả các ngày chiếu của một bộ phim  
            public async Task<IEnumerable<string>> GetShowDatesByMovieIdAsync(int movieId)
            {
                return await _context.Showtimes
                    .Where(s => s.MovieId == movieId)
                    .Select(s => s.ShowDate.HasValue ? s.ShowDate.Value.ToString("dd/MM/yyyy") : string.Empty)
                    .Distinct()
                    .ToListAsync();
            }
        }
    }

