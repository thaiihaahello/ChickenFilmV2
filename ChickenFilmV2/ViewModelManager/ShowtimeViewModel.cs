using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChickenFilmV2.ViewModelManager
{
    public class ShowtimeViewModel
    {
        public int ShowtimeId { get; set; }

        public int MovieId { get; set; }

        public int AuditoriumId { get; set; }

        public DateOnly? ShowDate { get; set; }

        public TimeOnly ShowTime1 { get; set; }

        public string? Status { get; set; }

        public string? MovieTitle { get; set; }

        public string? AuditoriumName { get; set; }

        public string? Language { get; set; }

        public string? Format { get; set; }

        public List<SelectListItem> MovieList { get; set; } = new();

        public List<SelectListItem> AuditoriumList { get; set; } = new();
    }
}
