using ChickenFilmV2.Models;

namespace ChickenFilmV2.ViewModels
{
    public class ListPhimViewModel
    {
        public List<MovieViewModel> AllMovies { get; set; } = new();
        public List<MovieViewModel> DangChieu { get; set; } = new();
        public List<MovieViewModel> SapChieu { get; set; } = new();
    }
}