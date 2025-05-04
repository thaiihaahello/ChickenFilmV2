using ChickenFilmV2.Models;

namespace ChickenFilmV2.ViewModels
{
    public class BookingViewModel
    {
        public List<Theater> Theaters { get; set; }
        public List<Movie> DangChieu { get; set; }
        public Dictionary<int, List<Showtime>> MovieShowtimes { get; set; }
    }
}