using ChickenFilmV2.Models;
using ChickenFilmV2.ViewModelManager;

namespace ChickenFilmV2.ViewModels
{
    public class MovieDetailViewModel
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string PosterUrl { get; set; }
        public string BannerUrl { get; set; }
        public string TrailerUrl { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Cast { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }

        public List<DateTime> ShowDates { get; set; }
        public List<ShowtimeByDateViewModel> GroupedShowtimesByDate { get; set; }
        public List<ShowtimeViewModel> Showtimes { get; set; }
    }
  
    public class ShowtimeByDateViewModel
    {
        public DateTime Date { get; set; } 
        public List<string> TimeSlots { get; set; }  
    }
    public class ShowtimeFormatGroup
    {
        public string Format { get; set; }
        public List<string> TimeSlots { get; set; }
    }
}
