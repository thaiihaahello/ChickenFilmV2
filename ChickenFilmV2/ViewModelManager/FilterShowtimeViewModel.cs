namespace ChickenFilmV2.ViewModelManager
{
    public class FilterShowtimeViewModel
    {
        public List<ShowtimeViewModel> Showtime { get; set; } = new();
        public DateOnly? SearchShowDate { get; set; }
        public string? SearchAuditoriumName { get; set; }

        public List<string>? AuditoriumName { get; set; }

        public ShowtimeViewModel createModel { get; set; } = new ShowtimeViewModel();
    }
}
