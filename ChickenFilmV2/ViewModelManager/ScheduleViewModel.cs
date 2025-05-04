using ChickenFilmV2.Models;

namespace ChickenFilmV2.ViewModelManager
{
    public class ScheduleViewModel
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = null!;
        public DateOnly? ReleaseDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public List<Movie>? Movies { get; set; }
    }
}
