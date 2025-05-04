namespace ChickenFilmV2.Services.Interfaces
{
    using global::ChickenFilmV2.Models;

    namespace ChickenFilmV2.Services.Interfaces
    {
        public interface IShowtimeService
        {
            Task<IEnumerable<Showtime>> GetShowtimesByMovieIdAsync(int movieId); // Lấy lịch chiếu theo movieId
            Task<IEnumerable<string>> GetShowDatesByMovieIdAsync(int movieId); // Lấy các ngày chiếu của phim
        }
    }

}
