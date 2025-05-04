using ChickenFilmV2.Models;

namespace ChickenFilmV2.ViewModels
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<Theater> Theaters { get; set; }
        public List<ShowtimeFormatGroup> ShowtimeFormats { get; set; }
    }
}
