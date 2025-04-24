using ChickenFilmV2.Models;

namespace ChickenFilmV2.ViewModelManager
{
    public class ScheduleViewModel
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<Movie>? Movies { get; set; }
    }
}
