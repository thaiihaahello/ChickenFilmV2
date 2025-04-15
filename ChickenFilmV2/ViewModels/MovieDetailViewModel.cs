namespace ChickenFilmV2.ViewModels
{
    public class MovieDetailViewModel
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string PosterUrl { get; set; }
        public string TrailerUrl { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Cast { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public DateTime ReleaseDate { get; set; }

        public List<string> ShowDates { get; set; } // danh sách ngày (vd: 18/03, 19/03...)
        public List<ShowtimeViewModel> Showtimes { get; set; } // các suất chiếu theo định dạng
    }

    public class ShowtimeViewModel
    {
        public string Format { get; set; } // Ví dụ: "2D Phụ Đề"
        public List<string> TimeSlots { get; set; } // Ví dụ: ["19:30", "22:15"]
    }
}
