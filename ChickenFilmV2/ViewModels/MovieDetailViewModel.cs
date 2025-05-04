using ChickenFilmV2.Models;

namespace ChickenFilmV2.ViewModels
{
    public class MovieDetailViewModel
    {
        // Thông tin cơ bản của phim
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

        // Danh sách ngày có suất chiếu
        public List<DateTime> ShowDates { get; set; }

        // Danh sách suất chiếu nhóm theo ngày (mỗi ngày có nhiều giờ chiếu)
        public List<ShowtimeByDateViewModel> GroupedShowtimesByDate { get; set; }
    }


    public class ShowtimeByDateViewModel
    {
        public DateTime Date { get; set; }  // Ngày chiếu
        public List<string> TimeSlots { get; set; }  // Danh sách giờ chiếu dạng "HH:mm"
    }

}
