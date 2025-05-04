using ChickenFilmV2.Models;
using ChickenFilmV2.ViewModelManager;

namespace ChickenFilmV2.Contacts
{
    public interface IShowtimesServices
    {
        public Showtime GetShowtimeById(int id);
        public void AddShowtime(Showtime showtime);
        public void UpdateShowtime(Showtime showtime);
        public void DeleteShowtime(int id);

        //List<ShowtimeViewModel> SearchShowtimes(string? AuditoriumName, DateOnly? showDate);
    }
}
