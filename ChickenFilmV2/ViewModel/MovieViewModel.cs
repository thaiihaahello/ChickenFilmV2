using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChickenFilmV2.ViewModel
{
    public class MovieViewModel
    {
        // Thông tin phim
        public string Title { get; set; } = null!;

        public IFormFile? BannerUrl { get; set; }

        public string? BannerPath { get; set; }

        public IFormFile? PosterUrl { get; set; }

        public string? PosterPath { get; set; }

        public string? AgeRating { get; set; }

        public string Genre { get; set; } = null!;

        public int Duration { get; set; }

        public DateOnly ReleaseDate { get; set; }

        public decimal? Rating { get; set; }

        public string? Status { get; set; }

        public string? Format { get; set; }

        public string? Language { get; set; }

        public string? Director { get; set; }

        public string? Cast { get; set; }

        public string? Description { get; set; }

        public string? TrailerUrl { get; set; }

        public string? Country { get; set; }
    }

}
