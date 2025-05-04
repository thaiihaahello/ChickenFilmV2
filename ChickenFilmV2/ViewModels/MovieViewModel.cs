using ChickenFilmV2.Models;

namespace ChickenFilmV2.ViewModels
{
    public class MovieViewModel
    {
        public int MovieId { get; set; }
        public string? Title { get; set; }

        public string? PosterUrl { get; set; }
        public string? Format { get; set; }
        public string? Language { get; set; }
    }
}
