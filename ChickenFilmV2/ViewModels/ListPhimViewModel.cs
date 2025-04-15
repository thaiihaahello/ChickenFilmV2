using ChickenFilmV2.Models;

namespace ChickenFilmV2.ViewModels
{
    public class ListPhimViewModel
    {
        public List<Movie> AllMovies { get; set; } = new();
        public List<Movie> DangChieu { get; set; } = new();
        public List<Movie> SapChieu { get; set; } = new();
    }
}

